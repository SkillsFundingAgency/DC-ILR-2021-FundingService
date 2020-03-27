using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Tests
{
    public class FM25PeriodisationFundingServiceTests
    {
        [Fact]
        public void ProcessFunding()
        {
            var ukprn = 123456789;

            var fm25Globals = new List<FM25Global>()
            {
                new FM25Global()
                {
                    Learners = new List<FM25Learner>()
                    {
                        new FM25Learner()
                        {
                            LearnRefNumber = "999999999"
                        }
                    }
                }
            };

            var learner = fm25Globals.Single().Learners.FirstOrDefault();
            var periodValues = new decimal[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

            var periodisationServiceMock = new Mock<IPeriodisationService>();
            periodisationServiceMock.Setup(psm => psm.GetPeriodisedValues(learner)).Returns(periodValues);

            var result = NewService(periodisationServiceMock.Object).ProcessFunding(ukprn, fm25Globals, CancellationToken.None);

            result.Count().Should().Be(1);
            result.Single().LearnerPeriodisedValues.Single().LearnRefNumber.Should().Be("999999999");
            result.Single().LearnerPeriodisedValues.Single().Period1.Should().Be(1);
            result.Single().LearnerPeriodisedValues.Single().Period2.Should().Be(2);
            result.Single().LearnerPeriodisedValues.Single().Period3.Should().Be(3);
            result.Single().LearnerPeriodisedValues.Single().Period4.Should().Be(4);
            result.Single().LearnerPeriodisedValues.Single().Period5.Should().Be(5);
            result.Single().LearnerPeriodisedValues.Single().Period6.Should().Be(6);
            result.Single().LearnerPeriodisedValues.Single().Period7.Should().Be(7);
            result.Single().LearnerPeriodisedValues.Single().Period8.Should().Be(8);
            result.Single().LearnerPeriodisedValues.Single().Period9.Should().Be(9);
            result.Single().LearnerPeriodisedValues.Single().Period10.Should().Be(10);
            result.Single().LearnerPeriodisedValues.Single().Period11.Should().Be(11);
            result.Single().LearnerPeriodisedValues.Single().Period12.Should().Be(12);
        }

        private FM25PeriodisationFundingService NewService(IPeriodisationService periodisationService = null)
        {
            return new FM25PeriodisationFundingService(periodisationService);
        }
    }
}
