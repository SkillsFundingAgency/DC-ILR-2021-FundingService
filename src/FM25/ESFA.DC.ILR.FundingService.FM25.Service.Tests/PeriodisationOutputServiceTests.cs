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

        private PeriodisationOutputService NewService(IDataEntityAttributeService dataEntityAttributeService = null)
        {
            return new PeriodisationOutputService(dataEntityAttributeService);
        }
    }
}
