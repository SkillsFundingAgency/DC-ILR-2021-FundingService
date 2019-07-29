using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop.Tasks
{
    public class FM70FundingTask<Tin, TOut> : IFundingTask
    {
        private readonly ILogger _logger;
        private readonly IFilePersistanceService _filePersistanceService;
        private readonly IFundingOutputCondenserService<TOut> _fundingOutputCondenserService;
        private readonly string _taskName;
        private readonly string _outputKey;

        public FM70FundingTask(
            IFilePersistanceService filePersistanceService,
            IFundingOutputCondenserService<TOut> fundingOutputCondenserService,
            ILogger logger,
            string taskName,
            string outputKey)
        {
            _filePersistanceService = filePersistanceService;
            _fundingOutputCondenserService = fundingOutputCondenserService;
            _logger = logger;
            _taskName = taskName;
            _outputKey = outputKey;
        }

        public async Task Execute(IEnumerable<FundingDto> fundingDtos, IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting {_taskName} Task");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var output = _fundingOutputCondenserService.Condense(Enumerable.Empty<TOut>(), fundingServiceContext.Ukprn, fundingServiceContext.Year);

            fundingServiceContext.FundingOutputKeys.TryGetValue(_outputKey, out var outputFileName);

            await _filePersistanceService.PersistAsync(outputFileName, fundingServiceContext.Container, output, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Persisted {_taskName} results - {stopWatch.ElapsedMilliseconds}");
        }
    }
}
