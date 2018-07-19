using System;
using System.Fabric;
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
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Implementations;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.RuleBaseTasks;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Output;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.Modules
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
            builder.RegisterType<ExternalDataCachePopulationService>().As<IExternalDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<LearnerPagingService<ILearner>>().As<IPagingService<ILearner>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContext>().As<IFundingContext>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContextPopulationService>().As<IFundingContextPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<FileDataCache>().As<IFileDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<ValidLearnersDataRetrievalService>().As<IValidLearnersDataRetrievalService>().InstancePerLifetimeScope();
            builder.RegisterType<UKPRNDataRetrievalService>().As<IUKPRNDataRetrievalService>().InstancePerLifetimeScope();

            builder.RegisterType<JsonSerializationService>().As<ISerializationService>();
            builder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>();
            builder.RegisterType<XmlSerializationService>().As<IXmlSerializationService>();

            // register the  callback handle when a new message is received from ServiceBus
            builder.Register<Func<JobContextMessage, CancellationToken, Task<bool>>>(c => c.Resolve<IMessageHandler>().Handle).InstancePerLifetimeScope();
            builder.RegisterType<JobContextManagerForTopics<JobContextMessage>>().As<IJobContextManager<JobContextMessage>>().InstancePerLifetimeScope();
            builder.RegisterType<JobContextMessage>().As<IJobContextMessage>().InstancePerLifetimeScope();

            builder.RegisterType<PreFundingSFOrchestrationService>().As<IPreFundingSFOrchestrationService>().InstancePerLifetimeScope();

            builder.RegisterType<FundingServiceDto>().As<IFundingServiceDto>().InstancePerLifetimeScope();

            builder.RegisterType<FundingOutputPersistenceSfService<FundingOutputs>>().As<IFundingOutputPersistenceService<FundingOutputs>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputPersistenceSfService<FM35FundingOutputs>>().As<IFundingOutputPersistenceService<FM35FundingOutputs>>().InstancePerLifetimeScope();

            builder.RegisterType<ALBOrchestrationSFTask>().As<IALBOrchestrationSFTask>().InstancePerLifetimeScope();
            builder.RegisterType<FM35OrchestrationSFTask>().As<IFM35OrchestrationSFTask>().InstancePerLifetimeScope();

            builder.RegisterInstance(new ActorProvider<IFM35Actor>(new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/FM35ActorService"))).As<IActorProvider<IFM35Actor>>();
            builder.RegisterInstance(new ActorProvider<IALBActor>(new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/ALBActorService"))).As<IActorProvider<IALBActor>>();
        }
    }
}
