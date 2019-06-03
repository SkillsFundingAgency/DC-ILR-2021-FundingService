using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface ILearnerPagingService
    {
        IEnumerable<IEnumerable<ILearner>> BuildPages(IEnumerable<int> fundModelFilter, IEnumerable<ILearner> learners);
    }
}
