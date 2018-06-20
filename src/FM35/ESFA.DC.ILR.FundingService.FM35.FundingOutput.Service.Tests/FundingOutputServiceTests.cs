using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ESFA.DC.DateTime.Provider;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service.Tests
{
    public class FundingOutputServiceTests
    {
        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - Exists"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ASSERT
            fundingOutputService.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - Global Exists"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_GlobalExists()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var fundingOutput = fundingOutputService.ProcessFundingOutputs(TestEntities());

            // ASSERT
            fundingOutput.Global.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - Global Correct"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_GlobalCorrect()
        {
            // ARRANGE
            IGlobalAttribute expectedGlobal = new GlobalAttribute
            {
                UKPRN = 12345678,
                LARSVersion = "Version_005",
                PostcodeDisadvantageVersion = "Version_002",
                OrgVersion = "Version_003",
                CurFundYr = "1819",
                RulebaseVersion = "1718.5.10",
            };

            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var fundingOutput = fundingOutputService.ProcessFundingOutputs(TestEntities());

            // ASSERT
            fundingOutput.Global.Should().BeEquivalentTo(expectedGlobal);
        }

        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - Learners Exist"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_LearnersExist()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var fundingOutput = fundingOutputService.ProcessFundingOutputs(TestEntities());

            // ASSERT
            fundingOutput.Learners.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - Learners Correct"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_LearnersCorrect()
        {
            // ARRANGE
            ILearnerAttribute[] expectedLearners = new LearnerAttribute[]
            {
                 new LearnerAttribute
                 {
                     LearnRefNumber = "TestLearner1",
                     LearningDeliveryAttributes = TestLearningDeliveryAttributeArray(1),
                 },
                 new LearnerAttribute
                 {
                     LearnRefNumber = "TestLearner2",
                     LearningDeliveryAttributes = TestLearningDeliveryAttributeArray(1),
                 }
            };

            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var fundingOutput = fundingOutputService.ProcessFundingOutputs(TestEntities());

            // ASSERT
            fundingOutput.Learners.Should().BeEquivalentTo(expectedLearners);
        }

        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - LearnerAttributes Exist"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_LearnerAttributesExist()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var fundingOutput = fundingOutputService.ProcessFundingOutputs(TestEntities());

            // ASSERT
            fundingOutput.Learners.Select(l => l.LearnRefNumber).Should().NotBeNull();
            fundingOutput.Learners.Select(l => l.LearningDeliveryAttributes).Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - LearnerAttributes LearnRefNumber"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_LearnerAttributes_LearnRefNumber()
        {
            // ARRANGE
            var expectedLearnRefNumbers = new List<string> { "TestLearner1", "TestLearner2" };

            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var fundingOutput = fundingOutputService.ProcessFundingOutputs(TestEntities());

            // ASSERT
            var learnRefNmbers = fundingOutput.Learners.Select(l => l.LearnRefNumber).ToList();

            expectedLearnRefNumbers.Should().BeEquivalentTo(learnRefNmbers);
        }

        /// <summary>
        /// Return FundingOutputs from the FundingOutput
        /// </summary>
        [Fact(DisplayName = "ProcessFundingOutputs - FundingOutput - LearnerAttributes LearnerDeliveryAttributes"), Trait("FundingOutput Service", "Unit")]
        public void ProcessFundingOutputs_FundingOutput_LearnerAttributes_LearnerDeliveryAttributes()
        {
            // ARRANGE
            IList<ILearningDeliveryAttribute[]> expectedLearningDeliveryAttributes = new List<ILearningDeliveryAttribute[]>
            {
                TestLearningDeliveryAttributeArray(1),
                TestLearningDeliveryAttributeArray(1),
            };

            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var fundingOutput = fundingOutputService.ProcessFundingOutputs(TestEntities());

            // ASSERT
            var learningDelAttributes = fundingOutput.Learners.Select(l => l.LearningDeliveryAttributes).ToList();

            expectedLearningDeliveryAttributes.Should().BeEquivalentTo(learningDelAttributes);
        }

        /// <summary>
        /// Return FundingOutputs GlobalAttribute
        /// </summary>
        [Fact(DisplayName = "FundingOutput - GlobalOutput Exists"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_GlobalOutput_Exists()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var globalOutput = fundingOutputService.GlobalOutput(GlobalAttributes());

            // ASSERT
            globalOutput.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs GlobalAttribute
        /// </summary>
        [Fact(DisplayName = "FundingOutput - GlobalOutput Correct"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_GlobalOutput_Correct()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var globalOutput = fundingOutputService.GlobalOutput(GlobalAttributes());

            // ASSERT
            var expectedGlobalOutput = new GlobalAttribute
            {
                UKPRN = 12345678,
                LARSVersion = "Version_005",
                PostcodeDisadvantageVersion = "Version_002",
                OrgVersion = "Version_003",
                CurFundYr = "1819",
                RulebaseVersion = "1718.5.10",
            };

            expectedGlobalOutput.Should().BeEquivalentTo(globalOutput);
        }

        /// <summary>
        /// Return FundingOutputs LearnerOutput
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearnerOutput Exists"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearnerOutput_Exists()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learnerOutput = fundingOutputService.LearnerOutput(TestLearnerEntity(null, "TestLearner", true));

            // ASSERT
            learnerOutput.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs LearnerOutput
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearnerOutput Correct"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearnerOutput_Correct()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learnerOutput = fundingOutputService.LearnerOutput(TestLearnerEntity(null, "TestLearner", true));

            // ASSERT
            var expectedLearnerOutput = new LearnerAttribute[]
            {
                new LearnerAttribute
                {
                    LearnRefNumber = "TestLearner",
                    LearningDeliveryAttributes = new LearningDeliveryAttribute[]
                    {
                        TestLearningDeliveryAttributeValues(1),
                    },
                },
            };
            expectedLearnerOutput.Should().BeEquivalentTo(learnerOutput);
        }

        /// <summary>
        /// Return FundingOutputs LearningDeliveryAttributes
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearningDeliveryAttributes Exists"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearningDeliveryAttributes_Exists()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learningDeliveryAttributes = fundingOutputService.LearningDeliveryAttributes(TestLearnerEntity(null, "TestLearner", true).SingleOrDefault());

            // ASSERT
            learningDeliveryAttributes.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs LearningDeliveryAttributes
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearningDeliveryAttributes Correct"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearningDeliveryAttributes_Correct()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learningDeliveryAttributes = fundingOutputService.LearningDeliveryAttributes(TestLearnerEntity(null, "TestLearner", true).SingleOrDefault());

            // ASSERT
            var expectedLearningDeliveryAttributes = new LearningDeliveryAttribute[]
            {
                new LearningDeliveryAttribute
                {
                    AimSeqNumber = 1,
                    LearningDeliveryAttributeDatas = TestLearningDeliveryAttributeValues(1).LearningDeliveryAttributeDatas,
                    LearningDeliveryPeriodisedAttributes = TestLearningDeliveryAttributeValues(1).LearningDeliveryPeriodisedAttributes,
                },
            };

            expectedLearningDeliveryAttributes.Should().BeEquivalentTo(learningDeliveryAttributes);
        }

        /// <summary>
        /// Return FundingOutputs LearningDeliveryAttributeDatas
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearningDeliveryAttributeDatas Exists"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearningDeliveryAttributeDatas_Exists()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learningDeliveryAttributeDatas =
                fundingOutputService.LearningDeliveryAttributeData(TestLearnerEntity(null, "TestLearner", true).SingleOrDefault().Children.SingleOrDefault());

            // ASSERT
            learningDeliveryAttributeDatas.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs LearningDeliveryAttributeDatas
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearningDeliveryAttributeDatas Correct"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearningDeliveryAttributeDatas_Correct()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learningDeliveryAttributeDatas =
                fundingOutputService.LearningDeliveryAttributeData(TestLearnerEntity(null, "TestLearner", true).SingleOrDefault().Children.SingleOrDefault());

            // ASSERT
            var expectedlearningDeliveryAttributeDatas = TestLearningDeliveryAttributeData();

            expectedlearningDeliveryAttributeDatas.Should().BeEquivalentTo(learningDeliveryAttributeDatas);
        }

        /// <summary>
        /// Return FundingOutputs LearningDeliveryPeriodisedAttributeData
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearningDeliveryPeriodisedAttributeData Exists"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearningDeliveryPeriodisedAttributeData_Exists()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learningDeliveryPeriodisedAttributeData =
                fundingOutputService.LearningDeliveryPeriodisedAttributeData(TestLearnerEntity(null, "TestLearner", true).SingleOrDefault().Children.SingleOrDefault());

            // ASSERT
            learningDeliveryPeriodisedAttributeData.Should().NotBeNull();
        }

        /// <summary>
        /// Return FundingOutputs LearningDeliveryPeriodisedAttributeData
        /// </summary>
        [Fact(DisplayName = "FundingOutput - LearningDeliveryPeriodisedAttributeData Correct"), Trait("FundingOutput Service", "Unit")]
        public void FundingOutput_LearningDeliveryPeriodisedAttributeData_Correct()
        {
            // ARRANGE
            var fundingOutputService = new FundingOutputService(new DateTimeProvider());

            // ACT
            var learningDeliveryPeriodisedAttributeData =
                fundingOutputService.LearningDeliveryPeriodisedAttributeData(TestLearnerEntity(null, "TestLearner", true).SingleOrDefault().Children.SingleOrDefault());

            // ASSERT
            var expectedLearningDeliveryPeriodisedAttributeData = TestLearningDeliveryPeriodisedAttributesDataArray();

            expectedLearningDeliveryPeriodisedAttributeData.Should().BeEquivalentTo(learningDeliveryPeriodisedAttributeData);
        }

        private static readonly IFormatProvider culture = new CultureInfo("en-GB", true);

        private static readonly Mock<IFundingService> FundingServiceContextMock = new Mock<IFundingService>();

        private IEnumerable<IDataEntity> TestEntities()
        {
            var entities = new List<DataEntity>();

            var entity1 =
                new DataEntity("global")
                {
                    EntityName = "global",
                    Attributes = GlobalAttributes(),
                    Parent = null,
                };

            entity1.AddChildren(TestLearnerEntity(entity1, "TestLearner1", false));

            entities.Add(entity1);

            var entity2 =
            new DataEntity("global")
            {
                EntityName = "global",
                Attributes = GlobalAttributes(),
                Parent = null,
            };

            entity1.AddChildren(TestLearnerEntity(entity2, "TestLearner2", true));

            entities.Add(entity2);

            return entities;
        }

        private IEnumerable<IDataEntity> TestLearnerEntity(DataEntity parent, string learnRefNumber, bool includeFM35ChangePoint)
        {
            var entities = new List<DataEntity>();
            if (includeFM35ChangePoint)
            {
                var entity1 = new DataEntity("Learner")
                {
                    EntityName = "Learner",
                    Attributes = new Dictionary<string, IAttributeData>
                    {
                        { "LearnRefNumber", new AttributeData("LearnRefNumber", learnRefNumber) },
                        { "DateOfBirth", new AttributeData("DateOfBirth", new System.DateTime(2000, 01, 01)) },
                    },
                    Parent = parent
                };

                entity1.AddChildren(TestLearningDeliveryEntity(entity1));

                entities.Add(entity1);

                return entities;
            }

            var entity2 = new DataEntity("Learner")
            {
                EntityName = "Learner",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "LearnRefNumber", new AttributeData("LearnRefNumber", learnRefNumber) },
                    { "DateOfBirth", new AttributeData("DateOfBirth", new System.DateTime(2000, 01, 01)) },
                },
                Parent = parent
            };

            entity2.AddChildren(TestLearningDeliveryEntity(entity2));

            entities.Add(entity2);

            return entities;
        }

        private IEnumerable<IDataEntity> TestLearningDeliveryEntity(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity("LearningDelivery")
            {
                EntityName = "LearningDelivery",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "AimSeqNumber", Attribute("AimSeqNumber", false, "1.0") },
                    { "AchApplicDate", Attribute("AchApplicDate", false, new System.DateTime(2018, 09, 01)) },
                    { "Achieved", Attribute("Achieved", false, "1.0") },
                    { "AchieveElement", Attribute("AchieveElement", false, "1.0") },
                    { "AchievePayElig", Attribute("AchievePayElig", false, "1.0") },
                    { "AchievePayPctPreTrans", Attribute("AchievePayPctPreTrans", false, "1.0") },
                    { "AchPayTransHeldBack", Attribute("AchPayTransHeldBack", false, "1.0") },
                    { "ActualDaysIL", Attribute("ActualDaysIL", false, "1.0") },
                    { "ActualNumInstalm", Attribute("ActualNumInstalm", false, "1.0") },
                    { "ActualNumInstalmPreTrans", Attribute("ActualNumInstalmPreTrans", false, "1.0") },
                    { "ActualNumInstalmTrans", Attribute("ActualNumInstalmTrans", false, "1.0") },
                    { "AdjLearnStartDate", Attribute("AdjLearnStartDate", false, new System.DateTime(2018, 09, 01)) },
                    { "AdltLearnResp", Attribute("AdltLearnResp", false, "1.0") },
                    { "AgeAimStart", Attribute("AgeAimStart", false, "1.0") },
                    { "AimValue", Attribute("AimValue", false, "1.0") },
                    { "AppAdjLearnStartDate", Attribute("AppAdjLearnStartDate", false, new System.DateTime(2018, 09, 01)) },
                    { "AppAgeFact", Attribute("AppAgeFact", false, "1.0") },
                    { "AppATAGTA", Attribute("AppATAGTA", false, "1.0") },
                    { "AppCompetency", Attribute("AppCompetency", false, "1.0") },
                    { "AppFuncSkill", Attribute("AppFuncSkill", false, "1.0") },
                    { "AppFuncSkill1618AdjFact", Attribute("AppFuncSkill1618AdjFact", false, "1.0") },
                    { "AppKnowl", Attribute("AppKnowl", false, "1.0") },
                    { "AppLearnStartDate", Attribute("AppLearnStartDate", false, new System.DateTime(2018, 09, 01)) },
                    { "ApplicEmpFactDate", Attribute("ApplicEmpFactDate", false, new System.DateTime(2018, 09, 01)) },
                    { "ApplicFactDate", Attribute("ApplicFactDate", false, new System.DateTime(2018, 09, 01)) },
                    { "ApplicFundRateDate", Attribute("ApplicFundRateDate", false, new System.DateTime(2018, 09, 01)) },
                    { "ApplicProgWeightFact", Attribute("ApplicProgWeightFact", false, "1.0") },
                    { "ApplicUnweightFundRate", Attribute("ApplicUnweightFundRate", false, "1.0") },
                    { "ApplicWeightFundRate", Attribute("ApplicWeightFundRate", false, "1.0") },
                    { "AppNonFund", Attribute("AppNonFund", false, "1.0") },
                    { "AreaCostFactAdj", Attribute("AreaCostFactAdj", false, "1.0") },
                    { "BalInstalmPreTrans", Attribute("BalInstalmPreTrans", false, "1.0") },
                    { "BaseValueUnweight", Attribute("BaseValueUnweight", false, "1.0") },
                    { "CapFactor", Attribute("CapFactor", false, "1.0") },
                    { "DisUpFactAdj", Attribute("DisUpFactAdj", false, "1.0") },
                    { "EmpOutcomePayElig", Attribute("EmpOutcomePayElig", false, "1.0") },
                    { "EmpOutcomePctHeldBackTrans", Attribute("EmpOutcomePctHeldBackTrans", false, "1.0") },
                    { "EmpOutcomePctPreTrans", Attribute("EmpOutcomePctPreTrans", false, "1.0") },
                    { "EmpRespOth", Attribute("EmpRespOth", false, "1.0") },
                    { "ESOL", Attribute("ESOL", false, "1.0") },
                    { "FullyFund", Attribute("FullyFund", false, "1.0") },
                    { "FundLine", Attribute("FundLine", false, "1.0") },
                    { "FundStart", Attribute("FundStart", false, "1.0") },
                    { "LargeEmployerFM35Fctr", Attribute("LargeEmployerFM35Fctr", false, "1.0") },
                    { "LargeEmployerID", Attribute("LargeEmployerID", false, "1.0") },
                    { "LargeEmployerStatusDate", Attribute("LargeEmployerStatusDate", false, new System.DateTime(2018, 09, 01)) },
                    { "LTRCUpliftFctr", Attribute("LTRCUpliftFctr", false, "1.0") },
                    { "NonGovCont", Attribute("NonGovCont", false, "1.0") },
                    { "OLASSCustody", Attribute("OLASSCustody", false, "1.0") },
                    { "OnProgPayPctPreTrans", Attribute("OnProgPayPctPreTrans", false, "1.0") },
                    { "OutstndNumOnProgInstalm", Attribute("OutstndNumOnProgInstalm", false, "1.0") },
                    { "OutstndNumOnProgInstalmTrans", Attribute("OutstndNumOnProgInstalmTrans", false, "1.0") },
                    { "PlannedNumOnProgInstalm", Attribute("PlannedNumOnProgInstalm", false, "1.0") },
                    { "PlannedNumOnProgInstalmTrans", Attribute("PlannedNumOnProgInstalmTrans", false, "1.0") },
                    { "PlannedTotalDaysIL", Attribute("PlannedTotalDaysIL", false, "1.0") },
                    { "PlannedTotalDaysILPreTrans", Attribute("PlannedTotalDaysILPreTrans", false, "1.0") },
                    { "PropFundRemain", Attribute("PropFundRemain", false, "1.0") },
                    { "PropFundRemainAch", Attribute("PropFundRemainAch", false, "1.0") },
                    { "PrscHEAim", Attribute("PrscHEAim", false, "1.0") },
                    { "Residential", Attribute("Residential", false, "1.0") },
                    { "Restart", Attribute("Restart", false, "1.0") },
                    { "SpecResUplift", Attribute("SpecResUplift", false, "1.0") },
                    { "StartPropTrans", Attribute("StartPropTrans", false, "1.0") },
                    { "ThresholdDays", Attribute("ThresholdDays", false, "1.0") },
                    { "Traineeship", Attribute("Traineeship", false, "1.0") },
                    { "Trans", Attribute("Trans", false, "1.0") },
                    { "TrnAdjLearnStartDate", Attribute("TrnAdjLearnStartDate", false, new System.DateTime(2018, 09, 01)) },
                    { "TrnWorkPlaceAim", Attribute("TrnWorkPlaceAim", false, "1.0") },
                    { "TrnWorkPrepAim", Attribute("TrnWorkPrepAim", false, "1.0") },
                    { "UnWeightedRateFromESOL", Attribute("UnWeightedRateFromESOL", false, "1.0") },
                    { "UnweightedRateFromLARS", Attribute("UnweightedRateFromLARS", false, "1.0") },
                    { "WeightedRateFromESOL", Attribute("WeightedRateFromESOL", false, "1.0") },
                    { "WeightedRateFromLARS", Attribute("WeightedRateFromLARS", false, "1.0") },
                    { "AchievePayment", Attribute("AchievePayment", false, "1.0") },
                    { "AchievePayPct", Attribute("AchievePayPct", false, "1.0") },
                    { "AchievePayPctTrans", Attribute("AchievePayPctTrans", false, "1.0") },
                    { "BalancePayment", Attribute("BalancePayment", false, "1.0") },
                    { "BalancePaymentUncapped", Attribute("BalancePaymentUncapped", false, "1.0") },
                    { "BalancePct", Attribute("BalancePct", false, "1.0") },
                    { "BalancePctTrans", Attribute("BalancePctTrans", false, "1.0") },
                    { "EmpOutcomePay", Attribute("EmpOutcomePay", false, "1.0") },
                    { "EmpOutcomePct", Attribute("EmpOutcomePct", false, "1.0") },
                    { "EmpOutcomePctTrans", Attribute("EmpOutcomePctTrans", false, "1.0") },
                    { "InstPerPeriod", Attribute("InstPerPeriod", false, "1.0") },
                    { "LearnSuppFund", Attribute("LearnSuppFund", false, "1.0") },
                    { "LearnSuppFundCash", Attribute("LearnSuppFundCash", false, "1.0") },
                    { "OnProgPayment", Attribute("OnProgPayment", false, "1.0") },
                    { "OnProgPaymentUncapped", Attribute("OnProgPaymentUncapped", false, "1.0") },
                    { "OnProgPayPct", Attribute("OnProgPayPct", false, "1.0") },
                    { "OnProgPayPctTrans", Attribute("OnProgPayPctTrans", false, "1.0") },
                    { "TransInstPerPeriod", Attribute("TransInstPerPeriod", false, "1.0") },
                },
                Parent = parent,
            };

            entities.Add(entity);

            return entities;
        }

        private new Dictionary<string, IAttributeData> GlobalAttributes()
        {
            return new Dictionary<string, IAttributeData>
            {
                { "UKPRN", new AttributeData("UKPRN", "12345678") },
                { "OrgVersion", new AttributeData("OrgVersion", "Version_003") },
                { "LARSVersion", new AttributeData("LARSVersion", "Version_005") },
                { "CurFundYr", new AttributeData("CurFundYr", "1819") },
                { "PostcodeDisadvantageVersion", new AttributeData("PostcodeDisadvantageVersion", "Version_002") },
                { "RulebaseVersion", new AttributeData("RulebaseVersion", "1718.5.10") },
            };
        }

        private IAttributeData Attribute(string attributeName, bool hasChangePoints, object attributeValue)
        {
            if (hasChangePoints)
            {
                var attribute = new AttributeData(attributeName, null);
                attribute.AddChangepoints(ChangePoints(decimal.Parse(attributeValue.ToString())));

                return attribute;
            }

            return new AttributeData(attributeName, attributeValue);
        }

        private IEnumerable<ITemporalValueItem> ChangePoints(decimal value)
        {
            var changePoints = new List<TemporalValueItem>();

            IEnumerable<TemporalValueItem> cps = new List<TemporalValueItem>
            {
                 new TemporalValueItem(new System.DateTime(2018, 08, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2018, 09, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2018, 10, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2018, 11, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2018, 12, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2019, 01, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2019, 02, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2019, 03, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2019, 04, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2019, 05, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2019, 06, 01), value, null),
                 new TemporalValueItem(new System.DateTime(2019, 07, 01), value, null),
            };

            changePoints.AddRange(cps);

            return changePoints;
        }

        private Message ILRFile(string filePath)
        {
            Message message;
            Stream stream = new FileStream(filePath, FileMode.Open);

            using (var reader = XmlReader.Create(stream))
            {
                var serializer = new XmlSerializer(typeof(Message));
                message = serializer.Deserialize(reader) as Message;
            }

            stream.Close();

            return message;
        }

        private int DecimalStrToInt(string value)
        {
            var valueInt = value.Substring(0, value.IndexOf('.', 0));
            return int.Parse(valueInt);
        }

        private readonly IList<ILearner> testLearners = new[]
        {
            new MessageLearner
            {
                LearnRefNumber = "Learner1",
                LearningDelivery = new[]
                {
                    new MessageLearnerLearningDelivery
                    {
                        LearnAimRef = "123456",
                        AimSeqNumber = 1,
                        CompStatus = 1,
                        DelLocPostCode = "CV1 2WT",
                        LearnActEndDateSpecified = true,
                        LearnActEndDate = System.DateTime.Parse("2018-06-30", culture),
                        LearnStartDate = System.DateTime.Parse("2017-08-30", culture),
                        LearnPlanEndDate = System.DateTime.Parse("2018-07-30", culture),
                        OrigLearnStartDateSpecified = true,
                        OrigLearnStartDate = System.DateTime.Parse("2017-08-30", culture),
                        OtherFundAdjSpecified = false,
                        OutcomeSpecified = false,
                        PriorLearnFundAdjSpecified = false,
                        LearningDeliveryFAM = new[]
                        {
                            new MessageLearnerLearningDeliveryLearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "ADL",
                                LearnDelFAMDateFromSpecified = true,
                                LearnDelFAMDateFrom = System.DateTime.Parse("2017-08-30", culture),
                                LearnDelFAMDateToSpecified = true,
                                LearnDelFAMDateTo = System.DateTime.Parse("2017-10-31", culture)
                            },
                            new MessageLearnerLearningDeliveryLearningDeliveryFAM
                            {
                                LearnDelFAMCode = "100",
                                LearnDelFAMType = "SOF",
                                LearnDelFAMDateFromSpecified = true,
                                LearnDelFAMDateFrom = System.DateTime.Parse("2017-10-31", culture),
                                LearnDelFAMDateToSpecified = true,
                                LearnDelFAMDateTo = System.DateTime.Parse("2017-11-30", culture)
                            },
                            new MessageLearnerLearningDeliveryLearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "RES",
                                LearnDelFAMDateFromSpecified = true,
                                LearnDelFAMDateFrom = System.DateTime.Parse("2017-12-01", culture),
                                LearnDelFAMDateToSpecified = false
                            }
                        }
                    }
                }
            },
            new MessageLearner
            {
                LearnRefNumber = "Learner2",
                LearningDelivery = new[]
                {
                    new MessageLearnerLearningDelivery
                    {
                        LearnAimRef = "123456",
                        AimSeqNumber = 1,
                        CompStatus = 1,
                        DelLocPostCode = "CV1 2WT",
                        LearnActEndDateSpecified = true,
                        LearnActEndDate = System.DateTime.Parse("2018-06-30", culture),
                        LearnStartDate = System.DateTime.Parse("2017-08-30", culture),
                        LearnPlanEndDate = System.DateTime.Parse("2018-07-30", culture),
                        OrigLearnStartDateSpecified = true,
                        OrigLearnStartDate = System.DateTime.Parse("2017-08-30", culture),
                        OtherFundAdjSpecified = false,
                        OutcomeSpecified = false,
                        PriorLearnFundAdjSpecified = false,
                        LearningDeliveryFAM = new[]
                        {
                            new MessageLearnerLearningDeliveryLearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "ADL",
                                LearnDelFAMDateFromSpecified = true,
                                LearnDelFAMDateFrom = System.DateTime.Parse("2017-08-30", culture),
                                LearnDelFAMDateToSpecified = true,
                                LearnDelFAMDateTo = System.DateTime.Parse("2017-10-31", culture)
                            },
                            new MessageLearnerLearningDeliveryLearningDeliveryFAM
                            {
                                LearnDelFAMCode = "100",
                                LearnDelFAMType = "SOF",
                                LearnDelFAMDateFromSpecified = true,
                                LearnDelFAMDateFrom = System.DateTime.Parse("2017-10-31", culture),
                                LearnDelFAMDateToSpecified = true,
                                LearnDelFAMDateTo = System.DateTime.Parse("2017-11-30", culture)
                            },
                            new MessageLearnerLearningDeliveryLearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "RES",
                                LearnDelFAMDateFromSpecified = true,
                                LearnDelFAMDateFrom = System.DateTime.Parse("2017-12-01", culture),
                                LearnDelFAMDateToSpecified = false
                            }
                        }
                    }
                }
            }
        };

        private ILearningDeliveryAttribute[] TestLearningDeliveryAttributeArray(int aimSeq)
        {
            return new LearningDeliveryAttribute[]
            {
                TestLearningDeliveryAttributeValues(1)
            };
        }

        private LearningDeliveryAttribute TestLearningDeliveryAttributeValues(int aimSeq)
        {
            return new LearningDeliveryAttribute
            {
                AimSeqNumber = aimSeq,
                LearningDeliveryAttributeDatas = TestLearningDeliveryAttributeData(),
                LearningDeliveryPeriodisedAttributes = TestLearningDeliveryPeriodisedAttributesDataArray(),
            };
        }

        private ILearningDeliveryAttributeData TestLearningDeliveryAttributeData()
        {
            return new LearningDeliveryAttributeData
            {
                AchApplicDate = new System.DateTime(2018, 09, 01),
                Achieved = false,
                AchieveElement = 1,
                AchievePayElig = false,
                AchievePayPctPreTrans = 1,
                AchPayTransHeldBack = 1,
                ActualDaysIL = 1,
                ActualNumInstalm = 1,
                ActualNumInstalmPreTrans = 1,
                ActualNumInstalmTrans = 1,
                AdjLearnStartDate = new System.DateTime(2018, 09, 01),
                AdltLearnResp = false,
                AgeAimStart = 1,
                AimValue = 1,
                AppAdjLearnStartDate = new System.DateTime(2018, 09, 01),
                AppAgeFact = 1.0m,
                AppATAGTA = false,
                AppCompetency = false,
                AppFuncSkill = false,
                AppFuncSkill1618AdjFact = 1,
                AppKnowl = false,
                AppLearnStartDate = new System.DateTime(2018, 09, 01),
                ApplicEmpFactDate = new System.DateTime(2018, 09, 01),
                ApplicFactDate = new System.DateTime(2018, 09, 01),
                ApplicFundRateDate = new System.DateTime(2018, 09, 01),
                ApplicProgWeightFact = "1.0",
                ApplicUnweightFundRate = 1,
                ApplicWeightFundRate = 1,
                AppNonFund = false,
                AreaCostFactAdj = 1,
                BalInstalmPreTrans = 1,
                BaseValueUnweight = 1,
                CapFactor = 1,
                DisUpFactAdj = 1,
                EmpOutcomePayElig = false,
                EmpOutcomePctHeldBackTrans = 1,
                EmpOutcomePctPreTrans = 1,
                EmpRespOth = false,
                ESOL = false,
                FullyFund = false,
                FundLine = "1.0",
                FundStart = false,
                LargeEmployerFM35Fctr = 1,
                LargeEmployerID = 1,
                LargeEmployerStatusDate = new System.DateTime(2018, 09, 01),
                LTRCUpliftFctr = 1,
                NonGovCont = 1,
                OLASSCustody = false,
                OnProgPayPctPreTrans = 1,
                OutstndNumOnProgInstalm = 1,
                OutstndNumOnProgInstalmTrans = 1,
                PlannedNumOnProgInstalm = 1,
                PlannedNumOnProgInstalmTrans = 1,
                PlannedTotalDaysIL = 1,
                PlannedTotalDaysILPreTrans = 1,
                PropFundRemain = 1,
                PropFundRemainAch = 1,
                PrscHEAim = false,
                Residential = false,
                Restart = false,
                SpecResUplift = 1,
                StartPropTrans = 1,
                ThresholdDays = 1,
                Traineeship = false,
                Trans = false,
                TrnAdjLearnStartDate = new System.DateTime(2018, 09, 01),
                TrnWorkPlaceAim = false,
                TrnWorkPrepAim = false,
                UnWeightedRateFromESOL = 1,
                UnweightedRateFromLARS = 1,
                WeightedRateFromESOL = 1,
                WeightedRateFromLARS = 1,
            };
        }

        private ILearningDeliveryPeriodisedAttribute[] TestLearningDeliveryPeriodisedAttributesDataArray()
        {
            return new LearningDeliveryPeriodisedAttribute[]
            {
                TestLearningDeliveryPeriodisedAttributesData("AchievePayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("AchievePayPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("AchievePayPctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePaymentUncapped", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("EmpOutcomePay", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("EmpOutcomePct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("EmpOutcomePctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("InstPerPeriod", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFund", 0.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFundCash", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPaymentUncapped", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPayPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPayPctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("TransInstPerPeriod", 1.0m),
            };
        }

        private LearningDeliveryPeriodisedAttribute TestLearningDeliveryPeriodisedAttributesData(string attribute, decimal value)
        {
            return new LearningDeliveryPeriodisedAttribute
            {
                AttributeName = attribute,
                Period1 = value,
                Period2 = value,
                Period3 = value,
                Period4 = value,
                Period5 = value,
                Period6 = value,
                Period7 = value,
                Period8 = value,
                Period9 = value,
                Period10 = value,
                Period11 = value,
                Period12 = value,
            };
        }
    }
}
