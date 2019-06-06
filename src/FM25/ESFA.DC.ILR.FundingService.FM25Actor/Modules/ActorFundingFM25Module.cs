using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Input;
using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using ESFA.DC.ILR.FundingService.FundingActor.Modules;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM25Actor.Modules
{
    public class ActorFundingFM25Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ActorModule>();

            builder.RegisterInstance(new RulebaseProvider("Rulebase")).As<IRulebaseProvider>();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM25LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM25Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<ILearner, FM25Global>>().As<IFundingService<ILearner, FM25Global>>().InstancePerLifetimeScope();
        }
    }
}