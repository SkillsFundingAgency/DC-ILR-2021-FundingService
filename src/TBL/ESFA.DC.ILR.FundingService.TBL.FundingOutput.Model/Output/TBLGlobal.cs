using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.TBL.FundingOutput.Model.Output
{
    public class TBLGlobal
    {
        public int UKPRN { get; set; }

        public string CurFundYr { get; set; }

        public string LARSVersion { get; set; }

        public string RulebaseVersion { get; set; }

        public List<TBLLearner> Learners { get; set; }
    }
}
