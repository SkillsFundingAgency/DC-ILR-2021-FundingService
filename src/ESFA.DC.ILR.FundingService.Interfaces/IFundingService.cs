using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFundingService<T>
    {
        T ProcessFunding(int ukprn, IList<ILearner> learnerList);
    }
}
