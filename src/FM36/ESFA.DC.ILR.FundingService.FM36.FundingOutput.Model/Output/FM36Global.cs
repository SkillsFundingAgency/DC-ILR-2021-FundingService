using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class FM36Global
    {
        public int UKPRN { get; set; }

        public string LARSVersion { get; set; }

        public string RulebaseVersion { get; set; }

        public string Year { get; set; }

        public List<FM36Learner> Learners { get; set; }
    }
}
