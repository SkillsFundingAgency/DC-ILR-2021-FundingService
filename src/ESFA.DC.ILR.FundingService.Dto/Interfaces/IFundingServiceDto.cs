using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.FundingService.Dto.Interfaces
{
    public interface IFundingServiceDto
    {
        IMessage Message { get; }

        string[] ValidLearners { get; }

        string[] InvalidLearners { get; }

        ReferenceDataRoot ReferenceData { get; }
    }
}
