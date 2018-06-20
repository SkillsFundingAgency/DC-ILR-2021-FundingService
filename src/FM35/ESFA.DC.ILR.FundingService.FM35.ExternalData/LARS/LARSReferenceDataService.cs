using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS
{
    public class LARSReferenceDataService : ILARSReferenceDataService
    {
        private readonly IReferenceDataCache _referenceDataCache;

        public LARSReferenceDataService(IReferenceDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public string LARSCurrentVersion()
        {
            return _referenceDataCache.LARSCurrentVersion;
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

        public IEnumerable<LARSFunding> LARSFundingsForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSFunding.TryGetValue(learnAimRef, out IEnumerable<LARSFunding> larsFunding);

            return larsFunding;
        }

        public LARSLearningDelivery LARSLearningDeliveriesForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSLearningDelivery.TryGetValue(learnAimRef, out LARSLearningDelivery larsLearningDelivery);

            return larsLearningDelivery;
        }

        public IEnumerable<LARSLearningDeliveryCategory> LARSLearningDeliveryCategoriesForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSLearningDeliveryCatgeory.TryGetValue(learnAimRef, out IEnumerable<LARSLearningDeliveryCategory> larsLearningDeliveryCategory);

            return larsLearningDeliveryCategory;
        }
    }
}
