using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators
{
    public class FundingOrchestrationService : IFundingOrchestrationService
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileProviderService<IMessage> _ilrFileProviderService;
        private readonly IFileProviderService<ReferenceDataRoot> _ilrReferenceDataProviderService;
        private readonly IExternalDataCachePopulationService _externalCachePopulationService;
        private readonly IFileDataCachePopulationService _fileCachePopulationService;
        private readonly ILogger _logger;
        private readonly IFundingTaskProvider _fundingTaskProvider;

        public FundingOrchestrationService(
            IJsonSerializationService jsonSerializationService,
            IFileProviderService<IMessage> ilrFileProviderService,
            IFileProviderService<ReferenceDataRoot> ilrReferenceDataProviderService,
            IExternalDataCachePopulationService externalCachePopulationService,
            IFileDataCachePopulationService fileCachePopulationService,
            IFundingTaskProvider fundingTaskProvider,
            ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _ilrReferenceDataProviderService = ilrReferenceDataProviderService;
            _externalCachePopulationService = externalCachePopulationService;
            _fileCachePopulationService = fileCachePopulationService;
            _logger = logger;
            _fundingTaskProvider = fundingTaskProvider;
        }

        public async Task ExecuteAsync(IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var stopWatchSteps = new Stopwatch();
            stopWatchSteps.Start();

            var message = await _ilrFileProviderService.ProvideAsync(fundingServiceContext, cancellationToken).ConfigureAwait(false);
            _logger.LogDebug($"Funding Service got file in: {stopWatchSteps.ElapsedMilliseconds}");
            stopWatchSteps.Restart();

            cancellationToken.ThrowIfCancellationRequested();

            var refereceData = await _ilrReferenceDataProviderService.ProvideAsync(fundingServiceContext, cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            stopWatchSteps.Restart();

            cancellationToken.ThrowIfCancellationRequested();

            string externalDataCache = _jsonSerializationService.Serialize(_externalCachePopulationService.PopulateAsync(refereceData, cancellationToken));
            string fileDataCache = _jsonSerializationService.Serialize(_fileCachePopulationService.PopulateAsync(message, cancellationToken));

            _logger.LogDebug($"Funding Service got external data: {stopWatchSteps.ElapsedMilliseconds}");

            await _fundingTaskProvider.ProvideAsync(fundingServiceContext, message, externalDataCache, fileDataCache, cancellationToken);

            _logger.LogDebug($"Completed Funding Service for given rule bases in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}
