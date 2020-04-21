using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Output
{
    public class FundingOutputService : IOutputService<IEnumerable<FM25Global>>
    {
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IDataEntityAttributeService dataEntityAttributeService)
        {
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public IEnumerable<FM25Global> ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            return dataEntities.Select(MapGlobal);
        }

        public FM25Global MapGlobal(IDataEntity dataEntity)
        {
            return new FM25Global()
            {
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LARSVersion),
                OrgVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.OrgVersion),
                PostcodeDisadvantageVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PostcodeDisadvantageVersion),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN),
                Learners = dataEntity
                    .Children?
                    .Where(c => c.EntityName == Attributes.EntityLearner)
                    .Select(MapLearner)
                    .ToList()
            };
        }

        public FM25Learner MapLearner(IDataEntity dataEntity)
        {
            return new FM25Learner()
            {
                AcadMonthPayment = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AcadMonthPayment),
                AcadProg = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AcadProg),
                ActualDaysILCurrYear = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualDaysILCurrYear),
                AreaCostFact1618Hist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AreaCostFact1618Hist),
                Block1DisadvUpliftNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Block1DisadvUpliftNew),
                Block2DisadvElementsNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Block2DisadvElementsNew),
                ConditionOfFundingEnglish = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.ConditionOfFundingEnglish),
                ConditionOfFundingMaths = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.ConditionOfFundingMaths),
                CoreAimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.CoreAimSeqNumber),
                FullTimeEquiv = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.FullTimeEquiv),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.FundLine),
                LearnerActEndDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LearnerActEndDate),
                LearnerPlanEndDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LearnerPlanEndDate),
                LearnerStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LearnerStartDate),
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnRefNumber),
                NatRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.NatRate),
                OnProgPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.OnProgPayment),
                PlannedDaysILCurrYear = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedDaysILCurrYear),
                ProgWeightHist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.ProgWeightHist),
                ProgWeightNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.ProgWeightNew),
                PrvDisadvPropnHist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PrvDisadvPropnHist),
                PrvHistLrgProgPropn = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PrvHistLrgProgPropn),
                PrvRetentFactHist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PrvRetentFactHist),
                RateBand = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RateBand),
                RetentNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.RetentNew),
                StartFund = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.StartFund),
                ThresholdDays = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ThresholdDays),
                TLevelStudent = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.TLevelStudent),
            };
        }
    }
}
