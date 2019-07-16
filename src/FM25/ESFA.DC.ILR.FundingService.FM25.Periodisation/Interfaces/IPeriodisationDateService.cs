using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using System;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces
{
    public interface IPeriodisationDateService
    {
        DateTime GetPeriodisationStartDate(FM25Learner learner);

        DateTime GetPeriodisationEndDate(FM25Learner learner, bool learnerIsTrainee);

        int GetMonthsBetweenDatesIgnoringDaysInclusive(DateTime periodisationStartDate, DateTime periodisationEndDate);

        int PeriodFromDate(DateTime date);
    }
}
