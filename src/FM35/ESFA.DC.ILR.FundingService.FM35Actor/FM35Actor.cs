﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.Model;
using ESFA.DC.Logging;

namespace ESFA.DC.ILR.FundingService.FM35Actor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.None)]
    internal class FM35Actor : AbstractFundingActor, IFM35Actor
    {
        public FM35Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope)
            : base(actorService, actorId, lifetimeScope)
        {
        }

        public Task<string> Process(FM35ActorModel albActorModel)
        {
            var jsonSerializationService = LifetimeScope.Resolve<ISerializationService>();
            var referenceDataCache = jsonSerializationService.Deserialize<ExternalDataCache>(
                Encoding.UTF8.GetString(albActorModel.ReferenceDataCache));

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(referenceDataCache).As<IExternalDataCache>();
            }))
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = albActorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();
                var logger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    logger.LogDebug("FM35 Actor started processing");
                    var fundingService = childLifetimeScope.Resolve<IFundingService<ILearner, FM35FundingOutputs>>();
                    IList<ILearner> validLearners = jsonSerializationService.Deserialize<List<MessageLearner>>(
                        new MemoryStream(albActorModel.ValidLearners)).ToArray();

                    var results = fundingService.ProcessFunding(validLearners);

                    logger.LogDebug("FM35 Actor completed processing");
                    return Task.FromResult(jsonSerializationService.Serialize(results));
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    logger.LogError("Error while processing Actor job", ex);
                    throw;
                }
            }
        }
    }
}
