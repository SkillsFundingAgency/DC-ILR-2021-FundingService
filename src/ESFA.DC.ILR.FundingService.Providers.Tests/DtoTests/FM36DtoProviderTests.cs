﻿using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Dtos;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Providers.Tests.DtoTests
{
    public class FM36DtoProviderTests
    {
        [Fact]
        public void Provide()
        {
            var fundingServiceContextMock = new Mock<IFundingServiceContext>();

            fundingServiceContextMock.Setup(f => f.JobId).Returns(1);
            fundingServiceContextMock.Setup(f => f.Container).Returns("Container");
            fundingServiceContextMock.Setup(f => f.FundingFm36OutputKey).Returns("Key");

            IMessage message = new TestMessage
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = 1
                }
            };

            var externalDataCache = "ExternalDataCache";
            var cancellationToken = CancellationToken.None;
            var learnerPagingReturn = new List<List<FM36LearnerDto>>
            {
                new List<FM36LearnerDto>
                {
                    new FM36LearnerDto(),
                    new FM36LearnerDto()
                },
                new List<FM36LearnerDto>
                {
                    new FM36LearnerDto(),
                    new FM36LearnerDto()
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

            var learnerPagingServiceMock = new Mock<ILearnerPagingService<FM36LearnerDto>>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();

            learnerPagingServiceMock.Setup(lpm => lpm.ProvideDtos(new List<int> { 36 }, message)).Returns(learnerPagingReturn);
            jsonSerializationServiceMock.Setup(jsm => jsm.Serialize(It.IsAny<IEnumerable<FM36LearnerDto>>())).Returns(string.Empty);

            NewProvider(learnerPagingServiceMock.Object, jsonSerializationServiceMock.Object)
                .Provide(fundingServiceContextMock.Object, message, externalDataCache, cancellationToken).Should().BeEquivalentTo(expectedResult);
        }

        private FM36DtoProvider NewProvider(ILearnerPagingService<FM36LearnerDto> learnerPagingService = null, IJsonSerializationService jsonSerializationService = null)
        {
            return new FM36DtoProvider(learnerPagingService, jsonSerializationService);
        }
    }
}
