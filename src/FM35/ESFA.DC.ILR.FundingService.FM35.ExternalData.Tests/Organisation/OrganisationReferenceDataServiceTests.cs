using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Tests.Organisation
{
    public class OrganisationReferenceDataServiceTests
    {
        /// <summary>
        /// Return Organisation Data.
        /// </summary>
        [Fact(DisplayName = "OrgVersion - Does exist"), Trait("OrganisationReferenceDataService", "Unit")]
        public void OrgVersion_Exists()
        {
            // ARRANGE
            var organisationServiceMock = OrgVersionTestRun();

            // ACT
            var orgVersion = organisationServiceMock.OrganisationVersion();

            // ASSERT
            orgVersion.Should().NotBeNull();
        }

        /// <summary>
        /// Return Organisation Data.
        /// </summary>
        [Fact(DisplayName = "OrgVersion - Correct"), Trait("OrganisationReferenceDataService", "Unit")]
        public void OrgVersion_Correct()
        {
            // ARRANGE
            var organisationServiceMock = OrgVersionTestRun();

            // ACT
            var orgVersion = organisationServiceMock.OrganisationVersion();

            // ASSERT
            orgVersion.Should().Be("Version_003");
        }

        /// <summary>
        /// Return Organisation Data.
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Does exist"), Trait("OrganisationReferenceDataService", "Unit")]
        public void OrgFunding_Exists()
        {
            // ARRANGE
            var organisationServiceMock = OrgFundingTestRun();

            // ACT
            var orgFunding = organisationServiceMock.OrganisationFundingForUKPRN(ukprn);

            // ASSERT
            orgFunding.Should().NotBeNull();
        }

        /// <summary>
        /// Return Organisation Data.
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Correct"), Trait("OrganisationReferenceDataService", "Unit")]
        public void OrgFunding_Correct()
        {
            // ARRANGE
            var organisationServiceMock = OrgFundingTestRun();

            // ACT
            var orgFunding = organisationServiceMock.OrganisationFundingForUKPRN(ukprn);

            // ASSERT
            orgFunding.Should().BeEquivalentTo(OrgFundingTestValue);
        }

        private readonly Mock<IReferenceDataCache> referenceDataCacheMock = new Mock<IReferenceDataCache>();

        private int ukprn = 12345678;

        private IOrganisationReferenceDataService MockTestObject(IReferenceDataCache @object)
        {
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(@object);

            return organisationReferenceDataService;
        }

        private IOrganisationReferenceDataService OrgVersionTestRun()
        {
            var organisationMock = referenceDataCacheMock;
            organisationMock.SetupGet(rdc => rdc.OrgVersion).Returns(OrgVersionTestValue.Version);

            return MockTestObject(organisationMock.Object);
        }

        private IOrganisationReferenceDataService OrgFundingTestRun()
        {
            var organisationMock = referenceDataCacheMock;
            organisationMock.SetupGet(rdc => rdc.OrgFunding).Returns(new Dictionary<long, IEnumerable<OrgFunding>>
            {
                { ukprn, OrgFundingList(OrgFundingTestValue) },
            });

            return MockTestObject(organisationMock.Object);
        }

        private static readonly OrgVersion OrgVersionTestValue =
            new OrgVersion
            {
                Version = "Version_003"
            };

        private IList<OrgFunding> OrgFundingList(OrgFunding orgFundingData)
        {
            return new List<OrgFunding>
            {
                orgFundingData,
            };
        }

        private static readonly OrgFunding OrgFundingTestValue =
           new OrgFunding
           {
               UKPRN = 12345678,
               OrgFundFactor = "Factor",
               OrgFundFactType = "Type",
               OrgFundFactValue = "Value",
               OrgFundEffectiveFrom = new DateTime(2018, 08, 01),
               OrgFundEffectiveTo = new DateTime(2019, 07, 31),
           };
    }
}
