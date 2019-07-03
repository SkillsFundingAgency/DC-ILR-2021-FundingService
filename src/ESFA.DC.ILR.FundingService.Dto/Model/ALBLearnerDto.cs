using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class ALBLearnerDto
    {
        public string LearnRefNumber { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
