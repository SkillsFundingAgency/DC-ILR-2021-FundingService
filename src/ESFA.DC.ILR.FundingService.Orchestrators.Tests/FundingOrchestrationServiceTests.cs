using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests
{
    public class FundingOrchestrationServiceTests
    {
        [Fact]
        public async Task ExecuteAsync()
        {
            IMessage message = new TestMessage();
            var referenceData = new ReferenceDataRoot();
            IExternalDataCache externalDataCache = new ExternalDataCache();
            var externalData = "ExtermalData";
            var cancellationToken = CancellationToken.None;

            var fundingServiceContextMock = new Mock<IFundingServiceContext>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var ilrFileProviderServiceMock = new Mock<IFileProviderService<IMessage>>();
            var ilrReferenceDataProviderServiceMock = new Mock<IFileProviderService<ReferenceDataRoot>>();
            var externalCachePopulationServiceMock = new Mock<IExternalDataCachePopulationService>();
            var fundingTaskProviderMock = new Mock<IFundingTaskProvider>();
            var loggerMock = new Mock<ILogger>();

            ilrFileProviderServiceMock.Setup(sm => sm.ProvideAsync(fundingServiceContextMock.Object, cancellationToken)).Returns(Task.FromResult(message)).Verifiable();
            ilrReferenceDataProviderServiceMock.Setup(sm => sm.ProvideAsync(fundingServiceContextMock.Object, cancellationToken)).Returns(Task.FromResult(referenceData)).Verifiable();
            externalCachePopulationServiceMock.Setup(sm => sm.PopulateAsync(referenceData, cancellationToken)).Returns(externalDataCache).Verifiable();
            jsonSerializationServiceMock.Setup(sm => sm.Serialize(externalDataCache)).Returns(externalData).Verifiable();

            fundingTaskProviderMock.Setup(sm => sm.ProvideAsync(fundingServiceContextMock.Object, message, externalData, cancellationToken)).Returns(Task.FromResult(true)).Verifiable();

            await NewService(
                jsonSerializationServiceMock.Object,
                ilrFileProviderServiceMock.Object,
                ilrReferenceDataProviderServiceMock.Object,
                externalCachePopulationServiceMock.Object,
                fundingTaskProviderMock.Object,
                loggerMock.Object).ExecuteAsync(fundingServiceContextMock.Object, cancellationToken);

            jsonSerializationServiceMock.VerifyAll();
            ilrFileProviderServiceMock.VerifyAll();
            ilrReferenceDataProviderServiceMock.VerifyAll();
            externalCachePopulationServiceMock.VerifyAll();
            fundingTaskProviderMock.VerifyAll();
        }

        private FundingOrchestrationService NewService(
            IJsonSerializationService jsonSerializationService = null,
            IFileProviderService<IMessage> ilrFileProviderService = null,
            IFileProviderService<ReferenceDataRoot> ilrReferenceDataProviderService = null,
            IExternalDataCachePopulationService externalCachePopulationService = null,
            IFundingTaskProvider fundingTaskProvider = null,
            ILogger logger = null)
        {
            return new FundingOrchestrationService(
                jsonSerializationService,
                ilrFileProviderService,
                ilrReferenceDataProviderService,
                externalCachePopulationService,
                fundingTaskProvider,
                logger);
        }
    }
}
