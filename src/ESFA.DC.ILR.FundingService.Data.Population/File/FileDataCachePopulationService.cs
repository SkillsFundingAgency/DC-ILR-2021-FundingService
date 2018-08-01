using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.File
{
    public class FileDataCachePopulationService : IFileDataCachePopulationService
    {
        private readonly IFileDataCache _fileDataCache;
        private readonly IFileDataRetrievalService _fileDataRetrievalService;

        public FileDataCachePopulationService(IFileDataCache fileDataCache, IFileDataRetrievalService ukprnDataRetrievalService)
        {
            _fileDataCache = fileDataCache;
            _fileDataRetrievalService = ukprnDataRetrievalService;
        }

        public void Populate()
        {
            var fileDataCache = (FileDataCache) _fileDataCache;

            fileDataCache.UKPRN = _fileDataRetrievalService.RetrieveUKPRN();
            fileDataCache.DPOutcomes = _fileDataRetrievalService.RetrieveDPOutcomes();
        }
    }
}
