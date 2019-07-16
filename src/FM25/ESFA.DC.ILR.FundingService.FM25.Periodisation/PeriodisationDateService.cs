using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Constants;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class PeriodisationDateService : IPeriodisationDateService
    {
        public DateTime GetPeriodisationStartDate(FM25Learner learner)
        {
            if (new PeriodisationService().IsLearnerTrainee(learner.FundLine))
            {
                return (learner.LearnerStartDate.Value > DateConstants.AcademicYearStartDate) ? learner.LearnerStartDate.Value : DateConstants.AcademicYearStartDate;
            }
            else
            {
                return DateConstants.AcademicYearStartDate;
            }
        }

        public DateTime GetPeriodisationEndDate(FM25Learner learner, bool learnerIsTrainee)
        {
            if(learnerIsTrainee)
            {
                if(learner.LearnerActEndDate.HasValue)
                {
                    if (learner.LearnerPlanEndDate.Value > DateConstants.AcademicYearStartDate)
                    {
                        return new[] { learner.LearnerPlanEndDate.Value, learner.LearnerActEndDate.Value, DateConstants.AcademicYearEndDate }.Min();
                    }
                    else
                    {
                        return learner.LearnerActEndDate.Value;
                    }
                }
                else
                {
                    if (learner.LearnerPlanEndDate > DateConstants.AcademicYearStartDate)
                    {
                        return new[] { learner.LearnerPlanEndDate.Value, DateConstants.AcademicYearEndDate }.Min();
                    }
                    else
                    {
                        return DateConstants.AcademicYearEndDate;
                    }
                }
            }
            else
            {
                return DateConstants.AcademicYearEndDate;
            }
        }

        public int GetMonthsBetweenDatesIgnoringDaysInclusive(DateTime periodisationStartDate, DateTime periodisationEndDate)
        {
            var yearMonths = (periodisationEndDate.Year - periodisationStartDate.Year) * 12;
            var months = periodisationEndDate.Month - periodisationStartDate.Month;

            return yearMonths + months + 1;
        }

        private readonly IReadOnlyDictionary<int, int> _periodToMonthLookup = new Dictionary<int, int>()
        {
            [1] = 6,
            [2] = 7,
            [3] = 8,
            [4] = 9,
            [5] = 10,
            [6] = 11,
            [7] = 12,
            [8] = 1,
            [9] = 2,
            [10] = 3,
            [11] = 4,
            [12] = 5
        };

        public int PeriodFromDate(DateTime date)
        {
            return _periodToMonthLookup[date.Month];
        }

    }
}
