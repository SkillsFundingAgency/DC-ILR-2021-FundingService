using FluentAssertions;
using System;
using Xunit;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using Moq;


namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Tests
{
    public class PeriodisationDateServiceTests
    {

        [Theory]
        [InlineData("2019-07-01","2020-08-01", "19+ Traineeships (Adult Funded)", true)]
        [InlineData("2020-08-01", "2020-08-01", "16-18 Traineeships (Adult Funded)", true)]
        [InlineData("2020-12-01", "2020-12-01", "19+ Traineeships (Adult Funded)", true)]
        [InlineData("2021-12-01", "2021-12-01", "16-18 Traineeships (Adult Funded)", true)]
        [InlineData("2019-07-01", "2020-08-01", "Apprenticeship", false)]
        [InlineData("2020-08-01", "2020-08-01", "", false)]
        [InlineData("2020-12-01", "2020-08-01", "Trailblazer", false)]
        [InlineData("2021-12-01", "2020-08-01", "16-18 AEB Learner", false)]
        public void GetPeriodisationStartDateTest(string learnStartDate, string expectedResult, string fundLine, bool expectedMock)
        {
            var learner = new FM25Learner();
            learner.LearnerStartDate = DateTime.Parse(learnStartDate);
            learner.FundLine = fundLine;

            var periodisationServiceMock = new Mock<IPeriodisationService>();
            periodisationServiceMock.Setup(pds => pds.IsLearnerTrainee(learner)).Returns(expectedMock);

            PeriodisationDateService(periodisationServiceMock.Object).GetPeriodisationStartDate(learner).Should().Be(DateTime.Parse(expectedResult));
        }

        [Theory]
        [InlineData(true, "2019-07-01", "2019-08-01", "2019-08-01")]
        [InlineData(true, "2019-08-01", "2020-08-01", "2020-08-01")]
        [InlineData(true, "2020-12-01", null, "2020-12-01")]
        [InlineData(true, "2021-12-01", null, "2021-07-31")]
        [InlineData(true, "2018-12-01", "2020-10-01", "2020-10-01")]
        [InlineData(true, "2018-12-01", null, "2021-07-31")]
        [InlineData(false, "2019-07-01", "2019-08-01", "2021-07-31")]
        [InlineData(false, "2019-08-01", "2020-08-01", "2021-07-31")]
        [InlineData(false, "2020-12-01", null, "2021-07-31")]
        [InlineData(false, "2021-12-01", null, "2021-07-31")]
        [InlineData(false, "2018-12-01", "2020-10-01", "2021-07-31")]
        [InlineData(false, "2018-12-01", null, "2021-07-31")]
        public void GetPeriodisationEndDateTest(bool learnerIsTrainee, string learnPlannedEndDate, string learnActEndDate, string expectedResult)
        {
            var learner = new FM25Learner();
            learner.LearnerPlanEndDate = DateTime.Parse(learnPlannedEndDate);
            learner.LearnerActEndDate = (learnActEndDate==null)?(DateTime?)null:DateTime.Parse(learnActEndDate);
            PeriodisationDateService().GetPeriodisationEndDate(learner, learnerIsTrainee).Should().Be(DateTime.Parse(expectedResult));
        }

        [Theory]
        [InlineData("2020-08-01", "2020-08-01", 1)]
        [InlineData("2020-08-01", "2020-09-15", 2)]
        [InlineData("2021-02-02", "2021-04-10", 3)]
        [InlineData("2020-10-03", "2021-01-31", 4)]
        [InlineData("2020-11-01", "2021-03-01", 5)]
        [InlineData("2020-12-18", "2021-05-31", 6)]
        [InlineData("2020-11-01", "2021-05-31", 7)]
        [InlineData("2020-10-09", "2021-05-14", 8)]
        [InlineData("2020-10-09", "2021-06-30", 9)]
        [InlineData("2020-10-01", "2021-07-31", 10)]
        [InlineData("2020-09-01", "2021-07-31", 11)]
        [InlineData("2020-08-01", "2021-07-31", 12)]
        [InlineData("2020-09-15", "2020-09-16", 1)]
        public void GetPeriodsInLearningTest(string periodisationStartDate, string periodisationEndDate, int expectedResult)
        {
            PeriodisationDateService().GetMonthsBetweenDatesIgnoringDaysInclusive(DateTime.Parse(periodisationStartDate), DateTime.Parse(periodisationEndDate)).Should().Be(expectedResult);
        }


        [Theory]
        [InlineData("2020-08-01", 1)]
        [InlineData("2020-09-04", 2)]
        [InlineData("2021-10-02", 3)]
        [InlineData("2020-11-03", 4)]
        [InlineData("2020-12-01", 5)]
        [InlineData("2020-01-18", 6)]
        [InlineData("2020-02-01", 7)]
        [InlineData("2020-03-09", 8)]
        [InlineData("2020-04-09", 9)]
        [InlineData("2020-05-01", 10)]
        [InlineData("2020-06-01", 11)]
        [InlineData("2020-07-01", 12)]
        public void PeriodFromDateTest(string periodisationStartDate, int expectedResult)
        {
            PeriodisationDateService().PeriodFromDate(DateTime.Parse(periodisationStartDate)).Should().Be(expectedResult);
        }

        private PeriodisationDateService PeriodisationDateService(IPeriodisationService periodisationService = null)
        {
            return new PeriodisationDateService(periodisationService);
        }

    }
}
