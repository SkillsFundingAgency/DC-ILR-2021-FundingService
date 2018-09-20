using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Stateless.Models;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IActorTask<TActor, TActorReturn>
    {
        Task Execute(IEnumerable<FundingActorDto> fundingActorDtos, string outputKey, CancellationToken cancellationToken);
    }
}
