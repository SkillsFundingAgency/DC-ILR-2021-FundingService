using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output
{
    public class ALBLearner
    {
        public string LearnRefNumber { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }

        public List<LearnerPeriodisedValue> LearnerPeriodisedValues { get; set; }
    }
}
