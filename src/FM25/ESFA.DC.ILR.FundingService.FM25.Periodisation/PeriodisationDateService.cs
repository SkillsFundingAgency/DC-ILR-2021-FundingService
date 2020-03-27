﻿using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Constants;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class PeriodisationDateService : IPeriodisationDateService
    {
        private readonly IPeriodisationService _periodisationService;
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

        public PeriodisationDateService(IPeriodisationService periodisationService)
        {
            _periodisationService = periodisationService;
        }

        public DateTime GetPeriodisationStartDate(FM25Learner learner)
        {
            if (_periodisationService.IsLearnerTrainee(learner))
            {
                return learner.LearnerStartDate > DateConstants.AcademicYearStartDate ? learner.LearnerStartDate.Value : DateConstants.AcademicYearStartDate;
            }

            return DateConstants.AcademicYearStartDate;
        }

        public DateTime GetPeriodisationEndDate(FM25Learner learner, bool learnerIsTrainee)
        {
            if (!learnerIsTrainee) return DateConstants.AcademicYearEndDate;

            if(learner.LearnerActEndDate.HasValue)
            {
                return learner.LearnerPlanEndDate > DateConstants.AcademicYearStartDate ? new[] { learner.LearnerPlanEndDate.Value, learner.LearnerActEndDate.Value, DateConstants.AcademicYearEndDate }.Min() : learner.LearnerActEndDate.Value;
            }

            return learner.LearnerPlanEndDate > DateConstants.AcademicYearStartDate ? new[] { learner.LearnerPlanEndDate.Value, DateConstants.AcademicYearEndDate }.Min() : DateConstants.AcademicYearEndDate;
        }

        public int GetMonthsBetweenDatesIgnoringDaysInclusive(DateTime periodisationStartDate, DateTime periodisationEndDate)
        {
            var yearMonths = (periodisationEndDate.Year - periodisationStartDate.Year) * 12;
            var months = periodisationEndDate.Month - periodisationStartDate.Month;

            return yearMonths + months + 1;
        }

        public int PeriodFromDate(DateTime date)
        {
            return _periodToMonthLookup[date.Month];
        }
    }
}
