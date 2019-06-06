using Autofac;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Dto.Providers;
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
using ESFA.DC.ILR.FundingService.FundingActor.Constants;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor.Tasks;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using ESFA.DC.ILR.FundingService.Providers.LearnerPaging;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class StatelessActorModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var actorNameParameter = "actorName";

            containerBuilder.RegisterType<ALBActorTask>().Keyed<IActorTask>(FundingTaskConstants.ALB).WithParameter(actorNameParameter, ActorServiceNameConstants.ALB).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25ActorTask>().Keyed<IActorTask>(FundingTaskConstants.FM25).WithParameter(actorNameParameter, ActorServiceNameConstants.FM25).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35ActorTask>().Keyed<IActorTask>(FundingTaskConstants.FM35).WithParameter(actorNameParameter, ActorServiceNameConstants.FM35).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36ActorTask>().Keyed<IActorTask>(FundingTaskConstants.FM36).WithParameter(actorNameParameter, ActorServiceNameConstants.FM36).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70ActorTask>().Keyed<IActorTask>(FundingTaskConstants.FM70).WithParameter(actorNameParameter, ActorServiceNameConstants.FM70).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM81ActorTask>().Keyed<IActorTask>(FundingTaskConstants.FM81).WithParameter(actorNameParameter, ActorServiceNameConstants.FM81).InstancePerLifetimeScope();

            containerBuilder.RegisterType<ALBActorDtoProvider>().Keyed<IActorDtoProvider>(FundingTaskConstants.ALB).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25ActorDtoProvider>().Keyed<IActorDtoProvider>(FundingTaskConstants.FM25).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35ActorDtoProvider>().Keyed<IActorDtoProvider>(FundingTaskConstants.FM35).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36ActorDtoProvider>().Keyed<IActorDtoProvider>(FundingTaskConstants.FM36).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70ActorDtoProvider>().Keyed<IActorDtoProvider>(FundingTaskConstants.FM70).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM81ActorDtoProvider>().Keyed<IActorDtoProvider>(FundingTaskConstants.FM81).InstancePerLifetimeScope();

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

            containerBuilder.RegisterType<ALBLearnerPagingService>().As<ILearnerPagingService<ALBLearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25LearnerPagingService>().As<ILearnerPagingService<FM25LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35LearnerPagingService>().As<ILearnerPagingService<FM35LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36LearnerPagingService>().As<ILearnerPagingService<FM36LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70LearnerPagingService>().As<ILearnerPagingService<FM70LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM81LearnerPagingService>().As<ILearnerPagingService<FM81LearnerDto>>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ExecutionContext>().As<IExecutionContext>();

            //containerBuilder.RegisterAssemblyTypes(ThisAssembly).Where(s => s.IsAssignableTo<IMapper>).AsImplementedInterfaces
        }
    }
}
