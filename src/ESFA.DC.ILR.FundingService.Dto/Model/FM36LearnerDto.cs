using System;
using System.Collections.Generic;
using ESFA.DC.ILR.Model;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class FM36LearnerDto
    {
        public string LearnRefNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PostcodePrior { get; set; }

        public int? PMUKPRN { get; set; }

        public int? PrevUKPRN { get; set; }

        public long ULN { get; set; }

        public List<LearnerEmploymentStatus> LearnerEmploymentStatuses { get; set; }

        public List<MessageLearnerLearningDelivery> LearningDeliveries { get; set; }
    }
}
