using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Tests.ExternalCache
{
    public class ReferenceDataCacheTests
    {
        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSFunding - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void LARSFunding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSFunding.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSFunding - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void LARSFunding_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSFunding.Select(v => v.Value).First().Should().BeEquivalentTo(LARSFundingTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSAnnualValue - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void LARSAnnualValue_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSAnnualValue.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSAnnualValue - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void LARSAnnualValue_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSAnnualValue.Select(v => v.Value).First().Should().BeEquivalentTo(LARSAnnualValueTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSFrameworkAims - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void LARSFrameworkAims_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSFrameworkAims.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSFrameworkAims - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void LARSFrameworkAims_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSFrameworkAims.Select(v => v.Value).First().Should().BeEquivalentTo(LARSFrameworkAimsTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDeliveryCatgeory - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void LARSLearningDeliveryCatgeory_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSLearningDeliveryCatgeory.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDeliveryCatgeory - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void LARSLearningDeliveryCatgeory_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSLearningDeliveryCatgeory.Select(v => v.Value).First().Should().BeEquivalentTo(LARSLearningDeliveryCategoryTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDelivery - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void LARSLearningDelivery_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSLearningDelivery.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSLearningDelivery - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void LARSLearningDelivery_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSLearningDelivery.Select(v => v.Value).First().Should().BeEquivalentTo(LARSLearningDeliveryTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSCurrentVersion - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void LARSCurrentVersion_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSCurrentVersion.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LARSCurrentVersion - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void LARSCurrentVersion_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LARSCurrentVersion.Should().BeEquivalentTo("Version_005");
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "Postcodes Version - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void PostcodesVersion_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.PostcodeCurrentVersion.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "Postcodes Version - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void PostcodesVersion_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.PostcodeCurrentVersion.Should().Be("Version_002");
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "SfaAreaCost - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void SfaAreaCost_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.SfaAreaCost.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "SfaAreaCost - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void SfaAreaCost_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.SfaAreaCost.Select(v => v.Value).First().Should().BeEquivalentTo(SfaAreaCostTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "SfaDisadvantage - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void SfaDisadvantage_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.SfaDisadvantage.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "SfaDisadvantage - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void SfaDisadvantage_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.SfaDisadvantage.Select(v => v.Value).First().Should().BeEquivalentTo(SfaDisadvantageTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "OrgVersion - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void OrgVersion_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.OrgVersion.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "OrgVersion - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void OrgVersion_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.OrgVersion.Should().Be("Version_003");
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void OrgFunding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.OrgFunding.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void OrgFunding_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.OrgFunding.Select(v => v.Value).First().Should().BeEquivalentTo(OrgFundingTestValue);
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LargeEmployers - Does exist"), Trait("ReferenceDataCache", "Unit")]
        public void LargeEmployers_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LargeEmployers.Should().NotBeNull();
        }

        /// <summary>
        /// Return Data from Reference Data Cache.
        /// </summary>
        [Fact(DisplayName = "LargeEmployers - Correct"), Trait("ReferenceDataCache", "Unit")]
        public void LargeEmployers_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var referenceDataCache = SetupReferenceDataCache();

            // ASSERT
            referenceDataCache.LargeEmployers.Select(v => v.Value).First().Should().BeEquivalentTo(LargeEmployersTestValue);
        }

        private IReferenceDataCache SetupReferenceDataCache()
        {
            IReferenceDataCache referenceDataCache = new ReferenceDataCache
            {
                LARSFunding = new Dictionary<string, IEnumerable<LARSFunding>>
                {
                    { "123456",  LARSFundingList(LARSFundingTestValue) },
                },
                LARSCurrentVersion = LARSCurrentVersion,
                LARSLearningDelivery = new Dictionary<string, LARSLearningDelivery>
                {
                    { "123456", LARSLearningDeliveryTestValue },
                },
                LARSAnnualValue = new Dictionary<string, IEnumerable<LARSAnnualValue>>
                {
                    { "123456", LARSAnnualValueList(LARSAnnualValueTestValue) },
                },
                LARSFrameworkAims = new Dictionary<string, IEnumerable<LARSFrameworkAims>>
                {
                    { "123456", LARSFrameworkAimsList(LARSFrameworkAimsTestValue) },
                },
                LARSLearningDeliveryCatgeory = new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>
                {
                    { "123456", LARSLearningDeliveryCategoryList(LARSLearningDeliveryCategoryTestValue) },
                },
                PostcodeCurrentVersion = PostcodesCurrentVersion,
                SfaAreaCost = new Dictionary<string, IEnumerable<SfaAreaCost>>
                    {
                        { "CV1 2WT", SFAAreaCostList(SfaAreaCostTestValue) },
                    },
                SfaDisadvantage = new Dictionary<string, IEnumerable<SfaDisadvantage>>
                    {
                        { "CV1 2WT", SFADisadvantageList(SfaDisadvantageTestValue) },
                    },
                OrgVersion = OrgVersionTestValue,
                OrgFunding = new Dictionary<long, IEnumerable<OrgFunding>>
                {
                    { 12345678, OrgFundingList(OrgFundingTestValue) },
                },
                LargeEmployers = new Dictionary<int, IEnumerable<LargeEmployers>>
                {
                    { 107733730, LargeEmployersList(LargeEmployersTestValue) },
                },
            };

            return referenceDataCache;
        }

        private static readonly string LARSCurrentVersion = "Version_005";

        private static readonly LARSFunding LARSFundingTestValue =
           new LARSFunding()
           {
               EffectiveFrom = new DateTime(2000, 01, 01),
               EffectiveTo = null,
               FundingCategory = "Matrix",
               LearnAimRef = "123456",
               RateWeighted = 1.5m,
               RateUnWeighted = 0.5m,
               WeightingFactor = "W-Factor",
           };

        private static readonly LARSLearningDelivery LARSLearningDeliveryTestValue =
             new LARSLearningDelivery()
             {
                 LearnAimRef = "123456",
                 EnglandFEHEStatus = "F",
                 EnglPrscID = 2,
                 FrameworkCommonComponent = 100,
             };

        private static readonly LARSAnnualValue LARSAnnualValueTestValue =
           new LARSAnnualValue()
           {
               LearnAimRef = "123456",
               EffectiveFrom = new DateTime(2000, 01, 01),
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
               EffectiveFrom = new DateTime(2000, 01, 01),
           };

        private static readonly LARSLearningDeliveryCategory LARSLearningDeliveryCategoryTestValue =
           new LARSLearningDeliveryCategory()
           {
               LearnAimRef = "123456",
               CategoryRef = 300,
               EffectiveFrom = new DateTime(2000, 01, 01),
               EffectiveTo = null,
           };

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

        private string PostcodesCurrentVersion => "Version_002";

        private IList<SfaAreaCost> SFAAreaCostList(SfaAreaCost sfaAreaCost)
        {
            return new List<SfaAreaCost>
            {
                SfaAreaCostTestValue,
            };
        }

        private IList<SfaDisadvantage> SFADisadvantageList(SfaDisadvantage sfaDisadvantage)
        {
            return new List<SfaDisadvantage>
            {
                SfaDisadvantageTestValue,
            };
        }

        private SfaAreaCost SfaAreaCostTestValue => new SfaAreaCost
        {
            Postcode = "CV1 2WT",
            AreaCostFactor = 1.5m,
            EffectiveFrom = new DateTime(2000, 01, 01),
            EffectiveTo = null,
        };

        private SfaDisadvantage SfaDisadvantageTestValue => new SfaDisadvantage
        {
            Postcode = "CV1 2WT",
            Uplift = 1.10420m,
            EffectiveFrom = new DateTime(2000, 01, 01),
            EffectiveTo = null,
        };

        private string OrgVersionTestValue => "Version_003";

        private IList<OrgFunding> OrgFundingList(OrgFunding orgFunding)
        {
            return new List<OrgFunding>
            {
                orgFunding,
            };
        }

        private static readonly OrgFunding OrgFundingTestValue =
            new OrgFunding
            {
                UKPRN = 10006341,
                OrgFundFactor = "TRANSITION FACTOR",
                OrgFundFactType = "Adult Skills",
                OrgFundFactValue = "1.03920",
                OrgFundEffectiveFrom = new DateTime(2018, 08, 01),
                OrgFundEffectiveTo = new DateTime(2019, 07, 31),
            };

        private IList<LargeEmployers> LargeEmployersList(LargeEmployers largeEmployers)
        {
            return new List<LargeEmployers>
            {
                largeEmployers,
            };
        }

        private static readonly LargeEmployers LargeEmployersTestValue =
            new LargeEmployers
            {
                ERN = 107733730,
                EffectiveFrom = new DateTime(2017, 08, 01),
                EffectiveTo = null,
            };
    }
}
