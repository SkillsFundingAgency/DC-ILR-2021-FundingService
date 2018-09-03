using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Stubs.Database.Interface;
using ESFA.DC.ILR.FundingService.Stubs.Database.Model;

namespace ESFA.DC.ILR.FundingService.Stubs.Database
{
    public class AppsEarningsHistoryStub : IAppsEarningsHistoryStub
    {
        public IQueryable<AEC_LatestInYearHistory> AEC_LatestInYearHistories { get; set; }
    }
}
