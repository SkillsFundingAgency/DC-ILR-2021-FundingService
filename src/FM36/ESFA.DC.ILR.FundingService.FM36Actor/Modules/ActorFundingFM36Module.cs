using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36.Service;
using ESFA.DC.ILR.FundingService.FM36.Service.Input;
using ESFA.DC.ILR.FundingService.FundingActor.Modules;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM36Actor.Modules
{
    public class ActorFundingFM36Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ActorModule>();

            builder.RegisterInstance(new RulebaseProvider("Apprenticeships Earnings Calc 18_19")).As<IRulebaseProvider>();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM36LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM36Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<ILearner, FM36Global>>().As<IFundingService<ILearner, FM36Global>>().InstancePerLifetimeScope();
        }
    }
}