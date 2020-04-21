using System;
using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Tests
{
    public class FundingOutputServiceTests
    {
        [Fact]
        public void MapGlobal()
        {
            var larsVersion = "LARSVersion";
            var orgVersion = "OrgVersion";
            var postcodeDisadvantageVersion = "PostcodeDisadvantageVersion";
            var rulebaseVersion = "RulebaseVersion";
            var ukprn = 1234;

            var dataEntity = new DataEntity(null);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LARSVersion")).Returns(larsVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "OrgVersion")).Returns(orgVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PostcodeDisadvantageVersion")).Returns(postcodeDisadvantageVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);

            var global = NewService(dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.LARSVersion.Should().Be(larsVersion);
            global.OrgVersion.Should().Be(orgVersion);
            global.PostcodeDisadvantageVersion.Should().Be(postcodeDisadvantageVersion);
            global.RulebaseVersion.Should().Be(rulebaseVersion);
            global.UKPRN.Should().Be(ukprn);

            global.Learners.Should().BeEmpty();
        }

        [Fact]
        public void MapLearner()
        {
            var acadMonthPayment = 1;
            var acadProg = true;
            var actualDaysILCurrYear = 2;
            var areaCostFact1618Hist = 1.0m;
            var block1DisadvUpliftNew = 1.1m;
            var block2DisadvElementsNew = 1.2m;
            var conditionOfFundingEnglish = "ConditionOfFundingEnglish";
            var conditionOfFundingMaths = "ConditionOfFundingMaths";
            var coreAimSeqNumber = 6;
            var fullTimeEquiv = 2.4m;
            var fundLine = "FundLine";
            var learnerActEndDate = new DateTime(2018, 1, 1);
            var learnerPlanEndDate = new DateTime(2019, 1, 1);
            var learnerStartDate = new DateTime(2020, 1, 1);
            var natRate = 1.4m;
            var onProgPayment = 1.5m;
            var plannedDaysILCurrYear = 3;
            var progWeightHist = 1.6m;
            var progWeightNew = 1.7m;
            var prvDisadvPropnHist = 1.8m;
            var prvHistLrgProgPropn = 1.9m;
            var prvRetentFactHist = 2.0m;
            var rateBand = "RateBand";
            var retentNew = 2.1m;
            var startFund = true;
            var thresholdDays = 8;
            var tLevelStudent = true;

            var dataEntity = new DataEntity(null);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AcadMonthPayment")).Returns(acadMonthPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AcadProg")).Returns(acadProg);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualDaysILCurrYear")).Returns(actualDaysILCurrYear);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AreaCostFact1618Hist")).Returns(areaCostFact1618Hist);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Block1DisadvUpliftNew")).Returns(block1DisadvUpliftNew);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "Block2DisadvElementsNew")).Returns(block2DisadvElementsNew);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "ConditionOfFundingEnglish")).Returns(conditionOfFundingEnglish);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "ConditionOfFundingMaths")).Returns(conditionOfFundingMaths);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "CoreAimSeqNumber")).Returns(coreAimSeqNumber);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "FullTimeEquiv")).Returns(fullTimeEquiv);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "FundLine")).Returns(fundLine);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnerActEndDate")).Returns(learnerActEndDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnerPlanEndDate")).Returns(learnerPlanEndDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnerStartDate")).Returns(learnerStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "NatRate")).Returns(natRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "OnProgPayment")).Returns(onProgPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedDaysILCurrYear")).Returns(plannedDaysILCurrYear);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ProgWeightHist")).Returns(progWeightHist);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ProgWeightNew")).Returns(progWeightNew);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PrvDisadvPropnHist")).Returns(prvDisadvPropnHist);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PrvHistLrgProgPropn")).Returns(prvHistLrgProgPropn);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PrvRetentFactHist")).Returns(prvRetentFactHist);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RateBand")).Returns(rateBand);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "RetentNew")).Returns(retentNew);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "StartFund")).Returns(startFund);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ThresholdDays")).Returns(thresholdDays);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "TLevelStudent")).Returns(tLevelStudent);

            var learner = NewService(dataEntityAttributeServiceMock.Object).MapLearner(dataEntity);

            learner.AcadMonthPayment.Should().Be(acadMonthPayment);
            learner.AcadProg.Should().Be(acadProg);
            learner.ActualDaysILCurrYear.Should().Be(actualDaysILCurrYear);
            learner.AreaCostFact1618Hist.Should().Be(areaCostFact1618Hist);
            learner.Block1DisadvUpliftNew.Should().Be(block1DisadvUpliftNew);
            learner.Block2DisadvElementsNew.Should().Be(block2DisadvElementsNew);
            learner.ConditionOfFundingEnglish.Should().Be(conditionOfFundingEnglish);
            learner.ConditionOfFundingMaths.Should().Be(conditionOfFundingMaths);
            learner.CoreAimSeqNumber.Should().Be(coreAimSeqNumber);
            learner.FullTimeEquiv.Should().Be(fullTimeEquiv);
            learner.FundLine.Should().Be(fundLine);
            learner.LearnerActEndDate.Should().Be(learnerActEndDate);
            learner.LearnerPlanEndDate.Should().Be(learnerPlanEndDate);
            learner.LearnerStartDate.Should().Be(learnerStartDate);
            learner.NatRate.Should().Be(natRate);
            learner.OnProgPayment.Should().Be(onProgPayment);
            learner.PlannedDaysILCurrYear.Should().Be(plannedDaysILCurrYear);
            learner.ProgWeightHist.Should().Be(progWeightHist);
            learner.ProgWeightNew.Should().Be(progWeightNew);
            learner.PrvDisadvPropnHist.Should().Be(prvDisadvPropnHist);
            learner.PrvHistLrgProgPropn.Should().Be(prvHistLrgProgPropn);
            learner.PrvRetentFactHist.Should().Be(prvRetentFactHist);
            learner.RateBand.Should().Be(rateBand);
            learner.RetentNew.Should().Be(retentNew);
            learner.StartFund.Should().Be(startFund);
            learner.ThresholdDays.Should().Be(thresholdDays);
            learner.TLevelStudent.Should().Be(tLevelStudent);
        }

        private FundingOutputService NewService(IDataEntityAttributeService dataEntityAttributeService = null)
        {
            return new FundingOutputService(dataEntityAttributeService);
        }
    }
}
