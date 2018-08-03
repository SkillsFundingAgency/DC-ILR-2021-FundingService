using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.FM25Actor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class FM25Actor : Actor, IFM25Actor
    {
        public FM25Actor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return this.StateManager.GetStateAsync<int>("count", cancellationToken);
        }

        public Task SetCountAsync(int count, CancellationToken cancellationToken)
        {
            // Requests are not guaranteed to be processed in order nor at most once.
            // The update function here verifies that the incoming count is greater than the current count to preserve order.
            return this.StateManager.AddOrUpdateStateAsync("count", count, (key, value) => count > value ? count : value, cancellationToken);
        }

        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            return this.StateManager.TryAddStateAsync("count", 0);
        }
    }
}
