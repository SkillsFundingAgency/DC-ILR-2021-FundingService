using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Population.Context;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.Context
{
    public class ValidLearnersDataRetrievalServiceTests
    {
        [Fact]
        public void Retrieve()
        {
            var validLearnerOne = new TestLearner()
            {
                LearnRefNumber = "abc"
            };

            var validLearnerTwo = new TestLearner()
            {
                LearnRefNumber = "def"
            };

            var invalidLearner = new TestLearner()
            {
                LearnRefNumber = "ghi"
            };

            var message = new TestMessage()
            {
                Learners = new List<TestLearner>()
                {
                    validLearnerOne,
                    validLearnerTwo,
                    invalidLearner
                }
            };

            var validLearnerReferenceNumbers = new string[] { "abc", "def" };

            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(message);
            fundingServiceDto.SetupGet(fs => fs.ValidLearners).Returns(validLearnerReferenceNumbers);

            NewService(fundingServiceDto.Object).Retrieve().Should().Contain(new[] { validLearnerOne, validLearnerTwo });
        }

        private ValidLearnersDataRetrievalService NewService(IFundingServiceDto fundingServiceDto = null)
        {
            return new ValidLearnersDataRetrievalService(fundingServiceDto);
        }
    }
}
