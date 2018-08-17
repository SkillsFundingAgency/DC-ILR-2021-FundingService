using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.File.Model;

namespace ESFA.DC.ILR.FundingService.Data.File.Interface
{
    public interface IFileDataService
    {
        int UKPRN();

        IEnumerable<DPOutcome> DPOutcomesForLearnRefNumber(string learnRefNumber);
    }
}
