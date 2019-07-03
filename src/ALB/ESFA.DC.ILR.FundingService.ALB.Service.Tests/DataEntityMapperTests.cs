using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.Service.Input;
using ESFA.DC.ILR.FundingService.ALB.Service.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void MapTo()
        {
            var ukprn = 1;
            var learnAimRef = "LearnAImRef";
            var delLocPostCode = "Postcode";

            var global = new Global
            {
                LARSVersion = "1.0.0",
                PostcodesVersion = "2.0.0",
                UKPRN = 1234
            };

            var learnerDtos = new List<ALBLearnerDto>
            {
                new ALBLearnerDto
                {
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            DelLocPostCode = delLocPostCode,
                            FundModel = 99
                        }
                    }
                },
                new ALBLearnerDto
                {
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            DelLocPostCode = delLocPostCode,
                            FundModel = 99
                        }
                    }
                },
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                RegulatedCreditValue = 2,
                NotionalNVQLevelv2 = "NVQLevel",
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding
                    {
                        FundingCategory = "Matrix",
                        RateWeighted = 1.0m,
                        WeightingFactor = "G",
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                    }
                },
                LARSCareerLearningPilots = new List<LARSCareerLearningPilot>
                {
                    new LARSCareerLearningPilot
                    {
                        AreaCode = "DelLocPostcode",
                        SubsidyRate = 1.2m,
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1)
                    }
                }
            };

            var sfaAreaCost = new List<SfaAreaCost>
            {
                new SfaAreaCost
                {
                    Postcode = "DelLocPostcode",
                    EffectiveFrom = new DateTime(2019, 1, 1),
                    EffectiveTo = new DateTime(2020, 1, 1),
                    AreaCostFactor = 1.2m
                }
            };

            var subsidyPilotPostcodeArea = new List<CareerLearningPilot>
            {
                new CareerLearningPilot
                {
                    Postcode = "DelLocPostcode",
                    EffectiveFrom = new DateTime(2019, 1, 1),
                    EffectiveTo = new DateTime(2020, 1, 1),
                    AreaCode = "AreaCode"
                }
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodeReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(global.LARSVersion);
            postcodeReferenceDataServiceMock.Setup(o => o.PostcodesCurrentVersion()).Returns(global.PostcodesVersion);

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learnAimRef)).Returns(larsLearningDelivery);
            postcodeReferenceDataServiceMock.Setup(o => o.SFAAreaCostsForPostcode(delLocPostCode)).Returns(sfaAreaCost);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object, postcodeReferenceDataServiceMock.Object).MapTo(ukprn, learnerDtos);

            dataEntities.Should().HaveCount(2);
            dataEntities.SelectMany(d => d.Children).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void MapTo_NullLearnerDto()
        {
            var ukprn = 1;

            var global = new Global
            {
                LARSVersion = "1.0.0",
                PostcodesVersion = "2.0.0",
                UKPRN = 1
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodeReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(global.LARSVersion);
            postcodeReferenceDataServiceMock.Setup(o => o.PostcodesCurrentVersion()).Returns(global.PostcodesVersion);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object, postcodeReferenceDataServiceMock.Object).MapTo(ukprn, null);

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(3);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["PostcodeAreaCostVersion"].Value.Should().Be(global.PostcodesVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void MapTo_EmptyLearners()
        {
            var ukprn = 1;

            var global = new Global
            {
                LARSVersion = "1.0.0",
                PostcodesVersion = "2.0.0",
                UKPRN = 1
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodeReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(global.LARSVersion);
            postcodeReferenceDataServiceMock.Setup(o => o.PostcodesCurrentVersion()).Returns(global.PostcodesVersion);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object, postcodeReferenceDataServiceMock.Object).MapTo(ukprn, new List<ALBLearnerDto>());

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(3);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["PostcodeAreaCostVersion"].Value.Should().Be(global.PostcodesVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global
            {
                LARSVersion = "1.0.0",
                PostcodesVersion = "2.0.0",
                UKPRN = 1234
            };

            var dataEntity = NewService().BuildGlobalDataEntity(new ALBLearnerDto(), global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntity.Attributes["PostcodeAreaCostVersion"].Value.Should().Be(global.PostcodesVersion);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsCurrentVersion = "1.0.0";
            var postcodesCurrentVersion = "2.0.0";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodeReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);
            postcodeReferenceDataServiceMock.Setup(o => o.PostcodesCurrentVersion()).Returns(postcodesCurrentVersion);

            var global = NewService(larsRefererenceDataServiceMock.Object, postcodeReferenceDataServiceMock.Object).BuildGlobal(ukprn);

            global.PostcodesVersion.Should().Be(postcodesCurrentVersion);
            global.LARSVersion.Should().Be(larsCurrentVersion);
            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new ALBLearnerDto()
            {
                LearnRefNumber = "ABC",
            };

            var dataEntity = NewService().BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(1);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearningDelivery()
        {
            var learningDelivery = new LearningDelivery()
            {
                AimSeqNumber = 1,
                AimType = 2,
                CompStatus = 3,
                DelLocPostCode = "DelLocPostcode",
                FundModel = 4,
                LearnActEndDate = new DateTime(2017, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2019, 1, 1),
                LearnStartDate = new DateTime(2020, 1, 1),
                OrigLearnStartDate = new DateTime(2020, 1, 1),
                OtherFundAdj = 5,
                Outcome = 6,
                PriorLearnFundAdj = 7,
                LearningDeliveryFAMs = new List<LearningDeliveryFAM>()
                {
                    new LearningDeliveryFAM() { LearnDelFAMType = "ADL", LearnDelFAMCode = "ADL" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
                }
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                RegulatedCreditValue = 2,
                NotionalNVQLevelv2 = "NVQLevel",
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding
                    {
                        FundingCategory = "Matrix",
                        RateWeighted = 1.0m,
                        WeightingFactor = "G",
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                    }
                },
                LARSCareerLearningPilots = new List<LARSCareerLearningPilot>
                {
                    new LARSCareerLearningPilot
                    {
                        AreaCode = "DelLocPostcode",
                        SubsidyRate = 1.2m,
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1)
                    }
                }
            };

            var sfaAreaCost = new List<SfaAreaCost>
            {
                new SfaAreaCost
                {
                    Postcode = "DelLocPostcode",
                    EffectiveFrom = new DateTime(2019, 1, 1),
                    EffectiveTo = new DateTime(2020, 1, 1),
                    AreaCostFactor = 1.2m
                }
            };

            var subsidyPilotPostcodeArea = new List<CareerLearningPilot>
            {
                new CareerLearningPilot
                {
                    Postcode = "DelLocPostcode",
                    EffectiveFrom = new DateTime(2019, 1, 1),
                    EffectiveTo = new DateTime(2020, 1, 1),
                    AreaCode = "AreaCode"
                }
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodeReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            postcodeReferenceDataServiceMock.Setup(o => o.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(sfaAreaCost);

            var dataEntity = NewService(larsReferenceDataServiceMock.Object, postcodeReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(13);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDate);
            dataEntity.Attributes["LearnAimRefType"].Value.Should().Be(larsLearningDelivery.LearnAimRefType);
            dataEntity.Attributes["LearnDelFundModel"].Value.Should().Be(learningDelivery.FundModel);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["NotionalNVQLevelv2"].Value.Should().Be(larsLearningDelivery.NotionalNVQLevelv2);
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDate);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdj);
            dataEntity.Attributes["Outcome"].Value.Should().Be(learningDelivery.Outcome);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdj);
            dataEntity.Attributes["RegulatedCreditValue"].Value.Should().Be(larsLearningDelivery.RegulatedCreditValue);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new LearningDeliveryFAM
            {
                LearnDelFAMType = "ADL",
                LearnDelFAMCode = "ADL"
            };

            var dataEntity = NewService().BuildLearningDeliveryFAM(learningDeliveryFAM);

            dataEntity.EntityName.Should().Be("LearningDeliveryFAM");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LearnDelFAMCode"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMCode);
            dataEntity.Attributes["LearnDelFAMDateFrom"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateFrom);
            dataEntity.Attributes["LearnDelFAMDateTo"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateTo);
            dataEntity.Attributes["LearnDelFAMType"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMType);
        }

        [Fact]
        public void BuildLARSFunding()
        {
            var larsFunding = new LARSFunding
            {
                FundingCategory = "FundingCategory",
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1),
                RateWeighted = 1.0m,
                WeightingFactor = "G"
            };

            var dataEntity = NewService().BuildLARSFunding(larsFunding);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARS_Funding");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["LARSFundCategory"].Value.Should().Be(larsFunding.FundingCategory);
            dataEntity.Attributes["LARSFundEffectiveFrom"].Value.Should().Be(larsFunding.EffectiveFrom);
            dataEntity.Attributes["LARSFundEffectiveTo"].Value.Should().Be(larsFunding.EffectiveTo);
            dataEntity.Attributes["LARSFundWeightedRate"].Value.Should().Be(larsFunding.RateWeighted);
            dataEntity.Attributes["LARSFundWeightingFactor"].Value.Should().Be(larsFunding.WeightingFactor);
        }

        [Fact]
        public void BuildSFAPostcodeAreaCost()
        {
            var sfaAreaCost = new SfaAreaCost
            {
                AreaCostFactor = 1.2m,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1),
            };

            var dataEntity = NewService().BuildSFAPostcodeAreaCost(sfaAreaCost);

            dataEntity.EntityName.Should().Be("SFA_PostcodeAreaCost");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["AreaCosFactor"].Value.Should().Be(sfaAreaCost.AreaCostFactor);
            dataEntity.Attributes["AreaCosEffectiveFrom"].Value.Should().Be(sfaAreaCost.EffectiveFrom);
            dataEntity.Attributes["AreaCosEffectiveTo"].Value.Should().Be(sfaAreaCost.EffectiveTo);
        }

        private DataEntityMapper NewService(ILARSReferenceDataService larsReferenceDataService = null, IPostcodesReferenceDataService postcodesReferenceDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService, postcodesReferenceDataService);
        }
    }
}
