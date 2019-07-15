using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces
{
    public interface IPeriodisationDateService
    {
        DateTime? GetPeriodisationStartDate(FM25Learner learner);
        DateTime? GetPeriodisationEndDate(FM25Learner learner, bool learnerIsTrainee);
        LearnerPeriodisedValues GetPeriodisedValues(FM25Learner learner);
    }
}
