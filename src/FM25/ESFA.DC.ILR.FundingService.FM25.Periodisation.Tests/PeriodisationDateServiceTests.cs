using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Constants;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;


namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Tests
{
    public class PeriodisationDateServiceTests
    {
        // Learner is Trainee Tests
        [Theory]
        [InlineData("19+ Traineeships (Adult Funded)")]
        [InlineData("16-18 Traineeships (Adult Funded)")]
        public void IsTraineeTrueLiterals(string fundLine)
        {
            NewService().IsLearnerTrainee(fundLine).Should().BeTrue();
        }

        [Theory]
        [InlineData(FundingLineConstants.traineeship1618)]
        [InlineData(FundingLineConstants.traineeship19Plus)]
        public void IsTraineeTrueConstants(string fundLine)
        {
            NewService().IsLearnerTrainee(fundLine).Should().BeTrue();
        }

        [Fact]
        public void IsTraineeFalse()
        {
            NewService().IsLearnerTrainee("NotAnApprentice").Should().BeFalse();
        }

        // Periodisation Start Date Test
        [Theory]
        [InlineData("2018-07-01","2019-08-01", "19+ Traineeships (Adult Funded)")]
        [InlineData("2019-08-01", "2019-08-01", "16-18 Traineeships (Adult Funded)")]
        [InlineData("2019-12-01", "2019-12-01", "19+ Traineeships (Adult Funded)")]
        [InlineData("2020-12-01", "2020-12-01", "16-18 Traineeships (Adult Funded)")]
        [InlineData("2018-07-01", "2019-08-01", "Apprenticeship")]
        [InlineData("2019-08-01", "2019-08-01", "")]
        [InlineData("2019-12-01", "2019-08-01", "Trailblazer")]
        [InlineData("2020-12-01", "2019-08-01", "16-18 AEB Learner")]
        public void GetPeriodisationStartDateTest(string learnStartDate, string expectedResult, string fundLine)
        {
            var learner = new FM25Learner();
            learner.LearnerStartDate = DateTime.Parse(learnStartDate);
            learner.FundLine = fundLine;
            NewService().GetPeriodisationStartDate(learner).Should().Be(DateTime.Parse(expectedResult));
        }

        // Periodisation End Date Test
        [Theory]
        [InlineData(true, "2018-07-01", "2018-08-01", "2018-08-01")]
        [InlineData(true, "2018-08-01", "2019-08-01", "2019-08-01")]
        [InlineData(true, "2019-12-01", null, "2019-12-01")]
        [InlineData(true, "2020-12-01", null, "2020-07-31")]
        [InlineData(true, "2017-12-01", "2019-10-01", "2019-10-01")]
        [InlineData(true, "2017-12-01", null, "2020-07-31")]
        [InlineData(false, "2018-07-01", "2018-08-01", "2020-07-31")]
        [InlineData(false, "2018-08-01", "2019-08-01", "2020-07-31")]
        [InlineData(false, "2019-12-01", null, "2020-07-31")]
        [InlineData(false, "2020-12-01", null, "2020-07-31")]
        [InlineData(false, "2017-12-01", "2019-10-01", "2020-07-31")]
        [InlineData(false, "2017-12-01", null, "2020-07-31")]
        public void GetPeriodisationEndDateTest(bool learnerIsTrainee, string learnPlannedEndDate, string learnActEndDate, string expectedResult)
        {
            var learner = new FM25Learner();
            learner.LearnerPlanEndDate = DateTime.Parse(learnPlannedEndDate);
            learner.LearnerActEndDate = (learnActEndDate==null)?(DateTime?)null:DateTime.Parse(learnActEndDate);
            NewService().GetPeriodisationEndDate(learner, learnerIsTrainee).Should().Be(DateTime.Parse(expectedResult));
        }

        // Periods In Learning Test
        [Theory]
        [InlineData("2019-08-01", "2019-08-01", 1)]
        [InlineData("2019-08-01", "2019-09-15", 2)]
        [InlineData("2020-02-02", "2020-04-10", 3)]
        [InlineData("2019-10-03", "2020-01-31", 4)]
        [InlineData("2019-11-01", "2020-03-01", 5)]
        [InlineData("2019-12-18", "2020-05-31", 6)]
        [InlineData("2019-11-01", "2020-05-31", 7)]
        [InlineData("2019-10-09", "2020-05-14", 8)]
        [InlineData("2019-10-09", "2020-06-30", 9)]
        [InlineData("2019-10-01", "2020-07-31", 10)]
        [InlineData("2019-09-01", "2020-07-31", 11)]
        [InlineData("2019-08-01", "2020-07-31", 12)]
        [InlineData("2019-09-15", "2019-09-16", 1)]
        public void GetPeriodsInLearningTest(string periodisationStartDate, string periodisationEndDate, int expectedResult)
        {
            NewService().GetMonthsBetweenDatesIgnoringDaysInclusive(DateTime.Parse(periodisationStartDate), DateTime.Parse(periodisationEndDate)).Should().Be(expectedResult);
        }

        // Get Periodised Values Test
        [Theory]
        [InlineData("2019-08-01", "2020-07-31", "2020-07-31", "16-18 Traineeships (Adult Funded)", 72, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6)]
        [InlineData("2019-09-15", "2019-10-16", "2019-10-16", "16-18 Traineeships (Adult Funded)", 84, 0, 42, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData("2018-09-15", "2022-10-16", "2022-10-16", "16-18 Traineeships (Adult Funded)", 84, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7)]
        [InlineData("2020-07-31", "2022-10-16", "2022-10-16", "16-18 Traineeships (Adult Funded)", 84, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 84)]
        [InlineData("2019-08-01", "2020-07-31", "2020-07-31", "19+ Traineeships (Adult Funded)", 72, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6)]
        [InlineData("2019-09-15", "2019-10-16", "2019-10-16", "19+ Traineeships (Adult Funded)", 84, 0, 42, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData("2018-09-15", "2022-10-16", "2022-10-16", "19+ Traineeships (Adult Funded)", 84, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7)]
        [InlineData("2020-07-31", "2022-10-16", "2022-10-16", "19+ Traineeships (Adult Funded)", 84, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 84)]
        [InlineData("2018-09-15", "2022-10-16", "2022-10-16", "16-18 Apprenticeship", 84, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        public void GetPeriodisedValuesTestFullMonths(string learnerStartDate, string learnerPlannedEndDate, string learnerActEndDate, string fundLine, decimal totalFunding, decimal p1, decimal p2, decimal p3, decimal p4, decimal p5, decimal p6, decimal p7, decimal p8, decimal p9, decimal p10, decimal p11, decimal p12)
        {
            var learner = new FM25Learner()
            {
                LearnerStartDate = DateTime.Parse(learnerStartDate),
                LearnerPlanEndDate = DateTime.Parse(learnerPlannedEndDate),
                LearnerActEndDate = DateTime.Parse(learnerActEndDate),
                OnProgPayment = totalFunding,
                FundLine = fundLine
            };

            var result = NewService().GetPeriodisedValues(learner);
            result.Period1.Should().Be(p1);
            result.Period2.Should().Be(p2);
            result.Period3.Should().Be(p3);
            result.Period4.Should().Be(p4);
            result.Period5.Should().Be(p5);
            result.Period6.Should().Be(p6);
            result.Period7.Should().Be(p7);
            result.Period8.Should().Be(p8);
            result.Period9.Should().Be(p9);
            result.Period10.Should().Be(p10);
            result.Period11.Should().Be(p11);
            result.Period12.Should().Be(p12);
        }





        private PeriodisationDateService NewService()
        {
            return new PeriodisationDateService();
        }
    }
}
