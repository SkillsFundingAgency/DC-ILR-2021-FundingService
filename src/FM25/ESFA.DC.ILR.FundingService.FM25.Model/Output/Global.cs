using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM25.Model.Output
{
    public class Global
    {
        public int UKPRN { get; set; }
        public string LARSVersion { get; set; }
        public string OrgVersion { get; set; }
        public string PostcodeDisadvantageVersion { get; set; }
        public string RulebaseVersion { get; set; }

        public List<Learner> Learners { get; set; }
    }
}
