using Autofac;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.FileService.Config;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Orchestrators;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Config;
using ESFA.DC.ILR.FundingService.Stateless.Context;
using ESFA.DC.ILR.FundingService.Stateless.Handlers;
using ESFA.DC.JobContextManager;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Mapping.Interface;
using ESFA.DC.ServiceFabric.Common.Config;
using ESFA.DC.ServiceFabric.Common.Modules;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class StatelessModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var serviceFabricConfigurationService = new ServiceFabricConfigurationService();

            var statelessServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAsStatelessServiceConfiguration();
            var azureStorageFileServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<AzureStorageFileServiceConfiguration>("AzureStorageFileServiceConfiguration");
            var ioConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<IOConfiguration>("IOConfiguration");

            containerBuilder.RegisterModule(new StatelessServiceModule(statelessServiceConfiguration));
            containerBuilder.RegisterModule(new IOModule(azureStorageFileServiceConfiguration, ioConfiguration));

            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterModule<CondenserModule>();
            containerBuilder.RegisterModule<DataCacheModule>();
            containerBuilder.RegisterModule<ProviderModule>();
            containerBuilder.RegisterModule<StatelessActorModule>();

            // register MessageHandler
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler<JobContextMessage>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultJobContextMessageMapper<JobContextMessage>>().As<IMapper<JobContextMessage, JobContextMessage>>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<DateTimeProvider.DateTimeProvider>().As<IDateTimeProvider>();

            containerBuilder.RegisterType<FundingServiceContext>().As<IFundingServiceContext>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FundingOrchestrationService>().As<IFundingOrchestrationService>();
        }
    }
}
