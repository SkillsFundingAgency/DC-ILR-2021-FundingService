using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Providers.Tests
{
    public class IlrFileProviderServiceTests
    {

        [Fact]
        public async Task Provide()
        {
            var cancellationToken = CancellationToken.None;

            var fileReference = "FileReference";
            var container = "Container";

            Stream stream = new MemoryStream();

            var message = new Message();

            var fundingSrviceContextContextMock = new Mock<IFundingServiceContext>();
            var fileServiceMock = new Mock<IFileService>();
            var xmlSerializationServiceMock = new Mock<IXmlSerializationService>();

            fundingSrviceContextContextMock.SetupGet(c => c.FileReference).Returns(fileReference);
            fundingSrviceContextContextMock.SetupGet(c => c.Container).Returns(container);

            fileServiceMock.Setup(s => s.OpenReadStreamAsync(fileReference, container, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();
            xmlSerializationServiceMock.Setup(s => s.Deserialize<Message>(stream)).Returns(message).Verifiable();

            var providedMessage = await NewProvider(fileServiceMock.Object, xmlSerializationServiceMock.Object).ProvideAsync(fundingSrviceContextContextMock.Object, cancellationToken);

            providedMessage.Should().Be(message);

            fileServiceMock.VerifyAll();
            xmlSerializationServiceMock.VerifyAll();
        }

        private IlrFileProviderService NewProvider(IFileService fileService = null, IXmlSerializationService xmlSerializationService = null)
        {
            return new IlrFileProviderService(fileService, xmlSerializationService);
        }
    }
}