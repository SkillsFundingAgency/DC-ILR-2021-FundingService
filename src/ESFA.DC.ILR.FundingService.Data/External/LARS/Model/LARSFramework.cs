using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSFramework
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public DateTime? EffectiveFromNullable { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public LARSFrameworkAim LARSFrameworkAim { get; set; }

        public IReadOnlyCollection<LARSFrameworkCommonComponent> LARSFrameworkCommonComponents { get; set; }

        public IReadOnlyCollection<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFundings { get; set; }
    }
}
