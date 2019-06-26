using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Modules
{
    public class DataCacheModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ExternalDataCachePopulationService>().As<IExternalDataCachePopulationService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<PostcodesMapperService>().As<IPostcodesMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<OrganisationsMapperService>().As<IOrganisationsMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LargeEmployersMapperService>().As<ILargeEmployersMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AppsEarningsHistoryMapperService>().As<IAppsEarningsHistoryMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FCSMapperService>().As<IFCSMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LARSMapperService>().As<ILARSMapperService>().InstancePerLifetimeScope();
        }
    }
}
