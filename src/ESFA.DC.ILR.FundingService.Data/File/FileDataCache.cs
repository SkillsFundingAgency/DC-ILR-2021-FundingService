using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.File
{
    public class FileDataCache : IFileDataCache
    {
        public int UKPRN { get; set; }

        public IDictionary<string, IEnumerable<DPOutcome>> DPOutcomes { get; set; }
    }
}
