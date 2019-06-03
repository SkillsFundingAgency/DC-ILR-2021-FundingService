using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.File;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
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
            var dpOutcomes = new Dictionary<string, IEnumerable<DPOutcome>>();

            var message = new TestMessage()
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = ukprn,
                },
                LearnerDestinationAndProgressions = new List<TestLearnerDestinationAndProgression>
                {
                    new TestLearnerDestinationAndProgression(),
                }
            };

            var fileDataRetrievalServiceMock = new Mock<IFileDataRetrievalService>();

            fileDataRetrievalServiceMock.Setup(rds => rds.RetrieveUKPRN(message)).Returns(ukprn);
            fileDataRetrievalServiceMock.Setup(rds => rds.RetrieveDPOutcomes(message)).Returns(dpOutcomes);

            var fileDataCache = NewService(fileDataRetrievalServiceMock.Object).PopulateAsync(message, CancellationToken.None);

            fileDataCache.UKPRN.Should().Be(ukprn);
            fileDataCache.DPOutcomes.Should().BeSameAs(dpOutcomes);
        }

        private FileDataCachePopulationService NewService(IFileDataRetrievalService fileDataRetrievalService = null)
        {
            return new FileDataCachePopulationService(fileDataRetrievalService);
        }
    }
}
