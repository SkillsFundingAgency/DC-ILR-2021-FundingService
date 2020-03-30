using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class LearnerEmploymentStatus
    {
        public int? EmpId { get; set; }

        public DateTime DateEmpStatApp { get; set; }

        public int EmpStat { get; set; }

        public int? SEM { get; set; }
    }
}
