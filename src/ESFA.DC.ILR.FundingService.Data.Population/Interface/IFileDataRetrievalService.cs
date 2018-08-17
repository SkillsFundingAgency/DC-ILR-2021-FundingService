using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.File.Model;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IFileDataRetrievalService
    {
        int RetrieveUKPRN();

        IDictionary<string, IEnumerable<DPOutcome>> RetrieveDPOutcomes();
    }
}
