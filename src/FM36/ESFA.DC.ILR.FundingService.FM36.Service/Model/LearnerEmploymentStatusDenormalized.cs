using System;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Model
{
    public class LearnerEmploymentStatusDenormalized
    {
        public string AgreeId { get; set; }

        public DateTime DateEmpStatApp { get; set; }

        public int? EmpId { get; set; }

        public int EMPStat { get; set; }

        public int SEM { get; set; }
    }
}
