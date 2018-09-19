using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Interface
{
    public interface IAppsEarningsHistoryReferenceDataService
    {
        IEnumerable<AECEarningsHistory> AECEarningsHistory(long uln);
    }
}
