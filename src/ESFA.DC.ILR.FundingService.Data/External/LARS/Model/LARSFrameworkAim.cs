using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSFrameworkAim
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int? FrameworkComponentType { get; set; }

        public LARSFramework LARSFramework { get; set; }
    }
}
