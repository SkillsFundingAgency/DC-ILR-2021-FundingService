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
    public class PeriodisationOutputService : IOutputService<PeriodisationGlobal>
    {
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public PeriodisationOutputService(IDataEntityAttributeService dataEntityAttributeService)
        {
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public PeriodisationGlobal ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            var globals = dataEntities.Select(MapGlobal).ToList();

            var firstGlobal = globals.FirstOrDefault();

            if (firstGlobal != null)
            {
                firstGlobal.LearnerPeriods = globals.SelectMany(g => g.LearnerPeriods).ToList();
                firstGlobal.LearnerPeriodisedValues = globals.SelectMany(g => g.LearnerPeriodisedValues).ToList();

                return firstGlobal;
            }

            return new PeriodisationGlobal();
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
    }
}
