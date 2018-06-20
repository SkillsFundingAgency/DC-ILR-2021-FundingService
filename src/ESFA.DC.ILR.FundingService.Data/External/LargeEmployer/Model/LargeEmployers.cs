using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model
{
    public class LargeEmployers
    {
        public int ERN { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
