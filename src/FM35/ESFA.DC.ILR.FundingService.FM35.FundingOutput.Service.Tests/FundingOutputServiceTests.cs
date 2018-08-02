using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.FM35.Service;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service.Tests
{
    public class FundingOutputServiceTests
    {
        [Fact]
        public void GlobalFromDataEntity()
        {
            var dataEntity = new DataEntity(string.Empty);

            var ukprn = 1;
            var curFundYr = "CurFundYr";
            var larsVersion = "LARSVersion";
            var orgVersion = "OrgVersion";
            var postcodeDisadvantageVersion = "PostcodeDisadvantageVersion";
            var rulebaseVersion = "RulebaseVersion";

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "CurFundYr")).Returns(curFundYr);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LARSVersion")).Returns(larsVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "OrgVersion")).Returns(orgVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PostcodeDisadvantageVersion")).Returns(postcodeDisadvantageVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);

            var global = NewService(dataEntityAttributeServiceMock.Object).GlobalOutput(dataEntity);

            global.UKPRN.Should().Be(ukprn);
            global.CurFundYr.Should().Be(curFundYr);
            global.LARSVersion.Should().Be(larsVersion);
            global.OrgVersion.Should().Be(orgVersion);
            global.PostcodeDisadvantageVersion.Should().Be(postcodeDisadvantageVersion);
            global.RulebaseVersion.Should().Be(rulebaseVersion);
        }

        [Fact]
        public void LearnerFromDataEntity()
        {
            var learnRefNumber = "LearnRefNumber";

            var dataEntity = new DataEntity(string.Empty)
            {
                EntityName = learnRefNumber,
            };

            var learner = NewService(new Mock<IDataEntityAttributeService>().Object).LearnerFromDataEntity(dataEntity);

            learner.LearnRefNumber = learnRefNumber;
        }

        [Fact]
        public void LearningDeliveryFromDataEntity()
        {
            var achApplicDate = new DateTime(2018, 09, 01);
            var achieved = false;
            var achieveElement = 1;
            var achievePayElig = false;
            var achievePayPctPreTrans = 1;
            var achPayTransHeldBack = 1;
            var actualDaysIL = 1;
            var actualNumInstalm = 1;
            var actualNumInstalmPreTrans = 1;
            var actualNumInstalmTrans = 1;
            var adjLearnStartDate = new DateTime(2018, 09, 01);
            var adltLearnResp = false;
            var ageAimStart = 1;
            var aimValue = 1;
            var appAdjLearnStartDate = new DateTime(2018, 09, 01);
            var appAgeFact = 1;
            var appATAGTA = false;
            var appCompetency = false;
            var appFuncSkill = false;
            var appFuncSkill1618AdjFact = 1;
            var appKnowl = false;
            var appLearnStartDate = new DateTime(2018, 09, 01);
            var applicEmpFactDate = new DateTime(2018, 09, 01);
            var applicFactDate = new DateTime(2018, 09, 01);
            var applicFundRateDate = new DateTime(2018, 09, 01);
            var applicProgWeightFact = "1.0";
            var applicUnweightFundRate = 1;
            var applicWeightFundRate = 1;
            var appNonFund = false;
            var areaCostFactAdj = 1;
            var balInstalmPreTrans = 1;
            var baseValueUnweight = 1;
            var capFactor = 1;
            var disUpFactAdj = 1;
            var empOutcomePayElig = false;
            var empOutcomePctHeldBackTrans = 1;
            var empOutcomePctPreTrans = 1;
            var empRespOth = false;
            var eSOL = false;
            var fullyFund = false;
            var fundLine = "1.0";
            var fundStart = false;
            var largeEmployerFM35Fctr = 1;
            var largeEmployerID = 1;
            var largeEmployerStatusDate = new DateTime(2018, 09, 01);
            var lTRCUpliftFctr = 1;
            var nonGovCont = 1;
            var oLASSCustody = false;
            var onProgPayPctPreTrans = 1;
            var outstndNumOnProgInstalm = 1;
            var outstndNumOnProgInstalmTrans = 1;
            var plannedNumOnProgInstalm = 1;
            var plannedNumOnProgInstalmTrans = 1;
            var plannedTotalDaysIL = 1;
            var plannedTotalDaysILPreTrans = 1;
            var propFundRemain = 1;
            var propFundRemainAch = 1;
            var prscHEAim = false;
            var residential = false;
            var restart = false;
            var specResUplift = 1;
            var startPropTrans = 1;
            var thresholdDays = 1;
            var traineeship = false;
            var trans = false;
            var trnAdjLearnStartDate = new DateTime(2018, 09, 01);
            var trnWorkPlaceAim = false;
            var trnWorkPrepAim = false;
            var unWeightedRateFromESOL = 1;
            var unweightedRateFromLARS = 1;
            var weightedRateFromESOL = 1;
            var weightedRateFromLARS = 1;

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AchApplicDate")).Returns(achApplicDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Achieved")).Returns(achieved);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchieveElement")).Returns(achieveElement);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AchievePayElig")).Returns(achievePayElig);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchievePayPctPreTrans")).Returns(achievePayPctPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchPayTransHeldBack")).Returns(achPayTransHeldBack);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualDaysIL")).Returns(actualDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalm")).Returns(actualNumInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalmPreTrans")).Returns(actualNumInstalmPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalmTrans")).Returns(actualNumInstalmTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AdjLearnStartDate")).Returns(adjLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AdltLearnResp")).Returns(adltLearnResp);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AgeAimStart")).Returns(ageAimStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AimValue")).Returns(aimValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AppAdjLearnStartDate")).Returns(appAdjLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AppAgeFact")).Returns(appAgeFact);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppATAGTA")).Returns(appATAGTA);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppCompetency")).Returns(appCompetency);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppFuncSkill")).Returns(appFuncSkill);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AppFuncSkill1618AdjFact")).Returns(appFuncSkill1618AdjFact);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppKnowl")).Returns(appKnowl);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AppLearnStartDate")).Returns(appLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicEmpFactDate")).Returns(applicEmpFactDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicFactDate")).Returns(applicFactDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicFundRateDate")).Returns(applicFundRateDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "ApplicProgWeightFact")).Returns(applicProgWeightFact);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ApplicUnweightFundRate")).Returns(applicUnweightFundRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ApplicWeightFundRate")).Returns(applicWeightFundRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppNonFund")).Returns(appNonFund);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AreaCostFactAdj")).Returns(areaCostFactAdj);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "BalInstalmPreTrans")).Returns(balInstalmPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "BaseValueUnweight")).Returns(baseValueUnweight);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "CapFactor")).Returns(capFactor);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "DisUpFactAdj")).Returns(disUpFactAdj);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "EmpOutcomePayElig")).Returns(empOutcomePayElig);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "EmpOutcomePctHeldBackTrans")).Returns(empOutcomePctHeldBackTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "EmpOutcomePctPreTrans")).Returns(empOutcomePctPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "EmpRespOth")).Returns(empRespOth);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "ESOL")).Returns(eSOL);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "FullyFund")).Returns(fullyFund);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "FundLine")).Returns(fundLine);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "FundStart")).Returns(fundStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LargeEmployerFM35Fctr")).Returns(largeEmployerFM35Fctr);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LargeEmployerID")).Returns(largeEmployerID);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LargeEmployerStatusDate")).Returns(largeEmployerStatusDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LTRCUpliftFctr")).Returns(lTRCUpliftFctr);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "NonGovCont")).Returns(nonGovCont);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "OLASSCustody")).Returns(oLASSCustody);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OnProgPayPctPreTrans")).Returns(onProgPayPctPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalm")).Returns(outstndNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalmTrans")).Returns(outstndNumOnProgInstalmTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm")).Returns(plannedNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalmTrans")).Returns(plannedNumOnProgInstalmTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedTotalDaysIL")).Returns(plannedTotalDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedTotalDaysILPreTrans")).Returns(plannedTotalDaysILPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PropFundRemain")).Returns(propFundRemain);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PropFundRemainAch")).Returns(propFundRemainAch);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "PrscHEAim")).Returns(prscHEAim);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Residential")).Returns(residential);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Restart")).Returns(restart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "SpecResUplift")).Returns(specResUplift);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "StartPropTrans")).Returns(startPropTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ThresholdDays")).Returns(thresholdDays);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Traineeship")).Returns(traineeship);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Trans")).Returns(trans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "TrnAdjLearnStartDate")).Returns(trnAdjLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "TrnWorkPlaceAim")).Returns(trnWorkPlaceAim);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "TrnWorkPrepAim")).Returns(trnWorkPrepAim);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "UnWeightedRateFromESOL")).Returns(unWeightedRateFromESOL);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "UnweightedRateFromLARS")).Returns(unweightedRateFromLARS);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "WeightedRateFromESOL")).Returns(weightedRateFromESOL);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "StartPropTrans")).Returns(startPropTrans);

            var learningDelivery = NewService(dataEntityAttributeServiceMock.Object).LearningDeliveryAttributeData(dataEntity);

            learningDelivery.AchApplicDate.Should().Be(achApplicDate);
            learningDelivery.Achieved.Should().Be(achieved);
            learningDelivery.AchieveElement.Should().Be(achieveElement);
            learningDelivery.AchievePayElig.Should().Be(achievePayElig);
            learningDelivery.AchievePayPctPreTrans.Should().Be(achievePayPctPreTrans);
            learningDelivery.AchPayTransHeldBack.Should().Be(achPayTransHeldBack);
            learningDelivery.ActualDaysIL.Should().Be(actualDaysIL);
            learningDelivery.ActualNumInstalm.Should().Be(actualNumInstalm);
            learningDelivery.ActualNumInstalmPreTrans.Should().Be(actualNumInstalmPreTrans);
            learningDelivery.ActualNumInstalmTrans.Should().Be(actualNumInstalmTrans);
            learningDelivery.AdjLearnStartDate.Should().Be(adjLearnStartDate);
            learningDelivery.AdltLearnResp.Should().Be(adltLearnResp);
            learningDelivery.AgeAimStart.Should().Be(ageAimStart);
            learningDelivery.AimValue.Should().Be(aimValue);
            learningDelivery.AppAdjLearnStartDate.Should().Be(appAdjLearnStartDate);
            learningDelivery.AppAgeFact.Should().Be(appAgeFact);
            learningDelivery.AppATAGTA.Should().Be(appATAGTA);
            learningDelivery.AppCompetency.Should().Be(appCompetency);
            learningDelivery.AppFuncSkill.Should().Be(appFuncSkill);
            learningDelivery.AppFuncSkill1618AdjFact.Should().Be(appFuncSkill1618AdjFact);
            learningDelivery.AppKnowl.Should().Be(appKnowl);
            learningDelivery.AppLearnStartDate.Should().Be(appLearnStartDate);
            learningDelivery.ApplicEmpFactDate.Should().Be(applicEmpFactDate);
            learningDelivery.ApplicFactDate.Should().Be(applicFactDate);
            learningDelivery.ApplicFundRateDate.Should().Be(applicFundRateDate);
            learningDelivery.ApplicProgWeightFact.Should().Be(applicProgWeightFact);
            learningDelivery.ApplicUnweightFundRate.Should().Be(applicUnweightFundRate);
            learningDelivery.ApplicWeightFundRate.Should().Be(applicWeightFundRate);
            learningDelivery.AppNonFund.Should().Be(appNonFund);
            learningDelivery.AreaCostFactAdj.Should().Be(areaCostFactAdj);
            learningDelivery.BalInstalmPreTrans.Should().Be(balInstalmPreTrans);
            learningDelivery.BaseValueUnweight.Should().Be(baseValueUnweight);
            learningDelivery.CapFactor.Should().Be(capFactor);
            learningDelivery.DisUpFactAdj.Should().Be(disUpFactAdj);
            learningDelivery.EmpOutcomePayElig.Should().Be(empOutcomePayElig);
            learningDelivery.EmpOutcomePctHeldBackTrans.Should().Be(empOutcomePctHeldBackTrans);
            learningDelivery.EmpOutcomePctPreTrans.Should().Be(empOutcomePctPreTrans);
            learningDelivery.EmpRespOth.Should().Be(empRespOth);
            learningDelivery.ESOL.Should().Be(eSOL);
            learningDelivery.FullyFund.Should().Be(fullyFund);
            learningDelivery.FundLine.Should().Be(fundLine);
            learningDelivery.FundStart.Should().Be(fundStart);
            learningDelivery.LargeEmployerFM35Fctr.Should().Be(largeEmployerFM35Fctr);
            learningDelivery.LargeEmployerID.Should().Be(largeEmployerID);
            learningDelivery.LargeEmployerStatusDate.Should().Be(largeEmployerStatusDate);
            learningDelivery.LTRCUpliftFctr.Should().Be(lTRCUpliftFctr);
            learningDelivery.NonGovCont.Should().Be(nonGovCont);
            learningDelivery.OLASSCustody.Should().Be(oLASSCustody);
            learningDelivery.OnProgPayPctPreTrans.Should().Be(onProgPayPctPreTrans);
            learningDelivery.OutstndNumOnProgInstalm.Should().Be(outstndNumOnProgInstalm);
            learningDelivery.OutstndNumOnProgInstalmTrans.Should().Be(outstndNumOnProgInstalmTrans);
            learningDelivery.PlannedNumOnProgInstalm.Should().Be(plannedNumOnProgInstalm);
            learningDelivery.PlannedNumOnProgInstalmTrans.Should().Be(plannedNumOnProgInstalmTrans);
            learningDelivery.PlannedTotalDaysIL.Should().Be(plannedTotalDaysIL);
            learningDelivery.PlannedTotalDaysILPreTrans.Should().Be(plannedTotalDaysILPreTrans);
            learningDelivery.PropFundRemain.Should().Be(propFundRemain);
            learningDelivery.PropFundRemainAch.Should().Be(propFundRemainAch);
            learningDelivery.PrscHEAim.Should().Be(prscHEAim);
            learningDelivery.Residential.Should().Be(residential);
            learningDelivery.Restart.Should().Be(restart);
            learningDelivery.SpecResUplift.Should().Be(specResUplift);
            learningDelivery.StartPropTrans.Should().Be(startPropTrans);
            learningDelivery.ThresholdDays.Should().Be(thresholdDays);
            learningDelivery.Traineeship.Should().Be(traineeship);
            learningDelivery.Trans.Should().Be(trans);
            learningDelivery.TrnAdjLearnStartDate.Should().Be(trnAdjLearnStartDate);
            learningDelivery.TrnWorkPlaceAim.Should().Be(trnWorkPlaceAim);
            learningDelivery.TrnWorkPrepAim.Should().Be(trnWorkPrepAim);
            learningDelivery.UnWeightedRateFromESOL.Should().Be(unWeightedRateFromESOL);
            learningDelivery.UnweightedRateFromLARS.Should().Be(unweightedRateFromLARS);
            learningDelivery.WeightedRateFromESOL.Should().Be(weightedRateFromESOL);
            learningDelivery.WeightedRateFromLARS.Should().Be(weightedRateFromLARS);
        }

        [Fact]
        public void FundingOutput_LearningDeliveryPeriodisedAttributeData_Correct()
        {
            // ARRANGE
            var fundingOutputService = NewService();

            // ACT
            var learningDeliveryPeriodisedAttributeData =
                fundingOutputService.LearningDeliveryPeriodisedAttributeData(TestLearnerEntity(null, "TestLearner", true).SingleOrDefault().Children.SingleOrDefault());

            // ASSERT
            var expectedLearningDeliveryPeriodisedAttributeData = TestLearningDeliveryPeriodisedAttributesDataArray();

            expectedLearningDeliveryPeriodisedAttributeData.Should().BeEquivalentTo(learningDeliveryPeriodisedAttributeData);
        }

        private IEnumerable<IDataEntity> TestEntities()
        {
            var entities = new List<DataEntity>();

            var entity1 = GlobalDataEntity();

            entity1.AddChildren(TestLearnerEntity(entity1, "TestLearner1", false));

            entities.Add(entity1);

            var entity2 = GlobalDataEntity();

            entity2.AddChildren(TestLearnerEntity(entity2, "TestLearner2", true));

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
                        { "DateOfBirth", new AttributeData("DateOfBirth", new Date(2000, 01, 01)) },
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
                    { "DateOfBirth", new AttributeData("DateOfBirth", new Date(2000, 01, 01)) },
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
                    { "AchApplicDate", Attribute("AchApplicDate", false, new Date(new DateTime(2018, 09, 01))) },
                    { "Achieved", Attribute("Achieved", false, "1.0") },
                    { "AchieveElement", Attribute("AchieveElement", false, "1.0") },
                    { "AchievePayElig", Attribute("AchievePayElig", false, "1.0") },
                    { "AchievePayPctPreTrans", Attribute("AchievePayPctPreTrans", false, "1.0") },
                    { "AchPayTransHeldBack", Attribute("AchPayTransHeldBack", false, "1.0") },
                    { "ActualDaysIL", Attribute("ActualDaysIL", false, "1.0") },
                    { "ActualNumInstalm", Attribute("ActualNumInstalm", false, "1.0") },
                    { "ActualNumInstalmPreTrans", Attribute("ActualNumInstalmPreTrans", false, "1.0") },
                    { "ActualNumInstalmTrans", Attribute("ActualNumInstalmTrans", false, "1.0") },
                    { "AdjLearnStartDate", Attribute("AdjLearnStartDate", false, new Date(new DateTime(2018, 09, 01))) },
                    { "AdltLearnResp", Attribute("AdltLearnResp", false, "1.0") },
                    { "AgeAimStart", Attribute("AgeAimStart", false, "1.0") },
                    { "AimValue", Attribute("AimValue", false, "1.0") },
                    { "AppAdjLearnStartDate", Attribute("AppAdjLearnStartDate", false, new Date(new DateTime(2018, 09, 01))) },
                    { "AppAgeFact", Attribute("AppAgeFact", false, "1.0") },
                    { "AppATAGTA", Attribute("AppATAGTA", false, "1.0") },
                    { "AppCompetency", Attribute("AppCompetency", false, "1.0") },
                    { "AppFuncSkill", Attribute("AppFuncSkill", false, "1.0") },
                    { "AppFuncSkill1618AdjFact", Attribute("AppFuncSkill1618AdjFact", false, "1.0") },
                    { "AppKnowl", Attribute("AppKnowl", false, "1.0") },
                    { "AppLearnStartDate", Attribute("AppLearnStartDate", false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicEmpFactDate", Attribute("ApplicEmpFactDate", false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicFactDate", Attribute("ApplicFactDate", false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicFundRateDate", Attribute("ApplicFundRateDate", false, new Date(new DateTime(2018, 09, 01))) },
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
                    { "LargeEmployerStatusDate", Attribute("LargeEmployerStatusDate", false, new Date(new DateTime(2018, 09, 01))) },
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
                    { "TrnAdjLearnStartDate", Attribute("TrnAdjLearnStartDate", false, new Date(new DateTime(2018, 09, 01))) },
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

        private DataEntity GlobalDataEntity()
        {
            return new DataEntity("global")
            {
                EntityName = "global",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "UKPRN", new AttributeData("UKPRN", "12345678") },
                    { "OrgVersion", new AttributeData("OrgVersion", "Version_003") },
                    { "LARSVersion", new AttributeData("LARSVersion", "Version_005") },
                    { "CurFundYr", new AttributeData("CurFundYr", "1819") },
                    { "PostcodeDisadvantageVersion", new AttributeData("PostcodeDisadvantageVersion", "Version_002") },
                    { "RulebaseVersion", new AttributeData("RulebaseVersion", "1718.5.10") },
                },
                Parent = null,
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
                 new TemporalValueItem(new DateTime(2018, 08, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 09, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 10, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 11, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 12, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 01, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 02, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 03, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 04, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 05, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 06, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 07, 01), value, null),
            };

            changePoints.AddRange(cps);

            return changePoints;
        }

        private LearningDeliveryAttribute[] TestLearningDeliveryAttributeArray(int aimSeq)
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

        private LearningDeliveryAttributeData TestLearningDeliveryAttributeData()
        {
            return new LearningDeliveryAttributeData
            {
                AchApplicDate = new DateTime(2018, 09, 01),
                Achieved = false,
                AchieveElement = 1,
                AchievePayElig = false,
                AchievePayPctPreTrans = 1,
                AchPayTransHeldBack = 1,
                ActualDaysIL = 1,
                ActualNumInstalm = 1,
                ActualNumInstalmPreTrans = 1,
                ActualNumInstalmTrans = 1,
                AdjLearnStartDate = new DateTime(2018, 09, 01),
                AdltLearnResp = false,
                AgeAimStart = 1,
                AimValue = 1,
                AppAdjLearnStartDate = new DateTime(2018, 09, 01),
                AppAgeFact = 1.0m,
                AppATAGTA = false,
                AppCompetency = false,
                AppFuncSkill = false,
                AppFuncSkill1618AdjFact = 1,
                AppKnowl = false,
                AppLearnStartDate = new DateTime(2018, 09, 01),
                ApplicEmpFactDate = new DateTime(2018, 09, 01),
                ApplicFactDate = new DateTime(2018, 09, 01),
                ApplicFundRateDate = new DateTime(2018, 09, 01),
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
                LargeEmployerStatusDate = new DateTime(2018, 09, 01),
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
                TrnAdjLearnStartDate = new DateTime(2018, 09, 01),
                TrnWorkPlaceAim = false,
                TrnWorkPrepAim = false,
                UnWeightedRateFromESOL = 1,
                UnweightedRateFromLARS = 1,
                WeightedRateFromESOL = 1,
                WeightedRateFromLARS = 1,
            };
        }

        private LearningDeliveryPeriodisedAttribute[] TestLearningDeliveryPeriodisedAttributesDataArray()
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

        private FundingOutputService NewService(IDataEntityAttributeService dataEntityAttributeService = null)
        {
            return new FundingOutputService(dataEntityAttributeService);
        }
    }
}
