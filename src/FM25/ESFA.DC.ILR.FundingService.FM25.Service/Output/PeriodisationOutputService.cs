using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Output
{
    public class PeriodisationOutputService : IOutputService<IEnumerable<PeriodisationGlobal>>
    {
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public PeriodisationOutputService(IDataEntityAttributeService dataEntityAttributeService)
        {
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public IEnumerable<PeriodisationGlobal> ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            return dataEntities.Select(MapGlobal);
        }

        public PeriodisationGlobal MapGlobal(IDataEntity dataEntity)
        {
            return new PeriodisationGlobal()
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, OutputAttributeNames.UKPRN),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.RulebaseVersion),
                LearnerPeriods = new List<LearnerPeriod>(),
                LearnerPeriodisedValues = new List<LearnerPeriodisedValues>()
            };
        }

        public LearnerPeriodisedValues MapLearnerPeriodisedValues(IDataEntity dataEntity)
        {
            return new LearnerPeriodisedValues()
            {
                AttributeName = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.AttributeName),
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, OutputAttributeNames.LearnRefNumber),
                Period1 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period1),
                Period2 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period2),
                Period3 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period3),
                Period4 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period4),
                Period5 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period5),
                Period6 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period6),
                Period7 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period7),
                Period8 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period8),
                Period9 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period9),
                Period10 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period10),
                Period11 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period11),
                Period12 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, OutputAttributeNames.Period12),
            };
        }

        private PeriodisationGlobal CondensePeriodisationGlobals(IEnumerable<PeriodisationGlobal> periodisationGlobals)
        {
            periodisationGlobals = periodisationGlobals.ToList();

            var firstGlobal = periodisationGlobals.FirstOrDefault();

            if (firstGlobal != null)
            {
                firstGlobal.LearnerPeriods = periodisationGlobals.SelectMany(g => g.LearnerPeriods).ToList();
                firstGlobal.LearnerPeriodisedValues = periodisationGlobals.SelectMany(g => g.LearnerPeriodisedValues).ToList();

                return firstGlobal;
            }

            return new PeriodisationGlobal();
        }
    }
}
