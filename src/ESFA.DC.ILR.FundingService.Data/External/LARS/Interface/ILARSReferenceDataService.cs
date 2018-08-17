using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Interface
{
    public interface ILARSReferenceDataService
    {
        string LARSCurrentVersion();

        IEnumerable<LARSFunding> LARSFundingsForLearnAimRef(string learnAimRef);

        LARSLearningDelivery LARSLearningDeliveryForLearnAimRef(string learnAimRef);

        IEnumerable<LARSFrameworkAims> LARSFFrameworkAimsForLearnAimRef(string learnAimRef);

        IEnumerable<LARSAnnualValue> LARSAnnualValuesForLearnAimRef(string learnAimRef);

        IEnumerable<LARSLearningDeliveryCategory> LARSLearningDeliveryCategoriesForLearnAimRef(string learnAimRef);

    }
}
