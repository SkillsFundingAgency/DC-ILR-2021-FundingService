using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface ILargeEmployersDataRetrievalService
    {
        IEnumerable<int> UniqueEmployerIds(IMessage message);

        IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployersForEmployerIds(IEnumerable<int> employerIds);
    }
}
