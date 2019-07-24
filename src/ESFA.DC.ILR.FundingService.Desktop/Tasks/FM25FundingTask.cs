using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop.Tasks
{
    public class FM25FundingTask<Tin, TOut, TPeriodsationOut> : IFundingTask
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly IFilePersistanceService _filePersistanceService;
        private readonly IFundingService<Tin, IEnumerable<TOut>> _fundingServiceFM25;
        private readonly IFundingService<TOut, IEnumerable<TPeriodsationOut>> _fundingServiceFM25Periodisation;
        private readonly IFM25FundingOutputCondenserService<TOut, TPeriodsationOut> _fundingOutputCondenserService;
        private readonly string _taskName;
        private readonly string _outputKey;

        public FM25FundingTask(
            IJsonSerializationService jsonSerializationService,
            IFilePersistanceService filePersistanceService,
            IFundingService<Tin, IEnumerable<TOut>> fundingServiceFM25,
            IFundingService<TOut, IEnumerable<TPeriodsationOut>> fundingServiceFM25Periodisation,
            IFM25FundingOutputCondenserService<TOut, TPeriodsationOut> fundingOutputCondenserService,
            ILogger logger,
            string taskName,
            string outputKey)
        {
            _jsonSerializationService = jsonSerializationService;
            _filePersistanceService = filePersistanceService;
            _fundingServiceFM25 = fundingServiceFM25;
            _fundingServiceFM25Periodisation = fundingServiceFM25Periodisation;
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

            List<Task<TOut>> taskList = new List<Task<TOut>>();

            foreach (FundingDto fundingDto in fundingDtos)
            {
                taskList.Add(Process(fundingDto, cancellationToken));
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);

            IEnumerable<TOut> results = taskList.Select(t => t.Result) ?? Enumerable.Empty<TOut>();
            var output = _fundingOutputCondenserService.Condense(results, fundingServiceContext.Ukprn, fundingServiceContext.Year);

            fundingServiceContext.FundingOutputKeys.TryGetValue(_outputKey, out var outputFileName);

            await _filePersistanceService.PersistAsync(outputFileName, fundingServiceContext.Container, output, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Persisted {_taskName} results - {stopWatch.ElapsedMilliseconds}");
        }

        private async Task<TOut> Process(FundingDto dto, CancellationToken cancellationToken)
        {
            return RunFunding(dto, cancellationToken);
        }

        private TOut RunFunding(FundingDto fundingDto, CancellationToken cancellationToken)
        {
            IEnumerable<TOut> fm25Results;
            IEnumerable<TPeriodsationOut> fm25PeriodisationResults;

            try
            {
                var learners = BuildLearners<Tin>(fundingDto.ValidLearners);

                fm25Results = _fundingServiceFM25.ProcessFunding(fundingDto.UKPRN, learners, cancellationToken).ToList();
                fm25PeriodisationResults = _fundingServiceFM25Periodisation.ProcessFunding(fundingDto.UKPRN, fm25Results, cancellationToken).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing {_taskName} Task", ex);
                throw;
            }

            return _fundingOutputCondenserService.CondensePeriodisationResults(fm25Results, fm25PeriodisationResults);
        }

        private List<T> BuildLearners<T>(string serializedLearners)
        {
            return _jsonSerializationService.Deserialize<List<T>>(serializedLearners);
        }
    }
}
