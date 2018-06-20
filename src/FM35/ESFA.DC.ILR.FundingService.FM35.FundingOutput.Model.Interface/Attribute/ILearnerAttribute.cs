namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute
{
    public interface ILearnerAttribute
    {
        string LearnRefNumber { get; }

        ILearningDeliveryAttribute[] LearningDeliveryAttributes { get; }
    }
}