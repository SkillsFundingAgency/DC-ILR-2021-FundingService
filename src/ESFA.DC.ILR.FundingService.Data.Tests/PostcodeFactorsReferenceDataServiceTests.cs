using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class PostcodeFactorsReferenceDataServiceTests
    {
        [Fact]
        public void PostcodeFactorsCurrentVersion_Exists()
        {
            var postcodeFactorsVersion = "version";

            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeCurrentVersion).Returns(postcodeFactorsVersion);

            NewService(referenceDataCacheMock.Object).PostcodesCurrentVersion().Should().Be(postcodeFactorsVersion);
        }

        [Fact]
        public void SFA_AreaCost()
        {
            var postcode = "postcode";
            var sfaAreaCosts = new List<SfaAreaCost>();

            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.SfaAreaCost)
                .Returns(new Dictionary<string, IEnumerable<SfaAreaCost>>()
                {
                    { postcode, sfaAreaCosts }
                });

            NewService(referenceDataCacheMock.Object).SFAAreaCostsForPostcode(postcode).Should().BeSameAs(sfaAreaCosts);
        }

        [Fact]
        public void SFA_AreaCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IReferenceDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.SfaAreaCost)
                .Returns(new Dictionary<string, IEnumerable<SfaAreaCost>>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).SFAAreaCostsForPostcode("notPostcode").Should().BeNull();
        }
        
        private PostcodesReferenceDataService NewService(IReferenceDataCache referenceDataCache = null)
        {
            return new PostcodesReferenceDataService(referenceDataCache);
        }
    }
}
