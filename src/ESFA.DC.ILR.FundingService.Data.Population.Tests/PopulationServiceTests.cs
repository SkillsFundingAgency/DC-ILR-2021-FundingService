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
            var internalDataCachePopulationServiceMock = new Mock<IInternalDataCachePopulationService>();
            var externalDataCachePopulationServiceMock = new Mock<IExternalDataCachePopulationService>();
            var fundingContextPopulationServiceMock = new Mock<IFundingContextPopulationService>();
            var fileDataCachePopulationServiceMock = new Mock<IFileDataCachePopulationService>();

            internalDataCachePopulationServiceMock.Setup(ps => ps.Populate()).Verifiable();
            externalDataCachePopulationServiceMock.Setup(ps => ps.Populate()).Verifiable();
            fundingContextPopulationServiceMock.Setup(ps => ps.Populate()).Verifiable();
            fileDataCachePopulationServiceMock.Setup(ps => ps.Populate()).Verifiable();

            NewService(internalDataCachePopulationServiceMock.Object, externalDataCachePopulationServiceMock.Object, fundingContextPopulationServiceMock.Object, fileDataCachePopulationServiceMock.Object).Populate();

            internalDataCachePopulationServiceMock.Verify();
            externalDataCachePopulationServiceMock.Verify();
            fundingContextPopulationServiceMock.Verify();
            fileDataCachePopulationServiceMock.Verify();
        }

        private PopulationService NewService(IInternalDataCachePopulationService internalDataCachePopulationService, IExternalDataCachePopulationService externalDataCachePopulationService = null, IFundingContextPopulationService fundingContextPopulationService = null, IFileDataCachePopulationService fileDataCachePopulationService = null)
        {
            return new PopulationService(internalDataCachePopulationService, externalDataCachePopulationService, fundingContextPopulationService, fileDataCachePopulationService);
        }
    }
}
