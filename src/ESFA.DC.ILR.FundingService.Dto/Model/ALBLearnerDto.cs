using System.Collections.Generic;
using ESFA.DC.ILR.Model;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class ALBLearnerDto
    {
        public string LearnRefNumber { get; set; }

        public List<MessageLearnerLearningDelivery> LearningDeliveries { get; set; }
    }
}
