using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.File
{
    public class FileDataService : IFileDataService
    {
        private readonly IFileDataCache _fileDataCache;

        private readonly IEnumerable<DPOutcome> _emptyDpOutcomes = new List<DPOutcome>();

        public FileDataService(IFileDataCache fileDataCache)
        {
            _fileDataCache = fileDataCache;
        }

        public int UKPRN()
        {
            return _fileDataCache.UKPRN;
        }

        public IEnumerable<DPOutcome> DPOutcomesForLearnRefNumber(string learnRefNumber)
        {
            _fileDataCache.DPOutcomes.TryGetValue(learnRefNumber, out var dpOutcomes);

            return dpOutcomes ?? _emptyDpOutcomes;
        }
    }
}
