using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.OrchestrationService
{
    public class PreFundingALBPopulationService : IPopulationService
    {
        private readonly IReferenceDataCachePopulationService _referenceDataCachePopulationService;
        private readonly IFundingContext _fundingContext;

        public PreFundingALBPopulationService(IReferenceDataCachePopulationService referenceDataCachePopulationService, IFundingContext fundingContext)
        {
            _referenceDataCachePopulationService = referenceDataCachePopulationService;
            _fundingContext = fundingContext;
        }

        public void Populate()
        {
            var postcodesList = _fundingContext.ValidLearners.SelectMany(l => l.LearningDeliveries).Select(ld => ld.DelLocPostCode).Distinct();
            var learnAimRefsList = _fundingContext.ValidLearners.SelectMany(l => l.LearningDeliveries).Select(ld => ld.LearnAimRef).Distinct();

            _referenceDataCachePopulationService.Populate(learnAimRefsList, postcodesList, new List<long>(), new List<int>());
        }
    }
}
