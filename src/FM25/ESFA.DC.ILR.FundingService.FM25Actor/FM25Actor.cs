using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Rulebase;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor;
using ESFA.DC.ILR.FundingService.FundingActor.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.FM25Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM25)]
    public class FM25Actor : AbstractFundingActor, IFM25Actor
    {
        public FM25Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId, lifetimeScope, executionContext, jsonSerializationService)
        {
        }

        public async Task<string> Process(FundingDto actorModel, CancellationToken cancellationToken)
        {
            FM25Global results = RunFunding(actorModel, cancellationToken);
            actorModel = null;

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            return BuildFundingOutput(results);
        }

        private FM25Global RunFunding(FundingDto actorModel, CancellationToken cancellationToken)
        {
            if (ExecutionContext is ExecutionContext executionContextObj)
            {
                executionContextObj.JobId = "-1";
                executionContextObj.TaskKey = ActorId.ToString();
            }

            ILogger logger = LifetimeScope.Resolve<ILogger>();

            IExternalDataCache externalDataCache;
            FM25Global condensedResults;

            try
            {
                logger.LogDebug($"{nameof(FM25Actor)} {ActorId} starting");

                externalDataCache = BuildExternalDataCache(actorModel.ExternalDataCache);

                logger.LogDebug($"{nameof(FM25Actor)} {ActorId} finished getting input data");

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                logger.LogError($"Error while processing {nameof(FM25Actor)}", ex);
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
                    jobLogger.LogDebug($"{nameof(FM25Actor)} {ActorId} {GC.GetGeneration(actorModel)} started processing");

                    IEnumerable<FM25Global> fm25Results;
                    IEnumerable<PeriodisationGlobal> fm25PeriodisationResults;

                    using (var fundingServiceLifetimeScope = childLifetimeScope.BeginLifetimeScope(c =>
                    {
                        c.RegisterInstance(new FM25RulebaseProvider()).As<IRulebaseStreamProvider<FM25LearnerDto>>();
                    }))
                    {
                        jobLogger.LogDebug("FM25 Rulebase Starting");

                        IFundingService<FM25LearnerDto, IEnumerable<FM25Global>> fundingService = fundingServiceLifetimeScope.Resolve<IFundingService<FM25LearnerDto, IEnumerable<FM25Global>>>();

                        var learners = BuildLearners<FM25LearnerDto>(actorModel.ValidLearners);

                        fm25Results = fundingService.ProcessFunding(actorModel.UKPRN, learners, cancellationToken).ToList();

                        jobLogger.LogDebug("FM25 Rulebase Finishing");
                    }

                    using (var fundingServiceLifetimeScope = childLifetimeScope.BeginLifetimeScope(c =>
                    {
                        c.RegisterInstance(new FM25PeriodisationRulebaseProvider()).As<IRulebaseStreamProvider<FM25Global>>();
                    }))
                    {
                        jobLogger.LogDebug("FM25 Periodisation Rulebase Starting");

                        IFundingService<FM25Global, IEnumerable<PeriodisationGlobal>> periodisationService = fundingServiceLifetimeScope.Resolve<IFundingService<FM25Global, IEnumerable<PeriodisationGlobal>>>();

                        fm25PeriodisationResults = periodisationService.ProcessFunding(actorModel.UKPRN, fm25Results, cancellationToken).ToList();

                        jobLogger.LogDebug("FM25 Periodisation Rulebase Finishing");

                        IFM25FundingOutputCondenserService<FM25Global, PeriodisationGlobal> condenser = fundingServiceLifetimeScope.Resolve<IFM25FundingOutputCondenserService<FM25Global, PeriodisationGlobal>>();

                        condensedResults = condenser.CondensePeriodisationResults(fm25Results, fm25PeriodisationResults);
                    }

                    jobLogger.LogDebug($"{nameof(FM25Actor)} {ActorId} {GC.GetGeneration(actorModel)} completed processing");
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    jobLogger.LogError($"Error while processing {nameof(FM25Actor)}", ex);
                    throw;
                }
            }

            externalDataCache = null;

            return condensedResults;
        }
    }
}