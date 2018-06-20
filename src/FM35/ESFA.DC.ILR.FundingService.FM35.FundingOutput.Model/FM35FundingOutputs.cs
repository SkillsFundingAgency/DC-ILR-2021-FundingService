using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model
{
    public class FM35FundingOutputs : IFM35FundingOutputs
    {
        public IGlobalAttribute Global { get; set; }

        public ILearnerAttribute[] Learners { get; set; }
    }
}
