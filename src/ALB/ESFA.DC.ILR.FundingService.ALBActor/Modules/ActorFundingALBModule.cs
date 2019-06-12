using Autofac;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.ALB.Service;
using ESFA.DC.ILR.FundingService.ALB.Service.Input;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FundingActor.Modules;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.ALBActor.Modules
{
    public class ActorFundingALBModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ActorModule>();

            builder.RegisterInstance(new RulebaseProvider("Loans Bursary 18_19")).As<IRulebaseProvider>();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<ALBLearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<ALBGlobal>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<ALBLearnerDto, ALBGlobal>>().As<IFundingService<ALBLearnerDto, ALBGlobal>>().InstancePerLifetimeScope();
        }
    }
}