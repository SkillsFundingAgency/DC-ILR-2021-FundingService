using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Interface
{
    public interface ILARSReferenceDataService
    {
        string LARSCurrentVersion();

        IEnumerable<LARSFunding> LARSFundingsForLearnAimRef(string learnAimRef);

        IEnumerable<LARSFrameworkAims> LARSFFrameworkAimsForLearnAimRef(string learnAimRef);

        IEnumerable<LARSAnnualValue> LARSAnnualValuesForLearnAimRef(string learnAimRef);

        IEnumerable<LARSLearningDeliveryCategory> LARSLearningDeliveryCategoriesForLearnAimRef(string learnAimRef);

        LARSLearningDelivery LARSLearningDeliveriesForLearnAimRef(string learnAimRef);
    }
}
