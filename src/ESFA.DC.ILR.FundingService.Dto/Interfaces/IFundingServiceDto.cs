using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Dto.Interfaces
{
    public interface IFundingServiceDto
    {
        IMessage Message { get; }

        string[] ValidLearners { get; }

        string[] InvalidLearners { get; }
    }
}
