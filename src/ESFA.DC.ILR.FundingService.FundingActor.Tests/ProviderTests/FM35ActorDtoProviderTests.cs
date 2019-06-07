﻿using System.Collections.Generic;
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
    public class FM35ActorDtoProviderTests
    {
        [Fact]
        public void Provide()
        {
            var fundingServiceContextMock = new Mock<IFundingServiceContext>();

            fundingServiceContextMock.Setup(f => f.JobId).Returns(1);
            fundingServiceContextMock.Setup(f => f.Container).Returns("Container");
            fundingServiceContextMock.Setup(f => f.FundingFm35OutputKey).Returns("Key");

            IMessage message = new TestMessage();
            var externalDataCache = "ExternalDataCache";
            var cancellationToken = CancellationToken.None;
            var learnerPagingReturn = new List<List<FM35LearnerDto>>
            {
                new List<FM35LearnerDto>
                {
                    new FM35LearnerDto(),
                    new FM35LearnerDto()
                },
                new List<FM35LearnerDto>
                {
                    new FM35LearnerDto(),
                    new FM35LearnerDto()
                }
            };

            var expectedResult = new List<FundingDto>
            {
                new FundingDto
                {
                    JobId = 1,
                    Container = "Container",
                    OutputKey = "Key",
                    ExternalDataCache = externalDataCache,
                    ValidLearners = string.Empty
                },
                new FundingDto
                {
                    JobId = 1,
                    Container = "Container",
                    OutputKey = "Key",
                    ExternalDataCache = externalDataCache,
                    ValidLearners = string.Empty
                },
            };

            var learnerPagingServiceMock = new Mock<ILearnerPagingService<FM35LearnerDto>>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();

            learnerPagingServiceMock.Setup(lpm => lpm.ProvideDtos(35, message)).Returns(learnerPagingReturn);
            jsonSerializationServiceMock.Setup(jsm => jsm.Serialize(It.IsAny<IEnumerable<FM35LearnerDto>>())).Returns(string.Empty);

            NewProvider(learnerPagingServiceMock.Object, jsonSerializationServiceMock.Object)
                .Provide(fundingServiceContextMock.Object, message, externalDataCache, cancellationToken).Should().BeEquivalentTo(expectedResult);
        }

        private FM35ActorDtoProvider NewProvider(ILearnerPagingService<FM35LearnerDto> learnerPagingService = null, IJsonSerializationService jsonSerializationService = null)
        {
            return new FM35ActorDtoProvider(learnerPagingService, jsonSerializationService);
        }
    }
}
