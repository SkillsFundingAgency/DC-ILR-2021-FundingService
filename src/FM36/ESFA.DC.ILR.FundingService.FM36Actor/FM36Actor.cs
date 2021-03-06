﻿using System;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor;
using ESFA.DC.ILR.FundingService.FundingActor.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Service.Interfaces;

namespace ESFA.DC.ILR.FundingService.FM36Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM36)]
    public class FM36Actor : AbstractFundingActor, IFM36Actor
    {
        public FM36Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId, lifetimeScope, executionContext, jsonSerializationService)
        {
        }

        public async Task<string> Process(FundingDto actorModel, CancellationToken cancellationToken)
        {
            FM36Global results = RunFunding(actorModel, cancellationToken);
            actorModel = null;

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            return BuildFundingOutput(results);
        }

        private FM36Global RunFunding(FundingDto actorModel, CancellationToken cancellationToken)
        {
            if (ExecutionContext is ExecutionContext executionContextObj)
            {
                executionContextObj.JobId = "-1";
                executionContextObj.TaskKey = ActorId.ToString();
            }

            ILogger logger = LifetimeScope.Resolve<ILogger>();

            IExternalDataCache externalDataCache;
            FM36Global results;

            try
            {
                logger.LogDebug($"{nameof(FM36Actor)} {ActorId} {GC.GetGeneration(actorModel)} starting");

                externalDataCache = BuildExternalDataCache(actorModel.ExternalDataCache);

                logger.LogDebug($"{nameof(FM36Actor)} {ActorId} {GC.GetGeneration(actorModel)} finished getting input data");

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                logger.LogError($"Error while processing {nameof(FM36Actor)}", ex);
                throw;
            }

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(externalDataCache).As<IExternalDataCache>();
            }))
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = actorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();
                var jobLogger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    jobLogger.LogDebug(
                        $"{nameof(FM36Actor)} {ActorId} {GC.GetGeneration(actorModel)} started processing");

                    IFundingService<FM36LearnerDto, FM36Global> fundingService =
                        childLifetimeScope.Resolve<IFundingService<FM36LearnerDto, FM36Global>>();

                    var learners = BuildLearners<FM36LearnerDto>(actorModel.ValidLearners);

                    results = fundingService.ProcessFunding(actorModel.UKPRN, learners, cancellationToken);
                    jobLogger.LogDebug($"{nameof(FM36Actor)} {ActorId} {GC.GetGeneration(actorModel)} completed processing");
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    jobLogger.LogError($"Error while processing {nameof(FM36Actor)}", ex);
                    throw;
                }
            }

            externalDataCache = null;

            return results;
        }
    }
}
