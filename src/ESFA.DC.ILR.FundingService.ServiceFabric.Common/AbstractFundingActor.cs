using Autofac;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common
{
    public abstract class AbstractFundingActor : Actor
    {
        protected AbstractFundingActor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId)
        {
            LifetimeScope = lifetimeScope;
            ExecutionContext = executionContext;
            JsonSerializationService = jsonSerializationService;
            ActorId = actorId;
        }

        protected ActorId ActorId { get; }

        protected ILifetimeScope LifetimeScope { get; }

        public IExecutionContext ExecutionContext { get; }

        public IJsonSerializationService JsonSerializationService { get; }
    }
}
