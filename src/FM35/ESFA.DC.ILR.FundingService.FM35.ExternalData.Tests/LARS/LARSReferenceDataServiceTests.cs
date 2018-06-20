using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Tests.LARS
{
    public class LARSReferenceDataServiceTests
    {
        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSVersion - Does exist"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSCurrentVersion_Exists()
        {
            // ARRANGE
            var larsServiceMock = LARSCurrentVersionTestRun(LARSCurrentVersionTestValue);

            // ACT
            var larsCurrentVersionExists = larsServiceMock.LARSCurrentVersion();

            // ASSERT
            larsCurrentVersionExists.Should().NotBeNull();
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSVersion - Correct"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSCurrentVersion_Correct()
        {
            // ARRANGE
            var larsServiceMock = LARSCurrentVersionTestRun(LARSCurrentVersionTestValue);

            // ACT
            var larsCurrentVersionExists = larsServiceMock.LARSCurrentVersion();

            // ASSERT
            larsCurrentVersionExists.Should().Be("Version_005");
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDelivery - Does exist"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSLearningDelivery_Exists()
        {
            // ARRANGE
            var larsServiceMock = LARSLearningDeliveryTestRun(LARSLearningDeliveryTestValue);

            // ACT
            var larsLearningDeliveryExists = larsServiceMock.LARSLearningDeliveriesForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsLearningDeliveryExists.Should().NotBeNull();
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDelivery - Correct"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSLearningDelivery_Correct()
        {
            // ARRANGE
            var larsServiceMock = LARSLearningDeliveryTestRun(LARSLearningDeliveryTestValue);

            // ACT
            var larsLearningDeliveryExists = larsServiceMock.LARSLearningDeliveriesForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsLearningDeliveryExists.Should().BeEquivalentTo(LARSLearningDeliveryTestValue);
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSAnnualValues - Does exist"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSAnnualValues_Exists()
        {
            // ARRANGE
            var larsServiceMock = LARSAnnualValueTestRun(LARSAnnualValueList(LARSAnnualValueTestValue));

            // ACT
            var larsAnnualValueExists = larsServiceMock.LARSAnnualValuesForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsAnnualValueExists.Should().NotBeNull();
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSAnnualValues - Correct"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSAnnualValues_Correct()
        {
            // ARRANGE
            var larsServiceMock = LARSAnnualValueTestRun(LARSAnnualValueList(LARSAnnualValueTestValue));

            // ACT
            var larsAnnualValueExists = larsServiceMock.LARSAnnualValuesForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsAnnualValueExists.Should().BeEquivalentTo(LARSAnnualValueTestValue);
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSFunding - Does exist"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSFunding_Exists()
        {
            // ARRANGE
            var larsServiceMock = LARSFundingTestRun(LARSFundingList(LARSFundingTestValue));

            // ACT
            var larsFundingExists = larsServiceMock.LARSFundingsForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsFundingExists.Should().NotBeNull();
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSFunding - Correct"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSFunding_Correct()
        {
            // ARRANGE
            var larsServiceMock = LARSFundingTestRun(LARSFundingList(LARSFundingTestValue));

            // ACT
            var larsFundingExists = larsServiceMock.LARSFundingsForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsFundingExists.Should().BeEquivalentTo(LARSFundingTestValue);
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDeliveryCategory - Does exist"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSLearningDeliveryCategory_Exists()
        {
            // ARRANGE
            var larsServiceMock = LARSLearningDeliveryCategoryTestRun(LARSLearningDeliveryCategoryList(LARSLearningDeliveryCategoryTestValue));

            // ACT
            var larsLearningDeliveryCategoryExists = larsServiceMock.LARSLearningDeliveryCategoriesForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsLearningDeliveryCategoryExists.Should().NotBeNull();
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDeliveryCategory - Correct"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSLearningDeliveryCategory_Correct()
        {
            // ARRANGE
            var larsServiceMock = LARSLearningDeliveryCategoryTestRun(LARSLearningDeliveryCategoryList(LARSLearningDeliveryCategoryTestValue));

            // ACT
            var larsLearningDeliveryCategoryExists = larsServiceMock.LARSLearningDeliveryCategoriesForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsLearningDeliveryCategoryExists.Should().BeEquivalentTo(LARSLearningDeliveryCategoryTestValue);
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSFrameworkAims - Does exist"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSFrameworkAims_Exists()
        {
            // ARRANGE
            var larsServiceMock = LARSFrameworkAimsTestRun(LARSFrameworkAimsList(LARSFrameworkAimsTestValue));

            // ACT
            var larsFrameworkAimsExists = larsServiceMock.LARSFFrameworkAimsForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsFrameworkAimsExists.Should().NotBeNull();
        }

        /// <summary>
        /// Return LARS Data.
        /// </summary>
        [Fact(DisplayName = "LARSFrameworkAims - Correct"), Trait("LARSReferenceDataService", "Unit")]
        public void LARSFrameworkAims_Correct()
        {
            // ARRANGE
            var larsServiceMock = LARSFrameworkAimsTestRun(LARSFrameworkAimsList(LARSFrameworkAimsTestValue));

            // ACT
            var larsFrameworkAimsExists = larsServiceMock.LARSFFrameworkAimsForLearnAimRef(LearnAimRefTestValue);

            // ASSERT
            larsFrameworkAimsExists.Should().BeEquivalentTo(LARSFrameworkAimsTestValue);
        }

        private readonly Mock<IReferenceDataCache> referenceDataCacheMock = new Mock<IReferenceDataCache>();

        private static readonly string LARSCurrentVersionTestValue = "Version_005";
        private static readonly string LearnAimRefTestValue = "123456";

        private static readonly LARSLearningDelivery LARSLearningDeliveryTestValue =
             new LARSLearningDelivery()
             {
                 LearnAimRef = "123456",
                 EnglandFEHEStatus = "F",
                 EnglPrscID = 2,
                 FrameworkCommonComponent = 180,
             };

        private static readonly LARSFunding LARSFundingTestValue =
            new LARSFunding()
            {
                LearnAimRef = "123456",
                FundingCategory = "Matrix",
                RateWeighted = 1.5m,
                WeightingFactor = "W-Factor",
                RateUnWeighted = 0.5m,
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };

        private static readonly LARSAnnualValue LARSAnnualValueTestValue =
           new LARSAnnualValue()
           {
               LearnAimRef = "123456",
               EffectiveFrom = DateTime.Parse("2000-01-01"),
               EffectiveTo = null,
               BasicSkillsType = 200,
           };

        private static readonly LARSFrameworkAims LARSFrameworkAimsTestValue =
          new LARSFrameworkAims()
          {
              LearnAimRef = "123456",
              FrameworkComponentType = 1,
              ProgType = 2,
              FworkCode = 3,
              PwayCode = 4,
              EffectiveFrom = DateTime.Parse("2000-01-01"),
          };

        private static readonly LARSLearningDeliveryCategory LARSLearningDeliveryCategoryTestValue =
           new LARSLearningDeliveryCategory()
           {
               LearnAimRef = "123456",
               CategoryRef = 300,
               EffectiveFrom = DateTime.Parse("2000-01-01"),
               EffectiveTo = null,
           };

        private ILARSReferenceDataService LARSCurrentVersionTestRun(string larsVersion)
        {
            var larsCurrentVersionMock = referenceDataCacheMock;
            larsCurrentVersionMock.SetupGet(rdc => rdc.LARSCurrentVersion).Returns(larsVersion);

            return MockTestObject(larsCurrentVersionMock.Object);
        }

        private ILARSReferenceDataService LARSLearningDeliveryTestRun(LARSLearningDelivery larsLearningDelivery)
        {
            var larsLearningDeliveryMock = referenceDataCacheMock;
            larsLearningDeliveryMock.SetupGet(rdc => rdc.LARSLearningDelivery).Returns(new Dictionary<string, LARSLearningDelivery>()
             {
                { LearnAimRefTestValue, larsLearningDelivery },
            });

            return MockTestObject(larsLearningDeliveryMock.Object);
        }

        private ILARSReferenceDataService LARSFundingTestRun(IList<LARSFunding> larsFundingList)
        {
            var larsFundingMock = referenceDataCacheMock;
            larsFundingMock.SetupGet(rdc => rdc.LARSFunding).Returns(new Dictionary<string, IEnumerable<LARSFunding>>()
            {
                { LearnAimRefTestValue, larsFundingList },
            });

            return MockTestObject(larsFundingMock.Object);
        }

        private ILARSReferenceDataService LARSAnnualValueTestRun(IList<LARSAnnualValue> larsAnnualValueList)
        {
            var larsLearningDeliveryMock = referenceDataCacheMock;
            larsLearningDeliveryMock.SetupGet(rdc => rdc.LARSAnnualValue).Returns(new Dictionary<string, IEnumerable<LARSAnnualValue>>()
             {
                { LearnAimRefTestValue, larsAnnualValueList },
            });

            return MockTestObject(larsLearningDeliveryMock.Object);
        }

        private ILARSReferenceDataService LARSLearningDeliveryCategoryTestRun(IList<LARSLearningDeliveryCategory> larsLearningDeliveryCategoryList)
        {
            var larsLearningDeliveryMock = referenceDataCacheMock;
            larsLearningDeliveryMock.SetupGet(rdc => rdc.LARSLearningDeliveryCatgeory).Returns(new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>()
             {
                { LearnAimRefTestValue, larsLearningDeliveryCategoryList },
            });

            return MockTestObject(larsLearningDeliveryMock.Object);
        }

        private ILARSReferenceDataService LARSFrameworkAimsTestRun(IList<LARSFrameworkAims> larsFrameworkAimsList)
        {
            var larsLearningDeliveryMock = referenceDataCacheMock;
            larsLearningDeliveryMock.SetupGet(rdc => rdc.LARSFrameworkAims).Returns(new Dictionary<string, IEnumerable<LARSFrameworkAims>>()
             {
                { LearnAimRefTestValue, larsFrameworkAimsList },
            });

            return MockTestObject(larsLearningDeliveryMock.Object);
        }

        private ILARSReferenceDataService MockTestObject(IReferenceDataCache @object)
        {
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(@object);

            return larsReferenceDataService;
        }

        private IList<LARSAnnualValue> LARSAnnualValueList(LARSAnnualValue larsAnnualValueData)
        {
            return new List<LARSAnnualValue>
            {
                larsAnnualValueData,
            };
        }

        private IList<LARSFunding> LARSFundingList(LARSFunding larsFundingData)
        {
            return new List<LARSFunding>
            {
                larsFundingData,
            };
        }

        private IList<LARSFrameworkAims> LARSFrameworkAimsList(LARSFrameworkAims larsFrameworkAimsData)
        {
            return new List<LARSFrameworkAims>
            {
                larsFrameworkAimsData,
            };
        }

        private IList<LARSLearningDeliveryCategory> LARSLearningDeliveryCategoryList(LARSLearningDeliveryCategory larsLearningDeliveryCategoryData)
        {
            return new List<LARSLearningDeliveryCategory>
            {
                larsLearningDeliveryCategoryData,
            };
        }
    }
}
