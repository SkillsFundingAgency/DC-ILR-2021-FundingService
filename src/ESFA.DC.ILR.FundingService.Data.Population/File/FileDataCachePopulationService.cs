using System.Threading;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.FundingService.Data.Population.File
{
    public class FileDataCachePopulationService : IFileDataCachePopulationService
    {
        private readonly IFileDataRetrievalService _fileDataRetrievalService;

        public FileDataCachePopulationService(IFileDataRetrievalService fileDataRetrievalService)
        {
            _fileDataRetrievalService = fileDataRetrievalService;
        }

        public IFileDataCache PopulateAsync(IMessage message, CancellationToken cancellationToken)
        {
            return new FileDataCache
            {
                UKPRN = _fileDataRetrievalService.RetrieveUKPRN(message),
                DPOutcomes = _fileDataRetrievalService.RetrieveDPOutcomes(message),
            };
        }
    }
}
