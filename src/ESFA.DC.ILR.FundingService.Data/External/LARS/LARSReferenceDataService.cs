using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.ExternalData.LARS
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

        public IEnumerable<LARSFunding> LARSFundingsForLearnAimRef(string learnAimRef)
        {
            try
            {
                return _referenceDataCache.LARSFunding[learnAimRef];
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(string.Format("Cannot find LARS Funding data for LearnAimRef: " + learnAimRef + " in the Dictionary. Exception details: " + ex));
            }
        }

        public LARSLearningDelivery LARSLearningDeliveriesForLearnAimRef(string learnAimRef)
        {
            try
            {
                return _referenceDataCache.LARSLearningDelivery[learnAimRef];
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(string.Format("Cannot find LARS Learning Delivery data for LearnAimRef: " + learnAimRef + " in the Dictionary. Exception details: " + ex));
            }
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
            _referenceDataCache.LARSLearningDeliveryCatgeory.TryGetValue(learnAimRef, out IEnumerable<LARSLearningDeliveryCategory> larsLearningDeliveryCategory);

            return larsLearningDeliveryCategory;
        }
    }
}
