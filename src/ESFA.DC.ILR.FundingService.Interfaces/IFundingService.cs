using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFundingService<in TIn, out TOut>
    {
        TOut ProcessFunding(IEnumerable<TIn> inputList);
    }
}
