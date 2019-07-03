using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IAppsEarningsHistoryMapperService
    {
        IDictionary<long, IReadOnlyCollection<AECEarningsHistory>> MapAppsEarningsHistories(IReadOnlyCollection<ApprenticeshipEarningsHistory> apprenticeshipEarnings);
    }
}
