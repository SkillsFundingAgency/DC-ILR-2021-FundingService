using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.Serialization.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Providers.Tests
{
    public class FilePersistanceServiceTests
    {
        [Fact]
        public async Task StoreAsync()
        {
            var cancellationToken = CancellationToken.None;

            var fileReference = "FileReference";
            var container = "Container";

            var albGLobal = new ALBGlobal();

            Stream stream = new MemoryStream();

            var fileServiceMock = new Mock<IFileService>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();

            fileServiceMock.Setup(s => s.OpenWriteStreamAsync(fileReference, container, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(albGLobal, It.IsAny<Stream>())).Verifiable();

            await NewService(jsonSerializationServiceMock.Object, fileServiceMock.Object).PersistAsync(fileReference, container, albGLobal, cancellationToken);

            fileServiceMock.VerifyAll();
            jsonSerializationServiceMock.VerifyAll();
        }

        private FilePersistanceService NewService(IJsonSerializationService jsonSerializationService, IFileService fileService = null)
        {
            return new FilePersistanceService(jsonSerializationService, fileService);
        }
    }
}