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
                .Returns(new Dictionary<long, IEnumerable<OrgFunding>>
                {
                    { ukprn, orgFundings },
                });

            NewService(referenceDataCacheMock.Object).OrganisationFundingForUKPRN(ukprn).Should().BeSameAs(orgFundings);
        }

        [Fact]
        public void OrgFundingForUKPRN_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.OrgFunding).Returns(new Dictionary<long, IEnumerable<OrgFunding>>
            {
                { 1234, null },
            });

            NewService(referenceDataCacheMock.Object).OrganisationFundingForUKPRN(5678).Should().BeEmpty();
        }
        
        private OrganisationReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new OrganisationReferenceDataService(referenceDataCache);
        }
    }
}
