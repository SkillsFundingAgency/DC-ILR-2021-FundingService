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

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { SfaAreaCosts = sfaAreaCosts } }
                });

            NewService(referenceDataCacheMock.Object).SFAAreaCostsForPostcode(postcode).Should().BeSameAs(sfaAreaCosts);
        }

        [Fact]
        public void SFA_AreaCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
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

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { SfaDisadvantages = sfaDisadvantages } }
                });

            NewService(referenceDataCacheMock.Object).SFADisadvantagesForPostcode(postcode).Should().BeSameAs(sfaDisadvantages);
        }

        [Fact]
        public void SfaDisadvantageCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).SFADisadvantagesForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void DasDisadvantageCost()
        {
            var postcode = "postcode";
            var dasDisadvantages = new List<DasDisadvantage>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { DasDisadvantages = dasDisadvantages } }
                });

            NewService(referenceDataCacheMock.Object).DASDisadvantagesForPostcode(postcode).Should().BeSameAs(dasDisadvantages);
        }

        [Fact]
        public void DasDisadvantageCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).DASDisadvantagesForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void EfaDisadvantageCost()
        {
            var postcode = "postcode";
            var efaDisadvantages = new List<EfaDisadvantage>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { EfaDisadvantages = efaDisadvantages } }
                });

            NewService(referenceDataCacheMock.Object).EFADisadvantagesForPostcode(postcode).Should().BeSameAs(efaDisadvantages);
        }

        [Fact]
        public void EfaDisadvantageCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).EFADisadvantagesForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void CareerLearningPilot()
        {
            var postcode = "postcode";
            var careerLearningPilot = new List<CareerLearningPilot>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { CareerLearningPilots = careerLearningPilot } }
                });

            NewService(referenceDataCacheMock.Object).CareerLearningPilotsForPostcode(postcode).Should().BeSameAs(careerLearningPilot);
        }

        [Fact]
        public void CareerLearningPilot_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).CareerLearningPilotsForPostcode("notPostcode").Should().BeEmpty();
        }

        private PostcodesReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new PostcodesReferenceDataService(referenceDataCache);
        }
    }
}
