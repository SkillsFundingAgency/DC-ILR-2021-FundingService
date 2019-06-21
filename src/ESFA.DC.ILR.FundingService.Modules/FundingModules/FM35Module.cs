using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35.Service;
using ESFA.DC.ILR.FundingService.FM35.Service.Input;
using ESFA.DC.ILR.FundingService.FM35.Service.Rulebase;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.Modules.FundingModules
{
    public class FM35Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FM35RulebaseProvider>().As<IRulebaseStreamProvider<FM35LearnerDto>>();
            builder.RegisterType<SessionFactory<FM35LearnerDto>>().As<ISessionFactory<FM35LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService<FM35LearnerDto>>().As<IOPAService<FM35LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM35LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM35Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM35LearnerDto, FM35Global>>().As<IFundingService<FM35LearnerDto, FM35Global>>().InstancePerLifetimeScope();
        }
    }
}