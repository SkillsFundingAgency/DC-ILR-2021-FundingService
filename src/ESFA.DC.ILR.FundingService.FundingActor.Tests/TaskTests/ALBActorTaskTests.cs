using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Dto;
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
    public class ALBActorTaskTests
    {
        [Fact]
        public async Task Execute()
        {
            var cancellationToken = CancellationToken.None;
            var fundingServiceContextMock = new Mock<IFundingServiceContext>();

            fundingServiceContextMock.Setup(f => f.JobId).Returns(1);
            fundingServiceContextMock.Setup(f => f.Container).Returns("Container");
            fundingServiceContextMock.Setup(f => f.FundingFm36OutputKey).Returns("Key");
            fundingServiceContextMock.Setup(f => f.Ukprn).Returns(12345678);
            fundingServiceContextMock.Setup(f => f.Year).Returns("1920");

            IEnumerable<FundingDto> fundingActorDtos = new List<FundingDto>
            {
                new FundingDto()
            };

            var albActor = new Mock<IALBActor>();
            var condenserOutput = new ALBGlobal();

            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var fundingActorProviderMock = new Mock<IActorProvider<IALBActor>>();
            var filePersistanceServiceMock = new Mock<IFilePersistanceService>();
            var fundingOutputCondenserServiceMock = new Mock<IFundingOutputCondenserService<ALBGlobal>>();
            var loggerMock = new Mock<ILogger>();

            albActor.Setup(a => a.Process(fundingActorDtos.FirstOrDefault(), cancellationToken)).Returns(() => Task<string>.Factory.StartNew(() => "string"));
            jsonSerializationServiceMock.Setup(sm => sm.Deserialize<ALBGlobal>(It.IsAny<string>())).Returns(new ALBGlobal()).Verifiable();
            fundingActorProviderMock.Setup(pm => pm.Provide()).Returns(albActor.Object).Verifiable();
            filePersistanceServiceMock.Setup(sm => sm.PersistAsync(fundingServiceContextMock.Object.FundingALBOutputKey, fundingServiceContextMock.Object.Container, condenserOutput, cancellationToken)).Returns(Task.CompletedTask).Verifiable();
            fundingOutputCondenserServiceMock.Setup(sm => sm.Condense(
                It.IsAny<IEnumerable<ALBGlobal>>(),
                fundingServiceContextMock.Object.Ukprn,
                fundingServiceContextMock.Object.Year)).Returns(condenserOutput).Verifiable();

            await NewTask(
                jsonSerializationServiceMock.Object,
                fundingActorProviderMock.Object,
                filePersistanceServiceMock.Object,
                fundingOutputCondenserServiceMock.Object,
                loggerMock.Object,
                "ALBActor").Execute(fundingActorDtos, fundingServiceContextMock.Object, cancellationToken);

            fundingActorProviderMock.VerifyAll();
            filePersistanceServiceMock.VerifyAll();
            fundingOutputCondenserServiceMock.VerifyAll();
        }

        private ALBActorTask NewTask(
           IJsonSerializationService jsonSerializationService = null,
           IActorProvider<IALBActor> fundingActorProvider = null,
           IFilePersistanceService filePersistanceService = null,
           IFundingOutputCondenserService<ALBGlobal> fundingOutputCondenserService = null,
           ILogger logger = null,
           string actorName = null)
        {
            return new ALBActorTask(
                jsonSerializationService,
                fundingActorProvider,
                filePersistanceService,
                fundingOutputCondenserService,
                logger,
                actorName);
        }
    }
}
