using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2, RemotingClientVersion = RemotingClientVersion.V2)]
namespace ESFA.DC.ILR.FundingService.FM25Actor.Interfaces
{
    public interface IFM25Actor : IFundingActor
    {
    }
}
