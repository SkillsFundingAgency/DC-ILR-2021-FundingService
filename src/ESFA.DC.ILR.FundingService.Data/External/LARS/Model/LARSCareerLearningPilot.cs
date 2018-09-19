using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSCareerLearningPilot
    {
        public string AreaCode { get; set; }

        public decimal? SubsidyRate { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
