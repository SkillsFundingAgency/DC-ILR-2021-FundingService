using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.Auditing;
using ESFA.DC.Auditing.Dto;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Config.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Configuration;
using ESFA.DC.ILR.FundingService.Stateless.Handlers;
using ESFA.DC.ILR.FundingService.Stateless.Mappers;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.AzureStorage;
using ESFA.DC.IO.AzureStorage.Config.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.IO.Redis;
using ESFA.DC.IO.Redis.Config;
using ESFA.DC.IO.Redis.Config.Interfaces;
using ESFA.DC.JobContext;
using ESFA.DC.JobStatus.Dto;
using ESFA.DC.JobStatus.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Mapping.Interface;
using ESFA.DC.Queueing;
using ESFA.DC.Queueing.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.ServiceFabric.Helpers;
using StatelessModule = ESFA.DC.ILR.FundingService.Stateless.Modules.StatelessModule;

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
                using (var container = ConfigureContainerBuilder().Build())
                {
                    var sss = container.Resolve<IReferenceDataConfig>();
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

        private static ContainerBuilder ConfigureContainerBuilder()
        {
            var builder = new ContainerBuilder();
            var configHelper = new ConfigurationHelper();

            var azureRedisCacheOptions = configHelper.GetSectionValues<AzureRedisCacheOptions>("AzureRedisSection");
            builder.Register(c => new RedisKeyValuePersistenceServiceConfig()
                {
                    ConnectionString = azureRedisCacheOptions.RedisCacheConnectionString,
                    KeyExpiry = new TimeSpan(14, 0, 0, 0)
                }).As<IRedisKeyValuePersistenceServiceConfig>().SingleInstance();

            builder.RegisterType<RedisKeyValuePersistenceService>().As<IKeyValuePersistenceService>().InstancePerLifetimeScope();
            builder.RegisterType<AzureStorageKeyValuePersistenceService>().Keyed<IKeyValuePersistenceService>(IOPersistenceKeys.Blob).InstancePerLifetimeScope();

            // register reference data configs
            var referenceDataConfig = configHelper.GetSectionValues<ReferenceDataConfig>("ReferenceDataSection");
            builder.RegisterInstance(referenceDataConfig).As<IReferenceDataConfig>().SingleInstance();

            // get ServiceBus, Azurestorage config values and register container
            var serviceBusOptions = configHelper.GetSectionValues<ServiceBusConfig>("ServiceBusSettings");
            builder.RegisterInstance(serviceBusOptions).As<IServiceBusConfig>().SingleInstance();

            // register logger
            var loggerOptions = configHelper.GetSectionValues<LoggerConfig>("LoggerSection");
            builder.RegisterInstance(loggerOptions).As<ILoggerConfig>().SingleInstance();
            builder.RegisterModule<LoggerModule>();

            var topicAndTaskSectionOptions = configHelper.GetSectionValues<TopicAndTaskSectionConfig>("TopicAndTaskSection");
            builder.RegisterInstance(topicAndTaskSectionOptions).As<ITopicAndTaskSectionConfig>().SingleInstance();

            // auditing
            var auditPublishConfig = new ServiceBusQueueConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.AuditQueueName,
                Environment.ProcessorCount);
            builder.Register(c => new QueuePublishService<AuditingDto>(
                    auditPublishConfig,
                    c.Resolve<ISerializationService>()))
                .As<IQueuePublishService<AuditingDto>>();
            builder.RegisterType<Auditor>().As<IAuditor>();

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

            builder.RegisterModule<StatelessModule>();

            builder.Register(c =>
            {
                var topicSubscriptionSevice =
                    new TopicSubscriptionSevice<JobContextDto>(
                        topicSubscribeConfig,
                        c.Resolve<ISerializationService>(),
                        c.Resolve<ILogger>(),
                        c.Resolve<IDateTimeProvider>());
                return topicSubscriptionSevice;
            }).As<ITopicSubscriptionService<JobContextDto>>();

            builder.Register(c =>
            {
                var topicPublishSevice =
                    new TopicPublishService<JobContextDto>(
                        topicPublishConfig,
                        c.Resolve<ISerializationService>());
                return topicPublishSevice;
            }).As<ITopicPublishService<JobContextDto>>();

            // register MessageHandler
            builder.RegisterType<MessageHandler>().As<IMessageHandler>().InstancePerLifetimeScope();

            // register Azure blob storage config
            var azureStorageConfig = configHelper.GetSectionValues<AzureStorageOptions>("AzureStorageSection");
            builder.Register(c =>
                    new AzureStorageKeyValuePersistenceServiceConfig(
                        azureStorageConfig.AzureBlobConnectionString,
                        azureStorageConfig.AzureBlobContainerName))
                .As<IAzureStorageKeyValuePersistenceServiceConfig>().SingleInstance();

            var jobStatusPublishConfig = new JobStatusQueueConfig(
                serviceBusOptions.ServiceBusConnectionString,
                serviceBusOptions.JobStatusQueueName,
                Environment.ProcessorCount);

            builder.Register(c => new QueuePublishService<JobStatusDto>(
                    jobStatusPublishConfig,
                    c.Resolve<IJsonSerializationService>()))
                .As<IQueuePublishService<JobStatusDto>>();
            builder.RegisterType<JobStatus.JobStatus>().As<IJobStatus>();

            // register ilrfile provider service
            builder.Register(c => new IlrFileProviderService(
                c.ResolveKeyed<IKeyValuePersistenceService>(IOPersistenceKeys.Blob),
                c.Resolve<IXmlSerializationService>())).As<IIlrFileProviderService>().InstancePerLifetimeScope();

            builder.RegisterType<JobContextMessageMapper>().As<IMapper<JobContextMessage, JobContextMessage>>().InstancePerLifetimeScope();

            // Register the Autofac magic for Service Fabric support.
            builder.RegisterServiceFabricSupport();

            // Register the stateless service.
            builder.RegisterStatelessService<Stateless>("ESFA.DC.ILR.1819.FundingService.StatelessType");

            return builder;
        }
    }
}
