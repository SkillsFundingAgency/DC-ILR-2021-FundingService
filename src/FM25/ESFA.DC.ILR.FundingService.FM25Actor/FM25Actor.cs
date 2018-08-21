using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.FM25Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM25)]
    public class FM25Actor : AbstractFundingActor, IFM25Actor
    {
        public FM25Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope)
            : base(actorService, actorId, lifetimeScope)
        {
        }

        public Task<string> Process(FundingActorDto fm25ActorModel)
        {
            string resultString;

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.Register(a => a.Resolve<IJsonSerializationService>().Deserialize<ExternalDataCache>(fm25ActorModel.ExternalDataCache)).As<IExternalDataCache>();
                c.Register(a => a.Resolve<IJsonSerializationService>().Deserialize<FileDataCache>(fm25ActorModel.FileDataCache)).As<IFileDataCache>();
            }))
            {
                var jsonSerializationService = childLifetimeScope.Resolve<IJsonSerializationService>();

                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();

                executionContext.JobId = fm25ActorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();

                var logger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    logger.LogDebug("FM25 Actor started processing");

                    IEnumerable<Global> fm25Results;
                    IEnumerable<PeriodisationGlobal> fm25PeriodisationResults;

                    using (var fundingServiceLifetimeScope = childLifetimeScope.BeginLifetimeScope(c =>
                    {
                        c.RegisterInstance(new RulebaseProvider("FM25")).As<IRulebaseProvider>();
                    }))
                    {
                        logger.LogDebug("FM25 Rulebase Starting");

                        var fundingService = fundingServiceLifetimeScope.Resolve<IFundingService<ILearner, IEnumerable<Global>>>();

                        var learners = jsonSerializationService.Deserialize<List<MessageLearner>>(fm25ActorModel.ValidLearners);

                        fm25ActorModel = null;

                        fm25Results = fundingService.ProcessFunding(learners).ToList();

                        logger.LogDebug("FM25 Rulebase Finishing");
                    }

                    using (var fundingServiceLifetimeScope = childLifetimeScope.BeginLifetimeScope(c =>
                    {
                        c.RegisterInstance(new RulebaseProvider("FM25 Periodisation")).As<IRulebaseProvider>();
                    }))
                    {
                        logger.LogDebug("FM25 Periodisation Rulebase Starting");

                        var periodisationService = fundingServiceLifetimeScope.Resolve<IFundingService<Global, IEnumerable<PeriodisationGlobal>>>();

                        fm25PeriodisationResults = periodisationService.ProcessFunding(fm25Results).ToList();

                        logger.LogDebug("FM25 Periodisation Rulebase Finishing");
                    }

                    logger.LogDebug("FM25 Actor completed processing");

                    var condensedResults = CondenseResults(fm25Results, fm25PeriodisationResults);

                    resultString = jsonSerializationService.Serialize(condensedResults);
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    logger.LogError("Error while processing Actor job", ex);
                    throw;
                }
            }

            return Task.FromResult(resultString);
        }

        private Global CondenseResults(IEnumerable<Global> globals, IEnumerable<PeriodisationGlobal> periodisationGlobals)
        {
            var first = globals.FirstOrDefault();

            if (first != null)
            {
                first.Learners = globals.SelectMany(g => g.Learners).ToList();

                return first;
            }

            return new Global();
        }
    }
}