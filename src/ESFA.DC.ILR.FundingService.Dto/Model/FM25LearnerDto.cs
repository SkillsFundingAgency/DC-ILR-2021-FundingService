using System;
using System.Collections.Generic;
using ESFA.DC.ILR.Model;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class FM25LearnerDto
    {
        public string LearnRefNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Postcode { get; set; }

        public string EngGrade { get; set; }

        public string MathGrade { get; set; }

        public int? PlanEEPHours { get; set; }

        public int? PlanLearnHours { get; set; }

        public long ULN { get; set; }

        public List<MessageLearnerDestinationandProgressionDPOutcome> DPOutcomes { get; set; }

        public List<MessageLearnerLearnerFAM> LearnerFAMs { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
