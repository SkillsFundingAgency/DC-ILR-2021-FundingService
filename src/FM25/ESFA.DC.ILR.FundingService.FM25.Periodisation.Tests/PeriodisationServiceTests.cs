using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using System;
using FluentAssertions;
using Xunit;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Constants;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Tests
{
    public class PeriodisationServiceTests
    {
        // Get Periodised Values Test
        [Theory]
        //[InlineData("2019-08-01", "2020-07-31", "2020-07-31", "16-18 Traineeships (Adult Funded)", 72, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6)]
        [InlineData("2019-09-15", "2019-10-16", "2019-10-16", "16-18 Traineeships (Adult Funded)", 84, 0, 42, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        //[InlineData("2018-09-15", "2022-10-16", "2022-10-16", "16-18 Traineeships (Adult Funded)", 84, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7)]
        //[InlineData("2020-07-31", "2022-10-16", "2022-10-16", "16-18 Traineeships (Adult Funded)", 84, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 84)]
        //[InlineData("2019-08-01", "2020-07-31", "2020-07-31", "19+ Traineeships (Adult Funded)", 72, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6)]
        //[InlineData("2019-09-15", "2019-10-16", "2019-10-16", "19+ Traineeships (Adult Funded)", 84, 0, 42, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
        //[InlineData("2018-09-15", "2022-10-16", "2022-10-16", "19+ Traineeships (Adult Funded)", 84, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7)]
        //[InlineData("2020-07-31", "2022-10-16", "2022-10-16", "19+ Traineeships (Adult Funded)", 84, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 84)]
        //[InlineData("2018-09-15", "2022-10-16", "2022-10-16", "16-18 Apprenticeship", 84, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
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

            var result = PeriodisationService().GetPeriodisedValues(learner);
            result[0].Should().Be(p1);
            result[1].Should().Be(p2);
            result[2].Should().Be(p3);
            result[3].Should().Be(p4);
            result[4].Should().Be(p5);
            result[5].Should().Be(p6);
            result[6].Should().Be(p7);
            result[7].Should().Be(p8);
            result[8].Should().Be(p9);
            result[9].Should().Be(p10);
            result[10].Should().Be(p11);
            result[11].Should().Be(p12);
        }

        // Learner is Trainee Tests
        [Theory]
        [InlineData("19+ Traineeships (Adult Funded)")]
        [InlineData("16-18 Traineeships (Adult Funded)")]
        public void IsTraineeTrueLiterals(string fundLine)
        {
            PeriodisationService().IsLearnerTrainee(fundLine).Should().BeTrue();
        }

        [Theory]
        [InlineData(FundingLineConstants.Traineeship1618)]
        [InlineData(FundingLineConstants.Traineeship19Plus)]
        public void IsTraineeTrueConstants(string fundLine)
        {
            PeriodisationService().IsLearnerTrainee(fundLine).Should().BeTrue();
        }

        [Fact]
        public void IsTraineeFalse()
        {
            PeriodisationService().IsLearnerTrainee("NotAnApprentice").Should().BeFalse();
        }

        [Fact]
        public void GetMonthlyValuesTest()
        {
            var monthlyValues = PeriodisationService().GetMonthlyValues();
            monthlyValues[0].Should().Be(0M);
            monthlyValues[1].Should().Be(0M);
            monthlyValues[2].Should().Be(0M);
            monthlyValues[3].Should().Be(0M);
            monthlyValues[4].Should().Be(0M);
            monthlyValues[5].Should().Be(0M);
            monthlyValues[6].Should().Be(0M);
            monthlyValues[7].Should().Be(0M);
            monthlyValues[8].Should().Be(0M);
            monthlyValues[9].Should().Be(0M);
            monthlyValues[10].Should().Be(0M);
            monthlyValues[11].Should().Be(0M);
        }

        public void GetMonthlyValuesSizeTest()
        {
            PeriodisationService().GetMonthlyValues().IsFixedSize.Should().BeTrue();
            PeriodisationService().GetMonthlyValues().Length.Should().Be(12);
        }


        private PeriodisationService PeriodisationService()
        {
            return new PeriodisationService();
        }
    }
}
