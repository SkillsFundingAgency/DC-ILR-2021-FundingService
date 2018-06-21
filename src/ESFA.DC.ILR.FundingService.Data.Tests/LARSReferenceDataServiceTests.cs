using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class LARSReferenceDataServiceTests
    {
        [Fact]
        public void LARSCurrentVersion()
        {
            var currentVersion = "version";

            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSCurrentVersion).Returns(currentVersion);

            NewService(referenceDataCacheMock.Object).LARSCurrentVersion().Should().Be(currentVersion);
        }

        [Fact]
        public void LARSLearningDelivery()
        {
            var learnAimRef = "learnAimRef";
            var learningDelivery = new LARSLearningDelivery();

            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDelivery)
                .Returns(
                    new Dictionary<string, LARSLearningDelivery>()
                    {
                        { learnAimRef, learningDelivery }
                    });

            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryForLearnAimRef(learnAimRef).Should().Be(learningDelivery);
        }

        [Fact]
        public void LARSLearningDelivery_NotExist()
        {
            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDelivery)
                .Returns(
                    new Dictionary<string, LARSLearningDelivery>()
                    {
                        { "learnAimRef", null }
                    });
            
            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        [Fact]
        public void LARSFunding_Exists()
        {
            var learnAimRef = "learnAimRef";
            var larsFundings = new List<LARSFunding>();

            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFunding)
                .Returns(new Dictionary<string, IEnumerable<LARSFunding>>()
                {
                    { learnAimRef, larsFundings }
                });

            NewService(referenceDataCacheMock.Object).LARSFundingsForLearnAimRef(learnAimRef).Should().BeSameAs(larsFundings);
        }

        [Fact]
        public void LARSFunding_NotExists()
        {
            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFunding)
                .Returns(new Dictionary<string, IEnumerable<LARSFunding>>()
                {
                    { "learnAimRef", null }
                });

            NewService(referenceDataCacheMock.Object).LARSFundingsForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        private LARSReferenceDataService NewService(IReferenceDataCache referenceDataCache = null)
        {
            return new LARSReferenceDataService(referenceDataCache);
        }
    }
}
