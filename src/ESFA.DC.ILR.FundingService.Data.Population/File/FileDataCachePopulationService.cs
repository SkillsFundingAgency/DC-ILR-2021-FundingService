using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.File
{
    public class FileDataCachePopulationService : IFileDataCachePopulationService
    {
        private readonly IFileDataCache _fileDataCache;
        private readonly IUKPRNDataRetrievalService _ukprnDataRetrievalService;

        public FileDataCachePopulationService(IFileDataCache fileDataCache, IUKPRNDataRetrievalService ukprnDataRetrievalService)
        {
            _fileDataCache = fileDataCache;
            _ukprnDataRetrievalService = ukprnDataRetrievalService;
        }

        public void Populate()
        {
            var fileDataCache = (FileDataCache) _fileDataCache;

            fileDataCache.UKPRN = _ukprnDataRetrievalService.Retrieve();
        }
    }
}
