using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;

namespace ESFA.DC.ILR.FundingService.Data.Interface
{
    public interface IExternalDataCache
    {
        IDictionary<string, LARSLearningDelivery> LARSLearningDelivery { get; }

        IDictionary<int, LARSStandard> LARSStandards { get; }

        string LARSCurrentVersion { get; }
        
        IDictionary<string, PostcodeRoot> PostcodeRoots { get; }

        string PostcodeCurrentVersion { get; }

        string OrgVersion { get; }

        IDictionary<int, IReadOnlyCollection<OrgFunding>> OrgFunding { get; }

        IDictionary<string, IReadOnlyCollection<CampusIdentifierSpecResource>> CampusIdentifierSpecResources { get;}

        IDictionary<int, IReadOnlyCollection<LargeEmployers>> LargeEmployers { get; }

        IDictionary<long, IReadOnlyCollection<AECEarningsHistory>> AECLatestInYearEarningHistory { get; }

        IReadOnlyCollection<FCSContractAllocation> FCSContractAllocations { get; }
    }
}
