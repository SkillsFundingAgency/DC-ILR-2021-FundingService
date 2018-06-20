using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.Service.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface.Builders;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests.Builders
{
    public class AttributeBuilderTests
    {
        /// <summary>
        /// Return Global Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Global - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_Global_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalAttributes = SetupGlobalAttributes(12345678, "Version_005", "Version_002", "Version_003");

            // ASSERT
            globalAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return Global Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Global - CorrectCount"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_Global_CorrectCount()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalAttributes = SetupGlobalAttributes(12345678, "Version_005", "Version_002", "Version_003");

            // ASSERT
            globalAttributes.Count.Should().Be(4);
        }

        /// <summary>
        /// Return Global Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Global - UKPRN Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_Global_CorrectUKPRN()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalAttributes = SetupGlobalAttributes(12345678, "Version_005", "Version_002", "Version_003");

            // ASSERT
            AttributeValue(globalAttributes, "UKPRN").Should().Be(12345678);
        }

        /// <summary>
        /// Return Global Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Global - LARS Version Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_Global_CorrectLARSVersion()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalAttributes = SetupGlobalAttributes(12345678, "Version_005", "Version_002", "Version_003");

            // ASSERT
            AttributeValue(globalAttributes, "LARSVersion").Should().Be("Version_005");
        }

        /// <summary>
        /// Return Global Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Global - PostcodeFactors Version Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_Global_CorrectPostcodeFactorsVersion()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var globalAttributes = SetupGlobalAttributes(12345678, "Version_005", "Version_002", "Version_003");

            // ASSERT
            AttributeValue(globalAttributes, "PostcodeDisadvantageVersion").Should().Be("Version_003");
        }

        /// <summary>
        /// Return OrgFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "OrgFunding - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_OrgFunding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgFundingAttributes = SetupOrgFundingAttributes(new System.DateTime(2018, 02, 01), null, "Factor", "Type", "1.2000");

            // ASSERT
            orgFundingAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return OrgFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "OrgFunding - Attributes Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_OrgFunding_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgFundingAttributes = SetupOrgFundingAttributes(new System.DateTime(2018, 02, 01), null, "Factor", "Type", "1.2000");

            // ASSERT
            orgFundingAttributes.Count.Should().Be(5);
        }

        /// <summary>
        /// Return OrgFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "OrgFunding - OrgFundEffectiveFrom Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_OrgFundEffectiveFrom_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var orgFundingAttributes = SetupOrgFundingAttributes(new System.DateTime(2018, 02, 01), null, "Factor", "Type", "1.2000");

            // ASSERT
            AttributeValue(orgFundingAttributes, "OrgFundEffectiveFrom").Should().BeEquivalentTo(new System.DateTime(2018, 02, 01));
        }

        /// <summary>
        /// Return Learner Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Learner - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_Learner_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerAttributes = SetupLearnerAttributes("Learner1", new System.DateTime(2000, 01, 01));

            // ASSERT
            learnerAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return Learner Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Learner - Correct Count"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_Learner_CorrectCount()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerAttributes = SetupLearnerAttributes("Learner1", new System.DateTime(2000, 01, 01));

            // ASSERT
            learnerAttributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Learner Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Learner - LearnRefNumber Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnRefNumber_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerAttributes = SetupLearnerAttributes("Learner1", new System.DateTime(2000, 01, 01));

            // ASSERT
            AttributeValue(learnerAttributes, "LearnRefNumber").Should().Be("Learner1");
        }

        /// <summary>
        /// Return Learner Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "Learner - DateOfBirth Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_DateOfBirth_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerAttributes = SetupLearnerAttributes("Learner1", new System.DateTime(2000, 01, 01));

            // ASSERT
            AttributeValue(learnerAttributes, "DateOfBirth").Should().BeEquivalentTo(new System.DateTime(2000, 01, 01));
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnerEmploymentStatus_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmploymentStatusAttributes = SetupLearnerEmploymentStatusAttributes(999999, new System.DateTime(2018, 02, 01));

            // ASSERT
            learnerEmploymentStatusAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - Attributes Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnerEmploymentStatus_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmploymentStatusAttributes = SetupLearnerEmploymentStatusAttributes(999999, new System.DateTime(2018, 02, 01));

            // ASSERT
            learnerEmploymentStatusAttributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - EmpId Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_EmpId_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmploymentStatusAttributes = SetupLearnerEmploymentStatusAttributes(999999, new System.DateTime(2018, 02, 01));

            // ASSERT
            AttributeValue(learnerEmploymentStatusAttributes, "EmpId").Should().Be(999999);
        }

        /// <summary>
        /// Return LearnerEmploymentStatus Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearnerEmploymentStatus - DateEmpStatApp Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_DateEmpStatApp_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learnerEmploymentStatusAttributes = SetupLearnerEmploymentStatusAttributes(999999, new System.DateTime(2018, 02, 01));

            // ASSERT
            AttributeValue(learnerEmploymentStatusAttributes, "DateEmpStatApp").Should().BeEquivalentTo(new System.DateTime(2018, 02, 01));
        }

        /// <summary>
        /// Return LargeEmployer Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LargeEmployer - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LargeEmployer_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployerAttributes = SetupLargeEmployerReferenceDataAttributes(new System.DateTime(2018, 02, 01), null);

            // ASSERT
            largeEmployerAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LargeEmployer Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LargeEmployer - Attributes Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LargeEmployer_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployerAttributes = SetupLargeEmployerReferenceDataAttributes(new System.DateTime(2018, 02, 01), null);

            // ASSERT
            largeEmployerAttributes.Count.Should().Be(2);
        }

        /// <summary>
        /// Return LargeEmployer Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LargeEmployer - LargeEmpEffectiveFrom Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LargeEmpEffectiveFrom_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployerAttributes = SetupLargeEmployerReferenceDataAttributes(new System.DateTime(2018, 02, 01), null);

            // ASSERT
            AttributeValue(largeEmployerAttributes, "LargeEmpEffectiveFrom").Should().BeEquivalentTo(new System.DateTime(2018, 02, 01));
        }

        /// <summary>
        /// Return LargeEmployer Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LargeEmployer - LargeEmpEffectiveTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LargeEmpEffectiveTo_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var largeEmployerAttributes = SetupLargeEmployerReferenceDataAttributes(new System.DateTime(2018, 02, 01), null);

            // ASSERT
            AttributeValue(largeEmployerAttributes, "LargeEmpEffectiveTo").Should().Be(null);
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_SFAPostcodeDisadvantage_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageAttributes = SetupSFAPostcodeDisadvantageAttributes(1.5m, new System.DateTime(2018, 02, 01), new System.DateTime(2020, 02, 01));

            // ASSERT
            sfaPostcodeDisadvantageAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - Attributes Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_SFAPostcodeDisadvantage_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageAttributes = SetupSFAPostcodeDisadvantageAttributes(1.5m, new System.DateTime(2018, 02, 01), new System.DateTime(2020, 02, 01));

            // ASSERT
            sfaPostcodeDisadvantageAttributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - DisUplift Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_DisUplift_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageAttributes = SetupSFAPostcodeDisadvantageAttributes(1.5m, new System.DateTime(2018, 02, 01), new System.DateTime(2020, 02, 01));

            // ASSERT
            AttributeValue(sfaPostcodeDisadvantageAttributes, "DisUplift").Should().Be(1.5m);
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - DisUpEffectiveTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_DisUpEffectiveFrom_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageAttributes = SetupSFAPostcodeDisadvantageAttributes(1.5m, new System.DateTime(2018, 02, 01), new System.DateTime(2020, 02, 01));

            // ASSERT
            AttributeValue(sfaPostcodeDisadvantageAttributes, "DisUpEffectiveFrom").Should().BeEquivalentTo(new System.DateTime(2018, 02, 01));
        }

        /// <summary>
        /// Return SFAPostcodeDisadvantage Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "SFAPostcodeDisadvantage - DisUpEffectiveTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_DisUpEffectiveTo_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var sfaPostcodeDisadvantageAttributes = SetupSFAPostcodeDisadvantageAttributes(1.5m, new System.DateTime(2018, 02, 01), new System.DateTime(2020, 02, 01));

            // ASSERT
            AttributeValue(sfaPostcodeDisadvantageAttributes, "DisUpEffectiveTo").Should().BeEquivalentTo(new System.DateTime(2020, 02, 01));
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - Attributes Exists"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            learningDeliveryAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - Correct Count"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_CorrectCount()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            learningDeliveryAttributes.Count.Should().Be(27);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - AchDate Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_AchDateCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "AchDate").Should().Be(new System.DateTime(2018, 10, 01));
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - AddHours Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_AddHoursCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "AddHours").Should().Be(10);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - AimSeqNumber Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_AimSeqNumberCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "AimSeqNumber").Should().Be(1);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - AimType Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_AimTypeCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "AimType").Should().Be(3);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - CompStatus Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_CompStatusCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "CompStatus").Should().Be(1);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - EmpOutcome Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_EmpOutcomeCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "EmpOutcome").Should().Be(1);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - EnglandFEHEStatus Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_EnglandFEHEStatusCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "EnglandFEHEStatus").Should().Be("EnglandStatus");
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - EnglPrscID Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_EnglPrscIDCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "EnglPrscID").Should().Be(100);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - FworkCode Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_FworkCodeCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "FworkCode").Should().Be(4);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - FrameworkCommonComponent Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_FrameworkCommonComponentCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "FrameworkCommonComponent").Should().Be(3);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - FrameworkComponentType Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_FrameworkComponentTypeCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "FrameworkComponentType").Should().Be(2);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LearnActEndDate Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LearnActEndDateCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LearnActEndDate").Should().Be(new System.DateTime(2018, 10, 01));
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LearnActEndDate NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LearnActEndDateNullCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, null, new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LearnActEndDate").Should().Be(null);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LearnPlanEndDate Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LearnPlanEndDateCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LearnPlanEndDate").Should().Be(new System.DateTime(2018, 11, 01));
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LearnPlanEndDate NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LearnPlanEndDateNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), null, new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LearnPlanEndDate").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LearnStartDate Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LearnStartDateCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LearnStartDate").Should().Be(new System.DateTime(2018, 08, 01));
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LearnStartDate NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LearnStartDateNullCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), null, 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LearnStartDate").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LrnDelFAM_EEF Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LrnDelFAM_EEFCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LrnDelFAM_EEF").Should().Be(100);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LrnDelFAM_LDM1 Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LrnDelFAM_LDM1Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LrnDelFAM_LDM1").Should().Be(200);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LrnDelFAM_LDM2 Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LrnDelFAM_LDM2Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LrnDelFAM_LDM2").Should().Be(300);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LrnDelFAM_LDM3 Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LrnDelFAM_LDM3Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LrnDelFAM_LDM3").Should().Be(400);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LrnDelFAM_LDM4 Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LrnDelFAM_LDM4Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LrnDelFAM_LDM4").Should().Be(500);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LrnDelFAM_FFI Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LrnDelFAM_FFICorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LrnDelFAM_FFI").Should().Be(600);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - LrnDelFAM_RES Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_LrnDelFAM_RESCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "LrnDelFAM_RES").Should().Be(700);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - OrigLearnStartDate Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_OrigLearnStartDateCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "OrigLearnStartDate").Should().Be(new System.DateTime(2018, 08, 01));
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - OrigLearnStartDate NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_OrigLearnStartDateNullCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, null, 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "OrigLearnStartDate").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - OtherFundAdj Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_OtherFundAdjCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "OtherFundAdj").Should().Be(0);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - Outcome Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_OutcomeCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "Outcome").Should().Be(1);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - PriorLearnFundAdj Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_PriorLearnFundAdjCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "PriorLearnFundAdj").Should().Be(0);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - ProgType Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_ProgTypeCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "ProgType").Should().Be(2);
        }

        /// <summary>
        /// Return LearningDelivery Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDelivery - PwayCode Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDelivery_PwayCodeCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryAttributes = SetupLearningDeliveryAttributes(
                new System.DateTime(2018, 10, 01), 10, 1, 3, 1, 1, "EnglandStatus", 100, 4, 3, 2, new System.DateTime(2018, 10, 01), new System.DateTime(2018, 11, 01), new System.DateTime(2018, 08, 01), 100, 200, 300, 400, 500, 600, 700, new System.DateTime(2018, 08, 01), 0, 1, 0, 2, 2);

            // ASSERT
            AttributeValue(learningDeliveryAttributes, "PwayCode").Should().Be(2);
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), "ADL");

            // ASSERT
            learningDeliveryFAMAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), "ADL");

            // ASSERT
            learningDeliveryFAMAttributes.Count.Should().Be(4);
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - LearnDelFAMCode Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_LearnDelFAMCodeValueCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), "ADL");

            // ASSERT
            AttributeValue(learningDeliveryFAMAttributes, "LearnDelFAMCode").Should().Be("1");
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - LearnDelFAMDateFrom Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_LearnDelFAMDateFromValueCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), "ADL");

            // ASSERT
            AttributeValue(learningDeliveryFAMAttributes, "LearnDelFAMDateFrom").Should().Be(System.DateTime.Parse("2018-07-31"));
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - LearnDelFAMDateFrom NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_LearnDelFAMDateFromNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", null, System.DateTime.Parse("2018-06-30"), "ADL");

            // ASSERT
            AttributeValue(learningDeliveryFAMAttributes, "LearnDelFAMDateFrom").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - LearnDelFAMDateTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_LearnDelFAMDateToValueCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), "ADL");

            // ASSERT
            AttributeValue(learningDeliveryFAMAttributes, "LearnDelFAMDateTo").Should().Be(System.DateTime.Parse("2018-06-30"));
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - LearnDelFAMDateTo NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_LearnDelFAMDateToNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", System.DateTime.Parse("2018-07-31"), null, "ADL");

            // ASSERT
            AttributeValue(learningDeliveryFAMAttributes, "LearnDelFAMDateTo").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryFAM Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryFAM - LearnDelFAMType Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryFAM_LearnDelFAMTypeValueCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryFAMAttributes = SetupLearningDeliveryFAMAttributes("1", System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), "ADL");

            // ASSERT
            AttributeValue(learningDeliveryFAMAttributes, "LearnDelFAMType").Should().Be("ADL");
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLARSAnnualValue_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueAttributes = SetupLearningDeliveryLARSAnnualValueAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2018, 06, 30));

            // ASSERT
            learningDeliveryLARSAnnualValueAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLARSAnnualValue_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueAttributes = SetupLearningDeliveryLARSAnnualValueAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2018, 06, 30));

            // ASSERT
            learningDeliveryLARSAnnualValueAttributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - LearnDelAnnValBasicSkillsTypeCode Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnDelAnnValBasicSkillsTypeCode_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueAttributes = SetupLearningDeliveryLARSAnnualValueAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2019, 06, 30));

            // ASSERT
            AttributeValue(learningDeliveryLARSAnnualValueAttributes, "LearnDelAnnValBasicSkillsTypeCode").Should().Be(1);
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - LearnDelAnnValDateFrom Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnDelAnnValDateFrom_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueAttributes = SetupLearningDeliveryLARSAnnualValueAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2019, 06, 30));

            // ASSERT
            AttributeValue(learningDeliveryLARSAnnualValueAttributes, "LearnDelAnnValDateFrom").Should().BeEquivalentTo(new System.DateTime(2018, 07, 31));
        }

        /// <summary>
        /// Return LearningDeliveryLARSAnnualValue Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSAnnualValue - LearnDelAnnValDateTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnDelAnnValDateTo_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueAttributes = SetupLearningDeliveryLARSAnnualValueAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2019, 06, 30));

            // ASSERT
            AttributeValue(learningDeliveryLARSAnnualValueAttributes, "LearnDelAnnValDateTo").Should().BeEquivalentTo(new System.DateTime(2019, 06, 30));
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Attributes Exist"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLARSCategory_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueAttributes = SetupLearningDeliveryLARSAnnualValueAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2018, 06, 30));

            // ASSERT
            learningDeliveryLARSAnnualValueAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLARSCategory_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSAnnualValueAttributes = SetupLearningDeliveryLARSCategoryAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2018, 06, 30));

            // ASSERT
            learningDeliveryLARSAnnualValueAttributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - LearnDelCatRef Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnDelCatRef_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryAttributes = SetupLearningDeliveryLARSCategoryAttributes(1, new System.DateTime(2018, 07, 31), new System.DateTime(2018, 06, 30));

            // ASSERT
            AttributeValue(learningDeliveryLARSCategoryAttributes, "LearnDelCatRef").Should().Be(1);
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - LearnDelCatDateFrom Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnDelCatDateFrom_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryAttributes = SetupLearningDeliveryLARSCategoryAttributes(1, new System.DateTime(2018, 06, 30), new System.DateTime(2018, 07, 31));

            // ASSERT
            AttributeValue(learningDeliveryLARSCategoryAttributes, "LearnDelCatDateFrom").Should().BeEquivalentTo(new System.DateTime(2018, 06, 30));
        }

        /// <summary>
        /// Return LearningDeliveryLARSCategory Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLARSCategory - LearnDelCatDateTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearnDelCatDateTo_Correct()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLARSCategoryAttributes = SetupLearningDeliveryLARSCategoryAttributes(1, new System.DateTime(2018, 06, 30), new System.DateTime(2018, 07, 31));

            // ASSERT
            AttributeValue(learningDeliveryLARSCategoryAttributes, "LearnDelCatDateTo").Should().BeEquivalentTo(new System.DateTime(2018, 07, 31));
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - Attributes Exists"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            learningDeliveryLarsFundingAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            learningDeliveryLarsFundingAttributes.Count.Should().Be(6);
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundCategory Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundCategoryCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundCategory").Should().Be("Matrix");
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundEffectiveFrom Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundEffectiveFromCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundEffectiveFrom").Should().Be(System.DateTime.Parse("2017-08-31"));
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundEffectiveTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundEffectiveToCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundEffectiveTo").Should().Be(System.DateTime.Parse("2018-06-30"));
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundEffectiveTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundEffectiveToNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), null, 1.8m, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundEffectiveTo").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundUnweightedRate Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundUnweightedRateCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundUnweightedRate").Should().Be(1.8m);
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundUnweightedRate NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundUnweightedRateNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), null, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundUnweightedRate").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundWeightedRate Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundWeightedRateCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundWeightedRate").Should().Be(1.8m);
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundWeightedRate NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundWeightedRateNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, null, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundWeightedRate").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliveryLarsFunding Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliveryLarsFunding - LARSFundWeightingFactor Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliveryLarsFunding_LARSFundWeightingFactorCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliveryLarsFundingAttributes = SetupLearningDeliveryLarsFundingAttributes("Matrix", System.DateTime.Parse("2017-08-31"), System.DateTime.Parse("2018-06-30"), 1.8m, 1.8m, "A");

            // ASSERT
            AttributeValue(learningDeliveryLarsFundingAttributes, "LARSFundWeightingFactor").Should().Be("A");
        }

        /// <summary>
        /// Return LearningDeliverySfaAreaCost Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFA AreaCost - Attributes Exists"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliverySFAAC_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySfaAreaCostAttributes = SetupLearningDeliverySfaAreaCostAttributes(System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), 1.2m);

            // ASSERT
            learningDeliverySfaAreaCostAttributes.Should().NotBeEmpty();
        }

        /// <summary>
        /// Return LearningDeliverySfaAreaCost Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFA AreaCost - Count Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliverySFAAC_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySfaAreaCostAttributes = SetupLearningDeliverySfaAreaCostAttributes(System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), 1.2m);

            // ASSERT
            learningDeliverySfaAreaCostAttributes.Count.Should().Be(3);
        }

        /// <summary>
        /// Return LearningDeliverySfaAreaCost Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFA AreaCost - AreaCosEffectiveFrom Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliverySFAAC_AreaCosEffectiveFromCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySfaAreaCostAttributes = SetupLearningDeliverySfaAreaCostAttributes(System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), 1.2m);

            // ASSERT
            AttributeValue(learningDeliverySfaAreaCostAttributes, "AreaCosEffectiveFrom").Should().Be(System.DateTime.Parse("2018-07-31"));
        }

        /// <summary>
        /// Return LearningDeliverySfaAreaCost Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFA AreaCost - AreaCosEffectiveFrom NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliverySFAAC_AreaCosEffectiveFromNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySfaAreaCostAttributes = SetupLearningDeliverySfaAreaCostAttributes(null, System.DateTime.Parse("2018-06-30"), 1.2m);

            // ASSERT
            AttributeValue(learningDeliverySfaAreaCostAttributes, "AreaCosEffectiveFrom").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliverySfaAreaCost Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFA AreaCost - AreaCosEffectiveTo Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliverySFAAC_AreaCosEffectiveToCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySfaAreaCostAttributes = SetupLearningDeliverySfaAreaCostAttributes(System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), 1.2m);

            // ASSERT
            AttributeValue(learningDeliverySfaAreaCostAttributes, "AreaCosEffectiveTo").Should().Be(System.DateTime.Parse("2018-06-30"));
        }

        /// <summary>
        /// Return LearningDeliverySfaAreaCost Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFA AreaCost - AreaCosEffectiveTo NULL Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliverySFAAC_AreaCosEffectiveToNULLCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySfaAreaCostAttributes = SetupLearningDeliverySfaAreaCostAttributes(System.DateTime.Parse("2018-07-31"), null, 1.2m);

            // ASSERT
            AttributeValue(learningDeliverySfaAreaCostAttributes, "AreaCosEffectiveTo").Should().BeNull();
        }

        /// <summary>
        /// Return LearningDeliverySfaAreaCost Attributes from AttributeBuilder and check values
        /// /// </summary>
        [Fact(DisplayName = "LearningDeliverySFA AreaCost - AreaCosFactor Correct"), Trait("Funding Service Attribute Builders", "Unit")]
        public void AttributeBuilder_LearningDeliverySFAAC_AreaCosFactorCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var learningDeliverySfaAreaCostAttributes = SetupLearningDeliverySfaAreaCostAttributes(System.DateTime.Parse("2018-07-31"), System.DateTime.Parse("2018-06-30"), 1.2m);

            // ASSERT
            AttributeValue(learningDeliverySfaAreaCostAttributes, "AreaCosFactor").Should().Be(1.2m);
        }

        private static IDictionary<string, IAttributeData> SetupGlobalAttributes(int ukprn, string larsVersion, string orgVersion, string postcodeDisadvantageVersion)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildGlobalAttributes(ukprn, larsVersion, orgVersion, postcodeDisadvantageVersion);
        }

        private static IDictionary<string, IAttributeData> SetupOrgFundingAttributes(System.DateTime orgFundEffectiveFrom, System.DateTime? orgFundEffectiveTo, string orgFundFactor, string orgFundFactType, string orgFundFactValue)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildOrgFundingAttributes(orgFundEffectiveFrom, orgFundEffectiveTo, orgFundFactor, orgFundFactType, orgFundFactValue);
        }

        private static IDictionary<string, IAttributeData> SetupLearnerAttributes(string learnRefNumber, System.DateTime dateOfBirth)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearnerAttributes(learnRefNumber, dateOfBirth);
        }

        private static IDictionary<string, IAttributeData> SetupLearnerEmploymentStatusAttributes(long? empId, System.DateTime? dateEmpStatApp)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearnerEmploymentStatusAttributes(empId, dateEmpStatApp);
        }

        private static IDictionary<string, IAttributeData> SetupLargeEmployerReferenceDataAttributes(System.DateTime? largeEmpEffectiveFrom, System.DateTime? largeEmpEffectiveTo)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLLargeEmployerReferenceDataAttributes(largeEmpEffectiveFrom, largeEmpEffectiveTo);
        }

        private static IDictionary<string, IAttributeData> SetupSFAPostcodeDisadvantageAttributes(decimal? disUplift, System.DateTime? disUpEffectiveFrom, System.DateTime? disUpEffectiveTo)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildSFAPostcodeDisadvantageAttributes(disUplift, disUpEffectiveFrom, disUpEffectiveTo);
        }

        private static IDictionary<string, IAttributeData> SetupLearningDeliveryAttributes(
            System.DateTime? achDate,
            long? addHours,
            long? aimSeqNumber,
            long? aimType,
            long? compStatus,
            long? empOutcome,
            string englandFEHEStatus,
            long? englPrscID,
            long? fworkCode,
            long? frameworkCommonComponent,
            long? frameworkComponentType,
            System.DateTime? learnActEndDate,
            System.DateTime? learnPlanEndDate,
            System.DateTime? learnStartDate,
            long? lrnDelFAM_EEF,
            long? lrnDelFAM_LDM1,
            long? lrnDelFAM_LDM2,
            long? lrnDelFAM_LDM3,
            long? lrnDelFAM_LDM4,
            long? lrnDelFAM_FFI,
            long? lrnDelFAM_RES,
            System.DateTime? origLearnStartDate,
            long? otherFundAdj,
            long? outcome,
            long? priorLearnFundAdj,
            long? progType,
            long? pwayCode)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearningDeliveryAttributes(
               achDate,
               addHours,
               aimSeqNumber,
               aimType,
               compStatus,
               empOutcome,
               englandFEHEStatus,
               englPrscID,
               fworkCode,
               frameworkCommonComponent,
               frameworkComponentType,
               learnActEndDate,
               learnPlanEndDate,
               learnStartDate,
               lrnDelFAM_EEF,
               lrnDelFAM_LDM1,
               lrnDelFAM_LDM2,
               lrnDelFAM_LDM3,
               lrnDelFAM_LDM4,
               lrnDelFAM_FFI,
               lrnDelFAM_RES,
               origLearnStartDate,
               otherFundAdj,
               outcome,
               priorLearnFundAdj,
               progType,
               pwayCode);
        }

        private static IDictionary<string, IAttributeData> SetupLearningDeliveryFAMAttributes(
            string learnDelFAMCode, System.DateTime? learnDelFAMDateFrom, System.DateTime? learnDelFAMDateTo, string learnDelFAMType)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearningDeliveryFAMAttributes(learnDelFAMCode, learnDelFAMDateFrom, learnDelFAMDateTo, learnDelFAMType);
        }

        private static IDictionary<string, IAttributeData> SetupLearningDeliveryLARSAnnualValueAttributes(long? learnDelAnnValBasicSkillsTypeCode, System.DateTime learnDelAnnValDateFrom, System.DateTime? learnDelAnnValDateTo)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearningDeliveryLARSAnnualValueAttributes(learnDelAnnValBasicSkillsTypeCode, learnDelAnnValDateFrom, learnDelAnnValDateTo);
        }

        private static IDictionary<string, IAttributeData> SetupLearningDeliveryLARSCategoryAttributes(long? learnDelCatRef, System.DateTime learnDelCatDateFrom, System.DateTime? learnDelCatDateTo)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearningDeliveryLARSCategoryAttributes(learnDelCatRef, learnDelCatDateFrom, learnDelCatDateTo);
        }

        private static IDictionary<string, IAttributeData> SetupLearningDeliveryLarsFundingAttributes(
            string larsFundCategory, System.DateTime larsFundEffectiveFrom, System.DateTime? larsFundEffectiveTo, decimal? larsFundUnWeightedRate, decimal? larsFundWeightedRate, string larsFundWeightingFactor)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearningDeliveryLarsFundingAttributes(larsFundCategory, larsFundEffectiveFrom, larsFundEffectiveTo, larsFundUnWeightedRate, larsFundWeightedRate, larsFundWeightingFactor);
        }

        private static IDictionary<string, IAttributeData> SetupLearningDeliverySfaAreaCostAttributes(
            System.DateTime? areaCosEffectiveFrom, System.DateTime? areaCosEffectiveTo, decimal areaCosFactor)
        {
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            return attributeBuilder.BuildLearningDeliverySfaAreaCostAttributes(areaCosEffectiveFrom, areaCosEffectiveTo, areaCosFactor);
        }

        private static object AttributeValue(IDictionary<string, IAttributeData> dictionary, string attributeName)
        {
            return dictionary.Where(k => k.Key == attributeName).Select(v => v.Value.Value).Single();
        }
    }
}
