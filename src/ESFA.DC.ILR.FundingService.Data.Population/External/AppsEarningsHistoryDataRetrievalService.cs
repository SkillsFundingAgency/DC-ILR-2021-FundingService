using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.FundingService.Stubs.Database.Interface;
using ESFA.DC.ILR.FundingService.Stubs.Database.Model;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class AppsEarningsHistoryDataRetrievalService : IAppsEarningsHistoryDataRetrievalService
    {
        private readonly IAppsEarningsHistoryStub _appsEarningsHistory;

        public AppsEarningsHistoryDataRetrievalService()
        {
        }

        public AppsEarningsHistoryDataRetrievalService(IAppsEarningsHistoryStub appsEarningsHistory)
        {
            _appsEarningsHistory = appsEarningsHistory;
        }

        public virtual IQueryable<AEC_LatestInYearHistory> AecLatestInYearHistory => _appsEarningsHistory.AEC_LatestInYearHistories;

        public IDictionary<long, IEnumerable<AECLatestInYearEarningHistory>> AppsEarningsHistoryForLearners(int providerUKPRN, IEnumerable<LearnRefNumberULNKey> learners)
        {
            var dictionary = new Dictionary<long, IEnumerable<AECLatestInYearEarningHistory>>();

            foreach (var learner in learners)
            {
                dictionary.Add(
                    learner.ULN,
                    AecLatestInYearHistory
                    .Where(
                        a => a.UKPRN == providerUKPRN
                        && a.LatestInYear == true
                        && a.LearnRefNumber == learner.LearnRefNumber
                        && a.ULN == learner.ULN)
                    .GroupBy(u => u.ULN)
                    .Select(u => u.Select(AECLatestInYearEarningsFromEntity).ToList() as IEnumerable<AECLatestInYearEarningHistory>)
                    .SingleOrDefault());
            }

            return dictionary;
        }

        public AECLatestInYearEarningHistory AECLatestInYearEarningsFromEntity(AEC_LatestInYearHistory entity)
        {
            return new AECLatestInYearEarningHistory
            {
                AppIdentifier = entity.AppIdentifier,
                AppProgCompletedInTheYearInput = entity.AppProgCompletedInTheYearOutput,
                CollectionYear = entity.CollectionYear,
                CollectionReturnCode = entity.CollectionReturnCode,
                DaysInYear = entity.DaysInYear,
                FworkCode = entity.FworkCode,
                HistoricEffectiveTNPStartDateInput = entity.HistoricEffectiveTNPStartDateOutput,
                HistoricEmpIdEndWithinYear = entity.HistoricEmpIdEndWithinYear,
                HistoricEmpIdStartWithinYear = entity.HistoricEmpIdStartWithinYear,
                HistoricLearner1618StartInput = entity.HistoricLearner1618StartOutput,
                HistoricPMRAmount = entity.HistoricPMRAmount,
                HistoricTNP1Input = entity.HistoricTNP1Output,
                HistoricTNP2Input = entity.HistoricTNP2Output,
                HistoricTNP3Input = entity.HistoricTNP3Output,
                HistoricTNP4Input = entity.HistoricTNP4Output,
                HistoricTotal1618 = entity.HistoricTotal1618,
                HistoricVirtualTNP3EndOfTheYearInput = entity.HistoricVirtualTNP3EndOfTheYearOutput,
                HistoricVirtualTNP4EndOfTheYearInput = entity.HistoricVirtualTNP4EndOfTheYearOutput,
                HistoricLearnDelProgEarliestACT2DateInput = entity.HistoricLearnDelProgEarliestACT2DateOutput,
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
