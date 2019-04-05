using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM35.Service.Input;
using ESFA.DC.ILR.FundingService.FM35.Service.Models;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global
            {
                LARSVersion = "1.0.0",
                PostcodeDisadvantageVersion = "2.0.0",
                OrgVersion = "3.0.0",
                UKPRN = 1234,
            };
            var organisationRefererenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();

            organisationRefererenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(global.UKPRN)).Returns(new List<OrgFunding> { new OrgFunding() });

            var dataEntity = NewService(organisationReferenceDataService: organisationRefererenceDataServiceMock.Object).BuildGlobalDataEntity(null, global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntity.Attributes["PostcodeDisadvantageVersion"].Value.Should().Be(global.PostcodeDisadvantageVersion);
            dataEntity.Attributes["OrgVersion"].Value.Should().Be(global.OrgVersion);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsVersion = "1.0.0";
            var postcodeDisadvantageVersion = "2.0.0";
            var orgVersion = "3.0.0";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var organisationRefererenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var fileDataServiceMock = new Mock<IFileDataService>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);
            postcodesRefererenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodeDisadvantageVersion);
            organisationRefererenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgVersion);
            fileDataServiceMock.Setup(f => f.UKPRN()).Returns(ukprn);

            var global = NewService(
                larsReferenceDataService: larsRefererenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesRefererenceDataServiceMock.Object,
                organisationReferenceDataService: organisationRefererenceDataServiceMock.Object,
                fileDataService: fileDataServiceMock.Object)
                .BuildGlobal();

            global.LARSVersion.Should().Be(larsVersion);
            global.UKPRN.Should().Be(ukprn);
            global.PostcodeDisadvantageVersion.Should().Be(postcodeDisadvantageVersion);
            global.OrgVersion.Should().Be(orgVersion);
        }

        [Fact]
        public void BuildOrgFunding()
        {
            var orgFunding = new OrgFunding()
            {
                UKPRN = 12345678,
                OrgFundFactor = "Factor",
                OrgFundFactType = "Type",
                OrgFundFactValue = "Value",
                OrgFundEffectiveFrom = new DateTime(2018, 8, 1),
                OrgFundEffectiveTo = null,
            };

            var dataEntity = NewService().BuildOrgFundingDataEntity(orgFunding);

            dataEntity.EntityName.Should().Be("OrgFunding");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["OrgFundFactor"].Value.Should().Be(orgFunding.OrgFundFactor);
            dataEntity.Attributes["OrgFundFactType"].Value.Should().Be(orgFunding.OrgFundFactType);
            dataEntity.Attributes["OrgFundFactValue"].Value.Should().Be(orgFunding.OrgFundFactValue);
            dataEntity.Attributes["OrgFundEffectiveFrom"].Value.Should().Be(orgFunding.OrgFundEffectiveFrom);
            dataEntity.Attributes["OrgFundEffectiveTo"].Value.Should().Be(orgFunding.OrgFundEffectiveTo);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new TestLearner()
            {
                LearnRefNumber = "ABC",
                DateOfBirthNullable = new DateTime(2000, 8, 1),
                ULN = 1234567890,
                PrevUKPRNNullable = 12345678,
                PMUKPRNNullable = 99999999,
            };

            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            postcodesRefererenceDataServiceMock.Setup(l => l.SFADisadvantagesForPostcode(It.IsAny<string>())).Returns(new List<SfaDisadvantage>());

            var dataEntity = NewService(
                postcodesReferenceDataService: postcodesRefererenceDataServiceMock.Object)
                .BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirthNullable);

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
                FworkCodeNullable = 5,
                LearnActEndDateNullable = new DateTime(2017, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2018, 1, 1),
                LearnStartDate = new DateTime(2019, 1, 1),
                OrigLearnStartDateNullable = new DateTime(2019, 1, 1),
                OtherFundAdjNullable = 6,
                OutcomeNullable = 7,
                PriorLearnFundAdjNullable = 8,
                ProgTypeNullable = 9,
                PwayCodeNullable = 10,
                StdCodeNullable = 11,
                LearningDeliveryFAMs = new List<TestLearningDeliveryFAM>()
                {
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "EEF", LearnDelFAMCode = "EEF" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "FFI", LearnDelFAMCode = "FFI" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
                },
            };

            var frameworkAim = new LARSFrameworkAim
            {
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
                FrameworkComponentType = 1,
                FworkCode = 5,
                ProgType = 9,
                PwayCode = 10,
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
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding
                    {
                        FundingCategory = "Matrix",
                        RateWeighted = 1.0m,
                        WeightingFactor = "G",
                        EffectiveFrom = new DateTime(2018, 1, 1),
                        EffectiveTo = new DateTime(2019, 1, 1),
                    },
                },
                LARSCareerLearningPilots = new List<LARSCareerLearningPilot>
                {
                    new LARSCareerLearningPilot
                    {
                        AreaCode = "DelLocPostcode",
                        SubsidyRate = 1.2m,
                        EffectiveFrom = new DateTime(2018, 1, 1),
                        EffectiveTo = new DateTime(2019, 1, 1),
                    },
                },
                LARSAnnualValues = new List<LARSAnnualValue>
                {
                    new LARSAnnualValue()
                },
                LARSLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>
                {
                    new LARSLearningDeliveryCategory()
                },
                LARSFrameworkAims = new List<LARSFrameworkAim>
                {
                   frameworkAim
                }
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            larsReferenceDataServiceMock.Setup(l => l.LARSFrameworkAimForForLearningDelivery(
                larsLearningDelivery.LARSFrameworkAims, learningDelivery.FworkCodeNullable, learningDelivery.ProgTypeNullable, learningDelivery.PwayCodeNullable)).Returns(frameworkAim);
            postcodesReferenceDataServiceMock.Setup(p => p.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(new List<SfaAreaCost> { new SfaAreaCost() });

            var dataEntity = NewService(larsReferenceDataService: larsReferenceDataServiceMock.Object, postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(27);
            dataEntity.Attributes["AchDate"].Value.Should().Be(learningDelivery.AchDateNullable);
            dataEntity.Attributes["AddHours"].Value.Should().Be(learningDelivery.AddHoursNullable);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["EmpOutcome"].Value.Should().Be(learningDelivery.EmpOutcomeNullable);
            dataEntity.Attributes["EnglandFEHEStatus"].Value.Should().Be(larsLearningDelivery.EnglandFEHEStatus);
            dataEntity.Attributes["EnglPrscID"].Value.Should().Be(larsLearningDelivery.EnglPrscID);
            dataEntity.Attributes["FrameworkCommonComponent"].Value.Should().Be(2);
            dataEntity.Attributes["FworkCode"].Value.Should().Be(learningDelivery.FworkCodeNullable);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDateNullable);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["LrnDelFAM_EEF"].Value.Should().Be("EEF");
            dataEntity.Attributes["LrnDelFAM_LDM1"].Value.Should().Be("LDM1");
            dataEntity.Attributes["LrnDelFAM_LDM2"].Value.Should().Be("LDM2");
            dataEntity.Attributes["LrnDelFAM_LDM3"].Value.Should().Be("LDM3");
            dataEntity.Attributes["LrnDelFAM_LDM4"].Value.Should().Be("LDM4");
            dataEntity.Attributes["LrnDelFAM_FFI"].Value.Should().Be("FFI");
            dataEntity.Attributes["LrnDelFAM_RES"].Value.Should().Be("RES");
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDateNullable);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdjNullable);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdjNullable);
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgTypeNullable);
            dataEntity.Attributes["PwayCode"].Value.Should().Be(learningDelivery.PwayCodeNullable);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new TestLearningDeliveryFAM
            {
                LearnDelFAMType = "EEF",
                LearnDelFAMCode = "1",
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
        public void BuildLearnerEmploymentStatus()
        {
            var learnerEmploymentStatus = new TestLearnerEmploymentStatus
            {
                AgreeId = "Id",
                DateEmpStatApp = new DateTime(2018, 1, 1),
                EmpIdNullable = 1,
                EmpStat = 2,
            };

            var largeEmployersReferenceDataServiceMock = new Mock<ILargeEmployersReferenceDataService>();

            largeEmployersReferenceDataServiceMock.Setup(l => l.LargeEmployersforEmpID(learnerEmploymentStatus.EmpIdNullable)).Returns(new List<LargeEmployers> { new LargeEmployers() });

            var dataEntity = NewService(largeEmployersReferenceDataService: largeEmployersReferenceDataServiceMock.Object).BuildLearnerEmploymentStatus(learnerEmploymentStatus);

            dataEntity.EntityName.Should().Be("LearnerEmploymentStatus");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["DateEmpStatApp"].Value.Should().Be(learnerEmploymentStatus.DateEmpStatApp);
            dataEntity.Attributes["EmpId"].Value.Should().Be(learnerEmploymentStatus.EmpIdNullable);
        }

        [Fact]
        public void BuildLargeEmployer()
        {
            var largeEmployer = new LargeEmployers
            {
                ERN = 100,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = null,
            };

            var dataEntity = NewService().BuildLargeEmployer(largeEmployer);

            dataEntity.EntityName.Should().Be("LargeEmployerReferenceData");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["LargeEmpEffectiveFrom"].Value.Should().Be(largeEmployer.EffectiveFrom);
            dataEntity.Attributes["LargeEmpEffectiveTo"].Value.Should().Be(largeEmployer.EffectiveTo);

            dataEntity.Children.Should().BeNullOrEmpty();
        }

        [Fact]
        public void BuildSFAPostcodeDisadvantage()
        {
            var sfaDisadvantage = new SfaDisadvantage
            {
                Uplift = 1.2m,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
            };

            var dataEntity = NewService().BuildSFAPostcodeDisadvantage(sfaDisadvantage);

            dataEntity.EntityName.Should().Be("SFA_PostcodeDisadvantage");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["DisUplift"].Value.Should().Be(sfaDisadvantage.Uplift);
            dataEntity.Attributes["DisUpEffectiveFrom"].Value.Should().Be(sfaDisadvantage.EffectiveFrom);
            dataEntity.Attributes["DisUpEffectiveTo"].Value.Should().Be(sfaDisadvantage.EffectiveTo);
        }

        [Fact]
        public void BuildSFAAreaCost()
        {
            var sfaAreaCost = new SfaAreaCost
            {
                AreaCostFactor = 1.2m,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
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
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
                RateWeighted = 1.0m,
                RateUnWeighted = 2.0m,
                WeightingFactor = "G",
            };

            var dataEntity = NewService().BuildLARSFunding(larsFunding);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARS_Funding");
            dataEntity.Attributes.Should().HaveCount(6);
            dataEntity.Attributes["LARSFundCategory"].Value.Should().Be(larsFunding.FundingCategory);
            dataEntity.Attributes["LARSFundEffectiveFrom"].Value.Should().Be(larsFunding.EffectiveFrom);
            dataEntity.Attributes["LARSFundEffectiveTo"].Value.Should().Be(larsFunding.EffectiveTo);
            dataEntity.Attributes["LARSFundWeightedRate"].Value.Should().Be(larsFunding.RateWeighted);
            dataEntity.Attributes["LARSFundUnweightedRate"].Value.Should().Be(larsFunding.RateUnWeighted);
            dataEntity.Attributes["LARSFundWeightingFactor"].Value.Should().Be(larsFunding.WeightingFactor);
        }

        [Fact]
        public void BuildLARSLearningDeliveryCategories()
        {
            var larsLearningDeliveryCategory = new LARSLearningDeliveryCategory
            {
                CategoryRef = 1,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildLARSLearningDeliveryCategories(larsLearningDeliveryCategory);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARSCategory");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LearnDelCatRef"].Value.Should().Be(larsLearningDeliveryCategory.CategoryRef);
            dataEntity.Attributes["LearnDelCatDateFrom"].Value.Should().Be(larsLearningDeliveryCategory.EffectiveFrom);
            dataEntity.Attributes["LearnDelCatDateTo"].Value.Should().Be(larsLearningDeliveryCategory.EffectiveTo);
        }

        [Fact]
        public void BuildLARSAnnualValue()
        {
            var larsAnnualValue = new LARSAnnualValue
            {
                BasicSkillsType = 1,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildLARSAnnualValue(larsAnnualValue);

            dataEntity.EntityName.Should().Be("LearningDeliveryAnnualValue");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LearnDelAnnValBasicSkillsTypeCode"].Value.Should().Be(larsAnnualValue.BasicSkillsType);
            dataEntity.Attributes["LearnDelAnnValDateFrom"].Value.Should().Be(larsAnnualValue.EffectiveFrom);
            dataEntity.Attributes["LearnDelAnnValDateTo"].Value.Should().Be(larsAnnualValue.EffectiveTo);
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>()
            {
                new TestLearningDeliveryFAM() { LearnDelFAMType = "EEF", LearnDelFAMCode = "1" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "2" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "3" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "4" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "5" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "FFI", LearnDelFAMCode = "6" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "7" },
            };

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.EEF.Should().Be("1");
            learningDeliveryFAMDenormalized.LDM1.Should().Be("2");
            learningDeliveryFAMDenormalized.LDM2.Should().Be("3");
            learningDeliveryFAMDenormalized.LDM3.Should().Be("4");
            learningDeliveryFAMDenormalized.LDM4.Should().Be("5");
            learningDeliveryFAMDenormalized.FFI.Should().Be("6");
            learningDeliveryFAMDenormalized.RES.Should().Be("7");
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_Null()
        {
            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(null);

            learningDeliveryFAMDenormalized.EEF.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
            learningDeliveryFAMDenormalized.FFI.Should().BeNull();
            learningDeliveryFAMDenormalized.RES.Should().BeNull();
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_NoMatches()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>();

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.EEF.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
            learningDeliveryFAMDenormalized.FFI.Should().BeNull();
            learningDeliveryFAMDenormalized.RES.Should().BeNull();
        }

        private DataEntityMapper NewService(
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = null,
            ILARSReferenceDataService larsReferenceDataService = null,
            IOrganisationReferenceDataService organisationReferenceDataService = null,
            IPostcodesReferenceDataService postcodesReferenceDataService = null,
            IFileDataService fileDataService = null)
        {
            return new DataEntityMapper(
                largeEmployersReferenceDataService,
                larsReferenceDataService,
                organisationReferenceDataService,
                postcodesReferenceDataService,
                fileDataService);
        }
    }
}
