using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFundingTask
    {
        Task Execute(IEnumerable<FundingDto> fundingActorDtos, IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken);
    }
}
