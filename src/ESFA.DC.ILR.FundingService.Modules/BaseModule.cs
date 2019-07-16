using Autofac;
using ESFA.DC.ILR.FundingService.Providers;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;

namespace ESFA.DC.ILR.FundingService.Modules
{
    public class BaseModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FilePersistanceService>().As<IFilePersistanceService>().InstancePerLifetimeScope();
            containerBuilder.RegisterModule<ReferenceDataServiceModule>();
            containerBuilder.RegisterModule<OpaModule>();
        }
    }
}
