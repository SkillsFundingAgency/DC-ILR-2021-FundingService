using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM25.Model.Output
{
    public class PeriodisationGlobal
    {
        public int? UKPRN { get; set; }

        public string RulebaseVersion { get; set; }

        public List<LearnerPeriod> LearnerPeriods { get; set; }

        public List<LearnerPeriodisedValues> LearnerPeriodisedValues { get; set; }
    }
}
