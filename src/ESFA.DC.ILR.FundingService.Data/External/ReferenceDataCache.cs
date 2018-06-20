using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External
{
    public class ReferenceDataCache : IReferenceDataCache
    {
        public IDictionary<string, IEnumerable<LARSFunding>> LARSFunding { get; set; } = new Dictionary<string, IEnumerable<LARSFunding>>();

        public IDictionary<string, LARSLearningDelivery> LARSLearningDelivery { get; set; } = new Dictionary<string, LARSLearningDelivery>();

        public string LARSCurrentVersion { get; set; }

        public IDictionary<string, IEnumerable<SfaAreaCost>> SfaAreaCost { get; set; } = new Dictionary<string, IEnumerable<SfaAreaCost>>();

        public string PostcodeCurrentVersion { get; set; }



        public IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValue { get; set; } = new Dictionary<string, IEnumerable<LARSAnnualValue>>();

        public IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAims { get; set; } = new Dictionary<string, IEnumerable<LARSFrameworkAims>>();

        public IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCatgeory { get; set; } = new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>();

        public IDictionary<string, IEnumerable<SfaDisadvantage>> SfaDisadvantage { get; set; } = new Dictionary<string, IEnumerable<SfaDisadvantage>>();

        public string OrgVersion { get; set; }

        public IDictionary<long, IEnumerable<OrgFunding>> OrgFunding { get; set; } = new Dictionary<long, IEnumerable<OrgFunding>>();

        public IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployers { get; set; } = new Dictionary<int, IEnumerable<LargeEmployers>>();
    }
}
