using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM81.Service.Input;
using ESFA.DC.ILR.FundingService.FM81.Service.Model;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM81.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void MapTo()
        {
            var ukprn = 1;
            var learnAimRef = "LearnAImRef";
            var stdCode = 1;

            var global = new Global
            {
                LARSVersion = "1.0.0",
                UKPRN = 1234
            };

            var learnerDtos = new List<FM81LearnerDto>
            {
                new FM81LearnerDto
                {
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            FundModel = 81,
                            ProgType = 25,
                            StdCode = stdCode
                        }
                    }
                },
                new FM81LearnerDto
                {
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            FundModel = 81,
                            ProgType = 25,
                            StdCode = stdCode
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
                        EffectiveFrom = new DateTime(2020, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                    }
                },
                LARSCareerLearningPilots = new List<LARSCareerLearningPilot>
                {
                    new LARSCareerLearningPilot
                    {
                        AreaCode = "DelLocPostcode",
                        SubsidyRate = 1.2m,
                        EffectiveFrom = new DateTime(2020, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1)
                    }
                }
            };

            var larsStandard = new LARSStandard
            {
                StandardCode = 1,
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(global.LARSVersion);

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learnAimRef)).Returns(larsLearningDelivery);
            larsReferenceDataServiceMock.Setup(l => l.LARSStandardForStandardCode(stdCode)).Returns(larsStandard);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object).MapTo(ukprn, learnerDtos);

            dataEntities.Should().HaveCount(2);
            dataEntities.SelectMany(d => d.Children).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void MapTo_NullLearnerDto()
        {
            var ukprn = 1234;
            var larsVersion = "1.0.0";

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = 1234
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object).MapTo(ukprn, null);

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.SelectMany(d => d.Children).Should().BeNullOrEmpty();
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(2);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void MapTo_EmptyLearners()
        {
            var ukprn = 1234;
            var larsVersion = "1.0.0";

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = 1234
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object).MapTo(ukprn, new List<FM81LearnerDto>());

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.SelectMany(d => d.Children).Should().BeNullOrEmpty();
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(2);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global
            {
                UKPRN = 1234,
                LARSVersion = "Version"
            };

            var dataEntity = NewService().BuildGlobalDataEntity(new FM81LearnerDto(), global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(global.LARSVersion);
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsCurrentVersion = "1.0.0";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);

            var global = NewService(larsRefererenceDataServiceMock.Object).BuildGlobal(ukprn);

            global.LARSVersion.Should().Be(larsCurrentVersion);
            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new FM81LearnerDto()
            {
                LearnRefNumber = "ABC",
                DateOfBirth = new DateTime(2000, 8, 1),
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
                DateEmpStatApp = new DateTime(2020, 1, 1),
                EmpId = 1,
                EmpStat = 2,
                SEM = 3
            };

            var dataEntity = NewService().BuildLearnerEmploymentStatus(learnerEmploymentStatus);

            dataEntity.EntityName.Should().Be("LearnerEmploymentStatus");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["DateEmpStatApp"].Value.Should().Be(learnerEmploymentStatus.DateEmpStatApp);
            dataEntity.Attributes["EmpId"].Value.Should().Be(learnerEmploymentStatus.EmpId);
            dataEntity.Attributes["EMPStat"].Value.Should().Be(learnerEmploymentStatus.EmpStat);
            dataEntity.Attributes["EmpStatMon_SEM"].Value.Should().Be(learnerEmploymentStatus.SEM);
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
                LearnPlanEndDate = new DateTime(2020, 1, 1),
                LearnStartDate = new DateTime(2020, 1, 1),
                OrigLearnStartDate = new DateTime(2020, 1, 1),
                OtherFundAdj = 6,
                Outcome = 7,
                PriorLearnFundAdj = 8,
                StdCode = 9,
                LearningDeliveryFAMs = new List<LearningDeliveryFAM>()
                {
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "EEF", LearnDelFAMCode = "EEF" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "FFI", LearnDelFAMCode = "FFI" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "SOF", LearnDelFAMCode = "SOF" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "SPP", LearnDelFAMCode = "SPP" },
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
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding
                    {
                        FundingCategory = "Matrix",
                        RateWeighted = 1.0m,
                        WeightingFactor = "G",
                        EffectiveFrom = new DateTime(2020, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                    },
                },
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            larsReferenceDataServiceMock.Setup(l => l.LARSStandardForStandardCode(learningDelivery.StdCode))
                .Returns(
                new LARSStandard
                {
                    LARSStandardCommonComponents = new List<LARSStandardCommonComponent>(),
                    LARSStandardFundings = new List<LARSStandardFunding>()
                });

            var dataEntity = NewService(larsReferenceDataService: larsReferenceDataServiceMock.Object)
                .BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(16);
            dataEntity.Attributes["AchDate"].Value.Should().Be(learningDelivery.AchDate);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["FrameworkCommonComponent"].Value.Should().Be(larsLearningDelivery.FrameworkCommonComponent);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDate);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDate);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdj);
            dataEntity.Attributes["Outcome"].Value.Should().Be(learningDelivery.Outcome);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdj);
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgType);
            dataEntity.Attributes["STDCode"].Value.Should().Be(learningDelivery.StdCode);
            dataEntity.Attributes["WithdrawReason"].Value.Should().Be(learningDelivery.WithdrawReason);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new LearningDeliveryFAM
            {
                LearnDelFAMType = "EEF",
                LearnDelFAMCode = "1"
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
        public void BuildApprenticeshipFinancialRecord()
        {
            var appFinRecord = new AppFinRecord
            {
                AFinCode = 1,
                AFinAmount = 2,
                AFinDate = new DateTime(2020, 8, 1),
                AFinType = "Type"
            };

            var dataEntity = NewService().BuildApprenticeshipFinancialRecord(appFinRecord);

            dataEntity.EntityName.Should().Be("ApprenticeshipFinancialRecord");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["AFinAmount"].Value.Should().Be(appFinRecord.AFinAmount);
            dataEntity.Attributes["AFinCode"].Value.Should().Be(appFinRecord.AFinCode);
            dataEntity.Attributes["AFinDate"].Value.Should().Be(appFinRecord.AFinDate);
            dataEntity.Attributes["AFinType"].Value.Should().Be(appFinRecord.AFinType);
        }

        [Fact]
        public void BuildLARSStandardCommonComponent()
        {
            var stdCode = 2;

            var larsStandardCommonComponent = new LARSStandardCommonComponent
            {
                CommonComponent = 1,
                EffectiveFrom = new DateTime(2020, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1)
            };

            var dataEntity = NewService().BuildLARSStandardCommonComponent(larsStandardCommonComponent, stdCode);

            dataEntity.EntityName.Should().Be("LARS_StandardCommonComponent");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSCommonComponent"].Value.Should().Be(larsStandardCommonComponent.CommonComponent);
            dataEntity.Attributes["LARSEffectiveFrom"].Value.Should().Be(larsStandardCommonComponent.EffectiveFrom);
            dataEntity.Attributes["LARSEffectiveTo"].Value.Should().Be(larsStandardCommonComponent.EffectiveTo);
            dataEntity.Attributes["LARSStandardCode"].Value.Should().Be(stdCode);
        }

        [Fact]
        public void BuildLARSStandardFunding()
        {
            var larsStandardFunding = new LARSStandardFunding
            {
                FundableWithoutEmployer = "Fundable",
                EffectiveFrom = new DateTime(2020, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1),
                AchievementIncentive = 1,
                SixteenToEighteenIncentive = 2,
                CoreGovContributionCap = 1.0m,
                SmallBusinessIncentive = 2.0m
            };

            var dataEntity = NewService().BuildLARSStandardFunding(larsStandardFunding);

            dataEntity.EntityName.Should().Be("LARS_StandardFunding");
            dataEntity.Attributes.Should().HaveCount(7);
            dataEntity.Attributes["FundableWithoutEmployer"].Value.Should().Be(larsStandardFunding.FundableWithoutEmployer);
            dataEntity.Attributes["SF1618Incentive"].Value.Should().Be(larsStandardFunding.SixteenToEighteenIncentive);
            dataEntity.Attributes["SFAchIncentive"].Value.Should().Be(larsStandardFunding.AchievementIncentive);
            dataEntity.Attributes["SFCoreGovContCap"].Value.Should().Be(larsStandardFunding.CoreGovContributionCap);
            dataEntity.Attributes["SFEffectiveFromDate"].Value.Should().Be(larsStandardFunding.EffectiveFrom);
            dataEntity.Attributes["SFEffectiveToDate"].Value.Should().Be(larsStandardFunding.EffectiveTo);
            dataEntity.Attributes["SFSmallBusIncentive"].Value.Should().Be(larsStandardFunding.SmallBusinessIncentive);
        }

        private DataEntityMapper NewService(
            ILARSReferenceDataService larsReferenceDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService);
        }
    }
}