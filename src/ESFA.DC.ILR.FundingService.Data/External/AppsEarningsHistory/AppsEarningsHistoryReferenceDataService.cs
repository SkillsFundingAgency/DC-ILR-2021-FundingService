﻿using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory
{
    public class AppsEarningsHistoryReferenceDataService : IAppsEarningsHistoryReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;
        private readonly IEnumerable<AECLatestInYearEarningHistory> _emptyAecEarningsHistory = new List<AECLatestInYearEarningHistory>();

        public AppsEarningsHistoryReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public IEnumerable<AECLatestInYearEarningHistory> AECEarningsHistory(long uln)
        {
            _referenceDataCache.AECLatestInYearEarningHistory.TryGetValue(uln, out IEnumerable<AECLatestInYearEarningHistory> aecEarningsHistory);

            return aecEarningsHistory ?? _emptyAecEarningsHistory;
        }
    }
}
