using System.Linq;
using ESFA.DC.ILR.FundingService.Stubs.Database.Model;

namespace ESFA.DC.ILR.FundingService.Stubs.Database.Interface
{
    public interface IAppsEarningsHistoryStub
    {
        IQueryable<AEC_LatestInYearHistory> AEC_LatestInYearHistories { get; }
    }
}
