using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Constants;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class PeriodisationService : IPeriodisationService
    {
        private readonly IPeriodisationDateService _periodisationDateService;

        public PeriodisationService(IPeriodisationDateService periodisationDateService)
        {
            _periodisationDateService = periodisationDateService;
        }

        public IEnumerable<decimal> GetPeriodisedValues(FM25Learner learner)
        {
            var periodisationStartDate = _periodisationDateService.GetPeriodisationStartDate(learner);
            var periodisationEndDate = _periodisationDateService.GetPeriodisationEndDate(learner, IsLearnerTrainee(learner));
            var learnerPeriods = _periodisationDateService.GetMonthsBetweenDatesIgnoringDaysInclusive(periodisationStartDate, periodisationEndDate);
            var values = GetMonthlyValues();

            if(IsLearnerTrainee(learner))
            {
                var monthlyPayment = learner.OnProgPayment.Value / learnerPeriods;
                var startPeriod = _periodisationDateService.PeriodFromDate(periodisationStartDate);
                var endPeriod = _periodisationDateService.PeriodFromDate(periodisationEndDate);

                for (var periodIndex = startPeriod - 1; periodIndex <= endPeriod -1; periodIndex++)
                {
                    values[periodIndex] = monthlyPayment;
                }
            }

            return values;
        }

        public bool IsLearnerTrainee(FM25Learner learner) => learner.FundLine == FundingLineConstants.Traineeship19Plus || learner.FundLine == FundingLineConstants.Traineeship1618;

        private decimal[] GetMonthlyValues() => new decimal[12] { 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M };
    }
}
