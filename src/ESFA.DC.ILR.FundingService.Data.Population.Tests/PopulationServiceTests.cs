using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests
{
    public class PopulationServiceTests
    {
        [Fact]
        public async Task Populate()
        {
            var internalDataCachePopulationServiceMock = new Mock<IInternalDataCachePopulationService>();
            var externalDataCachePopulationServiceMock = new Mock<IExternalDataCachePopulationService>();
            var fundingContextPopulationServiceMock = new Mock<IFundingContextPopulationService>();
            var fileDataCachePopulationServiceMock = new Mock<IFileDataCachePopulationService>();

            internalDataCachePopulationServiceMock.Setup(ps => ps.PopulateAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            externalDataCachePopulationServiceMock.Setup(ps => ps.PopulateAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            fundingContextPopulationServiceMock.Setup(ps => ps.PopulateAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            fileDataCachePopulationServiceMock.Setup(ps => ps.PopulateAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            await NewService(internalDataCachePopulationServiceMock.Object, externalDataCachePopulationServiceMock.Object, fundingContextPopulationServiceMock.Object, fileDataCachePopulationServiceMock.Object).PopulateAsync(CancellationToken.None);

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
