using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70.Service;
using ESFA.DC.ILR.FundingService.FM70.Service.Input;
using ESFA.DC.ILR.FundingService.FundingActor.Modules;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM70Actor.Modules
{
    public class ActorFundingFM70Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ActorModule>();

            builder.RegisterInstance(new RulebaseProvider("ESF 1819 Funding Calc")).As<IRulebaseProvider>();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM70LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM70Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM70LearnerDto, FM70Global>>().As<IFundingService<FM70LearnerDto, FM70Global>>().InstancePerLifetimeScope();
        }
    }
}