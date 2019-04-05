using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSFramework
    {
        public DateTime? EffectiveFromNullable { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public IReadOnlyCollection<LARSFrameworkCommonComponent> LARSFrameworkCommonComponents { get; set; }

        public IReadOnlyCollection<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFundings { get; set; }
    }
}
