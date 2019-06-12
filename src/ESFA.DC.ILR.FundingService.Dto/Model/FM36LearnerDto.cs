using System;
using System.Collections.Generic;

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

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
