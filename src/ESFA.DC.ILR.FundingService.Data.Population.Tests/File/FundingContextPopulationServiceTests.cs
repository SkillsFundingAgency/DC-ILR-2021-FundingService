using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.File;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.File
{
    public class FileDataCachePopulationServiceTests
    {
        [Fact]
        public void Populate()
        {
            var ukprn = 1234;
            
            var ukprnDataRetrievalServiceMock = new Mock<IUKPRNDataRetrievalService>();
            
            ukprnDataRetrievalServiceMock.Setup(rds => rds.Retrieve()).Returns(ukprn).Verifiable();
            
            var fundingContext = new Mock<FileDataCache>();

            fundingContext.SetupSet(fc => fc.UKPRN = ukprn).Verifiable();

            NewService(fundingContext.Object, ukprnDataRetrievalServiceMock.Object).Populate();

            fundingContext.Verify();
            ukprnDataRetrievalServiceMock.Verify();
        }

        private FileDataCachePopulationService NewService(IFileDataCache fileDataCache = null, IUKPRNDataRetrievalService ukprnDataRetrievalService = null)
        {
            return new FileDataCachePopulationService(fileDataCache, ukprnDataRetrievalService);
        }
    }
}
