using System;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor;
using ESFA.DC.ILR.FundingService.FundingActor.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.FM35Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM35)]
    public class FM35Actor : AbstractFundingActor, IFM35Actor
    {
        public FM35Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId, lifetimeScope, executionContext, jsonSerializationService)
        {
        }

        public async Task<string> Process(FundingDto actorModel, CancellationToken cancellationToken)
        {
            FM35Global results = RunFunding(actorModel, cancellationToken);
            actorModel = null;

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            return BuildFundingOutput(results);
        }

        private FM35Global RunFunding(FundingDto actorModel, CancellationToken cancellationToken)
        {
            if (ExecutionContext is ExecutionContext executionContextObj)
            {
                executionContextObj.JobId = "-1";
                executionContextObj.TaskKey = ActorId.ToString();
            }

            ILogger logger = LifetimeScope.Resolve<ILogger>();

            IExternalDataCache externalDataCache;
            FM35Global results;

            try
            {
                logger.LogDebug($"{nameof(FM35Actor)} {ActorId} {GC.GetGeneration(actorModel)} starting");

                externalDataCache = BuildExternalDataCache(actorModel.ExternalDataCache);

                logger.LogDebug($"{nameof(FM35Actor)} {ActorId} {GC.GetGeneration(actorModel)} finished getting input data");

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                logger.LogError($"Error while processing {nameof(FM35Actor)}", ex);
                throw;
            }

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(externalDataCache).As<IExternalDataCache>();
            }))
            {
                ExecutionContext executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = actorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();
                ILogger jobLogger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    jobLogger.LogDebug($"{nameof(FM35Actor)} {ActorId} {GC.GetGeneration(actorModel)} started processing");
                    IFundingService<FM35LearnerDto, FM35Global> fundingService = childLifetimeScope.Resolve<IFundingService<FM35LearnerDto, FM35Global>>();

                    var learners = BuildLearners<FM35LearnerDto>(actorModel.ValidLearners);

                    results = fundingService.ProcessFunding(actorModel.UKPRN, learners, cancellationToken);
                    jobLogger.LogDebug($"{nameof(FM35Actor)} {ActorId} {GC.GetGeneration(actorModel)} completed processing");
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    jobLogger.LogError($"Error while processing {nameof(FM35Actor)}", ex);
                    throw;
                }
            }

            externalDataCache = null;

            return results;
        }
    }
}
