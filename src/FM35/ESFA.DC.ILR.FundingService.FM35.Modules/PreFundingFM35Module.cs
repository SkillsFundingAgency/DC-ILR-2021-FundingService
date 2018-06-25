using System.Collections.Generic;
using Autofac;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Context;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.Data.Population;
using ESFA.DC.ILR.FundingService.Data.Population.Context;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Modules
{
    public class PreFundingFM35Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LARS>().As<ILARS>().InstancePerLifetimeScope();
            builder.RegisterType<Postcodes>().As<IPostcodes>().InstancePerLifetimeScope();
            builder.RegisterType<PreFundingFM35OrchestrationService>().As<IPreFundingFM35OrchestrationService>().InstancePerLifetimeScope();
            builder.RegisterType<PreFundingFM35PopulationService>().As<IPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceDataCachePopulationService>().As<IReferenceDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<InternalDataCache>().As<IInternalDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<LearnerPerActorServiceStub<ILearner, IList<ILearner>>>().As<ILearnerPerActorService<ILearner, IList<ILearner>>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContext>().As<IFundingContext>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContextPopulationService>().As<IFundingContextPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceDataCache>().As<IReferenceDataCache>().InstancePerLifetimeScope();
        }
    }
}
