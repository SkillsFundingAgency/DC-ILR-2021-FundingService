using System;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes.Abstract
{
    public class AbstractDisadvantage
    {
        public string Postcode { get; set; }

        public decimal? Uplift { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
