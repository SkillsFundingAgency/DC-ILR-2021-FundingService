using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop.FundingTasks
{
    public class DesktopALBActorTask : IFundingTask
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly IFilePersistanceService _filePersistanceService;
        private readonly IFundingService<ALBLearnerDto, ALBGlobal> _fundingService;
        private readonly IFundingOutputCondenserService<ALBGlobal> _fundingOutputCondenserService;

        public DesktopALBActorTask(
            IJsonSerializationService jsonSerializationService,
            IFilePersistanceService filePersistanceService,
            IFundingService<ALBLearnerDto, ALBGlobal> fundingService,
            IFundingOutputCondenserService<ALBGlobal> fundingOutputCondenserService,
            ILogger logger)
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

            ExternalDataCache externalDataCache;

            try
            {
                var deserialzedCache = _jsonSerializationService.Deserialize<ExternalDataCache>(fundingDtos.FirstOrDefault().ExternalDataCache);

                externalDataCache = new ExternalDataCache
                {
                    AECLatestInYearEarningHistory = deserialzedCache.AECLatestInYearEarningHistory,
                    FCSContractAllocations = deserialzedCache.FCSContractAllocations,
                    LargeEmployers = deserialzedCache.LargeEmployers,
                    LARSCurrentVersion = deserialzedCache.LARSCurrentVersion,
                    LARSLearningDelivery = deserialzedCache.LARSLearningDelivery.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase),
                    LARSStandards = deserialzedCache.LARSStandards,
                    OrgFunding = deserialzedCache.OrgFunding,
                    OrgVersion = deserialzedCache.OrgVersion,
                    PostcodeCurrentVersion = deserialzedCache.PostcodeCurrentVersion,
                    PostcodeRoots = deserialzedCache.PostcodeRoots.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase),
                    Periods = deserialzedCache.Periods,
                };

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing ALB Task", ex);
                throw;
            }

            List<Task<ALBGlobal>> taskList = new List<Task<ALBGlobal>>();

            foreach (FundingDto fundingDto in fundingDtos)
            {
                taskList.Add(Process(fundingDto, externalDataCache, cancellationToken));
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);

            IEnumerable<ALBGlobal> results = taskList.Select(t => t.Result);
            var output = _fundingOutputCondenserService.Condense(results);

            await _filePersistanceService.PersistAsync(fundingServiceContext.FundingALBOutputKey, fundingServiceContext.Container, output, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Persisted ALB results - {stopWatch.ElapsedMilliseconds}");
        }

        private async Task<ALBGlobal> Process(FundingDto dto, ExternalDataCache externalDataCache, CancellationToken cancellationToken)
        {
            return RunFunding(dto, externalDataCache, cancellationToken);
        }

        private ALBGlobal RunFunding(FundingDto fundingDto, ExternalDataCache externalDataCache, CancellationToken cancellationToken)
        {
            ALBGlobal results;

            try
            {
                var learners = _jsonSerializationService.Deserialize<List<ALBLearnerDto>>(fundingDto.ValidLearners);

                results = _fundingService.ProcessFunding(fundingDto.UKPRN, learners, cancellationToken);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Error while processing ALB Task", ex);
               throw;
            }

            externalDataCache = null;

            return results;
        }
    }
}
