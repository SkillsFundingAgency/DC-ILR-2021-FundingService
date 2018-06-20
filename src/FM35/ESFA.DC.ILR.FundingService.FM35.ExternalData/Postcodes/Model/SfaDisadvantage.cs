using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Model
{
    public class SfaDisadvantage
    {
        public string Postcode { get; set; }

        public decimal? Uplift { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
