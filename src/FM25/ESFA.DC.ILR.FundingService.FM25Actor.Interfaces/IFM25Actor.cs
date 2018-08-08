using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace ESFA.DC.ILR.FundingService.FM25Actor.Interfaces
{
    public interface IFM25Actor : IFundingActor
    {
    }
}
