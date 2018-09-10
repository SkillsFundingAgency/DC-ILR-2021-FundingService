using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class AppsEarningsHistoryDataRetrievalService : IAppsEarningsHistoryDataRetrievalService
    {
        private readonly IAppsEarningsHistory _appsEarningsHistory;

        public AppsEarningsHistoryDataRetrievalService()
        {
        }

        public AppsEarningsHistoryDataRetrievalService(IAppsEarningsHistory appsEarningsHistory)
        {
            _appsEarningsHistory = appsEarningsHistory;
        }

        public virtual IQueryable<AppsEarningsHistory> AecLatestInYearHistory => _appsEarningsHistory.AppsEarningsHistory;

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

        public AECLatestInYearEarningHistory AECLatestInYearEarningsFromEntity(AppsEarningsHistory entity)
        {
            return new AECLatestInYearEarningHistory
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
