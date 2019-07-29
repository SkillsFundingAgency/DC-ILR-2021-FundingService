using ESFA.DC.ILR.FundingService.FM25.Model.Output;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces
{
    public interface IPeriodisationService
    {
        decimal[] GetPeriodisedValues(FM25Learner learner);

        bool IsLearnerTrainee(string fundLine);

        decimal[] GetMonthlyValues();
    }
}
