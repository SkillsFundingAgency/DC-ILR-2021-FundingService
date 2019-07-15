using Autofac;
using ESFA.DC.ILR.FundingService.Providers;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ServiceFabric.Common.Modules;

namespace ESFA.DC.ILR.FundingService.Modules
{
    public class BaseModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FilePersistanceService>().As<IFilePersistanceService>().InstancePerLifetimeScope();
            containerBuilder.RegisterModule<SerializationModule>();
            containerBuilder.RegisterModule<ReferenceDataServiceModule>();
            containerBuilder.RegisterModule<OpaModule>();
        }
    }
}
