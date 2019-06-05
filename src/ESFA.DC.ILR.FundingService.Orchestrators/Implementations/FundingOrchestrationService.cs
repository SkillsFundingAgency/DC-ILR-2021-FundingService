using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.FundingActor.Constants;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class FundingOrchestrationService : IFundingOrchestrationService
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileProviderService<IMessage> _ilrFileProviderService;
        private readonly IFileProviderService<ReferenceDataRoot> _ilrReferenceDataProviderService;
        private readonly IExternalDataCachePopulationService _externalCachePopulationService;
        private readonly IFileDataCachePopulationService _fileCachePopulationService;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly ILearnerPagingService _learnerPagingService;
        private readonly ILogger _logger;
        private readonly IIndex<TaskKeys, IActorTask> _taskIndex;

        public FundingOrchestrationService(
            IJsonSerializationService jsonSerializationService,
            IFileProviderService<IMessage> ilrFileProviderService,
            IFileProviderService<ReferenceDataRoot> ilrReferenceDataProviderService,
            IExternalDataCachePopulationService externalCachePopulationService,
            IFileDataCachePopulationService fileCachePopulationService,
            IKeyValuePersistenceService keyValuePersistenceService,
            ILearnerPagingService learnerPagingService,
            ILogger logger,
            IIndex<TaskKeys, IActorTask> taskIndex)
        {
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _ilrReferenceDataProviderService = ilrReferenceDataProviderService;
            _externalCachePopulationService = externalCachePopulationService;
            _fileCachePopulationService = fileCachePopulationService;
            _keyValuePersistenceService = keyValuePersistenceService;
            _learnerPagingService = learnerPagingService;
            _logger = logger;
            _taskIndex = taskIndex;
        }

        public async Task ExecuteAsync(IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var stopWatchSteps = new Stopwatch();
            stopWatchSteps.Start();

            var message = await _ilrFileProviderService.Provide(fundingServiceContext, cancellationToken).ConfigureAwait(false);
            _logger.LogDebug($"Funding Service got file in: {stopWatchSteps.ElapsedMilliseconds}");
            stopWatchSteps.Restart();

            cancellationToken.ThrowIfCancellationRequested();

            var refereceData = await _ilrReferenceDataProviderService.Provide(fundingServiceContext, cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            stopWatchSteps.Restart();

            cancellationToken.ThrowIfCancellationRequested();

            string externalDataCache = _jsonSerializationService.Serialize(_externalCachePopulationService.PopulateAsync(refereceData, cancellationToken));
            string fileDataCache = _jsonSerializationService.Serialize(_fileCachePopulationService.PopulateAsync(message, cancellationToken));

            _logger.LogDebug($"Funding Service got external data: {stopWatchSteps.ElapsedMilliseconds}");

            List<string> taskNames = fundingServiceContext.TaskKeys.ToList();

            List<Task> fundingTasks = new List<Task>();

            if (taskNames.Contains("FM81"))
            {
                List<FundingActorDto> fundingActorDtos = GetFundingModelPages(new List<int> { 81 }, fundingServiceContext, message.Learners, externalDataCache, fileDataCache);
                _logger.LogDebug($"Funding Service FM81 {fundingActorDtos.Count} pages");
                fundingTasks.Add(_taskIndex[TaskKeys.FM81].Execute(fundingActorDtos, fundingServiceContext, cancellationToken));
            }

            if (taskNames.Contains("FM70"))
            {
                List<FundingActorDto> fundingActorDtos = GetFundingModelPages(new List<int> { 70 }, fundingServiceContext, message.Learners, externalDataCache, fileDataCache);
                _logger.LogDebug($"Funding Service FM70 {fundingActorDtos.Count} pages");
                fundingTasks.Add(_taskIndex[TaskKeys.FM70].Execute(fundingActorDtos, fundingServiceContext, cancellationToken));
            }

            if (taskNames.Contains("FM35"))
            {
                List<FundingActorDto> fundingActorDtos = GetFundingModelPages(new List<int> { 35 }, fundingServiceContext, message.Learners, externalDataCache, fileDataCache);
                _logger.LogDebug($"Funding Service FM35 {fundingActorDtos.Count} pages");
                fundingTasks.Add(_taskIndex[TaskKeys.FM35].Execute(fundingActorDtos, fundingServiceContext, cancellationToken));
            }

            if (taskNames.Contains("FM36"))
            {
                List<FundingActorDto> fundingActorDtos = GetFundingModelPages(new List<int> { 36 }, fundingServiceContext, message.Learners, externalDataCache, fileDataCache);
                _logger.LogDebug($"Funding Service FM36 {fundingActorDtos.Count} pages");
                fundingTasks.Add(_taskIndex[TaskKeys.FM36].Execute(fundingActorDtos, fundingServiceContext, cancellationToken));
            }

            if (taskNames.Contains("FM25"))
            {
                List<FundingActorDto> fundingActorDtos = GetFundingModelPages(new List<int> { 25 }, fundingServiceContext, message.Learners, externalDataCache, fileDataCache);
                _logger.LogDebug($"Funding Service FM25 {fundingActorDtos.Count} pages");
                fundingTasks.Add(_taskIndex[TaskKeys.FM25].Execute(fundingActorDtos, fundingServiceContext, cancellationToken));
            }

            if (taskNames.Contains("ALB"))
            {
                List<FundingActorDto> fundingActorDtos = GetFundingModelPages(new List<int> { 99, 81 }, fundingServiceContext, message.Learners, externalDataCache, fileDataCache);
                _logger.LogDebug($"Funding Service FM99/81 {fundingActorDtos.Count} pages");
                fundingTasks.Add(_taskIndex[TaskKeys.ALB].Execute(fundingActorDtos, fundingServiceContext, cancellationToken));
            }

            await Task.WhenAll(fundingTasks).ConfigureAwait(false);

            _logger.LogDebug($"Completed Funding Service for given rule bases in: {stopWatch.ElapsedMilliseconds}");
        }

        private List<FundingActorDto> GetFundingModelPages(IEnumerable<int> filter, IFundingServiceContext fundingServiceContext, IEnumerable<ILearner> learners, string externalDataCache, string fileDataCache)
        {
            return _learnerPagingService
                .BuildPages(filter, learners)
                .Select(p =>
                    new FundingActorDto
                    {
                        JobId = fundingServiceContext.JobId,
                        ExternalDataCache = externalDataCache,
                        FileDataCache = fileDataCache,
                        ValidLearners = _jsonSerializationService.Serialize(p),
                    }).ToList();
        }
    }
}
