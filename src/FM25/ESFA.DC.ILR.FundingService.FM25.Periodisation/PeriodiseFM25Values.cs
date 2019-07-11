using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class PeriodiseFM25Values
    {
        DateTime academicYearStart = new DateTime(2019, 08, 01);
        DateTime academicYearEnd = new DateTime(2020, 07, 31);
        bool lnrIsTrainee;
        DateTime? periodisationStartDate;
        DateTime? periodisationEndDate;
        int periodsInLearning;
        public List<PeriodisationGlobal> PeriodisationProcess(int UKPRN, IEnumerable<FM25Global> fm25Results)
        {

            foreach (var result in fm25Results)
            {
                foreach (var learner in result.Learners)
                {
                    lnrIsTrainee = IsLearnerTrainee(learner.FundLine);
                    periodisationStartDate = GetPeriodisationStartDate(learner.LearnerStartDate);
                    periodisationEndDate = GetPeriodisationEndDate(learner.LearnerPlanEndDate, learner.LearnerActEndDate, lnrIsTrainee);
                    periodsInLearning = GetPeriodsInLearning(periodisationStartDate, periodisationEndDate);
                }
            }

            var output = new List<PeriodisationGlobal>();
            var test = new PeriodisationGlobal();
            //test.LearnerPeriodisedValues
            return new List<PeriodisationGlobal>();
        }

        private bool IsLearnerTrainee(string fundLine)
        {
            if(fundLine== "19+ Traineeships (Adult Funded)" || fundLine == "16-18 Traineeships (Adult Funded)")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        private DateTime? GetPeriodisationStartDate(DateTime? learnerStartDate)
        {
            return (learnerStartDate > academicYearStart) ? learnerStartDate : academicYearStart;
        }

        private DateTime? GetPeriodisationEndDate(DateTime? learnerPlannedEndDate, DateTime? learnerActEndDate, bool lnrIsTrainee)
        {
            if (lnrIsTrainee && learnerPlannedEndDate > academicYearStart && learnerActEndDate != null)
            {
                return new[] { learnerPlannedEndDate, learnerActEndDate, academicYearEnd }.Min();
            }
            else if (lnrIsTrainee && learnerPlannedEndDate > academicYearStart && learnerActEndDate == null)
            {
                return new[] { learnerPlannedEndDate, academicYearEnd }.Min();
            }
            else if (lnrIsTrainee && learnerPlannedEndDate < academicYearStart && learnerActEndDate != null)
            {
                return learnerActEndDate;
            }
            else if (lnrIsTrainee && learnerPlannedEndDate < academicYearStart && learnerActEndDate == null)
            {
                return academicYearEnd;
            }
            else
            {
                return academicYearEnd;
            }
        }

        private int GetPeriodsInLearning(DateTime? periodisationStartDate, DateTime? periodisationEndDate)
        {
            return (periodisationEndDate.Value.Year - periodisationStartDate.Value.Year - 1) * 12 + periodisationEndDate.Value.Month - periodisationStartDate.Value.Month + 1;
        }


    }
}
