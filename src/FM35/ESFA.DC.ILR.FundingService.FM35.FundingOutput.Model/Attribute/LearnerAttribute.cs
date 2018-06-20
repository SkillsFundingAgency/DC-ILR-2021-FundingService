using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute
{
    public class LearnerAttribute : ILearnerAttribute
    {
        public string LearnRefNumber { get; set; }

        public ILearningDeliveryAttribute[] LearningDeliveryAttributes { get; set; }
    }
}
