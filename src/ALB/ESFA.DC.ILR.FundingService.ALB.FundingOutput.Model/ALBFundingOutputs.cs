using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Attribute;

namespace ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model
{
    public class ALBFundingOutputs
    {
        public GlobalAttribute Global { get; set; }

        public LearnerAttribute[] Learners { get; set; }
    }
}
