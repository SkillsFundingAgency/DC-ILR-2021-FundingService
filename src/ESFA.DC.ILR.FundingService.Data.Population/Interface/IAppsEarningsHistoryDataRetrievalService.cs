using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IAppsEarningsHistoryDataRetrievalService
    {
        IDictionary<long, IEnumerable<AECEarningsHistory>> AppsEarningsHistoryForLearners(int providerUKPRN, IEnumerable<LearnRefNumberULNKey> learners);
    }
}
