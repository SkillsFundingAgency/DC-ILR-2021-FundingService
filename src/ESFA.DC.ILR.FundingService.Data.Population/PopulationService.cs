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

        public void Populate()
        {
            _internalDataCachePopulationService.Populate();
            _referenceDataCachePopulationService.Populate();
            _fundingContextPopulationService.Populate();
            _fileDataCachePopulationService.Populate();
        }
    }
}
