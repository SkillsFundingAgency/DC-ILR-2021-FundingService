using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Interface
{
    public interface IInternalDataCache
    {
        int UKPRN { get; }

        IList<ILearner> ValidLearners { get; }
    }
}
