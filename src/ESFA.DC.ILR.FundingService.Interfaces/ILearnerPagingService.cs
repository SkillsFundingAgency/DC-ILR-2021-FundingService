using System.Collections.Generic;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface ILearnerPagingService
    {
        IEnumerable<IEnumerable<MessageLearner>> BuildPages(IEnumerable<int> fundModelFilter, IEnumerable<ILearner> learners);
    }
}
