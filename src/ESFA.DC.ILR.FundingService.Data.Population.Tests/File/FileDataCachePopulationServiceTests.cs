using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.File;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.File
{
    public class FileDataCachePopulationServiceTests
    {
        [Fact]
        public async Task Populate()
        {
            var ukprn = 1234;
            var dpOutcomes = new Dictionary<string, IEnumerable<DPOutcome>>();

            var fileDataRetrievalServiceMock = new Mock<IFileDataRetrievalService>();

            fileDataRetrievalServiceMock.Setup(rds => rds.RetrieveUKPRN()).Returns(ukprn);
            fileDataRetrievalServiceMock.Setup(rds => rds.RetrieveDPOutcomes()).Returns(dpOutcomes);

            var fileDataCache = new FileDataCache();

            await NewService(fileDataCache, fileDataRetrievalServiceMock.Object).PopulateAsync(CancellationToken.None);

            fileDataCache.UKPRN.Should().Be(ukprn);
            fileDataCache.DPOutcomes.Should().BeSameAs(dpOutcomes);
        }

        private FileDataCachePopulationService NewService(IFileDataCache fileDataCache = null, IFileDataRetrievalService ukprnDataRetrievalService = null)
        {
            return new FileDataCachePopulationService(fileDataCache, ukprnDataRetrievalService);
        }
    }
}
