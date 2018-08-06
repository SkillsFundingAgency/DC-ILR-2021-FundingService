﻿using System;
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
                ULN = 123456
            };

            var fileDataServiceMock = new Mock<IFileDataService>();

            fileDataServiceMock.Setup(fds => fds.DPOutcomesForLearnRefNumber(learner.LearnRefNumber)).Returns(new List<DPOutcome>());

            var dataEntity = NewService(fileDataService: fileDataServiceMock.Object).BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(14);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirthNullable);
            dataEntity.Attributes["EngGrade"].Value.Should().Be(learner.EngGrade);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["LrnFAM_ECF"].Should().BeNull();
            dataEntity.Attributes["LrnFAM_EDF1"].Should().BeNull();
            dataEntity.Attributes["LrnFAM_EDF2"].Should().BeNull();
            dataEntity.Attributes["LrnFAM_EHC"].Should().BeNull();
            dataEntity.Attributes["LrnFAM_HNS"].Should().BeNull();
            dataEntity.Attributes["LrnFAM_MCF"].Should().BeNull();
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

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(new LARSLearningDelivery());

            var dataEntity = NewService(larsReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(20);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["AwardOrgCode"].Should().BeNull();
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["EFACOFType"].Should().BeNull();
            dataEntity.Attributes["FundModel"].Value.Should().Be(learningDelivery.FundModel);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDateNullable);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnAimRefTitle"].Should().BeNull();
            dataEntity.Attributes["LearnAimRefType"].Should().BeNull();
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["LrnDelFAM_SOF"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM1"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM2"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM3"].Should().BeNull();
            dataEntity.Attributes["LearnDelFAM_LDM4"].Should().BeNull();
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgTypeNullable);
            dataEntity.Attributes["SectorSubjectAreaTier2"].Should().BeNull();
            dataEntity.Attributes["WithdrawReason"].Value.Should().Be(learningDelivery.WithdrawReasonNullable);
        }

        private DataEntityMapper NewService(ILARSReferenceDataService larsReferenceDataService = null, IOrganisationReferenceDataService organisationReferenceDataService = null, IFileDataService fileDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService, organisationReferenceDataService, fileDataService);
        }
    }
}
