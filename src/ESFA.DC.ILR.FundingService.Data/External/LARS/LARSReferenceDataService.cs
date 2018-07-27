using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS
{
    public class LARSReferenceDataService : ILARSReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public LARSReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public string LARSCurrentVersion()
        {
            return _referenceDataCache.LARSCurrentVersion;
        }

        public IEnumerable<LARSFunding> LARSFundingsForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSFunding.TryGetValue(learnAimRef, out IEnumerable<LARSFunding> larsFundings);

            return larsFundings;
        }

        public LARSLearningDelivery LARSLearningDeliveryForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSLearningDelivery.TryGetValue(learnAimRef, out LARSLearningDelivery learningDelivery);

            return learningDelivery;
        }

        public IEnumerable<LARSAnnualValue> LARSAnnualValuesForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSAnnualValue.TryGetValue(learnAimRef, out IEnumerable<LARSAnnualValue> larsAnnualValue);

            return larsAnnualValue;
        }

        public IEnumerable<LARSFrameworkAims> LARSFFrameworkAimsForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSFrameworkAims.TryGetValue(learnAimRef, out IEnumerable<LARSFrameworkAims> larsFrameworkAims);

            return larsFrameworkAims;
        }

        public IEnumerable<LARSLearningDeliveryCategory> LARSLearningDeliveryCategoriesForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSLearningDeliveryCategory.TryGetValue(learnAimRef, out IEnumerable<LARSLearningDeliveryCategory> larsLearningDeliveryCategory);

            return larsLearningDeliveryCategory;
        }
    }
}
