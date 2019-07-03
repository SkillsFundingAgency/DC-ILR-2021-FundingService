using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class AppsEarningsHistoryDataRetrievalServiceTests
    {
        [Fact]
        public void AppsEarningsHistoryForLearners()
        {
            var expectedEarningsHistory = ExpectedAppsEarnings();

            var appsEarnings = new List<ApprenticeshipEarningsHistory>()
            {
                new ApprenticeshipEarningsHistory()
                {
                    AppIdentifier = "1",
                    AppProgCompletedInTheYearInput = true,
                    CollectionYear = "2",
                    CollectionReturnCode = "3",
                    DaysInYear = 4,
                    FworkCode = 5,
                    HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                    HistoricEmpIdEndWithinYear = 6,
                    HistoricEmpIdStartWithinYear = 7,
                    HistoricLearner1618StartInput = true,
                    HistoricPMRAmount = 8m,
                    HistoricTNP1Input = 9m,
                    HistoricTNP2Input = 10m,
                    HistoricTNP3Input = 11m,
                    HistoricTNP4Input = 12m,
                    HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                    HistoricVirtualTNP3EndOfTheYearInput = 14m,
                    HistoricVirtualTNP4EndOfTheYearInput = 15m,
                    HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                    LatestInYear = true,
                    LearnRefNumber = "16",
                    ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                    ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                    ProgType = 17,
                    PwayCode = 18,
                    STDCode = 19,
                    TotalProgAimPaymentsInTheYear = 20m,
                    UptoEndDate = new DateTime(2018, 8, 1),
                    UKPRN = 123456,
                    ULN = 1234567890,
                },
                new ApprenticeshipEarningsHistory()
                {
                    AppIdentifier = "1.2",
                    AppProgCompletedInTheYearInput = true,
                    CollectionYear = "2",
                    CollectionReturnCode = "3",
                    DaysInYear = 4,
                    FworkCode = 5,
                    HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                    HistoricEmpIdEndWithinYear = 6,
                    HistoricEmpIdStartWithinYear = 7,
                    HistoricLearner1618StartInput = true,
                    HistoricPMRAmount = 8m,
                    HistoricTNP1Input = 9m,
                    HistoricTNP2Input = 10m,
                    HistoricTNP3Input = 11m,
                    HistoricTNP4Input = 12m,
                    HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                    HistoricVirtualTNP3EndOfTheYearInput = 14m,
                    HistoricVirtualTNP4EndOfTheYearInput = 15m,
                    HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                    LatestInYear = true,
                    LearnRefNumber = "16",
                    ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                    ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                    ProgType = 17,
                    PwayCode = 18,
                    STDCode = 19,
                    TotalProgAimPaymentsInTheYear = 20m,
                    UptoEndDate = new DateTime(2018, 8, 1),
                    UKPRN = 123456,
                    ULN = 1234567890,
                },
                new ApprenticeshipEarningsHistory()
                {
                    AppIdentifier = "1",
                    AppProgCompletedInTheYearInput = true,
                    CollectionYear = "2",
                    CollectionReturnCode = "3",
                    DaysInYear = 4,
                    FworkCode = 5,
                    HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                    HistoricEmpIdEndWithinYear = 6,
                    HistoricEmpIdStartWithinYear = 7,
                    HistoricLearner1618StartInput = true,
                    HistoricPMRAmount = 8m,
                    HistoricTNP1Input = 9m,
                    HistoricTNP2Input = 10m,
                    HistoricTNP3Input = 11m,
                    HistoricTNP4Input = 12m,
                    HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                    HistoricVirtualTNP3EndOfTheYearInput = 14m,
                    HistoricVirtualTNP4EndOfTheYearInput = 15m,
                    HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                    LatestInYear = true,
                    LearnRefNumber = "16",
                    ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                    ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                    ProgType = 17,
                    PwayCode = 18,
                    STDCode = 19,
                    TotalProgAimPaymentsInTheYear = 20m,
                    UptoEndDate = new DateTime(2018, 8, 1),
                    UKPRN = 123456,
                    ULN = 1234567898,
                },
                new ApprenticeshipEarningsHistory()
                {
                    AppIdentifier = "1",
                    AppProgCompletedInTheYearInput = true,
                    CollectionYear = "2",
                    CollectionReturnCode = "3",
                    DaysInYear = 4,
                    FworkCode = 5,
                    HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                    HistoricEmpIdEndWithinYear = 6,
                    HistoricEmpIdStartWithinYear = 7,
                    HistoricLearner1618StartInput = true,
                    HistoricPMRAmount = 8m,
                    HistoricTNP1Input = 9m,
                    HistoricTNP2Input = 10m,
                    HistoricTNP3Input = 11m,
                    HistoricTNP4Input = 12m,
                    HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                    HistoricVirtualTNP3EndOfTheYearInput = 14m,
                    HistoricVirtualTNP4EndOfTheYearInput = 15m,
                    HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                    LatestInYear = true,
                    LearnRefNumber = "16",
                    ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                    ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                    ProgType = 17,
                    PwayCode = 18,
                    STDCode = 19,
                    TotalProgAimPaymentsInTheYear = 20m,
                    UptoEndDate = new DateTime(2018, 8, 1),
                    UKPRN = 123456,
                    ULN = 1234567899,
                }
            };

            var result = NewService().MapAppsEarningsHistories(appsEarnings);

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(expectedEarningsHistory);
        }

        [Fact]
        public void AppsEarningsHistoryForLearners_Null()
        {
            NewService().MapAppsEarningsHistories(null).Should().BeNull();
        }

        private IDictionary<long, IReadOnlyCollection<AECEarningsHistory>> ExpectedAppsEarnings()
        {
            return new Dictionary<long, IReadOnlyCollection<AECEarningsHistory>>
            {
                {
                    1234567890,
                    new List<AECEarningsHistory>
                    {
                        new AECEarningsHistory()
                        {
                            AppIdentifier = "1",
                            AppProgCompletedInTheYearInput = true,
                            CollectionYear = "2",
                            CollectionReturnCode = "3",
                            DaysInYear = 4,
                            FworkCode = 5,
                            HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                            HistoricEmpIdEndWithinYear = 6,
                            HistoricEmpIdStartWithinYear = 7,
                            HistoricLearner1618StartInput = true,
                            HistoricPMRAmount = 8m,
                            HistoricTNP1Input = 9m,
                            HistoricTNP2Input = 10m,
                            HistoricTNP3Input = 11m,
                            HistoricTNP4Input = 12m,
                            HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                            HistoricVirtualTNP3EndOfTheYearInput = 14m,
                            HistoricVirtualTNP4EndOfTheYearInput = 15m,
                            HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                            LatestInYear = true,
                            LearnRefNumber = "16",
                            ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                            ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                            ProgType = 17,
                            PwayCode = 18,
                            STDCode = 19,
                            TotalProgAimPaymentsInTheYear = 20m,
                            UptoEndDate = new DateTime(2018, 8, 1),
                            UKPRN = 123456,
                            ULN = 1234567890,
                        },
                        new AECEarningsHistory()
                        {
                            AppIdentifier = "1.2",
                            AppProgCompletedInTheYearInput = true,
                            CollectionYear = "2",
                            CollectionReturnCode = "3",
                            DaysInYear = 4,
                            FworkCode = 5,
                            HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                            HistoricEmpIdEndWithinYear = 6,
                            HistoricEmpIdStartWithinYear = 7,
                            HistoricLearner1618StartInput = true,
                            HistoricPMRAmount = 8m,
                            HistoricTNP1Input = 9m,
                            HistoricTNP2Input = 10m,
                            HistoricTNP3Input = 11m,
                            HistoricTNP4Input = 12m,
                            HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                            HistoricVirtualTNP3EndOfTheYearInput = 14m,
                            HistoricVirtualTNP4EndOfTheYearInput = 15m,
                            HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                            LatestInYear = true,
                            LearnRefNumber = "16",
                            ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                            ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                            ProgType = 17,
                            PwayCode = 18,
                            STDCode = 19,
                            TotalProgAimPaymentsInTheYear = 20m,
                            UptoEndDate = new DateTime(2018, 8, 1),
                            UKPRN = 123456,
                            ULN = 1234567890,
                        }
                    }
                },
                {
                    1234567898,
                    new List<AECEarningsHistory>
                    {
                        new AECEarningsHistory()
                        {
                            AppIdentifier = "1",
                            AppProgCompletedInTheYearInput = true,
                            CollectionYear = "2",
                            CollectionReturnCode = "3",
                            DaysInYear = 4,
                            FworkCode = 5,
                            HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                            HistoricEmpIdEndWithinYear = 6,
                            HistoricEmpIdStartWithinYear = 7,
                            HistoricLearner1618StartInput = true,
                            HistoricPMRAmount = 8m,
                            HistoricTNP1Input = 9m,
                            HistoricTNP2Input = 10m,
                            HistoricTNP3Input = 11m,
                            HistoricTNP4Input = 12m,
                            HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                            HistoricVirtualTNP3EndOfTheYearInput = 14m,
                            HistoricVirtualTNP4EndOfTheYearInput = 15m,
                            HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                            LatestInYear = true,
                            LearnRefNumber = "16",
                            ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                            ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                            ProgType = 17,
                            PwayCode = 18,
                            STDCode = 19,
                            TotalProgAimPaymentsInTheYear = 20m,
                            UptoEndDate = new DateTime(2018, 8, 1),
                            UKPRN = 123456,
                            ULN = 1234567898,
                        }
                    }
                },
                {
                    1234567899,
                    new List<AECEarningsHistory>
                    {
                        new AECEarningsHistory()
                        {
                            AppIdentifier = "1",
                            AppProgCompletedInTheYearInput = true,
                            CollectionYear = "2",
                            CollectionReturnCode = "3",
                            DaysInYear = 4,
                            FworkCode = 5,
                            HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                            HistoricEmpIdEndWithinYear = 6,
                            HistoricEmpIdStartWithinYear = 7,
                            HistoricLearner1618StartInput = true,
                            HistoricPMRAmount = 8m,
                            HistoricTNP1Input = 9m,
                            HistoricTNP2Input = 10m,
                            HistoricTNP3Input = 11m,
                            HistoricTNP4Input = 12m,
                            HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                            HistoricVirtualTNP3EndOfTheYearInput = 14m,
                            HistoricVirtualTNP4EndOfTheYearInput = 15m,
                            HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                            LatestInYear = true,
                            LearnRefNumber = "16",
                            ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                            ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                            ProgType = 17,
                            PwayCode = 18,
                            STDCode = 19,
                            TotalProgAimPaymentsInTheYear = 20m,
                            UptoEndDate = new DateTime(2018, 8, 1),
                            UKPRN = 123456,
                            ULN = 1234567899,
                        }
                    }
                }
            };
        }

        private AppsEarningsHistoryMapperService NewService()
        {
            return new AppsEarningsHistoryMapperService();
        }
    }
}
