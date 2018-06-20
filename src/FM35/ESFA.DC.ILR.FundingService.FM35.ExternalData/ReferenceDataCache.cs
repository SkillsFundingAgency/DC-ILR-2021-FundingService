using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData
{
    public class ReferenceDataCache : IReferenceDataCache
    {
        public IDictionary<string, IEnumerable<LARSFunding>> LARSFunding { get; set; } = new Dictionary<string, IEnumerable<LARSFunding>>();

        public IDictionary<string, LARSLearningDelivery> LARSLearningDelivery { get; set; } = new Dictionary<string, LARSLearningDelivery>();

        public IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValue { get; set; } = new Dictionary<string, IEnumerable<LARSAnnualValue>>();

        public IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAims { get; set; } = new Dictionary<string, IEnumerable<LARSFrameworkAims>>();

        public IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCatgeory { get; set; } = new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>();

        public string LARSCurrentVersion { get; set; }

        public IDictionary<string, IEnumerable<SfaAreaCost>> SfaAreaCost { get; set; } = new Dictionary<string, IEnumerable<SfaAreaCost>>();

        public IDictionary<string, IEnumerable<SfaDisadvantage>> SfaDisadvantage { get; set; } = new Dictionary<string, IEnumerable<SfaDisadvantage>>();

        public string PostcodeCurrentVersion { get; set; }

        public string OrgVersion { get; set; }

        public IDictionary<long, IEnumerable<OrgFunding>> OrgFunding { get; set; } = new Dictionary<long, IEnumerable<OrgFunding>>();

        public IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployers { get; set; } = new Dictionary<int, IEnumerable<LargeEmployers>>();
    }
}
