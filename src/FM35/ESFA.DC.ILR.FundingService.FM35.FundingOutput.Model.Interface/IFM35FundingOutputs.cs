using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface
{
    public interface IFM35FundingOutputs
    {
        IGlobalAttribute Global { get; }

        ILearnerAttribute[] Learners { get; }
    }
}
