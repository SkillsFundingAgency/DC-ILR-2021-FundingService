using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IFundingOrchestrationService
    {
        Task ExecuteAsync(IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken);
    }
}
