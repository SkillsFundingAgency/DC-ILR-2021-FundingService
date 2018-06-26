using System.IO;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.ALBActor
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;
    using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
    using ESFA.DC.ILR.FundingService.Stateless.Models;
    using ESFA.DC.Logging;
    using ESFA.DC.Logging.Interfaces;
    using ESFA.DC.Serialization.Interfaces;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Actors.Runtime;

    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.None)]
    public class ALBActor : Actor, IALBActor
    {
        private readonly ILifetimeScope _parentLifetimeScope;
        private ActorId _actorId;

        /// <summary>
        /// Initializes a new instance of ALBActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public ALBActor(ActorService actorService, ActorId actorId, ILifetimeScope parentLifetimeScope)
            : base(actorService, actorId)
        {
            _parentLifetimeScope = parentLifetimeScope;
            _actorId = actorId;
        }

        public Task<string> Process(ALBActorModel albActorModel)
        {
            var jsonSerializationService = _parentLifetimeScope.Resolve<ISerializationService>();
            var referenceDataCache = jsonSerializationService.Deserialize<ExternalDataCache>(
                Encoding.UTF8.GetString(albActorModel.ReferenceDataCache));

            using (var childLifetimeScope = _parentLifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(referenceDataCache).As<IExternalDataCache>();
            }))
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = albActorModel.JobId.ToString();
                executionContext.TaskKey = _actorId.ToString();
                var logger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    logger.LogDebug("ALB Actor started processing");
                    var fundingService = childLifetimeScope.Resolve<IFundingService<IFundingOutputs>>();
                    IList<ILearner> validLearners = jsonSerializationService.Deserialize<List<MessageLearner>>(
                        new MemoryStream(albActorModel.AlbValidLearners)).ToArray();

                    var results = fundingService.ProcessFunding(albActorModel.Ukprn, validLearners);

                    logger.LogDebug("ALB Actor completed processing");
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
