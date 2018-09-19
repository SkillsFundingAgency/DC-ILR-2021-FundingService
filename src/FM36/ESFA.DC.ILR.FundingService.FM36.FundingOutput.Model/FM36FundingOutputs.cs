using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Attribute;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model
{
    public class FM36FundingOutputs
    {
        public GlobalAttribute Global { get; set; }

        public LearnerAttribute[] Learners { get; set; }
    }
}
