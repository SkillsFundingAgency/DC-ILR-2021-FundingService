using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.TBL.FundingOutput.Model.Output
{
    public class TBLLearner
    {
        public string LearnRefNumber { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
