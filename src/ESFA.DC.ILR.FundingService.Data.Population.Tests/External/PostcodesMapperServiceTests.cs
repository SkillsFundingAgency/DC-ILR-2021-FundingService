using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class PostcodesMapperServiceTests
    {
        [Fact]
        public void MapPostcodes()
        {
            var expectedPostcodeDictionary = ExpectedPostcodeDictionary();

            var postcodes = new List<ReferenceDataService.Model.Postcodes.Postcode>
            {
                new ReferenceDataService.Model.Postcodes.Postcode
                {
                     PostCode = "PostCode1",
                     CareerLearningPilots = new List<ReferenceDataService.Model.Postcodes.CareerLearningPilot>
                     {
                         new ReferenceDataService.Model.Postcodes.CareerLearningPilot
                         {
                            AreaCode = "AreaCode1",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                         }
                     },
                     DasDisadvantages = new List<ReferenceDataService.Model.Postcodes.DasDisadvantage>
                     {
                         new ReferenceDataService.Model.Postcodes.DasDisadvantage
                         {
                            Uplift = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                         },
                         new ReferenceDataService.Model.Postcodes.DasDisadvantage
                         {
                            Uplift = 2.0m,
                            EffectiveFrom = new DateTime(2018, 9, 1)
                         }
                     }
                },
                new ReferenceDataService.Model.Postcodes.Postcode
                {
                     PostCode = "PostCode2",
                     EfaDisadvantages = new List<ReferenceDataService.Model.Postcodes.EfaDisadvantage>
                     {
                         new ReferenceDataService.Model.Postcodes.EfaDisadvantage
                         {
                            Uplift = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                         },
                         new ReferenceDataService.Model.Postcodes.EfaDisadvantage
                         {
                            Uplift = 2.0m,
                            EffectiveFrom = new DateTime(2018, 9, 1)
                         },
                         new ReferenceDataService.Model.Postcodes.EfaDisadvantage
                         {
                            Uplift = 3.0m,
                            EffectiveFrom = new DateTime(2018, 10, 1)
                         }
                     }
                },
                new ReferenceDataService.Model.Postcodes.Postcode
                {
                     PostCode = "PostCode3",
                     SfaAreaCosts = new List<ReferenceDataService.Model.Postcodes.SfaAreaCost>
                     {
                         new ReferenceDataService.Model.Postcodes.SfaAreaCost
                         {
                            AreaCostFactor = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                         }
                     },
                     SfaDisadvantages = new List<ReferenceDataService.Model.Postcodes.SfaDisadvantage>
                     {
                         new ReferenceDataService.Model.Postcodes.SfaDisadvantage
                         {
                            Uplift = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                         }
                     }
                }
            };

            var result = NewService().MapPostcodes(postcodes);

            result.Should().HaveCount(3);
            result["Postcode1"].Should().BeEquivalentTo(expectedPostcodeDictionary["Postcode1"]);
            result["Postcode2"].Should().BeEquivalentTo(expectedPostcodeDictionary["Postcode2"]);
            result["Postcode3"].Should().BeEquivalentTo(expectedPostcodeDictionary["Postcode3"]);
        }

        [Fact]
        public void MapPostcodes_Null()
        {
            NewService().MapPostcodes(null).Should().BeNull();
        }

        private IDictionary<string, PostcodeRoot> ExpectedPostcodeDictionary()
        {
            return new Dictionary<string, PostcodeRoot>
            {
                {
                    "Postcode1",
                    new PostcodeRoot
                    {
                        Postcode = "PostCode1",
                        CareerLearningPilots = new List<CareerLearningPilot>
                        {
                            new CareerLearningPilot
                            {
                                Postcode = "PostCode1",
                                AreaCode = "AreaCode1",
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
                        },
                        DasDisadvantages = new List<DasDisadvantage>
                        {
                            new DasDisadvantage
                            {
                                Postcode = "PostCode1",
                                Uplift = 1.0m,
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            },
                            new DasDisadvantage
                            {
                                Postcode = "PostCode1",
                                Uplift = 2.0m,
                                EffectiveFrom = new DateTime(2018, 9, 1)
                            }
                        }
                    }
                },
                {
                    "Postcode2",
                    new PostcodeRoot
                    {
                        Postcode = "PostCode2",
                        EfaDisadvantages = new List<EfaDisadvantage>
                        {
                            new EfaDisadvantage
                            {
                                Postcode = "PostCode2",
                                Uplift = 1.0m,
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            },
                            new EfaDisadvantage
                            {
                                Postcode = "PostCode2",
                                Uplift = 2.0m,
                                EffectiveFrom = new DateTime(2018, 9, 1)
                            },
                            new EfaDisadvantage
                            {
                                Postcode = "PostCode2",
                                Uplift = 3.0m,
                                EffectiveFrom = new DateTime(2018, 10, 1)
                            }
                        }
                    }
                },
                 {
                    "Postcode3",
                    new PostcodeRoot
                    {
                        Postcode = "PostCode3",
                        SfaDisadvantages = new List<SfaDisadvantage>
                        {
                            new SfaDisadvantage
                            {
                                Postcode = "PostCode3",
                                Uplift = 1.0m,
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
                        },
                        SfaAreaCosts = new List<SfaAreaCost>
                        {
                            new SfaAreaCost
                            {
                                Postcode = "PostCode3",
                                AreaCostFactor = 1.0m,
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
                        }
                    }
                }
            };
        }

        private PostcodesMapperService NewService()
        {
            return new PostcodesMapperService();
        }
    }
}
