using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes;
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

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeCurrentVersion).Returns(postcodeFactorsVersion);

            NewService(referenceDataCacheMock.Object).PostcodesCurrentVersion().Should().Be(postcodeFactorsVersion);
        }

        [Fact]
        public void SFA_AreaCost()
        {
            var postcode = "postcode";
            var sfaAreaCosts = new List<SfaAreaCost>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

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
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.SfaAreaCost)
                .Returns(new Dictionary<string, IEnumerable<SfaAreaCost>>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).SFAAreaCostsForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void SfaDisadvantageCost()
        {
            var postcode = "postcode";
            var sfaDisadvantages = new List<SfaDisadvantage>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.SfaDisadvantage)
                .Returns(new Dictionary<string, IEnumerable<SfaDisadvantage>>()
                {
                    { postcode, sfaDisadvantages }
                });

            NewService(referenceDataCacheMock.Object).SFADisadvantagesForPostcode(postcode).Should().BeSameAs(sfaDisadvantages);
        }

        [Fact]
        public void SfaDisadvantageCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.SfaDisadvantage)
                .Returns(new Dictionary<string, IEnumerable<SfaDisadvantage>>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).SFADisadvantagesForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void EfaDisadvantageCost()
        {
            var postcode = "postcode";
            var efaDisadvantages = new List<EfaDisadvantage>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.EfaDisadvantage)
                .Returns(new Dictionary<string, IEnumerable<EfaDisadvantage>>()
                {
                    { postcode, efaDisadvantages }
                });

            NewService(referenceDataCacheMock.Object).EFADisadvantagesForPostcode(postcode).Should().BeSameAs(efaDisadvantages);
        }

        [Fact]
        public void EfaDisadvantageCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.EfaDisadvantage)
                .Returns(new Dictionary<string, IEnumerable<EfaDisadvantage>>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).EFADisadvantagesForPostcode("notPostcode").Should().BeEmpty();
        }

        private PostcodesReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new PostcodesReferenceDataService(referenceDataCache);
        }
    }
}
