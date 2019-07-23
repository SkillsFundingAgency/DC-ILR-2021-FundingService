using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM70.Service.Input;
using ESFA.DC.ILR.FundingService.FM70.Service.Models;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void MapTo()
        {
            var ukprn = 1;
            var learnAimRef = "LearnAimRef";
            var delLocPostCode = "Postcode";
            var conRefNumber = "ConRefNumber";

            var global = new Global
            {
                UKPRN = 1
            };

            var learnerDtos = new List<FM70LearnerDto>
            {
                new FM70LearnerDto
                {
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            DelLocPostCode = delLocPostCode,
                            FundModel = 70,
                            ConRefNumber = conRefNumber
                        }
                    }
                },
                new FM70LearnerDto
                {
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            DelLocPostCode = delLocPostCode,
                            FundModel = 70,
                            ConRefNumber = conRefNumber
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

            var fcsContract = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    CalcMethod = 1,
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable()
                    }
                }
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var fcsReferenceDataMock = new Mock<IFCSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learnAimRef)).Returns(larsLearningDelivery);
            postcodesReferenceDataServiceMock.Setup(p => p.SFAAreaCostsForPostcode(delLocPostCode)).Returns(new List<SfaAreaCost> { new SfaAreaCost() });
            fcsReferenceDataMock.Setup(f => f.FcsContractsForConRef(conRefNumber)).Returns(fcsContract);

            var dataEntities = NewService(
                fcsReferenceDataService: fcsReferenceDataMock.Object,
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object)
                .MapTo(ukprn, learnerDtos);

            dataEntities.Should().HaveCount(2);
            dataEntities.SelectMany(d => d.Children).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void MapTo_NullLearnerDto()
        {
            var ukprn = 1;

            var global = new Global
            {
                UKPRN = 1
            };

            var dataEntities = NewService().MapTo(ukprn, null);

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.SelectMany(d => d.Children).Should().BeNullOrEmpty();
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(1);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void MapTo_EmptyLearners()
        {
            var ukprn = 1;

            var global = new Global
            {
                UKPRN = 1
            };

            var dataEntities = NewService().MapTo(ukprn, new List<FM70LearnerDto>());

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(1);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global
            {
                UKPRN = 1234,
            };

            var dataEntity = NewService().BuildGlobalDataEntity(new FM70LearnerDto(), global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(1);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobal()
        {
            var ukprn = 1234;

            NewService().BuildGlobal(ukprn).UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new FM70LearnerDto()
            {
                LearnRefNumber = "ABC",
                DateOfBirth = new DateTime(2000, 8, 1)
            };

            var dataEntity = NewService().BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirth);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearnerEmploymentStatus()
        {
            var learnerEmploymentStatus = new LearnerEmploymentStatus
            {
                AgreeId = "Id",
                DateEmpStatApp = new DateTime(2019, 1, 1),
                EmpId = 1,
                EmpStat = 2,
            };

            var dataEntity = NewService().BuildLearnerEmploymentStatus(learnerEmploymentStatus);

            dataEntity.EntityName.Should().Be("LearnerEmploymentStatus");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["DateEmpStatApp"].Value.Should().Be(learnerEmploymentStatus.DateEmpStatApp);
            dataEntity.Attributes["EMPStat"].Value.Should().Be(learnerEmploymentStatus.EmpStat);
        }

        [Fact]
        public void BuildDPOutcome()
        {
            var dpOutcome = new DPOutcome
            {
                OutCode = 100,
                OutType = "Type",
                OutCollDate = new DateTime(2019, 1, 1),
                OutStartDate = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildDPOutcomes(dpOutcome);

            dataEntity.EntityName.Should().Be("DPOutcome");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["OutCode"].Value.Should().Be(dpOutcome.OutCode);
            dataEntity.Attributes["OutType"].Value.Should().Be(dpOutcome.OutType);
            dataEntity.Attributes["OutCollDate"].Value.Should().Be(dpOutcome.OutCollDate);
            dataEntity.Attributes["OutStartDate"].Value.Should().Be(dpOutcome.OutStartDate);
            dataEntity.Attributes["OutEndDate"].Value.Should().Be(dpOutcome.OutEndDate);

            dataEntity.Children.Should().BeNullOrEmpty();
        }

        [Fact]
        public void BuildLearningDelivery()
        {
            var learningDelivery = new LearningDelivery()
            {
                AimSeqNumber = 1,
                AimType = 2,
                CompStatus = 3,
                ConRefNumber = "ConRef1",
                DelLocPostCode = "DelLocPostcode",
                FundModel = 4,
                FworkCode = 5,
                LearnActEndDate = new DateTime(2017, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2019, 1, 1),
                LearnStartDate = new DateTime(2020, 1, 1),
                OrigLearnStartDate = new DateTime(2020, 1, 1),
                OtherFundAdj = 6,
                Outcome = 7,
                PriorLearnFundAdj = 8,
                LearningDeliveryFAMs = new List<LearningDeliveryFAM>()
                {
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
                },
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                FrameworkCommonComponent = 2,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                RegulatedCreditValue = 3,
                NotionalNVQLevelv2 = "NVQLevel",
                LearningDeliveryGenre = "Genre",
                LARSAnnualValues = new List<LARSAnnualValue>
                {
                    new LARSAnnualValue
                    {
                        BasicSkillsType = 1,
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                    }
                },
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding
                    {
                        FundingCategory = "Matrix",
                        RateWeighted = 1.0m,
                        WeightingFactor = "G",
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                    },
                },
            };

            var fcsContract = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    CalcMethod = 1,
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable()
                    }
                }
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var fcsReferenceDataMock = new Mock<IFCSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            postcodesReferenceDataServiceMock.Setup(p => p.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(new List<SfaAreaCost> { new SfaAreaCost() });
            fcsReferenceDataMock.Setup(f => f.FcsContractsForConRef(learningDelivery.ConRefNumber)).Returns(fcsContract);

            var dataEntity = NewService(
                fcsReferenceDataService: fcsReferenceDataMock.Object,
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object)
                .BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(15);
            dataEntity.Attributes["AchDate"].Value.Should().Be(learningDelivery.AchDate);
            dataEntity.Attributes["AddHours"].Value.Should().Be(learningDelivery.AddHours);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["CalcMethod"].Value.Should().Be(1);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["ConRefNumber"].Value.Should().Be(learningDelivery.ConRefNumber);
            dataEntity.Attributes["GenreCode"].Value.Should().Be(larsLearningDelivery.LearningDeliveryGenre);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDate);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDate);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdj);
            dataEntity.Attributes["Outcome"].Value.Should().Be(learningDelivery.Outcome);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdj);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new LearningDeliveryFAM
            {
                LearnDelFAMType = "RES",
                LearnDelFAMCode = "1",
                LearnDelFAMDateFrom = new DateTime(2019, 8, 1)
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
        public void BuildSFAAreaCost()
        {
            var sfaAreaCost = new SfaAreaCost
            {
                AreaCostFactor = 1.2m,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildSFAAreaCost(sfaAreaCost);

            dataEntity.EntityName.Should().Be("SFA_PostcodeAreaCost");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["AreaCosFactor"].Value.Should().Be(sfaAreaCost.AreaCostFactor);
            dataEntity.Attributes["AreaCosEffectiveFrom"].Value.Should().Be(sfaAreaCost.EffectiveFrom);
            dataEntity.Attributes["AreaCosEffectiveTo"].Value.Should().Be(sfaAreaCost.EffectiveTo);
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
                RateUnWeighted = 2.0m,
                WeightingFactor = "G",
            };

            var dataEntity = NewService().BuildLARSFunding(larsFunding);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARSFunding");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSFundingCategory"].Value.Should().Be(larsFunding.FundingCategory);
            dataEntity.Attributes["LARSFundingEffectiveFrom"].Value.Should().Be(larsFunding.EffectiveFrom);
            dataEntity.Attributes["LARSFundingEffectiveTo"].Value.Should().Be(larsFunding.EffectiveTo);
            dataEntity.Attributes["LARSFundingWeightedRate"].Value.Should().Be(larsFunding.RateWeighted);
        }

        [Fact]
        public void BuildLARSAnnualValue()
        {
            var larsAnnualValue = new LARSAnnualValue
            {
                BasicSkillsType = 1,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1),
            };

            var dataEntity = NewService().BuildLARSAnnualValue(larsAnnualValue);

            dataEntity.EntityName.Should().Be("LearningDeliveryAnnualValue");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LearnDelAnnValBasicSkillsTypeCode"].Value.Should().Be(larsAnnualValue.BasicSkillsType);
            dataEntity.Attributes["LearnDelAnnValDateFrom"].Value.Should().Be(larsAnnualValue.EffectiveFrom);
            dataEntity.Attributes["LearnDelAnnValDateTo"].Value.Should().Be(larsAnnualValue.EffectiveTo);
        }

        [Fact]
        public void BuildEsfDataEntity()
        {
            var esfData = new EsfData
            {
                CalcMethod = 1,
                EffectiveContractStartDate = new DateTime(2019, 1, 1),
                EffectiveContractEndDate = new DateTime(2020, 1, 1),
                ESFDataPremiumFactor = 1.0m,
                ESFDeliverableCode = "1",
                UnitCost = 1.2m
            };

            var dataEntity = NewService().BuildEsfDataEntity(esfData);

            dataEntity.EntityName.Should().Be("ESFData");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["EffectiveContractStartDate"].Value.Should().Be(esfData.EffectiveContractStartDate);
            dataEntity.Attributes["EffectiveContractEndDate"].Value.Should().Be(esfData.EffectiveContractEndDate);
            dataEntity.Attributes["ESFDataPremiumFactor"].Value.Should().Be(esfData.ESFDataPremiumFactor);
            dataEntity.Attributes["ESFDeliverableCode"].Value.Should().Be(esfData.ESFDeliverableCode);
            dataEntity.Attributes["UnitCost"].Value.Should().Be(esfData.UnitCost);
        }

        [Fact]
        public void BuildEsfDataFromContract()
        {
            var fcsContract = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    CalcMethod = 1,
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable(),
                        new FCSContractDeliverable()
                    }
                }
            };

            var esfData = NewService().BuildEsfDataFromContract(fcsContract);

            esfData.Should().HaveCount(2);
        }

        private DataEntityMapper NewService(
            IFCSReferenceDataService fcsReferenceDataService = null,
            ILARSReferenceDataService larsReferenceDataService = null,
            IPostcodesReferenceDataService postcodesReferenceDataService = null)
        {
            return new DataEntityMapper(fcsReferenceDataService, larsReferenceDataService, postcodesReferenceDataService);
        }
    }
}