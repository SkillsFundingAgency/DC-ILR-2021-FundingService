using System;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model
{
    public class SfaDisadvantage
    {
        public string Postcode { get; set; }

        public decimal? Uplift { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
