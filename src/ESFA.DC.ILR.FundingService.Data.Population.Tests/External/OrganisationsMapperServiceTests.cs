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
            var expectedOrgFundingDictionary = new Dictionary<int, IReadOnlyCollection<OrgFunding>>
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
            NewService().MapOrgFundings(null).Should().BeEquivalentTo(new Dictionary<int, IReadOnlyCollection<OrgFunding>>());
        }

        [Fact]
        public void MapOrgFundings_Empty()
        {
            NewService().MapOrgFundings(new List<ReferenceDataService.Model.Organisations.Organisation>()).Should().BeEquivalentTo(new Dictionary<int, IReadOnlyCollection<OrgFunding>>());
        }

        [Fact]
        public void MapCampusIdentifiers()
        {
            var expectedDictionary = new Dictionary<string, IReadOnlyCollection<CampusIdentifierSpecResource>>
            {
                {
                    "CampId1", new List<CampusIdentifierSpecResource>
                    {
                        new CampusIdentifierSpecResource
                        {
                            SpecialistResources = "N",
                            CampusIdentifier = "CampId1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new CampusIdentifierSpecResource
                        {
                            SpecialistResources = "Y",
                            CampusIdentifier = "CampId1",
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                },
                {
                    "CampId2", new List<CampusIdentifierSpecResource>
                    {
                        new CampusIdentifierSpecResource
                        {
                            SpecialistResources = "Y",
                            CampusIdentifier = "CampId2",
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        },
                    }
                }
            };

            var organisations = new List<ReferenceDataService.Model.Organisations.Organisation>
            {
                new ReferenceDataService.Model.Organisations.Organisation
                {
                    UKPRN = 1,
                    PartnerUKPRN = true,
                    CampusIdentifers = new List<ReferenceDataService.Model.Organisations.OrganisationCampusIdentifier>
                    {
                        new ReferenceDataService.Model.Organisations.OrganisationCampusIdentifier
                        {
                            UKPRN = 1,
                            CampusIdentifier = "CampId1",
                            SpecialistResources = new List<ReferenceDataService.Model.Organisations.OrganisationCampusIdSpecialistResource>
                            {
                                new ReferenceDataService.Model.Organisations.OrganisationCampusIdSpecialistResource
                                {
                                    IsSpecialistResource = false,
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                    EffectiveTo = new DateTime(2018, 9, 1)
                                },
                                new ReferenceDataService.Model.Organisations.OrganisationCampusIdSpecialistResource
                                {
                                    IsSpecialistResource = true,
                                    EffectiveFrom = new DateTime(2018, 9, 2)
                                },
                            }
                        }
                    }
                },
                new ReferenceDataService.Model.Organisations.Organisation
                {
                    UKPRN = 2,
                    PartnerUKPRN = true,
                    CampusIdentifers = new List<ReferenceDataService.Model.Organisations.OrganisationCampusIdentifier>
                    {
                        new ReferenceDataService.Model.Organisations.OrganisationCampusIdentifier
                        {
                            UKPRN = 2,
                            CampusIdentifier = "CampId2",
                            SpecialistResources = new List<ReferenceDataService.Model.Organisations.OrganisationCampusIdSpecialistResource>
                            {
                                new ReferenceDataService.Model.Organisations.OrganisationCampusIdSpecialistResource
                                {
                                    IsSpecialistResource = true,
                                    EffectiveFrom = new DateTime(2018, 9, 1)
                                },
                            }
                        }
                    }
                }
            };

            var result = NewService().MapCampusIdentifiers(organisations);

            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedDictionary);
        }

        [Fact]
        public void MapCampusIdentifiers_Null()
        {
            NewService().MapCampusIdentifiers(null).Should().BeEquivalentTo(new Dictionary<string, IReadOnlyCollection<CampusIdentifierSpecResource>>());
        }

        [Fact]
        public void MapCampusIdentifiers_Empty()
        {
            NewService().MapCampusIdentifiers(new List<ReferenceDataService.Model.Organisations.Organisation>()).Should().BeEquivalentTo(new Dictionary<string, IReadOnlyCollection<CampusIdentifierSpecResource>>());
        }

        [Fact]
        public void MapPostcodeSpecialistResources()
        {
            var expectedDictionary = new Dictionary<int, IReadOnlyCollection<PostcodeSpecialistResource>>
            {
                {
                    1, new List<PostcodeSpecialistResource>
                    {
                        new PostcodeSpecialistResource
                        {
                            SpecialistResources = "N",
                            Postcode = "Postcode1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new PostcodeSpecialistResource
                        {
                            SpecialistResources = "Y",
                            Postcode = "Postcode1",
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        },
                        new PostcodeSpecialistResource
                        {
                            SpecialistResources = "Y",
                            Postcode = "Postcode2",
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        },
                    }
                },
                {
                    2, new List<PostcodeSpecialistResource>
                    {
                        new PostcodeSpecialistResource
                        {
                            SpecialistResources = "Y",
                            Postcode = "Postcode2",
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        },
                    }
                }
            };

            var organisations = new List<ReferenceDataService.Model.Organisations.Organisation>
            {
                new ReferenceDataService.Model.Organisations.Organisation
                {
                    UKPRN = 1,
                    PartnerUKPRN = true,
                    PostcodeSpecialistResources = new List<ReferenceDataService.Model.Organisations.OrganisationPostcodeSpecialistResource>
                    {
                        new ReferenceDataService.Model.Organisations.OrganisationPostcodeSpecialistResource
                        {
                            UKPRN = 1,
                            SpecialistResources = "N",
                            Postcode = "Postcode1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new ReferenceDataService.Model.Organisations.OrganisationPostcodeSpecialistResource
                        {
                            UKPRN = 1,
                            SpecialistResources = "Y",
                            Postcode = "Postcode1",
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        },
                        new ReferenceDataService.Model.Organisations.OrganisationPostcodeSpecialistResource
                        {
                            UKPRN = 1,
                            SpecialistResources = "Y",
                            Postcode = "Postcode2",
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        },
                    }
                },
                new ReferenceDataService.Model.Organisations.Organisation
                {
                    UKPRN = 2,
                    PartnerUKPRN = true,
                    PostcodeSpecialistResources = new List<ReferenceDataService.Model.Organisations.OrganisationPostcodeSpecialistResource>
                    {
                        new ReferenceDataService.Model.Organisations.OrganisationPostcodeSpecialistResource
                        {
                            UKPRN = 2,
                            SpecialistResources = "Y",
                            Postcode = "Postcode2",
                            EffectiveFrom = new DateTime(2018, 9, 1)
                        }
                    }
                }
            };

            var result = NewService().MapPostcodeSpecialistResources(organisations);

            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedDictionary);
        }

        [Fact]
        public void MapPostcodeSpecialistResources_Null()
        {
            NewService().MapPostcodeSpecialistResources(null).Should().BeEquivalentTo(new Dictionary<int, IReadOnlyCollection<PostcodeSpecialistResource>>());
        }

        [Fact]
        public void MapPostcodeSpecialistResources_Empty()
        {
            NewService().MapPostcodeSpecialistResources(new List<ReferenceDataService.Model.Organisations.Organisation>()).Should().BeEquivalentTo(new Dictionary<int, IReadOnlyCollection<PostcodeSpecialistResource>>());
        }

        private OrganisationsMapperService NewService()
        {
            return new OrganisationsMapperService();
        }
    }
}
