using System.Fabric;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common
{
    public class ActorProvider<T> : IActorProvider<T>
        where T : IActor
    {
        private readonly string _actorServiceName;

        public ActorProvider(string actorServiceName)
        {
            _actorServiceName = actorServiceName;
        }

        public T Provide()
        {
            return ActorProxy.Create<T>(ActorId.CreateRandom(), FabricRuntime.GetActivationContext().ApplicationName, _actorServiceName);
        }
    }
}
