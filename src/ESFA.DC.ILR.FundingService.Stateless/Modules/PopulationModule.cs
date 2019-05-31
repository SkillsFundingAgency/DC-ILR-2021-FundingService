using Autofac;
using ESFA.DC.ILR.FundingService.Data.Population;
using ESFA.DC.ILR.FundingService.Data.Population.Context;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.File;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Internal;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class PopulationModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PopulationService>().As<IPopulationService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FileDataCachePopulationService>().As<IFileDataCachePopulationService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<InternalDataCachePopulationService>().As<IInternalDataCachePopulationService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ExternalDataCachePopulationService>().As<IExternalDataCachePopulationService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FundingContextPopulationService>().As<IFundingContextPopulationService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<PostcodesMapperService>().As<IPostcodesMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<OrganisationsMapperService>().As<IOrganisationsMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LargeEmployersMapperService>().As<ILargeEmployersMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AppsEarningsHistoryMapperService>().As<IAppsEarningsHistoryMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FCSMapperService>().As<IFCSMapperService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LARSMapperService>().As<ILARSMapperService>().InstancePerLifetimeScope();
        }
    }
}
