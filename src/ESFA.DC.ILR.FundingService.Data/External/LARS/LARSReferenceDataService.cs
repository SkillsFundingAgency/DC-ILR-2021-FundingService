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

        public LARSLearningDelivery LARSLearningDeliveryForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSLearningDelivery.TryGetValue(learnAimRef, out LARSLearningDelivery learningDelivery);

            return learningDelivery;
        }

        public LARSStandard LARSStandardForStandardCode(int? standardCode)
        {
            if (standardCode == null)
            {
                return null;
            }

            _referenceDataCache.LARSStandards.TryGetValue(standardCode.Value, out LARSStandard stamdard);

            return stamdard;
        }
    }
}
