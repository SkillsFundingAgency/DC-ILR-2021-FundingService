using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class FM25LearnerDto
    {
        public string LearnRefNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string CampId { get; set; }

        public string Postcode { get; set; }

        public string EngGrade { get; set; }

        public string MathGrade { get; set; }

        public int? PlanEEPHours { get; set; }

        public int? PlanLearnHours { get; set; }

        public long ULN { get; set; }

        public int? LrnFAM_ECF { get; set; }

        public int? LrnFAM_EDF1 { get; set; }

        public int? LrnFAM_EDF2 { get; set; }

        public int? LrnFAM_EHC { get; set; }

        public int? LrnFAM_HNS { get; set; }

        public int? LrnFAM_MCF { get; set; }

        public List<DPOutcome> DPOutcomes { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
