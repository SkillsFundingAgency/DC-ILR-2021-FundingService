using ESFA.DC.ILR.FundingService.Data.Population.File;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.File
{
    public class UKPRNDataRetrievalServiceTests
    {
        [Fact]
        public void Retrieve()
        {
            var ukprn = 1234;

            var message = new TestMessage()
            {
                LearningProviderEntity = new TestLearningProvider()
                {
                    UKPRN = ukprn
                },
            };

            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(message);

            NewService(fundingServiceDto.Object).Retrieve().Should().Be(ukprn);
        }

        private UKPRNDataRetrievalService NewService(IFundingServiceDto fundingServiceDto = null)
        {
            return new UKPRNDataRetrievalService(fundingServiceDto);
        }
    }
}
