using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35.Service;
using ESFA.DC.ILR.FundingService.FM35.Service.Input;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;

namespace ESFA.DC.ILR.FundingService.FM35Actor.Modules
{
    public class ActorFundingFM35Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SessionBuilder>().As<ISessionBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<OPADataEntityBuilder>().As<IOPADataEntityBuilder>().WithParameter("yearStartDate", new System.DateTime(2018, 8, 1)).InstancePerLifetimeScope();
            builder.RegisterInstance(new RulebaseProvider("FM35 Funding Calc 18_19")).As<IRulebaseProvider>();
            builder.RegisterType<OPAService>().As<IOPAService>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<ILearner>>().InstancePerLifetimeScope();
            builder.RegisterType<LargeEmployersReferenceDataService>().As<ILargeEmployersReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LARSReferenceDataService>().As<ILARSReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<OrganisationReferenceDataService>().As<IOrganisationReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<PostcodesReferenceDataService>().As<IPostcodesReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<FileDataCache>().As<IFileDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<FileDataService>().As<IFileDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM35Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<ILearner, FM35Global>>().As<IFundingService<ILearner, FM35Global>>().InstancePerLifetimeScope();
            builder.RegisterType<JsonSerializationService>().As<ISerializationService>().InstancePerLifetimeScope();
            builder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityAttributeService>().As<IDataEntityAttributeService>().InstancePerLifetimeScope();
        }
    }
}