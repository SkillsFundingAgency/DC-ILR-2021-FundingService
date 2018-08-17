using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSFunding
    {
        public string LearnAimRef { get; set; }

        public string FundingCategory { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
        public decimal? RateUnWeighted { get; set; }

        public decimal? RateWeighted { get; set; }

        public string WeightingFactor { get; set; }
    }
}
