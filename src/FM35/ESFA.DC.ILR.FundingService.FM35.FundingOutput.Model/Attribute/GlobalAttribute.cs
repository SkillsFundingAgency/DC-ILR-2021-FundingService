using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute
{
    public class GlobalAttribute : IGlobalAttribute
    {
        public int UKPRN { get; set; }

        public string CurFundYr { get; set; }

        public string LARSVersion { get; set; }

        public string OrgVersion { get; set; }

        public string PostcodeDisadvantageVersion { get; set; }

        public string RulebaseVersion { get; set; }
    }
}
