using ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod.Interface;
using ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod
{
    public class CollectionPeriodDataService : ICollectionPeriodDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public CollectionPeriodDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public Periods CollectionPeriods()
        {
            return _referenceDataCache.Periods;
        }
    }
}
