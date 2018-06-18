using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Dto.Interfaces
{
    public interface IFundingServiceDto
    {
        IMessage Message { get; }

        string[] ValidLearners { get; }
    }
}
