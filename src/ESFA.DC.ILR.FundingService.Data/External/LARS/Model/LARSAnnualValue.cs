using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSAnnualValue
    {
        public int? BasicSkillsType { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
