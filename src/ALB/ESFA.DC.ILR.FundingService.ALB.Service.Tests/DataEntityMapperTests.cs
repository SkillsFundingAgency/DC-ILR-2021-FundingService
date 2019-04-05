using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.Service.Input;
using ESFA.DC.ILR.FundingService.ALB.Service.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global
            {
                LARSVersion = "1.0.0",
                PostcodesVersion = "2.0.0",
                UKPRN = 1234
            };

            var dataEntity = NewService().BuildGlobalDataEntity(null, global);

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
            var fileDataServiceMock = new Mock<IFileDataService>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);
            postcodeReferenceDataServiceMock.Setup(o => o.PostcodesCurrentVersion()).Returns(postcodesCurrentVersion);
            fileDataServiceMock.Setup(f => f.UKPRN()).Returns(ukprn);

            var global = NewService(larsRefererenceDataServiceMock.Object, postcodeReferenceDataServiceMock.Object, fileDataServiceMock.Object).BuildGlobal();

            global.PostcodesVersion.Should().Be(postcodesCurrentVersion);
            global.LARSVersion.Should().Be(larsCurrentVersion);
            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new TestLearner()
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
            var learningDelivery = new TestLearningDelivery()
            {
                AimSeqNumber = 1,
                AimType = 2,
                CompStatus = 3,
                DelLocPostCode = "DelLocPostcode",
                FundModel = 4,
                LearnActEndDateNullable = new DateTime(2017, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2018, 1, 1),
                LearnStartDate = new DateTime(2019, 1, 1),
                OrigLearnStartDateNullable = new DateTime(2019, 1, 1),
                OtherFundAdjNullable = 5,
                OutcomeNullable = 6,
                PriorLearnFundAdjNullable = 7,
                LearningDeliveryFAMs = new List<TestLearningDeliveryFAM>()
                {
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "ADL", LearnDelFAMCode = "ADL" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
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
                        EffectiveFrom = new DateTime(2018, 1, 1),
                        EffectiveTo = new DateTime(2019, 1, 1),
                    }
                },
                LARSCareerLearningPilots = new List<LARSCareerLearningPilot>
                {
                    new LARSCareerLearningPilot
                    {
                        AreaCode = "DelLocPostcode",
                        SubsidyRate = 1.2m,
                        EffectiveFrom = new DateTime(2018, 1, 1),
                        EffectiveTo = new DateTime(2019, 1, 1)
                    }
                }
            };

            var sfaAreaCost = new List<SfaAreaCost>
            {
                new SfaAreaCost
                {
                    Postcode = "DelLocPostcode",
                    EffectiveFrom = new DateTime(2018, 1, 1),
                    EffectiveTo = new DateTime(2019, 1, 1),
                    AreaCostFactor = 1.2m
                }
            };

            var subsidyPilotPostcodeArea = new List<CareerLearningPilot>
            {
                new CareerLearningPilot
                {
                    Postcode = "DelLocPostcode",
                    EffectiveFrom = new DateTime(2018, 1, 1),
                    EffectiveTo = new DateTime(2019, 1, 1),
                    AreaCode = "AreaCode"
                }
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodeReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            postcodeReferenceDataServiceMock.Setup(o => o.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(sfaAreaCost);
            postcodeReferenceDataServiceMock.Setup(o => o.CareerLearningPilotsForPostcode(learningDelivery.DelLocPostCode)).Returns(subsidyPilotPostcodeArea);

            var dataEntity = NewService(larsReferenceDataServiceMock.Object, postcodeReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(19);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDateNullable);
            dataEntity.Attributes["LearnAimRefType"].Value.Should().Be(larsLearningDelivery.LearnAimRefType);
            dataEntity.Attributes["LearnDelFundModel"].Value.Should().Be(learningDelivery.FundModel);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["LrnDelFAM_ADL"].Value.Should().Be("ADL");
            dataEntity.Attributes["LrnDelFAM_LDM1"].Value.Should().Be("LDM1");
            dataEntity.Attributes["LrnDelFAM_LDM2"].Value.Should().Be("LDM2");
            dataEntity.Attributes["LrnDelFAM_LDM3"].Value.Should().Be("LDM3");
            dataEntity.Attributes["LrnDelFAM_LDM4"].Value.Should().Be("LDM4");
            dataEntity.Attributes["LrnDelFAM_RES"].Value.Should().Be("RES");
            dataEntity.Attributes["NotionalNVQLevelv2"].Value.Should().Be(larsLearningDelivery.NotionalNVQLevelv2);
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDateNullable);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdjNullable);
            dataEntity.Attributes["Outcome"].Value.Should().Be(learningDelivery.OutcomeNullable);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdjNullable);
            dataEntity.Attributes["RegulatedCreditValue"].Value.Should().Be(larsLearningDelivery.RegulatedCreditValue);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new TestLearningDeliveryFAM
            {
                LearnDelFAMType = "ADL",
                LearnDelFAMCode = "ADL"
            };

            var dataEntity = NewService().BuildLearningDeliveryFAM(learningDeliveryFAM);

            dataEntity.EntityName.Should().Be("LearningDeliveryFAM");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LearnDelFAMCode"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMCode);
            dataEntity.Attributes["LearnDelFAMDateFrom"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateFromNullable);
            dataEntity.Attributes["LearnDelFAMDateTo"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateToNullable);
            dataEntity.Attributes["LearnDelFAMType"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMType);
        }

        [Fact]
        public void BuildLARSCareerLearningPilot()
        {
            var larsCareerLearningPilot = new LARSCareerLearningPilot
            {
                AreaCode = "AreaCode",
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
                SubsidyRate = 1.0m
            };

            var dataEntity = NewService().BuildLARSCareerLearningPilot(larsCareerLearningPilot);

            dataEntity.EntityName.Should().Be("LARS_CareerLearningPilot");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LearnDelLARSCarPilFundAreaCode"].Value.Should().Be(larsCareerLearningPilot.AreaCode);
            dataEntity.Attributes["LearnDelLARSCarPilFundEffFromDate"].Value.Should().Be(larsCareerLearningPilot.EffectiveFrom);
            dataEntity.Attributes["LearnDelLARSCarPilFundEffToDate"].Value.Should().Be(larsCareerLearningPilot.EffectiveTo);
            dataEntity.Attributes["LearnDelLARSCarPilFundSubsidyRate"].Value.Should().Be(larsCareerLearningPilot.SubsidyRate);
        }

        [Fact]
        public void BuildSubsidyPilotPostcodeArea()
        {
            var careerLearningPilot = new CareerLearningPilot
            {
                AreaCode = "AreaCode",
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildSubsidyPilotPostcodeArea(careerLearningPilot);

            dataEntity.EntityName.Should().Be("SubsidyPilotPostcodeArea");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["SubsidyPilotAreaCode"].Value.Should().Be(careerLearningPilot.AreaCode);
            dataEntity.Attributes["SubsidyPilotEffectiveFrom"].Value.Should().Be(careerLearningPilot.EffectiveFrom);
            dataEntity.Attributes["SubsidyPilotEffectiveTo"].Value.Should().Be(careerLearningPilot.EffectiveTo);
        }

        [Fact]
        public void BuildLARSFunding()
        {
            var larsFunding = new LARSFunding
            {
                FundingCategory = "FundingCategory",
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
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
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildSFAPostcodeAreaCost(sfaAreaCost);

            dataEntity.EntityName.Should().Be("SFA_PostcodeAreaCost");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["AreaCosFactor"].Value.Should().Be(sfaAreaCost.AreaCostFactor);
            dataEntity.Attributes["AreaCosEffectiveFrom"].Value.Should().Be(sfaAreaCost.EffectiveFrom);
            dataEntity.Attributes["AreaCosEffectiveTo"].Value.Should().Be(sfaAreaCost.EffectiveTo);
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>()
            {
                new TestLearningDeliveryFAM() { LearnDelFAMType = "ADL", LearnDelFAMCode = "1" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "2" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "3" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "4" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "5" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "6" },
            };

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.ADL.Should().Be("1");
            learningDeliveryFAMDenormalized.LDM1.Should().Be("2");
            learningDeliveryFAMDenormalized.LDM2.Should().Be("3");
            learningDeliveryFAMDenormalized.LDM3.Should().Be("4");
            learningDeliveryFAMDenormalized.LDM4.Should().Be("5");
            learningDeliveryFAMDenormalized.RES.Should().Be("6");
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_Null()
        {
            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(null);

            learningDeliveryFAMDenormalized.ADL.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
            learningDeliveryFAMDenormalized.RES.Should().BeNull();
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_NoMatches()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>();

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.ADL.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
            learningDeliveryFAMDenormalized.RES.Should().BeNull();
        }

        private DataEntityMapper NewService(ILARSReferenceDataService larsReferenceDataService = null, IPostcodesReferenceDataService postcodesReferenceDataService = null, IFileDataService fileDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService, postcodesReferenceDataService, fileDataService);
        }
    }
}
