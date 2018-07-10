using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFundingService<out T>
    {
        T ProcessFunding(IEnumerable<ILearner> learnerList);
    }
}
