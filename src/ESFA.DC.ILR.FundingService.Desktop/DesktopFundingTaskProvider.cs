using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop
{
    public class DesktopFundingTaskProvider : IFundingTaskProvider
    {
        private readonly ILogger _logger;
        private readonly IIndex<string, IFundingTask> _taskIndex;
        private readonly IIndex<string, IFundingDtoProvider> _taskProviderIndex;

        public DesktopFundingTaskProvider(
            ILogger logger,
            IIndex<string, IFundingTask> taskIndex,
            IIndex<string, IFundingDtoProvider> taskProviderIndex)
        {
            _logger = logger;
            _taskIndex = taskIndex;
            _taskProviderIndex = taskProviderIndex;
        }

        public async Task ProvideAsync(IFundingServiceContext fundingServiceContext, IMessage message, string externalDataCache, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var stopWatchSteps = new Stopwatch();
            stopWatchSteps.Start();

            List<Task> fundingTasks = new List<Task>();

            //foreach (var taskName in fundingServiceContext.TaskKeys.ToList())
            //{
            //    fundingTasks.Add(_taskIndex[taskName].Execute(
            //        _taskProviderIndex[taskName].Provide(fundingServiceContext, message, externalDataCache, cancellationToken), fundingServiceContext, cancellationToken));
            //}

            fundingTasks.Add(_taskIndex["ALB"].Execute(
                    _taskProviderIndex["ALB"].Provide(fundingServiceContext, message, externalDataCache, cancellationToken), fundingServiceContext, cancellationToken));

            await Task.WhenAll(fundingTasks).ConfigureAwait(false);

            _logger.LogDebug($"Completed Funding Service for given rule bases in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}
