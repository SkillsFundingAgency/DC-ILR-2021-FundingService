using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor.Tasks;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FundingActor.Tests.TaskTests
{
    public class FM35ActorTaskTests
    {
        [Fact]
        public async Task Execute()
        {
            var cancellationToken = CancellationToken.None;
            var fundingServiceContextMock = new Mock<IFundingServiceContext>();

            fundingServiceContextMock.Setup(f => f.JobId).Returns(1);
            fundingServiceContextMock.Setup(f => f.Container).Returns("Container");
            fundingServiceContextMock.Setup(f => f.FundingFm36OutputKey).Returns("Key");

            IEnumerable<FundingDto> fundingActorDtos = new List<FundingDto>
            {
                new FundingDto()
            };

            var FM35Actor = new Mock<IFM35Actor>();
            var condenserOutput = new FM35Global();

            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var fundingActorProviderMock = new Mock<IActorProvider<IFM35Actor>>();
            var filePersistanceServiceMock = new Mock<IFilePersistanceService>();
            var fundingOutputCondenserServiceMock = new Mock<IFundingOutputCondenserService<FM35Global>>();
            var loggerMock = new Mock<ILogger>();

            FM35Actor.Setup(a => a.Process(fundingActorDtos.FirstOrDefault(), cancellationToken)).Returns(() => Task<string>.Factory.StartNew(() => "string"));
            jsonSerializationServiceMock.Setup(sm => sm.Deserialize<FM35Global>(It.IsAny<string>())).Returns(new FM35Global()).Verifiable();
            fundingActorProviderMock.Setup(pm => pm.Provide()).Returns(FM35Actor.Object).Verifiable();
            filePersistanceServiceMock.Setup(sm => sm.PersistAsync(fundingServiceContextMock.Object.FundingFm35OutputKey, fundingServiceContextMock.Object.Container, condenserOutput, cancellationToken)).Returns(Task.CompletedTask).Verifiable();
            fundingOutputCondenserServiceMock.Setup(sm => sm.Condense(It.IsAny<IEnumerable<FM35Global>>())).Returns(condenserOutput).Verifiable();

            await NewTask(
                jsonSerializationServiceMock.Object,
                fundingActorProviderMock.Object,
                filePersistanceServiceMock.Object,
                fundingOutputCondenserServiceMock.Object,
                loggerMock.Object,
                "FM35Actor").Execute(fundingActorDtos, fundingServiceContextMock.Object, cancellationToken);

            fundingActorProviderMock.VerifyAll();
            filePersistanceServiceMock.VerifyAll();
            fundingOutputCondenserServiceMock.VerifyAll();
        }

        private FM35ActorTask NewTask(
           IJsonSerializationService jsonSerializationService = null,
           IActorProvider<IFM35Actor> fundingActorProvider = null,
           IFilePersistanceService filePersistanceService = null,
           IFundingOutputCondenserService<FM35Global> fundingOutputCondenserService = null,
           ILogger logger = null,
           string actorName = null)
        {
            return new FM35ActorTask(
                jsonSerializationService,
                fundingActorProvider,
                filePersistanceService,
                fundingOutputCondenserService,
                logger,
                actorName);
        }
    }
}
