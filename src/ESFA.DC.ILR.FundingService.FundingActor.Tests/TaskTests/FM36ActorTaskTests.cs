using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
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
    public class FM36ActorTaskTests
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

            var fm36Actor = new Mock<IFM36Actor>();
            var condenserOutput = new FM36Global();

            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var fundingActorProviderMock = new Mock<IActorProvider<IFM36Actor>>();
            var filePersistanceServiceMock = new Mock<IFilePersistanceService>();
            var fundingOutputCondenserServiceMock = new Mock<IFundingOutputCondenserService<FM36Global>>();
            var loggerMock = new Mock<ILogger>();

            fm36Actor.Setup(a => a.Process(fundingActorDtos.FirstOrDefault(), cancellationToken)).Returns(() => Task<string>.Factory.StartNew(() => "string"));
            jsonSerializationServiceMock.Setup(sm => sm.Deserialize<FM36Global>(It.IsAny<string>())).Returns(new FM36Global()).Verifiable();
            fundingActorProviderMock.Setup(pm => pm.Provide()).Returns(fm36Actor.Object).Verifiable();
            filePersistanceServiceMock.Setup(sm => sm.PersistAsync(fundingServiceContextMock.Object.FundingFm36OutputKey, fundingServiceContextMock.Object.Container, condenserOutput, cancellationToken)).Returns(Task.CompletedTask).Verifiable();
            fundingOutputCondenserServiceMock.Setup(sm => sm.Condense(It.IsAny<IEnumerable<FM36Global>>())).Returns(condenserOutput).Verifiable();

            await NewTask(
                jsonSerializationServiceMock.Object,
                fundingActorProviderMock.Object,
                filePersistanceServiceMock.Object,
                fundingOutputCondenserServiceMock.Object,
                loggerMock.Object,
                "FM36Actor").Execute(fundingActorDtos, fundingServiceContextMock.Object, cancellationToken);

            fundingActorProviderMock.VerifyAll();
            filePersistanceServiceMock.VerifyAll();
            fundingOutputCondenserServiceMock.VerifyAll();
        }

        private FM36ActorTask NewTask(
           IJsonSerializationService jsonSerializationService = null,
           IActorProvider<IFM36Actor> fundingActorProvider = null,
           IFilePersistanceService filePersistanceService = null,
           IFundingOutputCondenserService<FM36Global> fundingOutputCondenserService = null,
           ILogger logger = null,
           string actorName = null)
        {
            return new FM36ActorTask(
                jsonSerializationService,
                fundingActorProvider,
                filePersistanceService,
                fundingOutputCondenserService,
                logger,
                actorName);
        }
    }
}
