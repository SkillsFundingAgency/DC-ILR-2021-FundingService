using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IFileDataRetrievalService
    {
        int RetrieveUKPRN(IMessage message);

        IDictionary<string, IEnumerable<DPOutcome>> RetrieveDPOutcomes(IMessage message);
    }
}
