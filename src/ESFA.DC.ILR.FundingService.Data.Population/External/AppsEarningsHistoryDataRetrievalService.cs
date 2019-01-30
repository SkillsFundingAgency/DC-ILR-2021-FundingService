using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class AppsEarningsHistoryDataRetrievalService : IAppsEarningsHistoryDataRetrievalService
    {
        private const int _fundModel = 36;
        private readonly IApprenticeshipsEarningsHistory _appsEarningsHistory;

        public AppsEarningsHistoryDataRetrievalService()
        {
        }

        public AppsEarningsHistoryDataRetrievalService(IApprenticeshipsEarningsHistory appsEarningsHistory)
        {
            _appsEarningsHistory = appsEarningsHistory;
        }

        public virtual IQueryable<AppsEarningsHistory> AecLatestInYearHistory => _appsEarningsHistory.AppsEarningsHistories;

        public IEnumerable<LearnRefNumberULNKey> UniqueFM36Learners(IMessage message)
        {
            return message
                .Learners
                .Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == _fundModel))
                .Select(k => new LearnRefNumberULNKey(k.LearnRefNumber, k.ULN))
                .ToList();
        }

        public IDictionary<long, IEnumerable<AECEarningsHistory>> AppsEarningsHistoryForLearners(int providerUKPRN, IEnumerable<LearnRefNumberULNKey> learners)
        {
            IDictionary<long, IEnumerable<AECEarningsHistory>> result = new Dictionary<long, IEnumerable<AECEarningsHistory>>();

            var learnerShards = learners.SplitList(2500);
            foreach (var shard in learnerShards)
            {
                var learnRefs = shard.Select(l => l.LearnRefNumber).ToCaseInsensitiveHashSet();
                var ulns = shard.Select(l => l.ULN).ToList();

                var history = AecLatestInYearHistory
                   .Where(a => a.LatestInYear == true
                   && a.ULN < 9999999999
                   && ulns.Contains(a.ULN))
                   .GroupBy(a => a.ULN)
                   .ToDictionary(
                    u => u.Key,
                    u => u.Select(AECLatestInYearEarningsFromEntity).ToList() as IEnumerable<AECEarningsHistory>);

                foreach (var kvp in history)
                {
                    result.Add(kvp);
                }
            }

            return result;
        }

        public AECEarningsHistory AECLatestInYearEarningsFromEntity(AppsEarningsHistory entity)
        {
            return new AECEarningsHistory
            {
                AppIdentifier = entity.AppIdentifier,
                AppProgCompletedInTheYearInput = entity.AppProgCompletedInTheYearInput,
                CollectionYear = entity.CollectionYear,
                CollectionReturnCode = entity.CollectionReturnCode,
                DaysInYear = entity.DaysInYear,
                FworkCode = entity.FworkCode,
                HistoricEffectiveTNPStartDateInput = entity.HistoricEffectiveTNPStartDateInput,
                HistoricEmpIdEndWithinYear = entity.HistoricEmpIdEndWithinYear,
                HistoricEmpIdStartWithinYear = entity.HistoricEmpIdStartWithinYear,
                HistoricLearner1618StartInput = entity.HistoricLearner1618StartInput,
                HistoricPMRAmount = entity.HistoricPMRAmount,
                HistoricTNP1Input = entity.HistoricTNP1Input,
                HistoricTNP2Input = entity.HistoricTNP2Input,
                HistoricTNP3Input = entity.HistoricTNP3Input,
                HistoricTNP4Input = entity.HistoricTNP4Input,
                HistoricTotal1618UpliftPaymentsInTheYearInput = entity.HistoricTotal1618UpliftPaymentsInTheYearInput,
                HistoricVirtualTNP3EndOfTheYearInput = entity.HistoricVirtualTNP3EndOfTheYearInput,
                HistoricVirtualTNP4EndOfTheYearInput = entity.HistoricVirtualTNP4EndOfTheYearInput,
                HistoricLearnDelProgEarliestACT2DateInput = entity.HistoricLearnDelProgEarliestACT2DateInput,
                LatestInYear = entity.LatestInYear,
                LearnRefNumber = entity.LearnRefNumber,
                ProgrammeStartDateIgnorePathway = entity.ProgrammeStartDateIgnorePathway,
                ProgrammeStartDateMatchPathway = entity.ProgrammeStartDateMatchPathway,
                ProgType = entity.ProgType,
                PwayCode = entity.PwayCode,
                STDCode = entity.STDCode,
                TotalProgAimPaymentsInTheYear = entity.STDCode,
                UptoEndDate = entity.UptoEndDate,
                UKPRN = entity.UKPRN,
                ULN = entity.ULN
            };
        }
    }
}
