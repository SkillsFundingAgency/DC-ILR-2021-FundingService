using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.Data.LargeEmployer.Model;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Config.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Context;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population;
using ESFA.DC.ILR.FundingService.Data.Population.Context;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.File;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Internal;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Implementations;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.FundingService.Stubs.Database;
using ESFA.DC.ILR.FundingService.Stubs.Database.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.Stateless.Modules
{
    public class StatelessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new LARS(referenceDataConfig.LARSConnectionString);
            }).As<ILARS>().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new Postcodes(referenceDataConfig.PostcodesConnectionString);
            }).As<IPostcodes>().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new Organisations(referenceDataConfig.OrganisationConnectionString);
            }).As<IOrganisations>().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var referenceDataConfig = c.Resolve<IReferenceDataConfig>();
                return new LargeEmployer(referenceDataConfig.LargeEmployersConnectionString);
            }).As<ILargeEmployer>().InstancePerLifetimeScope();

            builder.RegisterType<PopulationService>().As<IPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<FileDataCachePopulationService>().As<IFileDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<InternalDataCachePopulationService>().As<IInternalDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCachePopulationService>().As<IExternalDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<LearnerPagingService<ILearner>>().As<IPagingService<ILearner>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContext>().As<IFundingContext>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContextPopulationService>().As<IFundingContextPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<FileDataCache>().As<IFileDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<ValidLearnersDataRetrievalService>().As<IValidLearnersDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<FileDataRetrievalService>().As<IFileDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<PostcodesDataRetrievalService>().As<IPostcodesDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<LargeEmployersDataRetrievalService>().As<ILargeEmployersDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<LARSDataRetrievalService>().As<ILARSDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<OrganisationDataRetrievalService>().As<IOrganisationDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<AppsEarningsHistoryDataRetrievalService>().As<IAppsEarningsHistoryDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<AppsEarningsHistoryStub>().As<IAppsEarningsHistoryStub>().InstancePerLifetimeScope();

            builder.RegisterType<JsonSerializationService>().As<ISerializationService>();
            builder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>();
            builder.RegisterType<XmlSerializationService>().As<IXmlSerializationService>();

            builder.RegisterType<DateTimeProvider.DateTimeProvider>().As<IDateTimeProvider>();

            // register the  callback handle when a new message is received from ServiceBus
            builder.Register<Func<JobContextMessage, CancellationToken, Task<bool>>>(c => c.Resolve<IMessageHandler>().Handle).InstancePerLifetimeScope();
            builder.RegisterType<JobContextManagerForTopics<JobContextMessage>>().As<IJobContextManager<JobContextMessage>>().InstancePerLifetimeScope();
            builder.RegisterType<JobContextMessage>().As<IJobContextMessage>().InstancePerLifetimeScope();

            builder.RegisterType<PreFundingSFOrchestrationService>().As<IPreFundingSFOrchestrationService>().InstancePerLifetimeScope();

            builder.RegisterType<FundingServiceDto>().As<IFundingServiceDto>().InstancePerLifetimeScope();

            var actorNameParameter = "actorName";

            builder.RegisterType<ActorTask<IALBActor, ALBFundingOutputs>>().As<IActorTask<IALBActor, ALBFundingOutputs>>().WithParameter(actorNameParameter, ActorServiceNameConstants.ALB).InstancePerLifetimeScope();
            builder.RegisterType<ActorTask<IFM35Actor, FM35FundingOutputs>>().As<IActorTask<IFM35Actor, FM35FundingOutputs>>().WithParameter(actorNameParameter, ActorServiceNameConstants.FM35).InstancePerLifetimeScope();
            builder.RegisterType<ActorTask<IFM25Actor, Global>>().As<IActorTask<IFM25Actor, Global>>().WithParameter(actorNameParameter, ActorServiceNameConstants.FM25).InstancePerLifetimeScope();

            builder.RegisterType<FM35FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM35FundingOutputs>>().InstancePerLifetimeScope();
            builder.RegisterType<ALBFundingOutputCondenserService>().As<IFundingOutputCondenserService<ALBFundingOutputs>>().InstancePerLifetimeScope();
            builder.RegisterType<FM25FundingOutputCondenserService>().As<IFundingOutputCondenserService<Global>>().InstancePerLifetimeScope();

            builder.RegisterInstance(new ActorProvider<IFM35Actor>(ActorServiceNameConstants.FM35)).As<IActorProvider<IFM35Actor>>();
            builder.RegisterInstance(new ActorProvider<IALBActor>(ActorServiceNameConstants.ALB)).As<IActorProvider<IALBActor>>();
            builder.RegisterInstance(new ActorProvider<IFM25Actor>(ActorServiceNameConstants.FM25)).As<IActorProvider<IFM25Actor>>();
        }
    }
}
