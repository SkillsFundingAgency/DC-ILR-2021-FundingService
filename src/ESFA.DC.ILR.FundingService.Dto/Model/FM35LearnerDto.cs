using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class FM35LearnerDto
    {
        public string LearnRefNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PostcodePrior { get; set; }

        public List<LearnerEmploymentStatus> LearnerEmploymentStatuses { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
