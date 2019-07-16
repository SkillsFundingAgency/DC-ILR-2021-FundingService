using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Constants;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using System;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class PeriodisationService : IPeriodisationService
    {
        public decimal[] GetPeriodisedValues(FM25Learner learner)
        {
            var periodisationDateService = new PeriodisationDateService();
            var periodisationStartDate = periodisationDateService.GetPeriodisationStartDate(learner);
            var periodisationEndDate = periodisationDateService.GetPeriodisationEndDate(learner, IsLearnerTrainee(learner.FundLine));
            var learnerPeriods = periodisationDateService.GetMonthsBetweenDatesIgnoringDaysInclusive(periodisationStartDate, periodisationEndDate);
            var values = GetMonthlyValues();

            if(IsLearnerTrainee(learner.FundLine))
            {
                var monthlyPayment = learner.OnProgPayment.Value / learnerPeriods;
                var startPeriod = periodisationDateService.PeriodFromDate(periodisationStartDate);
                var endPeriod = periodisationDateService.PeriodFromDate(periodisationEndDate);

                for (var periodIndex = startPeriod - 1; periodIndex <= endPeriod -1; periodIndex++)
                {
                    values[periodIndex] = monthlyPayment;
                }
            }

            return values;
        }

        public bool IsLearnerTrainee(string fundLine) => fundLine == FundingLineConstants.Traineeship19Plus || fundLine == FundingLineConstants.Traineeship1618;

        public decimal[] GetMonthlyValues()
        {
            return new decimal[12]{ 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M };
        }
    }
}
