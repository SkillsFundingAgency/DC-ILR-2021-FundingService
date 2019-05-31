using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace ESFA.DC.ILR.FundingService.FundingActor.Interfaces
{
    public interface IFundingActor : IActor
    {
        Task<string> Process(IFundingActorDto actorModel, CancellationToken cancellationToken);
    }
}
