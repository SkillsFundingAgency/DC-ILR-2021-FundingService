using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Integration.ServiceFabric;
using DC.JobContextManager;
using DC.JobContextManager.Interface;
using ESFA.DC.Auditing;
using ESFA.DC.Auditing.Dto;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.ILR.FundingService.ALB.Contexts;
using ESFA.DC.ILR.FundingService.ALB.Contexts.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.ALB.Modules;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.ALB.Service.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Orchestrators.Implementations;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.RuleBaseTasks;
using ESFA.DC.ILR.FundingService.Providers;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Output;
using ESFA.DC.ILR.FundingService.Stateless.Configuration;
using ESFA.DC.ILR.FundingService.Stateless.Handlers;
using ESFA.DC.ILR.FundingService.Stateless.Mappers;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.AzureStorage;
using ESFA.DC.IO.AzureStorage.Config.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.IO.Redis;
using ESFA.DC.IO.Redis.Config.Interfaces;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.KeyGenerator.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Mapping.Interface;
using ESFA.DC.Queueing;
using ESFA.DC.Queueing.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.Serialization.Xml;
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ESFA.DC.ILR.FundingService.Stateless
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                var builder = BuildContainer();

                // Register the Autofac magic for Service Fabric support.
                builder.RegisterServiceFabricSupport();

                // Register the stateless service.
                builder.RegisterStatelessService<Stateless>("ESFA.DC.ILR.1819.FundingService.StatelessType");

                using (var container = builder.Build())
                {
                    var sss = container.Resolve<ReferenceDataConfig>();
                    var ss = container.Resolve<IPreFundingSFOrchestrationService>();
                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Stateless).Name);

                    // Prevents this host process from terminating so services keep running.
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ContainerBuilder BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            var configHelper = new ConfigurationHelper();

            // register Cosmos config
            var azureRedisCacheOptions = configHelper.GetSectionValues<AzureRedisCacheOptions>("AzureRedisSection");
            containerBuilder.Register(c => new AzureRedisKeyValuePersistenceConfig(
                    azureRedisCacheOptions.RedisCacheConnectionString))
                .As<IRedisKeyValuePersistenceServiceConfig>().SingleInstance();

            containerBuilder.RegisterType<RedisKeyValuePersistenceService>().As<IKeyValuePersistenceService>()
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<AzureStorageKeyValuePersistenceService>().Keyed<IKeyValuePersistenceService>(IOPersistenceKeys.Blob)
                .InstancePerLifetimeScope();

            // register serialization
            containerBuilder.RegisterType<JsonSerializationService>()
                .As<ISerializationService>();
            containerBuilder.RegisterType<JsonSerializationService>()
                .As<IJsonSerializationService>();
            containerBuilder.RegisterType<XmlSerializationService>()
                .As<IXmlSerializationService>();

            // register reference data configs
            var referenceDataConfig =
                configHelper.GetSectionValues<ReferenceDataConfig>("ReferenceDataSection");
            containerBuilder.RegisterInstance(referenceDataConfig).As<ReferenceDataConfig>().SingleInstance();

            // get ServiceBus, Azurestorage config values and register container
            var serviceBusOptions =
                configHelper.GetSectionValues<ServiceBusOptions>("ServiceBusSettings");
            containerBuilder.RegisterInstance(serviceBusOptions).As<ServiceBusOptions>().SingleInstance();

            // register logger
            var loggerOptions =
                configHelper.GetSectionValues<LoggerOptions>("LoggerSection");
            containerBuilder.RegisterInstance(loggerOptions).As<LoggerOptions>().SingleInstance();
            containerBuilder.RegisterModule<LoggerModule>();

            // auditing
            var auditPublishConfig = new ServiceBusQueueConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.AuditQueueName,
                Environment.ProcessorCount);
            containerBuilder.Register(c => new QueuePublishService<AuditingDto>(
                    auditPublishConfig,
                    c.Resolve<ISerializationService>()))
                .As<IQueuePublishService<AuditingDto>>();
            containerBuilder.RegisterType<Auditor>().As<IAuditor>();

            // register Jobcontext services
            var topicSubscribeConfig = new ServiceBusTopicConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.TopicName,
                serviceBusOptions.FundingCalcSubscriptionName,
                Environment.ProcessorCount);

            // register Jobcontext services
            var topicPublishConfig = new ServiceBusTopicConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.TopicName,
                serviceBusOptions.DataStoreSubscriptionName,
                Environment.ProcessorCount);

            containerBuilder.RegisterModule<PreFundingALBModule>();

            containerBuilder.Register(c =>
            {
                var topicSubscriptionSevice =
                    new TopicSubscriptionSevice<JobContextDto>(
                        topicSubscribeConfig,
                        c.Resolve<ISerializationService>(),
                        c.Resolve<ILogger>());
                return topicSubscriptionSevice;
            }).As<ITopicSubscriptionService<JobContextDto>>();

            containerBuilder.Register(c =>
            {
                var topicPublishSevice =
                    new TopicPublishService<JobContextDto>(
                        topicPublishConfig,
                        c.Resolve<ISerializationService>());
                return topicPublishSevice;
            }).As<ITopicPublishService<JobContextDto>>();

            // register message mapper
            containerBuilder.RegisterType<JobContextMessageMapper>()
                .As<IMapper<JobContextMessage, JobContextMessage>>();

            // register MessageHandler
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler>().InstancePerLifetimeScope();

            // register the  callback handle when a new message is received from ServiceBus
            containerBuilder.Register<Func<JobContextMessage, CancellationToken, Task<bool>>>(c =>
                c.Resolve<IMessageHandler>().Handle).InstancePerLifetimeScope();

            containerBuilder.RegisterType<JobContextManagerForTopics<JobContextMessage>>().As<IJobContextManager>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<JobContextMessage>().As<IJobContextMessage>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<PreFundingSFOrchestrationService>().As<IPreFundingSFOrchestrationService>()
                .InstancePerLifetimeScope();

            // register funding service dto
            containerBuilder.RegisterType<FundingServiceDto>().As<IFundingServiceDto>()
                .InstancePerLifetimeScope();

            // register Azure blob storage config
            var azureStorageConfig =
                configHelper.GetSectionValues<AzureStorageOptions>("AzureStorageSection");
            containerBuilder.Register(c =>
                    new AzureStorageKeyValuePersistenceServiceConfig(
                        azureStorageConfig.AzureBlobConnectionString,
                        azureStorageConfig.AzureBlobContainerName))
                .As<IAzureStorageKeyValuePersistenceServiceConfig>().SingleInstance();

            // register ilrfile provider service
            containerBuilder.Register(c => new IlrFileProviderService(
                c.ResolveKeyed<IKeyValuePersistenceService>(IOPersistenceKeys.Blob),
                c.Resolve<IXmlSerializationService>())).As<IIlrFileProviderService>().InstancePerLifetimeScope();

            // register fundingoutput persistence service
            containerBuilder.RegisterType<FundingOutputPersistenceSfService<IFundingOutputs>>()
                .As<IFundingOutputPersistenceService<IFundingOutputs>>()
                .InstancePerLifetimeScope();

            // register key generator
            containerBuilder.RegisterType<KeyGenerator.KeyGenerator>().As<IKeyGenerator>().SingleInstance();

            // register ALB SF orchestrator Task
            containerBuilder.RegisterType<ALBOrchestrationSFTask>().As<IALBOrchestrationSFTask>().InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
