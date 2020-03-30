using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class LearningDelivery
    {
        public DateTime? AchDate { get; set; }

        public int? AddHours { get; set; }

        public int AimSeqNumber { get; set; }

        public int AimType { get; set; }

        public int CompStatus { get; set; }

        public string ConRefNumber { get; set; }

        public int? EmpOutcome { get; set; }

        public int? EnglandFEHEStatus { get; set; }

        public int? EnglPrscID { get; set; }

        public string DelLocPostCode { get; set; }

        public int? FworkCode { get; set; }

        public int? FrameworkCommonComponent { get; set; }

        public int? FrameworkComponentType { get; set; }

        public int FundModel { get; set; }

        public string LearnAimRef { get; set; }

        public DateTime? LearnActEndDate { get; set; }

        public DateTime LearnPlanEndDate { get; set; }

        public DateTime LearnStartDate { get; set; }

        public DateTime? OrigLearnStartDate { get; set; }

        public int? OtherFundAdj { get; set; }

        public int? OtjActHours { get; set; }

        public int? Outcome { get; set; }

        public int? PriorLearnFundAdj { get; set; }

        public int? ProgType { get; set; }

        public int? PwayCode { get; set; }

        public int? StdCode { get; set; }

        public int? WithdrawReason { get; set; }

        public List<AppFinRecord> AppFinRecords { get; set; }

        public List<LearningDeliveryFAM> LearningDeliveryFAMs { get; set; }
    }
}
