using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Internal
{
    public class InternalDataCache : IInternalDataCache
    {
        public IList<ILearner> ValidLearners { get; set; }
    }
}
