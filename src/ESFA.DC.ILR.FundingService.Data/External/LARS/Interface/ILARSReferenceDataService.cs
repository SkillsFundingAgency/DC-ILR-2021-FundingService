using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Interface
{
    public interface ILARSReferenceDataService
    {
        string LARSCurrentVersion();

        LARSLearningDelivery LARSLearningDeliveryForLearnAimRef(string learnAimRef);

        LARSStandard LARSStandardForStandardCode(int? standardCode);

        LARSFrameworkAim LARSFrameworkAimForForLearningDelivery(IReadOnlyCollection<LARSFrameworkAim> larsFrameworkAims, int? fworkCode, int? progType, int? pwayCode);
    }
}
