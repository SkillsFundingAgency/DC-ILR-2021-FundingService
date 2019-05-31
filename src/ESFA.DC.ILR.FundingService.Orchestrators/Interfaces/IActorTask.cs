using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.FundingActor;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IActorTask<TActor, TActorReturn>
    {
        Task Execute(IEnumerable<IFundingActorDto> fundingActorDtos, string outputKey, CancellationToken cancellationToken);
    }
}
