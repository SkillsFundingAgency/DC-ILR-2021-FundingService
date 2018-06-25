using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.OrchestrationService
{
    public class PreFundingALBPopulationService : IPopulationService
    {
        private readonly IReferenceDataCachePopulationService _referenceDataCachePopulationService;
        private readonly IFundingContext _fundingContext;
        private readonly IInternalDataCache _internalDataCache;

        public PreFundingALBPopulationService(IReferenceDataCachePopulationService referenceDataCachePopulationService, IFundingContext fundingContext, IInternalDataCache internalDataCache)
        {
            _referenceDataCachePopulationService = referenceDataCachePopulationService;
            _fundingContext = fundingContext;
            _internalDataCache = internalDataCache;
        }

        public void Populate()
        {
            var internalDataCache = (InternalDataCache)_internalDataCache;
            internalDataCache.UKPRN = _fundingContext.UKPRN;

            internalDataCache.ValidLearners = _fundingContext.ValidLearners.Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == 99)).ToList();

            var postcodesList = _fundingContext.ValidLearners.SelectMany(l => l.LearningDeliveries).Select(ld => ld.DelLocPostCode).Distinct();
            var learnAimRefsList = _fundingContext.ValidLearners.SelectMany(l => l.LearningDeliveries).Select(ld => ld.LearnAimRef).Distinct();

            _referenceDataCachePopulationService.Populate(learnAimRefsList, postcodesList, new List<long>(), new List<int>());
        }
    }
}
