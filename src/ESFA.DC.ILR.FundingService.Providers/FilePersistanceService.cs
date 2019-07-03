using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Providers
{
    public class FilePersistanceService : IFilePersistanceService
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileService _fileService;

        public FilePersistanceService(IJsonSerializationService jsonSerializationService, IFileService fileService)
        {
            _jsonSerializationService = jsonSerializationService;
            _fileService = fileService;
        }

        public async Task PersistAsync<T>(string fileReference, string container, T output, CancellationToken cancellationToken)
        {
            using (var fileStream = await _fileService.OpenWriteStreamAsync(fileReference, container, cancellationToken))
            {
                _jsonSerializationService.Serialize(output, fileStream);
            }
        }
    }
}
