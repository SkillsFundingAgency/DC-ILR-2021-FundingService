using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFundingOutputCondenserService<T>
    {
        T Condense(IEnumerable<T> fundingOutputs);
    }
}
