using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface
{
    public interface IReferenceDataCache
    {
        IDictionary<string, IEnumerable<LARSFunding>> LARSFunding { get; }

        IDictionary<string, LARSLearningDelivery> LARSLearningDelivery { get; }

        IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValue { get; }

        IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAims { get; }

        IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCatgeory { get; }

        string LARSCurrentVersion { get; }

        IDictionary<string, IEnumerable<SfaAreaCost>> SfaAreaCost { get; }

        IDictionary<string, IEnumerable<SfaDisadvantage>> SfaDisadvantage { get; }

        string PostcodeCurrentVersion { get; }

        string OrgVersion { get; }

        IDictionary<long, IEnumerable<OrgFunding>> OrgFunding { get; }

        IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployers { get; }
    }
}
