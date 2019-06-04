using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External
{
    public class ExternalDataCache : IExternalDataCache
    {
        public string LARSCurrentVersion { get; set; }

        public IDictionary<string, LARSLearningDelivery> LARSLearningDelivery { get; set; }

        public IDictionary<int, LARSStandard> LARSStandards { get; set; }

        public string PostcodeCurrentVersion { get; set; }

        public IDictionary<string, PostcodeRoot> PostcodeRoots { get; set; }
        
        public string OrgVersion { get; set; }

        public IDictionary<int, IReadOnlyCollection<OrgFunding>> OrgFunding { get; set; }

        public IDictionary<int, IReadOnlyCollection<LargeEmployers>> LargeEmployers { get; set; }

        public IDictionary<long, IReadOnlyCollection<AECEarningsHistory>> AECLatestInYearEarningHistory { get; set; }

        public IReadOnlyCollection<FCSContractAllocation> FCSContractAllocations { get; set; }

        public Periods Periods { get; set; }
    }
}
