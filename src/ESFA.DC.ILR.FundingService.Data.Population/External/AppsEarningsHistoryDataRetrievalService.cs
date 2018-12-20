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
                var learnRefs = learners.Select(l => l.LearnRefNumber).ToCaseInsensitiveHashSet();

                var appsData = AecLatestInYearHistory
                    .Where(
                        a => a.UKPRN == providerUKPRN
                        && a.LatestInYear == true
                        && a.ULN < 9999999999)
                   .Join(
                        learnRefs,
                        a => a.LearnRefNumber,
                        l => l,
                        (a, l) => new { aec = a, learnRefNumber = l })
                        .Select(a => new AECEarningsHistory
                        {
                            AppIdentifier = a.aec.AppIdentifier,
                            AppProgCompletedInTheYearInput = a.aec.AppProgCompletedInTheYearInput,
                            CollectionYear = a.aec.CollectionYear,
                            CollectionReturnCode = a.aec.CollectionReturnCode,
                            DaysInYear = a.aec.DaysInYear,
                            FworkCode = a.aec.FworkCode,
                            HistoricEffectiveTNPStartDateInput = a.aec.HistoricEffectiveTNPStartDateInput,
                            HistoricEmpIdEndWithinYear = a.aec.HistoricEmpIdEndWithinYear,
                            HistoricEmpIdStartWithinYear = a.aec.HistoricEmpIdStartWithinYear,
                            HistoricLearner1618StartInput = a.aec.HistoricLearner1618StartInput,
                            HistoricPMRAmount = a.aec.HistoricPMRAmount,
                            HistoricTNP1Input = a.aec.HistoricTNP1Input,
                            HistoricTNP2Input = a.aec.HistoricTNP2Input,
                            HistoricTNP3Input = a.aec.HistoricTNP3Input,
                            HistoricTNP4Input = a.aec.HistoricTNP4Input,
                            HistoricTotal1618UpliftPaymentsInTheYearInput = a.aec.HistoricTotal1618UpliftPaymentsInTheYearInput,
                            HistoricVirtualTNP3EndOfTheYearInput = a.aec.HistoricVirtualTNP3EndOfTheYearInput,
                            HistoricVirtualTNP4EndOfTheYearInput = a.aec.HistoricVirtualTNP4EndOfTheYearInput,
                            HistoricLearnDelProgEarliestACT2DateInput = a.aec.HistoricLearnDelProgEarliestACT2DateInput,
                            LatestInYear = a.aec.LatestInYear,
                            LearnRefNumber = a.learnRefNumber,
                            ProgrammeStartDateIgnorePathway = a.aec.ProgrammeStartDateIgnorePathway,
                            ProgrammeStartDateMatchPathway = a.aec.ProgrammeStartDateMatchPathway,
                            ProgType = a.aec.ProgType,
                            PwayCode = a.aec.PwayCode,
                            STDCode = a.aec.STDCode,
                            TotalProgAimPaymentsInTheYear = a.aec.STDCode,
                            UptoEndDate = a.aec.UptoEndDate,
                            UKPRN = a.aec.UKPRN,
                            ULN = a.aec.ULN
                        }).ToList();

              var dataULNMatch =
                    appsData.Join(
                        shard,
                        a => new { a.LearnRefNumber, a.ULN },
                        l => new { l.LearnRefNumber, l.ULN },
                        (a, l) => new { aec = a, learner = l })
                         .Select(a => a.aec).ToList()
                         .GroupBy(u => u.ULN)
                         .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<AECEarningsHistory>);

                foreach (var kvp in dataULNMatch)
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
