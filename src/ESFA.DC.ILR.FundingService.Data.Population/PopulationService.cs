using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population
{
    public class PopulationService : IPopulationService
    {
        private readonly IInternalDataCachePopulationService _internalDataCachePopulationService;
        private readonly IExternalDataCachePopulationService _referenceDataCachePopulationService;
        private readonly IFundingContextPopulationService _fundingContextPopulationService;
        private readonly IFileDataCachePopulationService _fileDataCachePopulationService;

        public PopulationService(
            IInternalDataCachePopulationService internalDataCachePopulationService,
            IExternalDataCachePopulationService referenceDataCachePopulationService,
            IFundingContextPopulationService fundingContextPopulationService,
            IFileDataCachePopulationService fileDataCachePopulationService)
        {
            _internalDataCachePopulationService = internalDataCachePopulationService;
            _referenceDataCachePopulationService = referenceDataCachePopulationService;
            _fundingContextPopulationService = fundingContextPopulationService;
            _fileDataCachePopulationService = fileDataCachePopulationService;
        }

        public async Task PopulateAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(
                _internalDataCachePopulationService.PopulateAsync(cancellationToken),
                _referenceDataCachePopulationService.PopulateAsync(cancellationToken),
                _fundingContextPopulationService.PopulateAsync(cancellationToken),
                _fileDataCachePopulationService.PopulateAsync(cancellationToken))
            .ConfigureAwait(false);
        }
    }
}
