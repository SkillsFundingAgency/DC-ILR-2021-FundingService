using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IPreFundingSFOrchestrationService
    {
        Task ExecuteAsync(IJobContextMessage jobContextMessage, CancellationToken cancellationToken);
    }
}
