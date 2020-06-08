using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class OrganisationReferenceDataServiceTests
    {
        [Fact]
        public void OrgVersion()
        {
            var version = "version";

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.OrgVersion).Returns(version);

            NewService(referenceDataCacheMock.Object).OrganisationVersion().Should().Be(version);
        }

        [Fact]
        public void OrgFundingForUKPRN()
        {
            var ukprn = 1234;

            var orgFundings = new List<OrgFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.OrgFunding)
                .Returns(new Dictionary<int, IReadOnlyCollection<OrgFunding>>
                {
                    { ukprn, orgFundings },
                });

            NewService(referenceDataCacheMock.Object).OrganisationFundingForUKPRN(ukprn).Should().BeSameAs(orgFundings);
        }

        [Fact]
        public void OrgFundingForUKPRN_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.OrgFunding).Returns(new Dictionary<int, IReadOnlyCollection<OrgFunding>>
            {
                { 1234, null },
            });

            NewService(referenceDataCacheMock.Object).OrganisationFundingForUKPRN(5678).Should().BeEmpty();
        }

        [Fact]
        public void SpecialistResourcesForCampusIdentifier()
        {
            var campId = "Id1234";

            var specResources = new List<CampusIdentifierSpecResource>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.CampusIdentifierSpecResources)
                .Returns(new Dictionary<string, IReadOnlyCollection<CampusIdentifierSpecResource>>
                {
                    { campId, specResources },
                });

            NewService(referenceDataCacheMock.Object).SpecialistResourcesForCampusIdentifier(campId).Should().BeSameAs(specResources);
        }

        [Fact]
        public void SpecialistResourcesForCampusIdentifier_NotExists()
        {
            var campId = "Id1234";

            var specResources = new List<CampusIdentifierSpecResource>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.CampusIdentifierSpecResources)
                .Returns(new Dictionary<string, IReadOnlyCollection<CampusIdentifierSpecResource>>
                {
                    { campId, specResources },
                });

            NewService(referenceDataCacheMock.Object).SpecialistResourcesForCampusIdentifier("Id5678").Should().BeEmpty();
        }

        [Fact]
        public void PostcodeSpecialistResourcesForUkprn()
        {
            var specialistResources = new List<PostcodeSpecialistResource>
            {
                new PostcodeSpecialistResource
                {
                    Postcode = "Postcode",
                    SpecialistResources = "Y",
                    EffectiveFrom = new System.DateTime(2020, 8, 1)
                }
            };

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeSpecResources)
              .Returns(new Dictionary<int, IReadOnlyCollection<PostcodeSpecialistResource>>
              {
                    { 1, specialistResources }
              });

            NewService(referenceDataCacheMock.Object).PostcodeSpecialistResourcesForUkprn(1).Should().BeSameAs(specialistResources);
        }

        [Fact]
        public void PostcodeSpecialistResourcesForUkprn_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeSpecResources)
                .Returns(new Dictionary<int, IReadOnlyCollection<PostcodeSpecialistResource>>
                {
                   { 1, new List<PostcodeSpecialistResource>() },
                });

            NewService(referenceDataCacheMock.Object).PostcodeSpecialistResourcesForUkprn(2).Should().BeEmpty();
        }

        [Fact]
        public void PostcodeSpecialistResourcesForUkprn_Null()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeSpecResources)
                .Returns(new Dictionary<int, IReadOnlyCollection<PostcodeSpecialistResource>>());

            NewService(referenceDataCacheMock.Object).PostcodeSpecialistResourcesForUkprn(2).Should().BeEmpty();
        }

        private OrganisationReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new OrganisationReferenceDataService(referenceDataCache);
        }
    }
}
