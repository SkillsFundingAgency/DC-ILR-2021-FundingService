using System;

namespace ESFA.DC.ILR.FundingService.Data.External.Organisation.Model
{
    public class OrgFunding
    {
        public int UKPRN { get; set; }

        public DateTime OrgFundEffectiveFrom { get; set; }

        public DateTime? OrgFundEffectiveTo { get; set; }

        public string OrgFundFactor { get; set; }

        public string OrgFundFactType { get; set; }

        public string OrgFundFactValue { get; set; }
    }
}
