using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class PostcodesDataRetrievalServiceTests
    {
        [Fact]
        public void VersionInfos()
        {
            var postcodesMock = new Mock<IPostcodes>();

            var versionInfos = NewService(postcodesMock.Object).VersionInfos;

            postcodesMock.VerifyGet(p => p.VersionInfos);
        }

        [Fact]
        public void SfaPostcodeAreaCosts()
        {
            var postcodesMock = new Mock<IPostcodes>();

            var sfaPostcodeAreaCosts = NewService(postcodesMock.Object).SfaPostcodeAreaCosts;

            postcodesMock.VerifyGet(p => p.SFA_PostcodeAreaCost);
        }

        [Fact]
        public void SfaPostcodeDisadvantages()
        {
            var postcodesMock = new Mock<IPostcodes>();

            var sfaPostcodeDisadvantages = NewService(postcodesMock.Object).SfaPostcodeAreaCosts;

            postcodesMock.VerifyGet(p => p.SFA_PostcodeAreaCost);
        }

        [Fact]
        public void UniquePostcodes()
        {
            var message = new TestMessage()
            {
                Learners = new List<TestLearner>()
                {
                    new TestLearner()
                    {
                        Postcode = "A",
                        PostcodePrior = "B",
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                DelLocPostCode = "ABC",
                            },
                            new TestLearningDelivery()
                            {
                                DelLocPostCode = "DEF",
                            },
                            new TestLearningDelivery()
                            {
                                DelLocPostCode = "ABC",
                            }
                        }
                    },
                    new TestLearner
                    {
                        Postcode = "AB",
                        PostcodePrior = "B",
                    }
                }
            };

            var uniquePostcodes = NewService().UniquePostcodes(message).ToList();

            uniquePostcodes.Should().HaveCount(5);
            uniquePostcodes.Should().Contain("A", "B", "AB", "ABC", "DEF");
        }

        [Fact]
        public void VersionInfo()
        {
            var postcodesDataRetrievalServiceMock = NewMock();

            var versionInfos = new List<VersionInfo>()
            {
                new VersionInfo() { VersionNumber = "001" },
                new VersionInfo() { VersionNumber = "010" },
                new VersionInfo() { VersionNumber = "100" },
            }.AsQueryable();

            postcodesDataRetrievalServiceMock.SetupGet(v => v.VersionInfos).Returns(versionInfos);

            postcodesDataRetrievalServiceMock.Object.CurrentVersion().Should().Be("100");
        }

        [Fact]
        public void SfaPostcodeAreaCostsForPostcodes()
        {
            var sfaPostcodeAreaCosts = new List<SFA_PostcodeAreaCost>()
            {
                new SFA_PostcodeAreaCost()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
                    Postcode = "CV1 2WT",
                    AreaCostFactor = 1.2m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = null,
                },
                new SFA_PostcodeAreaCost()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    AreaCostFactor = 1.5m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = new DateTime(2015, 12, 31)
                },
                new SFA_PostcodeAreaCost()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    AreaCostFactor = 2.1m,
                    EffectiveFrom = new DateTime(2016, 01, 01),
                    EffectiveTo = null,
                },
                new SFA_PostcodeAreaCost()
                {
                    Postcode = "Fictional"
                }
            }.AsQueryable();

            var postcodesDataRetrievalServiceMock = NewMock();

            postcodesDataRetrievalServiceMock.SetupGet(p => p.SfaPostcodeAreaCosts).Returns(sfaPostcodeAreaCosts);

            var postcodes = new List<string>() { "CV1 2WT", "CV1 2TT" };

            var sfaAreaCosts = postcodesDataRetrievalServiceMock.Object.SfaAreaCostsForPostcodes(postcodes);

            sfaAreaCosts.Should().HaveCount(2);
            sfaAreaCosts.Should().ContainKeys("CV1 2WT", "CV1 2TT");
            sfaAreaCosts.Should().NotContainKey("Fictional");

            sfaAreaCosts["CV1 2TT"].Should().HaveCount(2);
            sfaAreaCosts["CV1 2WT"].Should().HaveCount(1);
        }

        [Fact]
        public void SfaAreaCostFromEntity()
        {
            var sfaPostcodeAreaCost = new SFA_PostcodeAreaCost()
            {
                MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                Postcode = "CV1 2TT",
                AreaCostFactor = 1.5m,
                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 12, 31),
            };

            var sfaAreaCost = NewService().SfaAreaCostFromEntity(sfaPostcodeAreaCost);

            sfaAreaCost.AreaCostFactor.Should().Be(sfaPostcodeAreaCost.AreaCostFactor);
            sfaAreaCost.EffectiveFrom.Should().Be(sfaPostcodeAreaCost.EffectiveFrom);
            sfaAreaCost.EffectiveTo.Should().Be(sfaPostcodeAreaCost.EffectiveTo);
            sfaAreaCost.Postcode.Should().Be(sfaPostcodeAreaCost.Postcode);
        }

        [Fact]
        public void SfaPostcodeDisadvantagesForPostcodes()
        {
            var sfaPostcodeDisadvantages = new List<SFA_PostcodeDisadvantage>()
            {
                new SFA_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
                    Postcode = "CV1 2WT",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = null,
                },
                new SFA_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = new DateTime(2015, 12, 31)
                },
                new SFA_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    Uplift = 2.1m,
                    EffectiveFrom = new DateTime(2016, 01, 01),
                    EffectiveTo = null,
                },
                new SFA_PostcodeDisadvantage()
                {
                    Postcode = "Fictional"
                }
            }.AsQueryable();

            var postcodesDataRetrievalServiceMock = NewMock();

            postcodesDataRetrievalServiceMock.SetupGet(p => p.SfaPostcodeDisadvantages).Returns(sfaPostcodeDisadvantages);

            var postcodes = new List<string>() { "CV1 2WT", "CV1 2TT" };

            var sfaDisadvantages = postcodesDataRetrievalServiceMock.Object.SfaDisadvantagesForPostcodes(postcodes);

            sfaDisadvantages.Should().HaveCount(2);
            sfaDisadvantages.Should().ContainKeys("CV1 2WT", "CV1 2TT");
            sfaDisadvantages.Should().NotContainKey("Fictional");

            sfaDisadvantages["CV1 2TT"].Should().HaveCount(2);
            sfaDisadvantages["CV1 2WT"].Should().HaveCount(1);
        }

        [Fact]
        public void SfaDisadvantageFromEntity()
        {
            var sfaPostcodeDisadvantage = new SFA_PostcodeDisadvantage()
            {
                MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                Postcode = "CV1 2TT",
                Uplift = 1.5m,
                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 12, 31),
            };

            var sfaDisadvantage = NewService().SfaDisadvantageFromEntity(sfaPostcodeDisadvantage);

            sfaDisadvantage.Uplift.Should().Be(sfaPostcodeDisadvantage.Uplift);
            sfaDisadvantage.EffectiveFrom.Should().Be(sfaPostcodeDisadvantage.EffectiveFrom);
            sfaDisadvantage.EffectiveTo.Should().Be(sfaPostcodeDisadvantage.EffectiveTo);
            sfaDisadvantage.Postcode.Should().Be(sfaPostcodeDisadvantage.Postcode);
        }

        [Fact]
        public void DasPostcodeDisadvantagesForPostcodes()
        {
            var dasPostcodeDisadvantages = new List<DAS_PostcodeDisadvantage>()
            {
                new DAS_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
                    Postcode = "CV1 2WT",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = null,
                },
                new DAS_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = new DateTime(2015, 12, 31)
                },
                new DAS_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    Uplift = 2.1m,
                    EffectiveFrom = new DateTime(2016, 01, 01),
                    EffectiveTo = null,
                },
                new DAS_PostcodeDisadvantage()
                {
                    Postcode = "Fictional"
                }
            }.AsQueryable();

            var postcodesDataRetrievalServiceMock = NewMock();

            postcodesDataRetrievalServiceMock.SetupGet(p => p.DasPostcodeDisadvantages).Returns(dasPostcodeDisadvantages);

            var postcodes = new List<string>() { "CV1 2WT", "CV1 2TT" };

            var dasDisadvantages = postcodesDataRetrievalServiceMock.Object.DasDisadvantagesForPostcodes(postcodes);

            dasDisadvantages.Should().HaveCount(2);
            dasDisadvantages.Should().ContainKeys("CV1 2WT", "CV1 2TT");
            dasDisadvantages.Should().NotContainKey("Fictional");

            dasDisadvantages["CV1 2TT"].Should().HaveCount(2);
            dasDisadvantages["CV1 2WT"].Should().HaveCount(1);
        }

        [Fact]
        public void DasDisadvantageFromEntity()
        {
            var dasPostcodeDisadvantage = new DAS_PostcodeDisadvantage()
            {
                MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                Postcode = "CV1 2TT",
                Uplift = 1.5m,
                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 12, 31),
            };

            var dasDisadvantage = NewService().DasDisadvantageFromEntity(dasPostcodeDisadvantage);

            dasDisadvantage.Uplift.Should().Be(dasPostcodeDisadvantage.Uplift);
            dasDisadvantage.EffectiveFrom.Should().Be(dasPostcodeDisadvantage.EffectiveFrom);
            dasDisadvantage.EffectiveTo.Should().Be(dasPostcodeDisadvantage.EffectiveTo);
            dasDisadvantage.Postcode.Should().Be(dasPostcodeDisadvantage.Postcode);
        }

        [Fact]
        public void EfaPostcodeDisadvantagesForPostcodes()
        {
            var efaPostcodeDisadvantages = new List<EFA_PostcodeDisadvantage>()
            {
                new EFA_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
                    Postcode = "CV1 2WT",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = null,
                },
                new EFA_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = new DateTime(2015, 12, 31)
                },
                new EFA_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    Uplift = 2.1m,
                    EffectiveFrom = new DateTime(2016, 01, 01),
                    EffectiveTo = null,
                },
                new EFA_PostcodeDisadvantage()
                {
                    Postcode = "Fictional"
                }
            }.AsQueryable();

            var postcodesDataRetrievalServiceMock = NewMock();

            postcodesDataRetrievalServiceMock.SetupGet(p => p.EfaPostcodeDisadvantages).Returns(efaPostcodeDisadvantages);

            var postcodes = new List<string>() { "CV1 2WT", "CV1 2TT" };

            var efaDisadvantages = postcodesDataRetrievalServiceMock.Object.EfaDisadvantagesForPostcodes(postcodes);

            efaDisadvantages.Should().HaveCount(2);
            efaDisadvantages.Should().ContainKeys("CV1 2WT", "CV1 2TT");
            efaDisadvantages.Should().NotContainKey("Fictional");

            efaDisadvantages["CV1 2TT"].Should().HaveCount(2);
            efaDisadvantages["CV1 2WT"].Should().HaveCount(1);
        }

        [Fact]
        public void CareerLearningPilotFromEntity()
        {
            var careerLearningPilotPostcode = new CareerLearningPilot_Postcode()
            {
                MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                Postcode = "CV1 2TT",
                AreaCode = "AreaCode",
                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 12, 31),
            };

            var careerLearningPilot = NewService().CareerLearningPilotFromEntity(careerLearningPilotPostcode);

            careerLearningPilotPostcode.AreaCode.Should().Be(careerLearningPilotPostcode.AreaCode);
            careerLearningPilotPostcode.EffectiveFrom.Should().Be(careerLearningPilotPostcode.EffectiveFrom);
            careerLearningPilotPostcode.EffectiveTo.Should().Be(careerLearningPilotPostcode.EffectiveTo);
            careerLearningPilotPostcode.Postcode.Should().Be(careerLearningPilotPostcode.Postcode);
        }

        [Fact]
        public void CareerLearningPilotsForPostcodes()
        {
            var careerLearningPilotsPostocde = new List<CareerLearningPilot_Postcode>()
            {
                new CareerLearningPilot_Postcode()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
                    Postcode = "CV1 2WT",
                    AreaCode = "AreaCode",
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = null,
                },
                new CareerLearningPilot_Postcode()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                    Postcode = "CV1 2TT",
                    AreaCode = "AreaCode",
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = new DateTime(2015, 12, 31)
                },
                new CareerLearningPilot_Postcode()
                {
                    Postcode = "Fictional"
                }
            }.AsQueryable();

            var postcodesDataRetrievalServiceMock = NewMock();

            postcodesDataRetrievalServiceMock.SetupGet(p => p.CareerLearningPilot_Postcodes).Returns(careerLearningPilotsPostocde);

            var postcodes = new List<string>() { "CV1 2WT", "CV1 2TT" };

            var careerLearningPilots = postcodesDataRetrievalServiceMock.Object.CareerLearningPilotsForPostcodes(postcodes);

            careerLearningPilots.Should().HaveCount(2);
            careerLearningPilots.Should().ContainKeys("CV1 2WT", "CV1 2TT");
            careerLearningPilots.Should().NotContainKey("Fictional");

            careerLearningPilots["CV1 2TT"].Should().HaveCount(1);
            careerLearningPilots["CV1 2WT"].Should().HaveCount(1);
        }

        [Fact]
        public void EfaDisadvantageFromEntity()
        {
            var efaPostcodeDisadvantage = new EFA_PostcodeDisadvantage()
            {
                MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
                Postcode = "CV1 2TT",
                Uplift = 1.5m,
                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 12, 31),
            };

            var efaDisadvantage = NewService().EfaDisadvantageFromEntity(efaPostcodeDisadvantage);

            efaDisadvantage.Uplift.Should().Be(efaPostcodeDisadvantage.Uplift);
            efaDisadvantage.EffectiveFrom.Should().Be(efaPostcodeDisadvantage.EffectiveFrom);
            efaDisadvantage.EffectiveTo.Should().Be(efaPostcodeDisadvantage.EffectiveTo);
            efaDisadvantage.Postcode.Should().Be(efaPostcodeDisadvantage.Postcode);
        }

        private PostcodesDataRetrievalService NewService(IPostcodes postcodes = null)
        {
            return new PostcodesDataRetrievalService(postcodes);
        }

        private Mock<PostcodesDataRetrievalService> NewMock()
        {
            return new Mock<PostcodesDataRetrievalService>
            {
                CallBase = true
            };
        }
    }
}
