using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model
{
    public class CareerLearningPilot
    {
        public string Postcode { get; set; }

        public string AreaCode { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
