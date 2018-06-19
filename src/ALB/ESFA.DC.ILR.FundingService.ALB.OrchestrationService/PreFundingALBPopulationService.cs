using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.Contexts.Interface;
using ESFA.DC.ILR.FundingService.ALB.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.ALB.InternalData;
using ESFA.DC.ILR.FundingService.ALB.InternalData.Interface;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.OrchestrationService
{
    public class PreFundingALBPopulationService : IPreFundingALBPopulationService
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

        public void PopulateData()
        {
            var internalDataCache = (InternalDataCache)_internalDataCache;
            internalDataCache.UKPRN = _fundingContext.UKPRN;

            internalDataCache.ValidLearners = _fundingContext.ValidLearners.Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == 99)).ToList();

            var postcodesList = _fundingContext.ValidLearners.SelectMany(l => l.LearningDeliveries).Select(ld => ld.DelLocPostCode).Distinct();
            var learnAimRefsList = _fundingContext.ValidLearners.SelectMany(l => l.LearningDeliveries).Select(ld => ld.LearnAimRef).Distinct();

            _referenceDataCachePopulationService.Populate(learnAimRefsList, postcodesList);
        }
    }
}
