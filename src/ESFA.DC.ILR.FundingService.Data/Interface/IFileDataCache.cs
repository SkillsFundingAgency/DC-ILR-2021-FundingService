using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.File.Model;

namespace ESFA.DC.ILR.FundingService.Data.Interface
{
    public interface IFileDataCache
    {
        int UKPRN { get; }

        IDictionary<string, IEnumerable<DPOutcome>> DPOutcomes { get; }
    }
}
