using System;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class DPOutcome
    {
        public string OutType { get; set; }

        public int OutCode { get; set; }

        public DateTime OutStartDate { get; set; }

        public DateTime? OutEndDate { get; set; }

        public DateTime OutCollDate { get; set; }
    }
}
