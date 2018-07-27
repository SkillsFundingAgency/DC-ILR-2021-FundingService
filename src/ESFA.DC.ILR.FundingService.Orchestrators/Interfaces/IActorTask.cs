using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Stateless.Models;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IActorTask
    {
        Task Execute(IEnumerable<FundingActorDto> fundingActorDtos, string outputKey);
    }
}
