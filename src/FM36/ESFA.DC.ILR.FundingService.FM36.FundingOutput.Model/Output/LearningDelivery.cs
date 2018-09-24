using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class LearningDelivery
    {
        public int AimSeqNumber { get; set; }

        public List<LearningDeliveryValues> LearningDeliveryValues { get; set; }

        public List<LearningDeliveryPeriodisedValues> LearningDeliveryPeriodisedValues { get; set; }
    }
}
