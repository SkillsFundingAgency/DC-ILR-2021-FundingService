using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using Microsoft.ServiceFabric.Actors;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces
{
    public interface IFundingActor : IActor
    {
        Task<string> Process(FundingActorDto fm35ActorModel);
    }
}
