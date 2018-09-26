using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output
{
    public class FM35Learner
    {
        public string LearnRefNumber { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
