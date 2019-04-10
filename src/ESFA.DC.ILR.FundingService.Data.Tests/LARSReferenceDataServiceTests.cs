using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS;
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

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSCurrentVersion).Returns(currentVersion);

            NewService(referenceDataCacheMock.Object).LARSCurrentVersion().Should().Be(currentVersion);
        }

        [Fact]
        public void LARSLearningDeliveryForLearnAimRef()
        {
            var learnAimRef = "learnAimRef";
            var learningDelivery = new LARSLearningDelivery();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDelivery)
                .Returns(
                    new Dictionary<string, LARSLearningDelivery>()
                    {
                        { learnAimRef, learningDelivery }
                    });

            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryForLearnAimRef(learnAimRef).Should().Be(learningDelivery);
        }

        [Fact]
        public void LARSLearningDeliveryForLearnAimRef_NotExist()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDelivery)
                .Returns(
                    new Dictionary<string, LARSLearningDelivery>()
                    {
                        { "learnAimRef", null }
                    });

            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        [Fact]
        public void LARSStandardForStandardCode()
        {
            var stdCode = 1;
            var larsStandard = new LARSStandard();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandards)
                .Returns(
                    new Dictionary<int, LARSStandard>()
                    {
                        { stdCode, larsStandard }
                    });

            NewService(referenceDataCacheMock.Object).LARSStandardForStandardCode(stdCode).Should().Be(larsStandard);
        }

        [Fact]
        public void LARSStandardForStandardCode_NotExist()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandards)
                .Returns(
                    new Dictionary<int, LARSStandard>()
                    {
                        { 1, null }
                    });

            NewService(referenceDataCacheMock.Object).LARSStandardForStandardCode(2).Should().BeNull();
        }

        private LARSReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new LARSReferenceDataService(referenceDataCache);
        }
    }
}
