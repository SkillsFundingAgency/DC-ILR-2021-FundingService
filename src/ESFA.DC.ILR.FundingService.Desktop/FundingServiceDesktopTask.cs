using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.FundingService.Desktop.Context;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop
{
    public class FundingServiceDesktopTask
    {
        private readonly IFundingOrchestrationService _orchestrator;

        public FundingServiceDesktopTask(IFundingOrchestrationService orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task ExecuteAsync(IDesktopContext desktopContext, CancellationToken cancellationToken)
        {
            // Create context
            var fundingServiceContext = new FundingServiceContext(desktopContext);

            // call Orchestrator
            await _orchestrator.ExecuteAsync(fundingServiceContext, cancellationToken);
        }
    }
}
