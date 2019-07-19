using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81Actor.Interfaces;
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
    public class FM81ActorTaskTests
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

            var FM81Actor = new Mock<IFM81Actor>();
            var condenserOutput = new FM81Global();

            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var fundingActorProviderMock = new Mock<IActorProvider<IFM81Actor>>();
            var filePersistanceServiceMock = new Mock<IFilePersistanceService>();
            var fundingOutputCondenserServiceMock = new Mock<IFundingOutputCondenserService<FM81Global>>();
            var loggerMock = new Mock<ILogger>();

            FM81Actor.Setup(a => a.Process(fundingActorDtos.FirstOrDefault(), cancellationToken)).Returns(() => Task<string>.Factory.StartNew(() => "string"));
            jsonSerializationServiceMock.Setup(sm => sm.Deserialize<FM81Global>(It.IsAny<string>())).Returns(new FM81Global()).Verifiable();
            fundingActorProviderMock.Setup(pm => pm.Provide()).Returns(FM81Actor.Object).Verifiable();
            filePersistanceServiceMock.Setup(sm => sm.PersistAsync(fundingServiceContextMock.Object.FundingFm81OutputKey, fundingServiceContextMock.Object.Container, condenserOutput, cancellationToken)).Returns(Task.CompletedTask).Verifiable();
            fundingOutputCondenserServiceMock.Setup(sm => sm.Condense(
                It.IsAny<IEnumerable<FM81Global>>(),
                fundingServiceContextMock.Object.Ukprn,
                fundingServiceContextMock.Object.Year)).Returns(condenserOutput).Verifiable();

            await NewTask(
                jsonSerializationServiceMock.Object,
                fundingActorProviderMock.Object,
                filePersistanceServiceMock.Object,
                fundingOutputCondenserServiceMock.Object,
                loggerMock.Object,
                "FM81Actor").Execute(fundingActorDtos, fundingServiceContextMock.Object, cancellationToken);

            fundingActorProviderMock.VerifyAll();
            filePersistanceServiceMock.VerifyAll();
            fundingOutputCondenserServiceMock.VerifyAll();
        }

        private FM81ActorTask NewTask(
           IJsonSerializationService jsonSerializationService = null,
           IActorProvider<IFM81Actor> fundingActorProvider = null,
           IFilePersistanceService filePersistanceService = null,
           IFundingOutputCondenserService<FM81Global> fundingOutputCondenserService = null,
           ILogger logger = null,
           string actorName = null)
        {
            return new FM81ActorTask(
                jsonSerializationService,
                fundingActorProvider,
                filePersistanceService,
                fundingOutputCondenserService,
                logger,
                actorName);
        }
    }
}
