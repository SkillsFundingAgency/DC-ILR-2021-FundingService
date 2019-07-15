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
        public DateTime? GetPeriodisationStartDate(FM25Learner learner)
        {
            if (IsLearnerTrainee(learner.FundLine))
                return (learner.LearnerStartDate > DateConstants.academicYearStartDate) ? learner.LearnerStartDate : DateConstants.academicYearStartDate;
            else
                return DateConstants.academicYearStartDate;
        }

        public DateTime? GetPeriodisationEndDate(FM25Learner learner, bool learnerIsTrainee)
        {
            if (learnerIsTrainee && learner.LearnerPlanEndDate > DateConstants.academicYearStartDate && learner.LearnerActEndDate != null)
            {
                return new[] { learner.LearnerPlanEndDate, learner.LearnerActEndDate, DateConstants.academicYearEndDate }.Min();
            }
            else if (learnerIsTrainee && learner.LearnerPlanEndDate > DateConstants.academicYearStartDate && learner.LearnerActEndDate == null)
            {
                return new[] { learner.LearnerPlanEndDate, DateConstants.academicYearEndDate }.Min();
            }
            else if (learnerIsTrainee && learner.LearnerPlanEndDate < DateConstants.academicYearStartDate && learner.LearnerActEndDate != null)
            {
                return learner.LearnerActEndDate;
            }
            else if (learnerIsTrainee && learner.LearnerPlanEndDate < DateConstants.academicYearStartDate && learner.LearnerActEndDate == null)
            {
                return DateConstants.academicYearEndDate;
            }
            else
            {
                return DateConstants.academicYearEndDate;
            }
        }

        public int GetMonthsBetweenDatesIgnoringDaysInclusive(DateTime? periodisationStartDate, DateTime? periodisationEndDate)
        {
            return ((periodisationEndDate.Value.Year - periodisationStartDate.Value.Year) * 12) + periodisationEndDate.Value.Month - periodisationStartDate.Value.Month + 1;
        }

        public LearnerPeriodisedValues GetPeriodisedValues(FM25Learner learner)
        {
            var periodisationStartDate = GetPeriodisationStartDate(learner);
            var periodisationEndDate = GetPeriodisationEndDate(learner, IsLearnerTrainee(learner.FundLine));
            var learnerPeriods = GetMonthsBetweenDatesIgnoringDaysInclusive(periodisationStartDate, periodisationEndDate);
            var periodisationStartDateBeginningofMonth = new DateTime(periodisationStartDate.Value.Year, periodisationStartDate.Value.Month, 1);
            var periodisationEndDateEndofMonth = new DateTime(periodisationEndDate.Value.Year, periodisationEndDate.Value.Month, DateTime.DaysInMonth(periodisationEndDate.Value.Year, periodisationEndDate.Value.Month));
            decimal?[] values = new decimal?[12];
            for (var month = 0; month<=11; month++)
            {
                var monthToCheck = DateConstants.academicYearStartDate.AddMonths(month);
                if(IsLearnerTrainee(learner.FundLine) && monthToCheck >= periodisationStartDateBeginningofMonth && monthToCheck <= periodisationEndDateEndofMonth)
                {
                    values[month] = learner.OnProgPayment / learnerPeriods;
                }
                else
                {
                    values[month] = 0;
                }
            }

            return new LearnerPeriodisedValues()
            {
                AttributeName = Attributes.LnrOnProgPay,
                LearnRefNumber = learner.LearnRefNumber,
                Period1 = values[0],
                Period2 = values[1],
                Period3 = values[2],
                Period4 = values[3],
                Period5 = values[4],
                Period6 = values[5],
                Period7 = values[6],
                Period8 = values[7],
                Period9 = values[8],
                Period10 = values[9],
                Period11 = values[10],
                Period12 = values[11],
            };
        }

        public bool IsLearnerTrainee(string fundLine) => fundLine == FundingLineConstants.traineeship19Plus || fundLine == FundingLineConstants.traineeship1618;
    }
}
