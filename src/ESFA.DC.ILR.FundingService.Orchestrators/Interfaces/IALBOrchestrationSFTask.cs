using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IALBOrchestrationSFTask
    {
        Task Execute(IEnumerable<FundingActorDto> fundingActorDtos, string outputKey);
    }
}