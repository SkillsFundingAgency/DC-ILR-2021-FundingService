namespace ESFA.DC.ILR.FundingService.ALBActor
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
    using ESFA.DC.OPA.Model.Interface;
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
        /// <summary>
        /// Initializes a new instance of ALBActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public ALBActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public IEnumerable<IDataEntity> Process()
        {
            throw new NotImplementedException();
        }
    }
}
