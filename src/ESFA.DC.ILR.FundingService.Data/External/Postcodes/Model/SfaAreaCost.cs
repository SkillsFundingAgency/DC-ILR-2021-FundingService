﻿using System;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model
{
    public class SfaAreaCost
    {
        public string Postcode { get; set; }

        public decimal AreaCostFactor { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
