using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class OrganisationsMapperServiceTests
    {
        [Fact]
        public void MapOrgFundings()
        {
            var expectedOrgFundingDictionary = ExpectedOrgFundingDictionary();

            var organisations = new List<ReferenceDataService.Model.Organisations.Organisation>
            {
                new ReferenceDataService.Model.Organisations.Organisation
                {
                    UKPRN = 1,
                    PartnerUKPRN = true,
                    OrganisationFundings = new List<ReferenceDataService.Model.Organisations.OrganisationFunding>
                    {
                        new ReferenceDataService.Model.Organisations.OrganisationFunding
                        {
                           OrgFundFactValue = "1.0",
                           OrgFundFactType = "Type",
                           OrgFundFactor = "Factor",
                           EffectiveFrom = new DateTime(2018, 8, 1),
                           EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new ReferenceDataService.Model.Organisations.OrganisationFunding
                        {
                           OrgFundFactValue = "2.0",
                           OrgFundFactType = "Type",
                           OrgFundFactor = "Factor",
                           EffectiveFrom = new DateTime(2018, 9, 2)
                        },
                    },
                }
            };

            var result = NewService().MapOrgFundings(organisations);

            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(expectedOrgFundingDictionary);
        }

        [Fact]
        public void MapOrgFundings_Null()
        {
            NewService().MapOrgFundings(null).Should().BeNull();
        }

        private IDictionary<int, IReadOnlyCollection<OrgFunding>> ExpectedOrgFundingDictionary()
        {
            return new Dictionary<int, IReadOnlyCollection<OrgFunding>>
            {
                {
                    1,
                    new List<OrgFunding>
                    {
                        new OrgFunding
                        {
                            UKPRN = 1,
                            OrgFundFactValue = "1.0",
                            OrgFundFactType = "Type",
                            OrgFundFactor = "Factor",
                            OrgFundEffectiveFrom = new DateTime(2018, 8, 1),
                            OrgFundEffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new OrgFunding
                        {
                            UKPRN = 1,
                            OrgFundFactValue = "2.0",
                            OrgFundFactType = "Type",
                            OrgFundFactor = "Factor",
                            OrgFundEffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                }
            };
        }

        private OrganisationsMapperService NewService()
        {
            return new OrganisationsMapperService();
        }
    }
}
