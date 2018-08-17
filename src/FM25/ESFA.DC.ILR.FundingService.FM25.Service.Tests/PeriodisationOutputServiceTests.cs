using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Tests
{
    public class PeriodisationOutputServiceTests
    {
        [Fact]
        public void MapGlobal()
        {
            var rulebaseVersion = "RulebaseVersion";
            var ukprn = 1234;

            var dataEntity = new DataEntity(null);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);

            var global = NewService(dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.RulebaseVersion.Should().Be(rulebaseVersion);
            global.UKPRN.Should().Be(ukprn);

            global.LearnerPeriods.Should().BeEmpty();
            global.LearnerPeriodisedValues.Should().BeEmpty();
        }

        [Fact]
        public void MapLearnerPeriodisedValues()
        {
            var attributeName = "AttributeName";
            var learnRefNumber = "LearnRefNumber";
            var period1 = 1.1m;
            var period2 = 1.2m;
            var period3 = 1.3m;
            var period4 = 1.4m;
            var period5 = 1.5m;
            var period6 = 1.6m;
            var period7 = 1.7m;
            var period8 = 1.8m;
            var period9 = 1.9m;
            var period10 = 1.10m;
            var period11 = 1.11m;
            var period12 = 1.12m;

            var dataEntity = new DataEntity(null);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "AttributeName")).Returns(attributeName);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnRefNumber")).Returns(learnRefNumber);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_1")).Returns(period1);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_2")).Returns(period2);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_3")).Returns(period3);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_4")).Returns(period4);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_5")).Returns(period5);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_6")).Returns(period6);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_7")).Returns(period7);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_8")).Returns(period8);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_9")).Returns(period9);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_10")).Returns(period10);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_11")).Returns(period11);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Period_12")).Returns(period12);

            var learnerPeriodisedValues = NewService(dataEntityAttributeServiceMock.Object).MapLearnerPeriodisedValues(dataEntity);

            learnerPeriodisedValues.AttributeName.Should().Be(attributeName);
            learnerPeriodisedValues.LearnRefNumber.Should().Be(learnRefNumber);
            learnerPeriodisedValues.Period1.Should().Be(period1);
            learnerPeriodisedValues.Period2.Should().Be(period2);
            learnerPeriodisedValues.Period3.Should().Be(period3);
            learnerPeriodisedValues.Period4.Should().Be(period4);
            learnerPeriodisedValues.Period5.Should().Be(period5);
            learnerPeriodisedValues.Period6.Should().Be(period6);
            learnerPeriodisedValues.Period7.Should().Be(period7);
            learnerPeriodisedValues.Period8.Should().Be(period8);
            learnerPeriodisedValues.Period9.Should().Be(period9);
            learnerPeriodisedValues.Period10.Should().Be(period10);
            learnerPeriodisedValues.Period11.Should().Be(period11);
            learnerPeriodisedValues.Period12.Should().Be(period12);
        }

        private PeriodisationOutputService NewService(IDataEntityAttributeService dataEntityAttributeService = null)
        {
            return new PeriodisationOutputService(dataEntityAttributeService);
        }
    }
}
