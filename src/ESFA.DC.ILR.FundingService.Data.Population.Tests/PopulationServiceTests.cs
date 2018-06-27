using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests
{
    public class PopulationServiceTests
    {
        [Fact]
        public void Populate()
        {
            var externalDataCachePopulationServiceMock = new Mock<IExternalDataCachePopulationService>();
            var fundingContextPopulationServiceMock = new Mock<IFundingContextPopulationService>();
            var fileDataCachePopulationServiceMock = new Mock<IFileDataCachePopulationService>();

            externalDataCachePopulationServiceMock.Setup(ps => ps.Populate()).Verifiable();
            fundingContextPopulationServiceMock.Setup(ps => ps.Populate()).Verifiable();
            fileDataCachePopulationServiceMock.Setup(ps => ps.Populate()).Verifiable();

            NewService(externalDataCachePopulationServiceMock.Object, fundingContextPopulationServiceMock.Object, fileDataCachePopulationServiceMock.Object).Populate();

            externalDataCachePopulationServiceMock.Verify();
            fundingContextPopulationServiceMock.Verify();
            fileDataCachePopulationServiceMock.Verify();
        }

        private PopulationService NewService(IExternalDataCachePopulationService externalDataCachePopulationService = null, IFundingContextPopulationService fundingContextPopulationService = null, IFileDataCachePopulationService fileDataCachePopulationService = null)
        {
            return new PopulationService(externalDataCachePopulationService, fundingContextPopulationService, fileDataCachePopulationService);
        }
    }
}
