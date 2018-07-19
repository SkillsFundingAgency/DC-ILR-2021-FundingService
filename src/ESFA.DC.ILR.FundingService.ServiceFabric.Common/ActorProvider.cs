using System;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common
{
    public class ActorProvider<T> : IActorProvider<T>
        where T : IActor
    {
        private readonly Uri _actorUri;

        public ActorProvider(Uri actorUri)
        {
            _actorUri = actorUri;
        }

        public T Provide()
        {
            return ActorProxy.Create<T>(ActorId.CreateRandom(), _actorUri);
        }
    }
}
