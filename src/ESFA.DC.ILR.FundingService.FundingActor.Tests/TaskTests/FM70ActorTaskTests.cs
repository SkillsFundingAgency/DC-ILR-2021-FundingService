using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70Actor.Interfaces;
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
    public class FM70ActorTaskTests
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

            var FM70Actor = new Mock<IFM70Actor>();
            var condenserOutput = new FM70Global();

            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var fundingActorProviderMock = new Mock<IActorProvider<IFM70Actor>>();
            var filePersistanceServiceMock = new Mock<IFilePersistanceService>();
            var fundingOutputCondenserServiceMock = new Mock<IFundingOutputCondenserService<FM70Global>>();
            var loggerMock = new Mock<ILogger>();

            FM70Actor.Setup(a => a.Process(fundingActorDtos.FirstOrDefault(), cancellationToken)).Returns(() => Task<string>.Factory.StartNew(() => "string"));
            jsonSerializationServiceMock.Setup(sm => sm.Deserialize<FM70Global>(It.IsAny<string>())).Returns(new FM70Global()).Verifiable();
            fundingActorProviderMock.Setup(pm => pm.Provide()).Returns(FM70Actor.Object).Verifiable();
            filePersistanceServiceMock.Setup(sm => sm.PersistAsync(fundingServiceContextMock.Object.FundingFm70OutputKey, fundingServiceContextMock.Object.Container, condenserOutput, cancellationToken)).Returns(Task.CompletedTask).Verifiable();
            fundingOutputCondenserServiceMock.Setup(sm => sm.Condense(It.IsAny<IEnumerable<FM70Global>>())).Returns(condenserOutput).Verifiable();

            await NewTask(
                jsonSerializationServiceMock.Object,
                fundingActorProviderMock.Object,
                filePersistanceServiceMock.Object,
                fundingOutputCondenserServiceMock.Object,
                loggerMock.Object,
                "FM70Actor").Execute(fundingActorDtos, fundingServiceContextMock.Object, cancellationToken);

            fundingActorProviderMock.VerifyAll();
            filePersistanceServiceMock.VerifyAll();
            fundingOutputCondenserServiceMock.VerifyAll();
        }

        private FM70ActorTask NewTask(
           IJsonSerializationService jsonSerializationService = null,
           IActorProvider<IFM70Actor> fundingActorProvider = null,
           IFilePersistanceService filePersistanceService = null,
           IFundingOutputCondenserService<FM70Global> fundingOutputCondenserService = null,
           ILogger logger = null,
           string actorName = null)
        {
            return new FM70ActorTask(
                jsonSerializationService,
                fundingActorProvider,
                filePersistanceService,
                fundingOutputCondenserService,
                logger,
                actorName);
        }
    }
}
