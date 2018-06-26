using System.Collections.Generic;
using Autofac;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.Data.Context;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Context;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Modules
{
    public class PreFundingALBModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<ReferenceDataConfig>();
                return new LARS(referenceDataConfig.LARSConnectionString);
            }).As<ILARS>().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<ReferenceDataConfig>();
                return new Postcodes(referenceDataConfig.PostCodeConnectionString);
            }).As<IPostcodes>().InstancePerLifetimeScope();

            builder.RegisterType<PreFundingALBOrchestrationService>().As<IPreFundingALBOrchestrationService>().InstancePerLifetimeScope();
            builder.RegisterType<PreFundingALBPopulationService>().As<IPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCachePopulationService>().As<IExternalDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<LearnerPerActorServiceStub<ILearner, IList<ILearner>>>().As<ILearnerPerActorService<ILearner, IList<ILearner>>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContext>().As<IFundingContext>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContextPopulationService>().As<IFundingContextPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();
        }
    }
}
