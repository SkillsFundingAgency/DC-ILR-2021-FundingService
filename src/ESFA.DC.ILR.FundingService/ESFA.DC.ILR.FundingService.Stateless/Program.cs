using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.ServiceFabric;
using DC.JobContextManager;
using DC.JobContextManager.Interface;
using ESFA.DC.Auditing;
using ESFA.DC.Auditing.Dto;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.ILR.FundingService.ALB.Modules;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Stateless.Configuration;
using ESFA.DC.ILR.FundingService.Stateless.Handlers;
using ESFA.DC.ILR.FundingService.Stateless.Mappers;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.AzureCosmos;
using ESFA.DC.IO.AzureCosmos.Config.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Mapping.Interface;
using ESFA.DC.Queueing;
using ESFA.DC.Queueing.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ESFA.DC.ILR.FundingService.Stateless
{
    enum SerializationTypes
    {
        Json,
        Xml
    }

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
                builder.RegisterStatelessService<Stateless>("ESFA.DC.ILR.FundingService.StatelessType");

                using (var container = builder.Build())
                {
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
            var azureCosmosOptions = configHelper.GetSectionValues<AzureCosmosOptions>("AzureCosmosSection");
            containerBuilder.Register(c => new AzureCosmosKeyValuePersistenceConfig(
                    azureCosmosOptions.CosmosEndpointUrl,
                    azureCosmosOptions.CosmosAuthKeyOrResourceToken))
                .As<IAzureCosmosKeyValuePersistenceServiceConfig>().SingleInstance();

            containerBuilder.RegisterType<AzureCosmosKeyValuePersistenceService>().As<IKeyValuePersistenceService>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterModule<PreFundingALBModule>();

            // register serialization
            containerBuilder.RegisterType<JsonSerializationService>()
                .Keyed<ISerializationService>(SerializationTypes.Json);

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
                    c.ResolveKeyed<ISerializationService>(SerializationTypes.Json)))
                .As<IQueuePublishService<AuditingDto>>();
            containerBuilder.RegisterType<Auditor>().As<IAuditor>();

            // register Jobcontext services
            var topicConfig = new ServiceBusTopicConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.TopicName,
                serviceBusOptions.FundingCalcSubscriptionName,
                Environment.ProcessorCount);

            containerBuilder.Register(c =>
            {
                var topicSubscriptionSevice =
                    new TopicSubscriptionSevice<JobContextMessage>(
                        topicConfig,
                        c.ResolveKeyed<ISerializationService>(SerializationTypes.Json),
                        c.Resolve<ILogger>());
                return topicSubscriptionSevice;
            }).As<ITopicSubscriptionService<JobContextMessage>>();

            containerBuilder.Register(c =>
            {
                var topicPublishSevice =
                    new TopicPublishService<JobContextMessage>(
                        topicConfig,
                        c.ResolveKeyed<ISerializationService>(SerializationTypes.Json));
                return topicPublishSevice;
            }).As<ITopicPublishService<JobContextMessage>>();

            // register message mapper
            containerBuilder.RegisterType<JobContextMessageMapper>()
                .As<IMapper<JobContextMessage, JobContextMessage>>();

            // register MessageHandler
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler>();

            // register the  callback handle when a new message is received from ServiceBus
            containerBuilder.Register<Func<JobContextMessage, CancellationToken, Task<bool>>>(c =>
                c.Resolve<IMessageHandler>().Handle);

            containerBuilder.RegisterType<JobContextManagerForTopics<JobContextMessage>>().As<IJobContextManager>();

            return containerBuilder;
        }
    }
}
