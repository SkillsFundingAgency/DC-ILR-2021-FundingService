using Microsoft.ServiceFabric.Actors;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces
{
    public interface IActorProvider<out T>
        where T : IActor
    {
        T Provide();
    }
}
