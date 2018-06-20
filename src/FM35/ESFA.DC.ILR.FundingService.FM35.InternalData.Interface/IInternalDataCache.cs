using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.InternalData.Interface
{
    public interface IInternalDataCache
    {
        int UKPRN { get; }

        IList<ILearner> ValidLearners { get; }
    }
}
