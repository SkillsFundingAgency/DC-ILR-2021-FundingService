using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.FM25Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM25)]
    internal class FM25Actor : Actor, IFM25Actor
    {
        public FM25Actor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public Task<string> Process(FundingActorDto fm25ActorModel)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
