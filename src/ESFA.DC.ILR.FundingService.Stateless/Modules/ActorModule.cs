using Autofac;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Implementations;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class ActorModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var actorNameParameter = "actorName";

            containerBuilder.RegisterType<ActorTask<IALBActor, ALBGlobal>>().As<IActorTask<IALBActor, ALBGlobal>>().WithParameter(actorNameParameter, ActorServiceNameConstants.ALB).InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActorTask<IFM81Actor, FM81Global>>().As<IActorTask<IFM81Actor, FM81Global>>().WithParameter(actorNameParameter, ActorServiceNameConstants.FM81).InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActorTask<IFM70Actor, FM70Global>>().As<IActorTask<IFM70Actor, FM70Global>>().WithParameter(actorNameParameter, ActorServiceNameConstants.FM70).InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActorTask<IFM35Actor, FM35Global>>().As<IActorTask<IFM35Actor, FM35Global>>().WithParameter(actorNameParameter, ActorServiceNameConstants.FM35).InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActorTask<IFM36Actor, FM36Global>>().As<IActorTask<IFM36Actor, FM36Global>>().WithParameter(actorNameParameter, ActorServiceNameConstants.FM36).InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActorTask<IFM25Actor, FM25Global>>().As<IActorTask<IFM25Actor, FM25Global>>().WithParameter(actorNameParameter, ActorServiceNameConstants.FM25).InstancePerLifetimeScope();

            containerBuilder.RegisterType<FM81FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM81Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM70Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM35Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM36Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ALBFundingOutputCondenserService>().As<IFundingOutputCondenserService<ALBGlobal>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM25Global>>().InstancePerLifetimeScope();

            containerBuilder.RegisterInstance(new ActorProvider<IFM81Actor>(ActorServiceNameConstants.FM81)).As<IActorProvider<IFM81Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM70Actor>(ActorServiceNameConstants.FM70)).As<IActorProvider<IFM70Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM35Actor>(ActorServiceNameConstants.FM35)).As<IActorProvider<IFM35Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM36Actor>(ActorServiceNameConstants.FM36)).As<IActorProvider<IFM36Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IALBActor>(ActorServiceNameConstants.ALB)).As<IActorProvider<IALBActor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM25Actor>(ActorServiceNameConstants.FM25)).As<IActorProvider<IFM25Actor>>();

            containerBuilder.RegisterType<ExecutionContext>().As<IExecutionContext>();
        }
    }
}
