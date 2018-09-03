using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.FundingService.Stubs.Database.Interface;
using ESFA.DC.ILR.FundingService.Stubs.Database.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class AppsEarningsHistoryDataRetrievalServiceTests
    {
        [Fact]
        public void AppEarningsHistory()
        {
            var appsHistoryMock = new Mock<IAppsEarningsHistoryStub>();

            var appsHistory = NewService(appsHistoryMock.Object).AecLatestInYearHistory;

            appsHistoryMock.VerifyGet(a => a.AEC_LatestInYearHistories);
        }

        [Fact]
        public void AppsEarningsHistoryForLearners()
        {
            var aec_Histories = new List<AEC_LatestInYearHistory>()
            {
                new AEC_LatestInYearHistory()
                {
                    LearnRefNumber = "123",
                    ULN = 1234567890,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Output = 1
                },
                new AEC_LatestInYearHistory()
                {
                    LearnRefNumber = "123",
                    ULN = 1234567890,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Output = 2
                },
                new AEC_LatestInYearHistory()
                {
                    LearnRefNumber = "123",
                    ULN = 12345678,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Output = 2
                },
                new AEC_LatestInYearHistory()
                {
                    LearnRefNumber = "456",
                    ULN = 1234567899,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Output = 1
                },
                new AEC_LatestInYearHistory()
                {
                    LearnRefNumber = "456",
                    ULN = 1234567890,
                    UKPRN = 1234,
                    LatestInYear = true,
                    HistoricTNP1Output = 1
                },
                new AEC_LatestInYearHistory()
                {
                    LearnRefNumber = "456",
                    ULN = 1234567890,
                    UKPRN = 123456,
                    LatestInYear = false,
                    HistoricTNP1Output = 1
                },
                new AEC_LatestInYearHistory(),
            }.AsQueryable();

            var appsEarningHistoryDataRetrievalServiceMock = NewMock();

            appsEarningHistoryDataRetrievalServiceMock.SetupGet(a => a.AecLatestInYearHistory).Returns(aec_Histories);

            var ukprn = 123456;
            var learners = new List<LearnRefNumberULNKey> { new LearnRefNumberULNKey("123", 1234567890), new LearnRefNumberULNKey("456", 1234567899) };

            var aecHistories = appsEarningHistoryDataRetrievalServiceMock.Object.AppsEarningsHistoryForLearners(ukprn, learners);

            aecHistories.Should().HaveCount(2);
            aecHistories.Should().ContainKeys(1234567890, 1234567899);
            aecHistories.SelectMany(v => v.Value).Should().HaveCount(3);
            aecHistories[1234567890].Should().HaveCount(2);
            aecHistories[1234567899].Should().HaveCount(1);
        }

        [Fact]
        public void AECLatestInYearEarningsFromEntity()
        {
            var aec_LatestInYearHistory = new AEC_LatestInYearHistory()
            {
                AppIdentifier = "1",
                AppProgCompletedInTheYearOutput = true,
                CollectionYear = "2",
                CollectionReturnCode = "3",
                DaysInYear = 4,
                FworkCode = 5,
                HistoricEffectiveTNPStartDateOutput = new DateTime(2018, 8, 1),
                HistoricEmpIdEndWithinYear = 6,
                HistoricEmpIdStartWithinYear = 7,
                HistoricLearner1618StartOutput = true,
                HistoricPMRAmount = 8m,
                HistoricTNP1Output = 9m,
                HistoricTNP2Output = 10m,
                HistoricTNP3Output = 11m,
                HistoricTNP4Output = 12m,
                HistoricTotal1618 = 13m,
                HistoricVirtualTNP3EndOfTheYearOutput = 14m,
                HistoricVirtualTNP4EndOfTheYearOutput = 15m,
                HistoricLearnDelProgEarliestACT2DateOutput = new DateTime(2018, 8, 1),
                LatestInYear = true,
                LearnRefNumber = "16",
                ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                ProgType = 17,
                PwayCode = 18,
                STDCode = 19,
                TotalProgAimPaymentsInTheYear = 20m,
                UptoEndDate = new DateTime(2018, 8, 1),
                UKPRN = 21,
                ULN = 22,
            };

            var aecLatestInYearHistory = NewService().AECLatestInYearEarningsFromEntity(aec_LatestInYearHistory);

            aecLatestInYearHistory.AppIdentifier.Should().Be(aec_LatestInYearHistory.AppIdentifier);
            aecLatestInYearHistory.AppProgCompletedInTheYearInput.Should().Be(aec_LatestInYearHistory.AppProgCompletedInTheYearOutput);
            aecLatestInYearHistory.CollectionYear.Should().Be(aec_LatestInYearHistory.CollectionYear);
            aecLatestInYearHistory.CollectionReturnCode.Should().Be(aec_LatestInYearHistory.CollectionReturnCode);
            aecLatestInYearHistory.DaysInYear.Should().Be(aec_LatestInYearHistory.DaysInYear);
            aecLatestInYearHistory.FworkCode.Should().Be(aec_LatestInYearHistory.FworkCode);
            aecLatestInYearHistory.HistoricEffectiveTNPStartDateInput.Should().Be(aec_LatestInYearHistory.HistoricEffectiveTNPStartDateOutput);
            aecLatestInYearHistory.HistoricEmpIdEndWithinYear.Should().Be(aec_LatestInYearHistory.HistoricEmpIdEndWithinYear);
            aecLatestInYearHistory.HistoricEmpIdStartWithinYear.Should().Be(aec_LatestInYearHistory.HistoricEmpIdStartWithinYear);
            aecLatestInYearHistory.HistoricLearner1618StartInput.Should().Be(aec_LatestInYearHistory.HistoricLearner1618StartOutput);
            aecLatestInYearHistory.HistoricPMRAmount.Should().Be(aec_LatestInYearHistory.HistoricPMRAmount);
            aecLatestInYearHistory.HistoricTNP1Input.Should().Be(aec_LatestInYearHistory.HistoricTNP1Output);
            aecLatestInYearHistory.HistoricTNP2Input.Should().Be(aec_LatestInYearHistory.HistoricTNP2Output);
            aecLatestInYearHistory.HistoricTNP3Input.Should().Be(aec_LatestInYearHistory.HistoricTNP3Output);
            aecLatestInYearHistory.HistoricTNP4Input.Should().Be(aec_LatestInYearHistory.HistoricTNP4Output);
            aecLatestInYearHistory.HistoricTotal1618.Should().Be(aec_LatestInYearHistory.HistoricTotal1618);
            aecLatestInYearHistory.HistoricVirtualTNP3EndOfTheYearInput.Should().Be(aec_LatestInYearHistory.HistoricVirtualTNP3EndOfTheYearOutput);
            aecLatestInYearHistory.HistoricVirtualTNP4EndOfTheYearInput.Should().Be(aec_LatestInYearHistory.HistoricVirtualTNP4EndOfTheYearOutput);
            aecLatestInYearHistory.HistoricLearnDelProgEarliestACT2DateInput.Should().Be(aec_LatestInYearHistory.HistoricLearnDelProgEarliestACT2DateOutput);
            aecLatestInYearHistory.LatestInYear.Should().Be(aec_LatestInYearHistory.LatestInYear);
            aecLatestInYearHistory.LearnRefNumber.Should().Be(aec_LatestInYearHistory.LearnRefNumber);
            aecLatestInYearHistory.ProgrammeStartDateIgnorePathway.Should().Be(aec_LatestInYearHistory.ProgrammeStartDateIgnorePathway);
            aecLatestInYearHistory.ProgrammeStartDateMatchPathway.Should().Be(aec_LatestInYearHistory.ProgrammeStartDateMatchPathway);
            aecLatestInYearHistory.ProgType.Should().Be(aec_LatestInYearHistory.ProgType);
            aecLatestInYearHistory.PwayCode.Should().Be(aec_LatestInYearHistory.PwayCode);
            aecLatestInYearHistory.STDCode.Should().Be(aec_LatestInYearHistory.STDCode);
            aecLatestInYearHistory.TotalProgAimPaymentsInTheYear.Should().Be(aec_LatestInYearHistory.STDCode);
            aecLatestInYearHistory.UptoEndDate.Should().Be(aec_LatestInYearHistory.UptoEndDate);
            aecLatestInYearHistory.UKPRN.Should().Be(aec_LatestInYearHistory.UKPRN);
            aecLatestInYearHistory.ULN.Should().Be(aec_LatestInYearHistory.ULN);
        }

        private AppsEarningsHistoryDataRetrievalService NewService(IAppsEarningsHistoryStub appsHistory = null)
        {
            return new AppsEarningsHistoryDataRetrievalService(appsHistory);
        }

        private Mock<AppsEarningsHistoryDataRetrievalService> NewMock()
        {
            return new Mock<AppsEarningsHistoryDataRetrievalService>();
        }
    }
}
