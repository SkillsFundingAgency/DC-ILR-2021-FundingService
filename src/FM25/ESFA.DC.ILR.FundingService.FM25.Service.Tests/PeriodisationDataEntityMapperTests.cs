using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Input;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Tests
{
    public class PeriodisationDataEntityMapperTests
    {
        [Fact]
        public void BuildGlobal()
        {
            var ukprn = 1234;

            var global = new Global()
            {
                UKPRN = ukprn
            };

            var dataEntity = NewService().BuildGlobalDataEntity(global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes["UKPRN"].Value.Should().Be(ukprn);
        }

        private PeriodisationDataEntityMapper NewService()
        {
            return new PeriodisationDataEntityMapper();
        }
    }
}
