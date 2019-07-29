using Autofac;
using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM70Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM81Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor;
using ESFA.DC.ILR.FundingService.FundingActor.Constants;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor.Tasks;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class StatelessActorModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var actorNameParameter = "actorName";

            containerBuilder.RegisterType<ALBActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskALB).WithParameter(actorNameParameter, ActorServiceNameConstants.ALB).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM25).WithParameter(actorNameParameter, ActorServiceNameConstants.FM25).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM35).WithParameter(actorNameParameter, ActorServiceNameConstants.FM35).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM36).WithParameter(actorNameParameter, ActorServiceNameConstants.FM36).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM70).WithParameter(actorNameParameter, ActorServiceNameConstants.FM70).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM81ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM81).WithParameter(actorNameParameter, ActorServiceNameConstants.FM81).InstancePerLifetimeScope();

            containerBuilder.RegisterInstance(new ActorProvider<IFM81Actor>(ActorServiceNameConstants.FM81)).As<IActorProvider<IFM81Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM70Actor>(ActorServiceNameConstants.FM70)).As<IActorProvider<IFM70Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM35Actor>(ActorServiceNameConstants.FM35)).As<IActorProvider<IFM35Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM36Actor>(ActorServiceNameConstants.FM36)).As<IActorProvider<IFM36Actor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IALBActor>(ActorServiceNameConstants.ALB)).As<IActorProvider<IALBActor>>();
            containerBuilder.RegisterInstance(new ActorProvider<IFM25Actor>(ActorServiceNameConstants.FM25)).As<IActorProvider<IFM25Actor>>();

            containerBuilder.RegisterType<ExecutionContext>().As<IExecutionContext>();

            //containerBuilder.RegisterAssemblyTypes(ThisAssembly).Where(s => s.IsAssignableTo<IMapper>).AsImplementedInterfaces
        }
    }
}
