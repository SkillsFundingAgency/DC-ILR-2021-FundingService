using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSStandard
    {
        public int StandardCode { get; set; }

        public string StandardSectorCode { get; set; }

        public string NotionalEndLevel { get; set; }

        public IReadOnlyCollection<LARSStandardApprenticeshipFunding> LARSStandardApprenticeshipFundings { get; set; }

        public IReadOnlyCollection<LARSStandardCommonComponent> LARSStandardCommonComponents { get; set; }

        public IReadOnlyCollection<LARSStandardFunding> LARSStandardFundings { get; set; }
    }
}
