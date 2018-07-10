using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model
{
    public class FM35FundingOutputs
    {
        public GlobalAttribute Global { get; set; }

        public LearnerAttribute[] Learners { get; set; }
    }
}
