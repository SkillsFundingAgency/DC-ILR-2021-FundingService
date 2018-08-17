using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IValidLearnersDataRetrievalService
    {
        IEnumerable<ILearner> Retrieve();
    }
}
