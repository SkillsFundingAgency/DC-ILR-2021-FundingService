using Autofac;
using ESFA.DC.DateTime.Provider;
using ESFA.DC.DateTime.Provider.Interface;
using ESFA.DC.ILR.FundingService.ALB.ExternalData.LARS;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service.Interface;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.FM35.Service.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Rulebase;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM35.Modules
{
    public class ActorFundingFM35Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SessionBuilder>().As<ISessionBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<OPADataEntityBuilder>().As<IOPADataEntityBuilder>().WithParameter("yearStartDate", new System.DateTime(2018, 8, 1)).InstancePerLifetimeScope();
            builder.RegisterType<RulebaseProviderFactory>().As<IRulebaseProviderFactory>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService>().As<IOPAService>().InstancePerLifetimeScope();
            builder.RegisterType<AttributeBuilder>().As<IAttributeBuilder<IAttributeData>>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityBuilder>().As<IDataEntityBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<LargeEmployersReferenceDataService>().As<ILargeEmployersReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LARSReferenceDataService>().As<ILARSReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<OrganisationReferenceDataService>().As<IOrganisationReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<PostcodesReferenceDataService>().As<IPostcodesReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceDataCache>().As<IReferenceDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IFundingOutputService>().InstancePerLifetimeScope();
            builder.RegisterType<FM35.Service.FundingService>().As<IFundingService>().InstancePerLifetimeScope();
            builder.RegisterType<FM35OrchestrationService>().As<IFM35OrchestrationService>().InstancePerLifetimeScope();
        }
    }
}