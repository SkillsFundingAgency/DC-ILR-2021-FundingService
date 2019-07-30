using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces
{
    public interface IPeriodisationService
    {
        IEnumerable<decimal> GetPeriodisedValues(FM25Learner learner);

        bool IsLearnerTrainee(FM25Learner learner);
    }
}
