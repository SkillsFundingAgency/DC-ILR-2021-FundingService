using Autofac;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common
{
    public abstract class AbstractFundingActor : Actor
    {
        protected AbstractFundingActor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope)
            : base(actorService, actorId)
        {
            LifetimeScope = lifetimeScope;
            ActorId = actorId;
        }

        protected ActorId ActorId { get; }

        protected ILifetimeScope LifetimeScope { get; }
    }
}
