using System;
using System.Collections.Generic;
using ESFA.DC.ILR.Model;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class FM35LearnerDto
    {
        public string LearnRefNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PostcodePrior { get; set; }

        public List<MessageLearnerLearnerEmploymentStatus> LearnerEmploymentStatuses { get; set; }

        public List<MessageLearnerLearningDelivery> LearningDeliveries { get; set; }
    }
}
