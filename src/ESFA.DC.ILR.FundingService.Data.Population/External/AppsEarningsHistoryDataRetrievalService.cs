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
        private readonly IApprenticeshipsEarningsHistory _appsEarningsHistory;

        public AppsEarningsHistoryDataRetrievalService()
        {
        }

        public AppsEarningsHistoryDataRetrievalService(IApprenticeshipsEarningsHistory appsEarningsHistory)
        {
            _appsEarningsHistory = appsEarningsHistory;
        }

        public virtual IQueryable<AppsEarningsHistory> AecLatestInYearHistory => _appsEarningsHistory.AppsEarningsHistories;

        public IDictionary<long, IEnumerable<AECEarningsHistory>> AppsEarningsHistoryForLearners(int providerUKPRN, IEnumerable<LearnRefNumberULNKey> learners)
        {
            var dictionary = new Dictionary<long, IEnumerable<AECEarningsHistory>>();

            foreach (var learner in learners)
            {
                var aecHistory = AecLatestInYearHistory?
                    .Where(
                        a => a.UKPRN == providerUKPRN
                        && a.LatestInYear == true
                        && a.ULN < 9999999999
                        && a.LearnRefNumber == learner.LearnRefNumber
                        && a.ULN == learner.ULN)
                        .Select(u => u).ToList() as List<AppsEarningsHistory>;

                if (aecHistory.Count > 0)
                {
                    dictionary.Add(
                    learner.ULN,
                    aecHistory
                    .GroupBy(u => u.ULN)
                    .Select(u => u.Select(AECLatestInYearEarningsFromEntity).ToList() as List<AECEarningsHistory>)
                    .SingleOrDefault());
                }
            }

            return dictionary;
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
