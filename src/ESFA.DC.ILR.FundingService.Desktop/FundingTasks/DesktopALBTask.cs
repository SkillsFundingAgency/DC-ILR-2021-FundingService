using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop.FundingTasks
{
    public class DesktopALBTask : AbstractDesktopFundingTask, IFundingTask
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly IFilePersistanceService _filePersistanceService;
        private readonly IFundingService<ALBLearnerDto, ALBGlobal> _fundingService;
        private readonly IFundingOutputCondenserService<ALBGlobal> _fundingOutputCondenserService;

        public DesktopALBTask(
            IJsonSerializationService jsonSerializationService,
            IFilePersistanceService filePersistanceService,
            IFundingService<ALBLearnerDto, ALBGlobal> fundingService,
            IFundingOutputCondenserService<ALBGlobal> fundingOutputCondenserService,
            ILogger logger)
            : base(jsonSerializationService)
        {
            _jsonSerializationService = jsonSerializationService;
            _filePersistanceService = filePersistanceService;
            _fundingService = fundingService;
            _fundingOutputCondenserService = fundingOutputCondenserService;
            _logger = logger;
        }

        public async Task Execute(IEnumerable<FundingDto> fundingDtos, IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting ALB Task");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Task<ALBGlobal>> taskList = new List<Task<ALBGlobal>>();

            foreach (FundingDto fundingDto in fundingDtos)
            {
                taskList.Add(Process(fundingDto, cancellationToken));
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);

            IEnumerable<ALBGlobal> results = taskList.Select(t => t.Result);
            var output = _fundingOutputCondenserService.Condense(results);

            await _filePersistanceService.PersistAsync(fundingServiceContext.FundingALBOutputKey, fundingServiceContext.Container, output, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Persisted ALB results - {stopWatch.ElapsedMilliseconds}");
        }

        private async Task<ALBGlobal> Process(FundingDto dto, CancellationToken cancellationToken)
        {
            return RunFunding(dto, cancellationToken);
        }

        private ALBGlobal RunFunding(FundingDto fundingDto, CancellationToken cancellationToken)
        {
            ALBGlobal results;

            try
            {
                var learners = BuildLearners<ALBLearnerDto>(fundingDto.ValidLearners);

                results = _fundingService.ProcessFunding(fundingDto.UKPRN, learners, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing ALB Task", ex);
                throw;
            }

            return results;
        }
    }
}
