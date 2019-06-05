using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.FundingActor.Interfaces
{
    public interface IActorTask
    {
        Task Execute(IEnumerable<FundingActorDto> fundingActorDtos, IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken);
    }
}
