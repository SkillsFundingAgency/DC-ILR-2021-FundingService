using Autofac;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.ALB.Service;
using ESFA.DC.ILR.FundingService.ALB.Service.Input;
using ESFA.DC.ILR.FundingService.ALB.Service.Rulebase;
using ESFA.DC.ILR.FundingService.Dto.Model;
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
    public class ALBModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ALBRulebaseProvider>().As<IRulebaseStreamProvider<ALBLearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<SessionFactory<ALBLearnerDto>>().As<ISessionFactory<ALBLearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService<ALBLearnerDto>>().As<IOPAService<ALBLearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<ALBLearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<ALBGlobal>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<ALBLearnerDto, ALBGlobal>>().As<IFundingService<ALBLearnerDto, ALBGlobal>>().InstancePerLifetimeScope();
        }
    }
}