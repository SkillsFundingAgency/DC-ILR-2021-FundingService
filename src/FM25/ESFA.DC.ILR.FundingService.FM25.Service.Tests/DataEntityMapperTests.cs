using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.FM25.Service.Input;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void BuildDPOutcome()
        {
            var dpOutcome = new DPOutcome()
            {
                OutCode = 1,
                OutType = "Type",
            };

            var dataEntity = NewService().BuildDPOutcome(dpOutcome);

            dataEntity.EntityName.Should().Be("DPOutcome");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["OutCode"].Value.Should().Be(dpOutcome.OutCode);
            dataEntity.Attributes["OutType"].Value.Should().Be(dpOutcome.OutType);
        }

        [Fact]
        public void BuildLearningDeliveryLARSValidity()
        {
            var larsValidity = new LARSValidity()
            {
                Category = "Category",
                LastNewStartDate = new DateTime(2017, 1, 1),
                StartDate = new DateTime(2018, 1, 1)
            };

            var dataEntity = NewService().BuildLearningDeliveryLARSValidity(larsValidity);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARSValidity");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["ValidityCategory"].Value.Should().Be(larsValidity.Category);
            dataEntity.Attributes["ValidityLastNewStartDate"].Value.Should().Be(larsValidity.LastNewStartDate);
            dataEntity.Attributes["ValidityStartDate"].Value.Should().Be(larsValidity.StartDate);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new TestLearner()
            {
                DateOfBirthNullable = new DateTime(1990, 12, 25),
                EngGrade = "A",
                LearnRefNumber = "ABC",
                MathGrade = "B",
                PlanEEPHoursNullable = 2,
                PlanLearnHoursNullable = 3,
                ULN = 123456,
                LearnerFAMs = new List<TestLearnerFAM>()
                {
                    new TestLearnerFAM() { LearnFAMType = "ECF", LearnFAMCode = 1 },
                    new TestLearnerFAM() { LearnFAMType = "EDF", LearnFAMCode = 2 },
                    new TestLearnerFAM() { LearnFAMType = "EDF", LearnFAMCode = 3 },
                    new TestLearnerFAM() { LearnFAMType = "EHC", LearnFAMCode = 4 },
                    new TestLearnerFAM() { LearnFAMType = "HNS", LearnFAMCode = 5 },
                    new TestLearnerFAM() { LearnFAMType = "MCF", LearnFAMCode = 6 },
                }
            };

            var fileDataServiceMock = new Mock<IFileDataService>();

            fileDataServiceMock.Setup(fds => fds.DPOutcomesForLearnRefNumber(learner.LearnRefNumber)).Returns(new List<DPOutcome>());

            var dataEntity = NewService(fileDataService: fileDataServiceMock.Object).BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(14);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirthNullable);
            dataEntity.Attributes["EngGrade"].Value.Should().Be(learner.EngGrade);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["LrnFAM_ECF"].Value.Should().Be(1);
            dataEntity.Attributes["LrnFAM_EDF1"].Value.Should().Be(2);
            dataEntity.Attributes["LrnFAM_EDF2"].Value.Should().Be(3);
            dataEntity.Attributes["LrnFAM_EHC"].Value.Should().Be(4);
            dataEntity.Attributes["LrnFAM_HNS"].Value.Should().Be(5);
            dataEntity.Attributes["LrnFAM_MCF"].Value.Should().Be(6);
            dataEntity.Attributes["MathGrade"].Value.Should().Be(learner.MathGrade);
            dataEntity.Attributes["PlanEEPHours"].Value.Should().Be(learner.PlanEEPHoursNullable);
            dataEntity.Attributes["PostcodeDisadvantageUplift"].Should().BeNull();
            dataEntity.Attributes["ULN"].Value.Should().Be(learner.ULN);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsCurrentVersion = "1.0.0";
            var orgCurrentVersion = "2.0.0";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var fileDataServiceMock = new Mock<IFileDataService>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgCurrentVersion);
            fileDataServiceMock.Setup(f => f.UKPRN()).Returns(ukprn);

            var dataEntity = NewService(larsRefererenceDataServiceMock.Object, orgReferenceDataServiceMock.Object, fileDataServiceMock.Object).BuildGlobalDataEntity(null);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(9);
            dataEntity.Attributes["AreaCostFactor1618"].Should().BeNull();
            dataEntity.Attributes["DisadvantageProportion"].Should().BeNull();
            dataEntity.Attributes["HistoricLargeProgrammeProportion"].Should().BeNull();
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(larsCurrentVersion);
            dataEntity.Attributes["OrgVersion"].Value.Should().Be(orgCurrentVersion);
            dataEntity.Attributes["ProgrammeWeighting"].Should().BeNull();
            dataEntity.Attributes["RetentionFactor"].Should().BeNull();
            dataEntity.Attributes["SpecialistResources"].Should().BeNull();
            dataEntity.Attributes["UKPRN"].Value.Should().Be(ukprn);

            dataEntity.Children.Should().HaveCount(0);
        }

        [Fact]
        public void BuildLearningDelivery()
        {
            var learningDelivery = new TestLearningDelivery()
            {
                AimSeqNumber = 1,
                AimType = 2,
                CompStatus = 3,
                FundModel = 4,
                LearnActEndDateNullable = new DateTime(2017, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2018, 1, 1),
                LearnStartDate = new DateTime(2019, 1, 1),
                ProgTypeNullable = 7,
                WithdrawReasonNullable = 8
            };

            var larsLearningDelivery = new LARSLearningDelivery()
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                SectorSubjectAreaTier2 = 1.0m
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);

            var dataEntity = NewService(larsReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(20);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["AwardOrgCode"].Value.Should().Be(larsLearningDelivery.AwardOrgCode);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["EFACOFType"].Value.Should().Be(larsLearningDelivery.EFACOFType);
            dataEntity.Attributes["FundModel"].Value.Should().Be(learningDelivery.FundModel);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDateNullable);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnAimRefTitle"].Value.Should().Be(larsLearningDelivery.LearnAimRefTitle);
            dataEntity.Attributes["LearnAimRefType"].Value.Should().Be(larsLearningDelivery.LearnAimRefType);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["LrnDelFAM_SOF"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM1"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM2"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM3"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM4"].Should().BeNull();
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgTypeNullable);
            dataEntity.Attributes["SectorSubjectAreaTier2"].Value.Should().Be(larsLearningDelivery.SectorSubjectAreaTier2);
            dataEntity.Attributes["WithdrawReason"].Value.Should().Be(learningDelivery.WithdrawReasonNullable);
        }

        [Fact]
        public void BuildLearnerFAMDenormalized()
        {
            var learnerFams = new List<TestLearnerFAM>()
            {
                new TestLearnerFAM() { LearnFAMType = "ECF", LearnFAMCode = 1 },
                new TestLearnerFAM() { LearnFAMType = "EDF", LearnFAMCode = 2 },
                new TestLearnerFAM() { LearnFAMType = "EDF", LearnFAMCode = 3 },
                new TestLearnerFAM() { LearnFAMType = "EHC", LearnFAMCode = 4 },
                new TestLearnerFAM() { LearnFAMType = "HNS", LearnFAMCode = 5 },
                new TestLearnerFAM() { LearnFAMType = "MCF", LearnFAMCode = 6 },
            };

            var learnerFamDenormalized = NewService().BuildLearnerFAMDenormalized(learnerFams);

            learnerFamDenormalized.ECF.Should().Be(1);
            learnerFamDenormalized.EDF1.Should().Be(2);
            learnerFamDenormalized.EDF2.Should().Be(3);
            learnerFamDenormalized.EHC.Should().Be(4);
            learnerFamDenormalized.HNS.Should().Be(5);
            learnerFamDenormalized.MCF.Should().Be(6);
        }

        [Fact]
        public void BuildLearnerFAMDenormalized_Null()
        {
            var learnerFamDenormalized = NewService().BuildLearnerFAMDenormalized(null);

            learnerFamDenormalized.ECF.Should().BeNull();
            learnerFamDenormalized.EDF1.Should().BeNull();
            learnerFamDenormalized.EDF2.Should().BeNull();
            learnerFamDenormalized.EHC.Should().BeNull();
            learnerFamDenormalized.HNS.Should().BeNull();
            learnerFamDenormalized.MCF.Should().BeNull();
        }

        [Fact]
        public void BuildLearnerFAMDenormalized_NoMatches()
        {
            var learnerFams = new List<TestLearnerFAM>();

            var learnerFamDenormalized = NewService().BuildLearnerFAMDenormalized(learnerFams);

            learnerFamDenormalized.ECF.Should().BeNull();
            learnerFamDenormalized.EDF1.Should().BeNull();
            learnerFamDenormalized.EDF2.Should().BeNull();
            learnerFamDenormalized.EHC.Should().BeNull();
            learnerFamDenormalized.HNS.Should().BeNull();
            learnerFamDenormalized.MCF.Should().BeNull();
        }

        [Fact]
        public void BuildLearnerFAMDenormalized_EDF2()
        {
            var learnerFams = new List<TestLearnerFAM>()
            {
                new TestLearnerFAM() { LearnFAMType = "ECF", LearnFAMCode = 1 },
                new TestLearnerFAM() { LearnFAMType = "EDF", LearnFAMCode = 2 },
                new TestLearnerFAM() { LearnFAMType = "EHC", LearnFAMCode = 4 },
                new TestLearnerFAM() { LearnFAMType = "HNS", LearnFAMCode = 5 },
                new TestLearnerFAM() { LearnFAMType = "MCF", LearnFAMCode = 6 },
            };

            var learnerFamDenormalized = NewService().BuildLearnerFAMDenormalized(learnerFams);

            learnerFamDenormalized.ECF.Should().Be(1);
            learnerFamDenormalized.EDF1.Should().Be(2);
            learnerFamDenormalized.EDF2.Should().BeNull();
            learnerFamDenormalized.EHC.Should().Be(4);
            learnerFamDenormalized.HNS.Should().Be(5);
            learnerFamDenormalized.MCF.Should().Be(6);
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>()
            {
                new TestLearningDeliveryFAM() { LearnDelFAMType = "SOF", LearnDelFAMCode = "1" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "2" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "3" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "4" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "5" },
            };

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.SOF.Should().Be("1");
            learningDeliveryFAMDenormalized.LDM1.Should().Be("2");
            learningDeliveryFAMDenormalized.LDM2.Should().Be("3");
            learningDeliveryFAMDenormalized.LDM3.Should().Be("4");
            learningDeliveryFAMDenormalized.LDM4.Should().Be("5");
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_Null()
        {
            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(null);

            learningDeliveryFAMDenormalized.SOF.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_NoMatches()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>();

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.SOF.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_EDF2()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>()
            {
                new TestLearningDeliveryFAM() { LearnDelFAMType = "SOF", LearnDelFAMCode = "1" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "2" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "3" },
            };

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.SOF.Should().Be("1");
            learningDeliveryFAMDenormalized.LDM1.Should().Be("2");
            learningDeliveryFAMDenormalized.LDM2.Should().Be("3");
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
        }

        private DataEntityMapper NewService(ILARSReferenceDataService larsReferenceDataService = null, IOrganisationReferenceDataService organisationReferenceDataService = null, IFileDataService fileDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService, organisationReferenceDataService, fileDataService);
        }
    }
}
