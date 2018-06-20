using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Tests.Postcodes
{
    public class PostcodesReferenceDataServiceTests
    {
        /// <summary>
        /// Return Postcodes Data.
        /// </summary>
        [Fact(DisplayName = "PostcodesVersion - Does exist"), Trait("PostcodesReferenceDataService", "Unit")]
        public void PostcodesVersion_Exists()
        {
            // ARRANGE
            var postcodesServiceMock = PostcodesVersionTestRun();

            // ACT
            var postcodeVersion = postcodesServiceMock.PostcodesCurrentVersion();

            // ASSERT
            postcodeVersion.Should().NotBeNull();
        }

        /// <summary>
        /// Return Postcodes Data.
        /// </summary>
        [Fact(DisplayName = "PostcodesVersion - Correct"), Trait("PostcodesReferenceDataService", "Unit")]
        public void PostcodesVersion_Correct()
        {
            // ARRANGE
            var postcodesServiceMock = PostcodesVersionTestRun();

            // ACT
            var postcodeVersion = postcodesServiceMock.PostcodesCurrentVersion();

            // ASSERT
            postcodeVersion.Should().Be("Version_002");
        }

        /// <summary>
        /// Return Postcodes Data.
        /// </summary>
        [Fact(DisplayName = "SfaAreaCost - Does exist"), Trait("PostcodesReferenceDataService", "Unit")]
        public void SfaAreaCost_Exists()
        {
            // ARRANGE
            var postcodesServiceMock = SfaAreaCostTestRun();

            // ACT
            var sfaAreaCost = postcodesServiceMock.SFAAreaCostsForPostcode(postcode);

            // ASSERT
            sfaAreaCost.Should().NotBeNull();
        }

        /// <summary>
        /// Return Postcodes Data.
        /// </summary>
        [Fact(DisplayName = "SfaAreaCost - Correct"), Trait("PostcodesReferenceDataService", "Unit")]
        public void SfaAreaCost_Correct()
        {
            // ARRANGE
            var postcodesServiceMock = SfaAreaCostTestRun();

            // ACT
            var sfaAreaCost = postcodesServiceMock.SFAAreaCostsForPostcode(postcode);

            // ASSERT
            sfaAreaCost.Should().BeEquivalentTo(SfaAreaCostTestValue);
        }

        /// <summary>
        /// Return Postcodes Data.
        /// </summary>
        [Fact(DisplayName = "SfaDisadvantageCost - Does exist"), Trait("PostcodesReferenceDataService", "Unit")]
        public void SfaDisadvantageCost_Exists()
        {
            // ARRANGE
            var postcodesServiceMock = SfaDisadvantageTestRun();

            // ACT
            var sfaDisadvantageCost = postcodesServiceMock.SFADisadvantagesForPostcode(postcode);

            // ASSERT
            sfaDisadvantageCost.Should().NotBeNull();
        }

        /// <summary>
        /// Return Postcodes Data.
        /// </summary>
        [Fact(DisplayName = "SfaDisadvantageCost - Correct"), Trait("PostcodesReferenceDataService", "Unit")]
        public void SfaDisadvantageCost_Correct()
        {
            // ARRANGE
            var postcodesServiceMock = SfaDisadvantageTestRun();

            // ACT
            var sfaDisadvantageCost = postcodesServiceMock.SFADisadvantagesForPostcode(postcode);

            // ASSERT
            sfaDisadvantageCost.Should().BeEquivalentTo(SfaDisadvantageTestValue);
        }

        private readonly Mock<IReferenceDataCache> referenceDataCacheMock = new Mock<IReferenceDataCache>();

        private readonly string postcode = "CV1 2WT";

        private IPostcodesReferenceDataService MockTestObject(IReferenceDataCache @object)
        {
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(@object);

            return postcodesReferenceDataService;
        }

        private IPostcodesReferenceDataService PostcodesVersionTestRun()
        {
            var postcodesMock = referenceDataCacheMock;
            postcodesMock.SetupGet(rdc => rdc.PostcodeCurrentVersion).Returns(PostCodesVersionTestValue.PostcodeFactorsCurrentVersion);

            return MockTestObject(postcodesMock.Object);
        }

        private IPostcodesReferenceDataService SfaAreaCostTestRun()
        {
            var postcodesMock = referenceDataCacheMock;
            postcodesMock.SetupGet(rdc => rdc.SfaAreaCost).Returns(new Dictionary<string, IEnumerable<SfaAreaCost>>
            {
                { postcode, SfaAreaCostList(SfaAreaCostTestValue) },
            });

            return MockTestObject(postcodesMock.Object);
        }

        private IPostcodesReferenceDataService SfaDisadvantageTestRun()
        {
            var postcodesMock = referenceDataCacheMock;
            postcodesMock.SetupGet(rdc => rdc.SfaDisadvantage).Returns(new Dictionary<string, IEnumerable<SfaDisadvantage>>
            {
                { postcode, SfaDisadvantageList(SfaDisadvantageTestValue) },
            });

            return MockTestObject(postcodesMock.Object);
        }

        private static readonly PostCodeVersion PostCodesVersionTestValue =
            new PostCodeVersion
            {
                PostcodeFactorsCurrentVersion = "Version_002",
            };

        private IList<SfaAreaCost> SfaAreaCostList(SfaAreaCost sfaAreaCostData)
        {
            return new List<SfaAreaCost>
            {
                sfaAreaCostData,
            };
        }

        private static readonly SfaAreaCost SfaAreaCostTestValue =
           new SfaAreaCost
           {
               Postcode = "CV1 2WT",
               AreaCostFactor = 1.2m,
               EffectiveFrom = new DateTime(2000, 08, 01),
               EffectiveTo = null,
           };

        private IList<SfaDisadvantage> SfaDisadvantageList(SfaDisadvantage sfaDisadvantageData)
        {
            return new List<SfaDisadvantage>
            {
                sfaDisadvantageData,
            };
        }

        private static readonly SfaDisadvantage SfaDisadvantageTestValue =
           new SfaDisadvantage
           {
               Postcode = "CV1 2WT",
               Uplift = 1.543m,
               EffectiveFrom = new DateTime(2000, 08, 01),
               EffectiveTo = null,
           };
    }
}
