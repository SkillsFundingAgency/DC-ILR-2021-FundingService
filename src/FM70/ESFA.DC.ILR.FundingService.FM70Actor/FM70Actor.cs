﻿using System;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor;
using ESFA.DC.ILR.FundingService.FundingActor.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.FM70Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM70)]
    public class FM70Actor : AbstractFundingActor, IFM70Actor
    {
        public FM70Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId, lifetimeScope, executionContext, jsonSerializationService)
        {
        }

        public async Task<string> Process(FundingActorDto actorModel, CancellationToken cancellationToken)
        {
            FM70Global results = RunFunding(actorModel, cancellationToken);
            actorModel = null;

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            return BuildFundingOutput(results);
        }

        private FM70Global RunFunding(FundingActorDto actorModel, CancellationToken cancellationToken)
        {
            if (ExecutionContext is ExecutionContext executionContextObj)
            {
                executionContextObj.JobId = "-1";
                executionContextObj.TaskKey = ActorId.ToString();
            }

            ILogger logger = LifetimeScope.Resolve<ILogger>();

            IExternalDataCache externalDataCache;
            FM70Global results;

            try
            {
                logger.LogDebug($"{nameof(FM70Actor)} {ActorId} {GC.GetGeneration(actorModel)} starting");

                externalDataCache = BuildExternalDataCache(actorModel.ExternalDataCache);

                logger.LogDebug($"{nameof(FM70Actor)} {ActorId} {GC.GetGeneration(actorModel)} finished getting input data");

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                logger.LogError($"Error while processing {nameof(FM70Actor)}", ex);
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
                        $"{nameof(FM70Actor)} {ActorId} {GC.GetGeneration(actorModel)} started processing");

                    IFundingService<ILearner, FM70Global> fundingService =
                        childLifetimeScope.Resolve<IFundingService<ILearner, FM70Global>>();

                    var learners = BuildLearners(actorModel.ValidLearners);

                    if (learners == null)
                    {
                        results = null;

                        jobLogger.LogDebug($"{nameof(FM70Actor)} {ActorId} {GC.GetGeneration(actorModel)} completed processing - Zero learners");
                    }
                    else
                    {
                        results = fundingService.ProcessFunding(learners, cancellationToken);
                        jobLogger.LogDebug($"{nameof(FM70Actor)} {ActorId} {GC.GetGeneration(actorModel)} completed processing");
                    }
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    jobLogger.LogError($"Error while processing {nameof(FM70Actor)}", ex);
                    throw;
                }
            }

            externalDataCache = null;

            return results;
        }
    }
}
