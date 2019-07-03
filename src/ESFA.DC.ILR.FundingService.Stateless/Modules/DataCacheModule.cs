using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class DataCacheModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();
        }
    }
}
