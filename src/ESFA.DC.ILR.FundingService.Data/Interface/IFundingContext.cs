using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Interface
{
    public interface IFundingContext
    {
        IEnumerable<ILearner> ValidLearners { get; }
    }
}
