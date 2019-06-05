using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Providers.Tests
{
    public class IlrReferenceDataProviderServiceTests
    {

        [Fact]
        public async Task Provide()
        {
            var cancellationToken = CancellationToken.None;

            var fileReference = "FileReference";
            var container = "Container";

            Stream stream = new MemoryStream();

            var referenceDataRoot = new ReferenceDataRoot();

            var fundingSrviceContextContextMock = new Mock<IFundingServiceContext>();
            var fileServiceMock = new Mock<IFileService>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();

            fundingSrviceContextContextMock.SetupGet(c => c.IlrReferenceDataKey).Returns(fileReference);
            fundingSrviceContextContextMock.SetupGet(c => c.Container).Returns(container);

            fileServiceMock.Setup(s => s.OpenReadStreamAsync(fileReference, container, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Deserialize<ReferenceDataRoot>(stream)).Returns(referenceDataRoot).Verifiable();

            var providedReferenceData = await NewProvider(fileServiceMock.Object, jsonSerializationServiceMock.Object).ProvideAsync(fundingSrviceContextContextMock.Object, cancellationToken);

            providedReferenceData.Should().Be(referenceDataRoot);

            fileServiceMock.VerifyAll();
            jsonSerializationServiceMock.VerifyAll();
        }

        private IlrReferenceDataProviderService NewProvider(IFileService fileService = null, IJsonSerializationService jsonSerializationService = null)
        {
            return new IlrReferenceDataProviderService(fileService, jsonSerializationService);
        }
    }
}