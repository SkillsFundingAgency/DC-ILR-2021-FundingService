using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class DataCacheModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<InternalDataCache>().As<IInternalDataCache>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FileDataCache>().As<IFileDataCache>().InstancePerLifetimeScope();
        }
    }
}
