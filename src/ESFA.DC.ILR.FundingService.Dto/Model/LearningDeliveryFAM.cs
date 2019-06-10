using System;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class LearningDeliveryFAM
    {
        public string LearnDelFAMCode { get; set; }

        public DateTime? LearnDelFAMDateTo { get; set; }

        public DateTime? LearnDelFAMDateFrom { get; set; }

        public string LearnDelFAMType { get; set; }
    }
}
