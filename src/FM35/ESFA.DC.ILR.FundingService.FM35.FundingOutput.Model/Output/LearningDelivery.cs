using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output
{
    public class LearningDelivery
    {
        public int? AimSeqNumber { get; set; }

        public LearningDeliveryValue LearningDeliveryAttributeDatas { get; set; }

        public List<LearningDeliveryPeriodisedValue> LearningDeliveryPeriodisedValues { get; set; }
    }
}
