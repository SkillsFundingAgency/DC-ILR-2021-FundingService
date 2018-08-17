using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSValidity
    {
        public string Category { get; set; }

        public DateTime? LastNewStartDate { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
