using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Output
{
    public class FundingOutputService : IOutputService<IEnumerable<Global>>
    {
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IDataEntityAttributeService dataEntityAttributeService)
        {
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public IEnumerable<Global> ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            return dataEntities.Select(MapGlobal);
        }

        public Global MapGlobal(IDataEntity dataEntity)
        {
            return new Global()
            {
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.LARSVersion),
                OrgVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.OrgVersion),
                PostcodeDisadvantageVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.PostcodeDisadvantageVersion),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.RulebaseVersion),
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, OutputAttributeNames.UKPRN),
                Learners = dataEntity
                    .Children?
                    .Where(c => c.EntityName == OutputAttributeNames.Learner)
                    .Select(MapLearner)
                    .ToList()
            };
        }

        public Learner MapLearner(IDataEntity dataEntity)
        {
            return new Learner()
            {
                AcadMonthPayment = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, OutputAttributeNames.AcadMonthPayment),
                AcadProg = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, OutputAttributeNames.AcadProg),
                ActualDaysILCurrYear = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, OutputAttributeNames.ActualDaysILCurrYear),
                AreaCostFact1618Hist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.AreaCostFact1618Hist),
                Block1DisadvUpliftNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Block1DisadvUpliftNew),
                Block2DisadvElementsNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Block2DisadvElementsNew),
                ConditionOfFundingEnglish = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.ConditionOfFundingEnglish),
                ConditionOfFundingMaths = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.ConditionOfFundingMaths),
                CoreAimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, OutputAttributeNames.CoreAimSeqNumber),
                FullTimeEquiv = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.FullTimeEquiv),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.FundLine),
                LearnerActEndDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, OutputAttributeNames.LearnerActEndDate),
                LearnerPlanEndDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, OutputAttributeNames.LearnerPlanEndDate),
                LearnerStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, OutputAttributeNames.LearnerStartDate),
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.LearnRefNumber),
                NatRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.NatRate),
                OnProgPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.OnProgPayment),
                PlannedDaysILCurrYear = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, OutputAttributeNames.PlannedDaysILCurrYear),
                ProgWeightHist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.ProgWeightHist),
                ProgWeightNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.ProgWeightNew),
                PrvDisadvPropnHist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.PrvDisadvPropnHist),
                PrvHistLrgProgPropn = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.PrvHistLrgProgPropn),
                PrvRetentFactHist = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.PrvRetentFactHist),
                RateBand = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.RateBand),
                RetentNew = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.RetentNew),
                StartFund = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, OutputAttributeNames.StartFund),
                ThresholdDays = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, OutputAttributeNames.ThresholdDays)
            };
        }
    }
}
