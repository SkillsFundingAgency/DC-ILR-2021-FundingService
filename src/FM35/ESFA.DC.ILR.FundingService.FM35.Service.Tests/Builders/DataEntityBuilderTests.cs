using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.ExternalData;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Model;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Model;
using ESFA.DC.ILR.FundingService.FM35.Service.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests.Builders
{
    public class DataEntityBuilderTests
    {
        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).EntityName.Should().BeEquivalentTo("global");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global IsGlobal True"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_IsGlobalTrue()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).IsGlobal.Should().BeTrue();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).Children.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).Children.Select(c => c.EntityName).Should().BeEquivalentTo("Learner", "OrgFunding");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).Attributes.Count.Should().Be(4);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Global Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Global_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();

            // ASSERT
            Entity(dataEntity).Attributes.Should().BeEquivalentTo(ExpectedGlobalAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).EntityName.Should().BeEquivalentTo("OrgFunding");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).Children.Select(c => c.EntityName).Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).Attributes.Count.Should().Be(5);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - OrgFunding Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_OrgFunding_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var orgFundingEntity = dataEntity.Children.Where(e => e.EntityName == "OrgFunding").Single();

            // ASSERT
            Entity(orgFundingEntity).Attributes.Should().BeEquivalentTo(ExpectedOrgFundingAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).EntityName.Should().BeEquivalentTo("Learner");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).Children.Count.Should().Be(3);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).Children.Select(c => c.EntityName).Should().BeEquivalentTo("LearningDelivery", "LearnerEmploymentStatus", "SFA_PostcodeDisadvantage");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).Attributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - Learner Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_Learner_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();

            // ASSERT
            Entity(learnerEntity).Attributes.Should().BeEquivalentTo(ExpectedLearnerAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).EntityName.Should().BeEquivalentTo("SFA_PostcodeDisadvantage");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).Children.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFAPostcodeDisadvantage Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFAPostcodeDisadvantage_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var sfaPostcodeDisadvantageEntity = learnerEntity.Children.Where(e => e.EntityName == "SFA_PostcodeDisadvantage").Single();

            // ASSERT
            Entity(sfaPostcodeDisadvantageEntity).Attributes.Should().BeEquivalentTo(ExpectedSFAPostcodeDisadvantageAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            Entity(learnerEmploymentStatusEntity).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            Entity(learnerEmploymentStatusEntity).EntityName.Should().BeEquivalentTo("LearnerEmploymentStatus");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            Entity(learnerEmploymentStatusEntity).IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            Entity(learnerEmploymentStatusEntity).Children.Count.Should().Be(1);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            var expectedEntityNames = new List<string>
            {
                "LargeEmployerReferenceData",
            };

            var actualEntityNames = Entity(learnerEmploymentStatusEntity).Children.Select(c => c.EntityName).ToList();

            expectedEntityNames.Should().BeEquivalentTo(actualEntityNames);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            Entity(learnerEmploymentStatusEntity).Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            Entity(learnerEmploymentStatusEntity).Attributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearnerEmploymentStatus Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearnerEmploymentStatus_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();

            // ASSERT
            Entity(learnerEmploymentStatusEntity).Attributes.Should().BeEquivalentTo(ExpectedLearnerEmpStatusAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).EntityName.Should().BeEquivalentTo("LargeEmployerReferenceData");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).Children.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).Attributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LargeEmployerReferenceData Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LargeEmployerReferenceData_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learnerEmploymentStatusEntity = learnerEntity.Children.Where(e => e.EntityName == "LearnerEmploymentStatus").Single();
            var largeEmployerEntity = learnerEmploymentStatusEntity.Children.Single();

            // ASSERT
            Entity(largeEmployerEntity).Attributes.Should().BeEquivalentTo(ExpectedLargeEmployerAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            Entity(learningDeliveryEntity).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            Entity(learningDeliveryEntity).EntityName.Should().BeEquivalentTo("LearningDelivery");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            Entity(learningDeliveryEntity).IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            Entity(learningDeliveryEntity).Children.Count.Should().Be(9);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            var expectedEntityNames = new List<string>
            {
                "LearningDeliveryFAM",
                "LearningDeliveryFAM",
                "LearningDeliveryFAM",
                "LearningDeliveryFAM",
                "LearningDeliveryFAM",
                "SFA_PostcodeAreaCost",
                "LearningDeliveryLARS_Funding",
                "LearningDeliveryLARSCategory",
                "LearningDeliveryAnnualValue",
            };

            var actualEntityNames = Entity(learningDeliveryEntity).Children.Select(c => c.EntityName).ToList();

            expectedEntityNames.Should().BeEquivalentTo(actualEntityNames);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            Entity(learningDeliveryEntity).Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            Entity(learningDeliveryEntity).Attributes.Count.Should().Be(27);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDelivery Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDelivery_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();

            // ASSERT
            Entity(learningDeliveryEntity).Attributes.Should().BeEquivalentTo(ExpectedLearningDeliveryAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM Entities Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).First().EntityName.Should().BeEquivalentTo("LearningDeliveryFAM");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).First().IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).First().Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).First().Children.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).First().Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).First().Attributes.Count.Should().Be(4);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryFAM Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryFAM_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryFAM = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryFAM");

            // ASSERT
            Entities(learningDeliveryFAM).First().Attributes.Should().BeEquivalentTo(ExpectedLearningDeliveryFAMAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost Entities Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).First().EntityName.Should().BeEquivalentTo("SFA_PostcodeAreaCost");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).First().IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).First().Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).First().Children.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).First().Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).First().Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - SFA_PostcodeAreaCost Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_SFA_PostcodeAreaCost_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var sfaPostcodeAreaCost = learningDeliveryEntity.Children.Where(c => c.EntityName == "SFA_PostcodeAreaCost");

            // ASSERT
            Entities(sfaPostcodeAreaCost).First().Attributes.Should().BeEquivalentTo(ExpectedLearningDeliverySFAPostcodeAreaCostAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding Entities Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).First().EntityName.Should().BeEquivalentTo("LearningDeliveryLARS_Funding");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).First().IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).First().Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).First().Children.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).First().Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).First().Attributes.Count.Should().Be(6);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARS_Funding Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARS_Funding_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSFunding = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARS_Funding");

            // ASSERT
            Entities(learningDeliveryLARSFunding).First().Attributes.Should().BeEquivalentTo(ExpectedLearningDeliveryLARSFundingAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue Entities Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).First().EntityName.Should().BeEquivalentTo("LearningDeliveryAnnualValue");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).First().IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).First().Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).First().Children.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).First().Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).First().Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryAnnualValue Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryAnnualValue_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSAnnualValue = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryAnnualValue");

            // ASSERT
            Entities(learningDeliveryLARSAnnualValue).First().Attributes.Should().BeEquivalentTo(ExpectedLearningDeliveryLARSAnnualValueAttributes());
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory Entities Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).Should().NotBeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).First().EntityName.Should().BeEquivalentTo("LearningDeliveryLARSCategory");
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).First().IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).First().Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).First().Children.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).First().Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).First().Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return Entity from EntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "EntityBuilder - LearningDeliveryLARSCategory Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void EntityBuilder_LearningDeliveryLARSCategory_AttributesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var dataEntity = SetupDataEntity().First();
            var learnerEntity = dataEntity.Children.Where(e => e.EntityName == "Learner").Single();
            var learningDeliveryEntity = learnerEntity.Children.Where(e => e.EntityName == "LearningDelivery").Single();
            var learningDeliveryLARSCategory = learningDeliveryEntity.Children.Where(c => c.EntityName == "LearningDeliveryLARSCategory");

            // ASSERT
            Entities(learningDeliveryLARSCategory).First().Attributes.Should().BeEquivalentTo(ExpectedLearningDeliveryLARSCategoryAttributes());
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.EntityName.Should().Be("global");
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - IsGlobal True"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_IsGlobal()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.IsGlobal.Should().BeTrue();
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.Children.Count.Should().Be(1);
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - Entity Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.Attributes.Should().NotBeNull();
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - Entity Attribute Count"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_AttributeCount()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.Attributes.Count.Should().Be(4);
        }

        /// <summary>
        /// Return Global Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Global - Entity Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Global_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedGlobalAttributes();

            // ACT
            var globalEntity = SetupGlobalEntity();

            // ASSERT
            globalEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.EntityName.Should().Be("OrgFunding");
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_IsGlobal()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.Parent.Should().BeNull(); // Null here because parent is created through AddChild Method.
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Entity Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.Attributes.Should().NotBeNull();
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Entity Attribute Count"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_AttributeCount()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.Attributes.Count.Should().Be(5);
        }

        /// <summary>
        /// Return OrgFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "OrgFunding - Entity Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_OrgFunding_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedOrgFundingAttributes();

            // ACT
            var orgEntity = SetupOrgEntity();

            // ASSERT
            orgEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.EntityName.Should().Be("Learner");
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - IsGlobal Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_IsGlobal()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.Children.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.Attributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Learner Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "Learner - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_Learner_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedLearnerAttributes();

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmpStatusEntity = SetupLearnerEmploymentStatusEntity();

            // ASSERT
            learnerEmpStatusEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmpStatusEntity = SetupLearnerEmploymentStatusEntity();

            // ASSERT
            learnerEmpStatusEntity.EntityName.Should().Be("LearnerEmploymentStatus");
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - IsGlobal Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_IsGlobal()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmpStatusEntity = SetupLearnerEmploymentStatusEntity();

            // ASSERT
            learnerEmpStatusEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmpStatusEntity = SetupLearnerEmploymentStatusEntity();

            // ASSERT
            learnerEmpStatusEntity.Children.Count.Should().Be(1);
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmpStatusEntity = SetupLearnerEmploymentStatusEntity();

            // ASSERT
            learnerEmpStatusEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEntity = SetupLearnerEntity();

            // ASSERT
            learnerEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmpStatusEntity = SetupLearnerEmploymentStatusEntity();

            // ASSERT
            learnerEmpStatusEntity.Attributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearnerLearnerEmploymentStatus - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearnerEmploymentStatus_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedLearnerEmpStatusAttributes();

            // ACT
            var learnerEmpStatusEntity = SetupLearnerEmploymentStatusEntity();

            // ASSERT
            learnerEmpStatusEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.EntityName.Should().Be("LargeEmployerReferenceData");
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - IsGlobal Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_IsGlobal()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.Attributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return LargeEmployer Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LargeEmployer - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LargeEmployer_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedLargeEmployerAttributes();

            // ACT
            var largeEmployersEntity = SetupLargeEmployersEntity();

            // ASSERT
            largeEmployersEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.EntityName.Should().Be("SFA_PostcodeDisadvantage");
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - IsGlobal Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_IsGlobal()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_SFAPostcodeDisadvantage_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedSFAPostcodeDisadvantageAttributes();

            // ACT
            var sfaPostcodeDisadvantageEntity = SetupSFAPostcodeDisadvantageEntity();

            // ASSERT
            sfaPostcodeDisadvantageEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return PivotLearningDeliveryFAMS Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "PivotLearningDeliveryFAMS - Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_PivotLearningDeliveryFAMS_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var famPivot = SetupPivotLearningDeliveryFAMS();

            // ASSERT
            famPivot.Should().NotBeNull();
        }

        /// <summary>
        /// Return PivotLearningDeliveryFAMS Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "PivotLearningDeliveryFAMS - Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_PivotLearningDeliveryFAMS_Correct()
        {
            // ARRANGE
            var expectedPivot = new LearningDeliveryFAMPivot
            {
                EEF = 2,
                FFI = 3,
                LDM1 = 100,
                LDM2 = 200,
                LDM3 = null,
                LDM4 = null,
                RES = 1
            };

            // ACT
            var famPivot = SetupPivotLearningDeliveryFAMS();

            // ASSERT
            famPivot.Should().BeEquivalentTo(expectedPivot);
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.EntityName.Should().Be("LearningDelivery");
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - IsGlobal Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_IsGlobal()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - Children Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_ChildrenCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.Children.Count.Should().Be(9);
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            /// ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.Attributes.Count.Should().Be(27);
        }

        /// <summary>
        /// Return LearningDelivery Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDelivery - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDelivery_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedLearningDeliveryAttributes();

            // ACT
            var learningDeliveryEntity = SetupLearningDeliveryEntity();

            // ASSERT
            learningDeliveryEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.EntityName.Should().Be("LearningDeliveryFAM");
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.Attributes.Count.Should().Be(4);
        }

        /// <summary>
        /// Return LearningDeliveryFAM Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryFAM_AttributesCorrect()
        {
            // ARRANGE
            var expectedLearningDeliveryFAM = ExpectedLearningDeliveryFAMAttributes();

            // ACT
            var learningDeliveryFAMEntity = SetupLearningDeliveryFAMEntity();

            // ASSERT
            learningDeliveryFAMEntity.Attributes.Should().BeEquivalentTo(expectedLearningDeliveryFAM);
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_EntityExists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.EntityName.Should().Be("SFA_PostcodeAreaCost");
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return LearningDeliverySFAPostcodeAreaCost Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFAPostcodeAreaCost - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliverySFAPostcodeAreaCost_AttributesCorrect()
        {
            // ARRANGE
            var expectedLearningDeliverySFAPostcodeAreaCost = ExpectedLearningDeliverySFAPostcodeAreaCostAttributes();

            // ACT
            var learningDeliverySFAPostcodeAreaCostEntity = SetupLearningDeliverySFAPostcodeAreaCostEntity();

            // ASSERT
            learningDeliverySFAPostcodeAreaCostEntity.Attributes.Should().BeEquivalentTo(expectedLearningDeliverySFAPostcodeAreaCost);
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_EntityExists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.EntityName.Should().Be("LearningDeliveryLARS_Funding");
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.Attributes.Count.Should().Be(6);
        }

        /// <summary>
        /// Return LearningDeliveryLARSFunding Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSFunding - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSFunding_AttributesCorrect()
        {
            // ARRANGE
            var expectedLearningDeliveryLARSFunding = ExpectedLearningDeliveryLARSFundingAttributes();

            // ACT
            var learningDeliveryLARSFundingEntity = SetupLearningDeliveryLARSFundingEntity();

            // ASSERT
            learningDeliveryLARSFundingEntity.Attributes.Should().BeEquivalentTo(expectedLearningDeliveryLARSFunding);
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_EntityExists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.EntityName.Should().Be("LearningDeliveryAnnualValue");
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSAnnualValue_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedLearningDeliveryLARSAnnualValueAttributes();

            // ACT
            var learningDeliveryLARSAnnualValueEntity = SetupLearningDeliveryLARSAnnualValueEntity();

            // ASSERT
            learningDeliveryLARSAnnualValueEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Entity Exists"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_EntityExists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.Should().NotBeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Entity Name Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_EntityNameCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.EntityName.Should().Be("LearningDeliveryLARSCategory");
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - IsGlobal False"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_IsGlobalFalse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.IsGlobal.Should().BeFalse();
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// </summaryLearningDeliveryLARSCategory
        [Fact(DisplayName = "LearningDeliveryFAM - Children Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_ChildrenCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.Children.Count.Should().Be(0);
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Parent Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_ParentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.Parent.Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Attributes Exist"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_AttributesExist()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.Attributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Attributes Count Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_AttributesCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.Attributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Entity from DataEntityBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Attributes Correct"), Trait("Funding Service DataEntity Builders", "Unit")]
        public void DataEntityBuilder_LearningDeliveryLARSCategory_AttributesCorrect()
        {
            // ARRANGE
            var expectedAttributes = ExpectedLearningDeliveryLARSCategoryAttributes();

            // ACT
            var learningDeliveryLARSCategoryEntity = SetupLearningDeliveryLARSCategoryEntity();

            // ASSERT
            learningDeliveryLARSCategoryEntity.Attributes.Should().BeEquivalentTo(expectedAttributes);
        }

        private IEnumerable<IDataEntity> SetupDataEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var dataEntityBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);
            var testLearners = new List<ILearner>
            {
                TestLearner,
                TestLearner,
            };

            return dataEntityBuilder.EntityBuilder(12345678, testLearners);
        }

        private IDataEntity SetupGlobalEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var globalBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            return globalBuilder.GlobalEntity(12345678);
        }

        private IDataEntity SetupOrgEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var orgBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            var orgFunding = referenceDataCacheMock.OrgFunding.Select(v => v.Value).First();

            return orgBuilder.OrgFundingEntity(orgFunding.First());
        }

        private IDataEntity SetupLearnerEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learnerBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            return learnerBuilder.LearnerEntity(TestLearner);
        }

        private IDataEntity SetupLearnerEmploymentStatusEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learnerBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            return learnerBuilder.LearnerEmploymentStatusEntity(TestLearner.LearnerEmploymentStatuses.FirstOrDefault());
        }

        private IDataEntity SetupLargeEmployersEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learnerBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            var lEmp = referenceDataCacheMock.LargeEmployers
                   .Where(k => k.Key == TestLearner.LearnerEmploymentStatuses.FirstOrDefault().EmpIdNullable)
                   .Select(v => v.Value).Single();

            return learnerBuilder.LargeEmployersEntity(lEmp.Single());
        }

        private IDataEntity SetupSFAPostcodeDisadvantageEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learnerBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            var sfaPostcode = referenceDataCacheMock.SfaDisadvantage
                   .Where(k => k.Key == TestLearner.PostcodePrior)
                   .Select(v => v.Value).Single();

            return learnerBuilder.SFAPostcodeDisadvantageEntity(sfaPostcode.Single());
        }

        private LearningDeliveryFAMPivot SetupPivotLearningDeliveryFAMS()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learnerBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            return learnerBuilder.PivotLearningDeliveryFAMS(TestLearner.LearningDeliveries.Single());
        }

        private IDataEntity SetupLearningDeliveryEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learnerBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            var larsLearningDelivery = referenceDataCacheMock.LARSLearningDelivery.Select(v => v.Value).FirstOrDefault();
            var larsFrameworkAims = referenceDataCacheMock.LARSFrameworkAims.Select(v => v.Value.ToList());

            return learnerBuilder.LearningDeliveryEntity(TestLearner.LearningDeliveries.FirstOrDefault(), larsLearningDelivery, larsFrameworkAims.Single());
        }

        private IDataEntity SetupLearningDeliveryFAMEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learningDeilveryFAMBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            return learningDeilveryFAMBuilder.LearningDeliveryFAMEntity(TestLearningDelivery.LearningDeliveryFAM.FirstOrDefault());
        }

        private IDataEntity SetupLearningDeliverySFAPostcodeAreaCostEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learningDeilverySFAAreaCostBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            IEnumerable<SfaAreaCost> SFAAreaCost = referenceDataCacheMock.SfaAreaCost.Select(s => s.Value).Single();

            return learningDeilverySFAAreaCostBuilder.SFAAreaCostEntity(SFAAreaCost.FirstOrDefault());
        }

        private IDataEntity SetupLearningDeliveryLARSFundingEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learningDeilveryLARSFundingBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            IEnumerable<LARSFunding> LARSFunding = referenceDataCacheMock.LARSFunding.Select(l => l.Value).Single();

            return learningDeilveryLARSFundingBuilder.LARSFundingEntity(LARSFunding.Single());
        }

        private IDataEntity SetupLearningDeliveryLARSAnnualValueEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learningDeilveryLARSAnnualValueBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            IEnumerable<LARSAnnualValue> LARSAnnualValue = referenceDataCacheMock.LARSAnnualValue.Select(l => l.Value).Single();

            return learningDeilveryLARSAnnualValueBuilder.LARSAnnualValueEntity(LARSAnnualValue.Single());
        }

        private IDataEntity SetupLearningDeliveryLARSCategoryEntity()
        {
            var referenceDataCacheMock = SetupReferenceDataMock();
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(SetupReferenceDataMock());
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(SetupReferenceDataMock());
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(SetupReferenceDataMock());
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(SetupReferenceDataMock());

            var learningDeilveryLARSCategoryBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            IEnumerable<LARSLearningDeliveryCategory> LARSLearningDeliveryCategory = referenceDataCacheMock.LARSLearningDeliveryCatgeory.Select(l => l.Value).Single();

            return learningDeilveryLARSCategoryBuilder.LARSLearningDeliveryCategoryEntity(LARSLearningDeliveryCategory.Single());
        }

        private static IReferenceDataCache SetupReferenceDataMock()
        {
            return Mock.Of<IReferenceDataCache>(l =>
                l.LARSCurrentVersion == "Version_005"
                && l.PostcodeCurrentVersion == "Version_003"
                && l.SfaDisadvantage == new Dictionary<string, IEnumerable<SfaDisadvantage>>
                {
                    {
                        "CV1 2WT", new List<SfaDisadvantage>
                        {
                            new SfaDisadvantage
                            {
                                Postcode = "CV1 2WT",
                                Uplift = 1.54m,
                                EffectiveFrom = new System.DateTime(2000, 01, 01),
                                EffectiveTo = null,
                            }
                        }
                    }
                }
                && l.OrgVersion == "Version_002"
                && l.OrgFunding == new Dictionary<long, IEnumerable<OrgFunding>>
                {
                    { 12345678, new List<OrgFunding>
                        {
                            new OrgFunding
                            {
                                UKPRN = 12345678,
                                OrgFundFactor = "Factor",
                                OrgFundFactType = "FactorType",
                                OrgFundFactValue = "1.918",
                                OrgFundEffectiveFrom = new System.DateTime(2018, 08, 01),
                                OrgFundEffectiveTo = new System.DateTime(2019, 07, 31),
                            }
                        }
                    }
                }
                 && l.LargeEmployers == new Dictionary<int, IEnumerable<LargeEmployers>>
                 {
                     {
                         99999, new List<LargeEmployers>
                         {
                            new LargeEmployers
                            {
                                ERN = 99999,
                                EffectiveFrom = new System.DateTime(2018, 05, 01),
                                EffectiveTo = null,
                            },
                         }
                     }
                 }
                && l.LARSLearningDelivery == new Dictionary<string, LARSLearningDelivery>
                {
                    { "123456", new LARSLearningDelivery
                        {
                            LearnAimRef = "123456",
                            EnglandFEHEStatus = "EnglandStatus",
                            EnglPrscID = 100,
                            FrameworkCommonComponent = 20,
                        }
                    }
                }
                && l.SfaAreaCost == new Dictionary<string, IEnumerable<SfaAreaCost>>
                {
                    { "CV1 2WT", new List<SfaAreaCost>
                        {
                            new SfaAreaCost
                            {
                                Postcode = "CV1 2WT",
                                EffectiveFrom = new System.DateTime(2000, 08, 30),
                                AreaCostFactor = 1.2m,
                            }
                        }
                    }
                }
                && l.LARSFunding == new Dictionary<string, IEnumerable<LARSFunding>>
                {
                    {
                        "123456", new List<LARSFunding>
                        {
                            new LARSFunding
                            {
                                LearnAimRef = "123456",
                                EffectiveFrom = new System.DateTime(2018, 08, 30),
                                WeightingFactor = "B",
                                RateUnWeighted = 0.5m,
                                RateWeighted = 12345m,
                                FundingCategory = "Matrix",
                            }
                        }
                    }
                }
                && l.LARSFrameworkAims == new Dictionary<string, IEnumerable<LARSFrameworkAims>>
                {
                    {
                        "123456", new List<LARSFrameworkAims>
                        {
                            new LARSFrameworkAims
                            {
                                LearnAimRef = "123456",
                                EffectiveFrom = new System.DateTime(2018, 08, 30),
                                EffectiveTo = null,
                                FrameworkComponentType = 0,
                                FworkCode = 20,
                                ProgType = 2,
                                PwayCode = 3,
                            },
                            new LARSFrameworkAims
                            {
                                LearnAimRef = "123456",
                                EffectiveFrom = new System.DateTime(2018, 08, 30),
                                EffectiveTo = null,
                                FrameworkComponentType = 0,
                                FworkCode = 20,
                                ProgType = 4,
                                PwayCode = 5,
                            }
                        }
                    }
                }
                 && l.LARSAnnualValue == new Dictionary<string, IEnumerable<LARSAnnualValue>>
                {
                    {
                        "123456", new List<LARSAnnualValue>
                        {
                            new LARSAnnualValue
                            {
                                LearnAimRef = "123456",
                                EffectiveFrom = new System.DateTime(2018, 08, 30),
                                EffectiveTo = null,
                                BasicSkillsType = 1,
                            }
                        }
                    }
                }
                  && l.LARSLearningDeliveryCatgeory == new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>
                {
                    {
                        "123456", new List<LARSLearningDeliveryCategory>
                        {
                            new LARSLearningDeliveryCategory
                            {
                                LearnAimRef = "123456",
                                EffectiveFrom = new System.DateTime(2018, 08, 30),
                                EffectiveTo = null,
                                CategoryRef = 10,
                            }
                        }
                    }
                });
        }

        private ILearner TestLearner => new MessageLearner
        {
            LearnRefNumber = "Learner1",
            DateOfBirthSpecified = true,
            DateOfBirth = new System.DateTime(2000, 01, 01),
            PostcodePrior = "CV1 2WT",
            LearnerEmploymentStatus = new MessageLearnerLearnerEmploymentStatus[]
            {
                new MessageLearnerLearnerEmploymentStatus
                {
                    EmpIdSpecified = true,
                    EmpId = 99999,
                    AgreeId = "AgreeID",
                    EmpStat = 1,
                    DateEmpStatApp = new System.DateTime(2018, 08, 01),
                }
            },
            LearningDelivery = new MessageLearnerLearningDelivery[]
            {
                TestLearningDelivery,
            }
        };

        private static readonly MessageLearnerLearningDelivery TestLearningDelivery = new MessageLearnerLearningDelivery
        {
            LearnAimRef = "123456",
            AchDateSpecified = true,
            AchDate = new System.DateTime(2018, 10, 01),
            AddHoursSpecified = true,
            AddHours = 20,
            AimSeqNumber = 1,
            AimType = 2,
            CompStatus = 3,
            DelLocPostCode = "CV1 2WT",
            EmpOutcomeSpecified = true,
            EmpOutcome = 4,
            FworkCodeSpecified = true,
            FworkCode = 20,
            LearnActEndDateSpecified = true,
            LearnActEndDate = System.DateTime.Parse("2018-10-01"),
            LearnStartDate = System.DateTime.Parse("2018-08-01"),
            LearnPlanEndDate = System.DateTime.Parse("2018-12-01"),
            OrigLearnStartDateSpecified = true,
            OrigLearnStartDate = System.DateTime.Parse("2018-08-01"),
            OtherFundAdjSpecified = false,
            OutcomeSpecified = true,
            Outcome = 1,
            PriorLearnFundAdjSpecified = false,
            ProgTypeSpecified = true,
            ProgType = 2,
            PwayCodeSpecified = true,
            PwayCode = 3,
            LearningDeliveryFAM = new[]
            {
                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                {
                    LearnDelFAMCode = "100",
                    LearnDelFAMType = "LDM",
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-08-30"),
                    LearnDelFAMDateToSpecified = true,
                    LearnDelFAMDateTo = System.DateTime.Parse("2017-10-30")
                },
                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                {
                    LearnDelFAMCode = "200",
                    LearnDelFAMType = "LDM",
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-10-31"),
                    LearnDelFAMDateToSpecified = true,
                    LearnDelFAMDateTo = System.DateTime.Parse("2018-11-30")
                },
                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                {
                    LearnDelFAMCode = "1",
                    LearnDelFAMType = "RES",
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-12-01"),
                    LearnDelFAMDateToSpecified = false
                },
                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                {
                    LearnDelFAMCode = "2",
                    LearnDelFAMType = "EEF",
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-12-01"),
                    LearnDelFAMDateToSpecified = false
                },
                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                {
                    LearnDelFAMCode = "3",
                    LearnDelFAMType = "FFI",
                    LearnDelFAMDateFromSpecified = true,
                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-12-01"),
                    LearnDelFAMDateToSpecified = false
                }
            }
        };

        private IDictionary<string, IAttributeData> ExpectedGlobalAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "UKPRN", new AttributeData("UKPRN", 12345678) },
                { "LARSVersion", new AttributeData("LARSVersion", "Version_005") },
                { "OrgVersion", new AttributeData("OrgVersion", "Version_002") },
                { "PostcodeDisadvantageVersion", new AttributeData("PostcodeDisadvantageVersion", "Version_003") },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedOrgFundingAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "OrgFundEffectiveTo", new AttributeData("OrgFundEffectiveTo", new System.DateTime(2019, 07, 31)) },
                { "OrgFundEffectiveFrom", new AttributeData("OrgFundEffectiveFrom", new System.DateTime(2018, 08, 01)) },
                { "OrgFundFactor", new AttributeData("OrgFundFactor", "Factor") },
                { "OrgFundFactValue", new AttributeData("OrgFundFactValue", "1.918") },
                { "OrgFundFactType", new AttributeData("OrgFundFactType", "FactorType") },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearnerAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "LearnRefNumber", new AttributeData("LearnRefNumber", "Learner1") },
                { "DateOfBirth", new AttributeData("DateOfBirth", new System.DateTime(2000, 01, 01)) },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearnerEmpStatusAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "EmpId", new AttributeData("EmpId", 99999) },
                { "DateEmpStatApp", new AttributeData("DateEmpStatApp", new System.DateTime(2018, 08, 01)) },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLargeEmployerAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "LargeEmpEffectiveFrom", new AttributeData("LargeEmpEffectiveFrom", new System.DateTime(2018, 05, 01)) },
                { "LargeEmpEffectiveTo", new AttributeData("LargeEmpEffectiveTo", null) },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedSFAPostcodeDisadvantageAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "DisUplift", new AttributeData("DisUplift",  1.54m) },
                { "DisUpEffectiveFrom", new AttributeData("DisUpEffectiveFrom", new System.DateTime(2000, 01, 01)) },
                { "DisUpEffectiveTo", new AttributeData("DisUpEffectiveTo", null) },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearningDeliveryAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "AchDate", new AttributeData("AchDate", new System.DateTime(2018, 10, 01)) },
                { "AddHours", new AttributeData("AddHours", 20) },
                { "AimSeqNumber", new AttributeData("AimSeqNumber", 1) },
                { "AimType", new AttributeData("AimType", 2) },
                { "CompStatus", new AttributeData("CompStatus", 3) },
                { "EmpOutcome", new AttributeData("EmpOutcome", 4) },
                { "EnglandFEHEStatus", new AttributeData("EnglandFEHEStatus", "EnglandStatus") },
                { "EnglPrscID", new AttributeData("EnglPrscID", 100) },
                { "FworkCode", new AttributeData("FworkCode", 20) },
                { "FrameworkCommonComponent", new AttributeData("FrameworkCommonComponent", 20) },
                { "FrameworkComponentType", new AttributeData("FrameworkComponentType", 0) },
                { "LearnActEndDate", new AttributeData("LearnActEndDate", new System.DateTime(2018, 10, 01)) },
                { "LearnPlanEndDate", new AttributeData("LearnPlanEndDate", new System.DateTime(2018, 12, 01)) },
                { "LearnStartDate", new AttributeData("LearnStartDate", new System.DateTime(2018, 08, 01)) },
                { "LrnDelFAM_EEF", new AttributeData("LrnDelFAM_EEF", 2) },
                { "LrnDelFAM_LDM1", new AttributeData("LrnDelFAM_LDM1", 100) },
                { "LrnDelFAM_LDM2", new AttributeData("LrnDelFAM_LDM2", 200) },
                { "LrnDelFAM_LDM3", new AttributeData("LrnDelFAM_LDM3", null) },
                { "LrnDelFAM_LDM4", new AttributeData("LrnDelFAM_LDM4", null) },
                { "LrnDelFAM_FFI", new AttributeData("LrnDelFAM_FFI", 3) },
                { "LrnDelFAM_RES", new AttributeData("LrnDelFAM_RES", 1) },
                { "OrigLearnStartDate", new AttributeData("OrigLearnStartDate", new System.DateTime(2018, 08, 01)) },
                { "OtherFundAdj", new AttributeData("OtherFundAdj", null) },
                { "Outcome", new AttributeData("Outcome", 1) },
                { "PriorLearnFundAdj", new AttributeData("PriorLearnFundAdj", null) },
                { "ProgType", new AttributeData("ProgType", 2) },
                { "PwayCode", new AttributeData("PwayCode", 3) },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearningDeliveryFAMAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "LearnDelFAMCode", new AttributeData("LearnDelFAMCode", "100") },
                { "LearnDelFAMDateFrom", new AttributeData("LearnDelFAMDateFrom", new System.DateTime(2017, 08, 30)) },
                { "LearnDelFAMDateTo", new AttributeData("LearnDelFAMDateTo", new System.DateTime(2017, 10, 30)) },
                { "LearnDelFAMType", new AttributeData("LearnDelFAMType", "LDM") }
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearningDeliverySFAPostcodeAreaCostAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "AreaCosEffectiveFrom", new AttributeData("AreaCosEffectiveFrom", new System.DateTime(2000, 08, 30)) },
                { "AreaCosEffectiveTo", new AttributeData("AreaCosEffectiveTo", null) },
                { "AreaCosFactor", new AttributeData("AreaCosFactor", 1.2m) }
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearningDeliveryLARSFundingAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "LARSFundCategory", new AttributeData("LARSFundCategory", "Matrix") },
                { "LARSFundEffectiveFrom", new AttributeData("LARSFundEffectiveFrom",  new System.DateTime(2018, 08, 30)) },
                { "LARSFundEffectiveTo", new AttributeData("LARSFundEffectiveTo", null) },
                { "LARSFundUnweightedRate", new AttributeData("LARSFundUnweightedRate", 0.5m) },
                { "LARSFundWeightedRate", new AttributeData("LARSFundWeightedRate", 12345m) },
                { "LARSFundWeightingFactor", new AttributeData("LARSFundWeightingFactor", "B") }
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearningDeliveryLARSAnnualValueAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "LearnDelAnnValBasicSkillsTypeCode", new AttributeData("LearnDelAnnValBasicSkillsTypeCode", 1) },
                { "LearnDelAnnValDateFrom", new AttributeData("LearnDelAnnValDateFrom",  new System.DateTime(2018, 08, 30)) },
                { "LearnDelAnnValDateTo", new AttributeData("LearnDelAnnValDateTo", null) },
            };
        }

        private IDictionary<string, IAttributeData> ExpectedLearningDeliveryLARSCategoryAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "LearnDelCatRef", new AttributeData("LearnDelCatRef", 10) },
                { "LearnDelCatDateFrom", new AttributeData("LearnDelCatDateFrom",  new System.DateTime(2018, 08, 30)) },
                { "LearnDelCatDateTo", new AttributeData("LearnDelCatDateTo", null) },
            };
        }

        private IDataEntity Entity(IDataEntity entity)
        {
            return entity;
        }

        private IEnumerable<IDataEntity> Entities(IEnumerable<IDataEntity> entities)
        {
            return entities;
        }
    }
}
