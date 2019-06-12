using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FundingActor.Providers;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FundingActor.Tests.ProviderTests
{
    public class FM81ActorDtoProviderTests
    {
        [Fact]
        public void Provide()
        {
            var fundingServiceContextMock = new Mock<IFundingServiceContext>();

            fundingServiceContextMock.Setup(f => f.JobId).Returns(1);
            fundingServiceContextMock.Setup(f => f.Container).Returns("Container");
            fundingServiceContextMock.Setup(f => f.FundingFm81OutputKey).Returns("Key");

            IMessage message = new TestMessage
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = 1
                }
            };

            var externalDataCache = "ExternalDataCache";
            var cancellationToken = CancellationToken.None;
            var learnerPagingReturn = new List<List<FM81LearnerDto>>
            {
                new List<FM81LearnerDto>
                {
                    new FM81LearnerDto(),
                    new FM81LearnerDto()
                },
                new List<FM81LearnerDto>
                {
                    new FM81LearnerDto(),
                    new FM81LearnerDto()
                }
            };

            var expectedResult = new List<FundingDto>
            {
                new FundingDto
                {
                    JobId = 1,
                    Container = "Container",
                    OutputKey = "Key",
                    UKPRN = 1,
                    ExternalDataCache = externalDataCache,
                    ValidLearners = string.Empty
                },
                new FundingDto
                {
                    JobId = 1,
                    Container = "Container",
                    OutputKey = "Key",
                    UKPRN = 1,
                    ExternalDataCache = externalDataCache,
                    ValidLearners = string.Empty
                },
            };

            var learnerPagingServiceMock = new Mock<ILearnerPagingService<FM81LearnerDto>>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();

            learnerPagingServiceMock.Setup(lpm => lpm.ProvideDtos(81, message)).Returns(learnerPagingReturn);
            jsonSerializationServiceMock.Setup(jsm => jsm.Serialize(It.IsAny<IEnumerable<FM81LearnerDto>>())).Returns(string.Empty);

            NewProvider(learnerPagingServiceMock.Object, jsonSerializationServiceMock.Object)
                .Provide(fundingServiceContextMock.Object, message, externalDataCache, cancellationToken).Should().BeEquivalentTo(expectedResult);
        }

        private FM81ActorDtoProvider NewProvider(ILearnerPagingService<FM81LearnerDto> learnerPagingService = null, IJsonSerializationService jsonSerializationService = null)
        {
            return new FM81ActorDtoProvider(learnerPagingService, jsonSerializationService);
        }
    }
}
