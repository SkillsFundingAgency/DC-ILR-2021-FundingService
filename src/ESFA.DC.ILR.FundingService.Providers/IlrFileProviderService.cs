using System;
using System.IO;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Providers
{
    public class IlrFileProviderService : IIlrFileProviderService
    {
        private readonly IStreamableKeyValuePersistenceService _blobStoragePersistenceService;
        private readonly IXmlSerializationService _xmlSerializationService;
        private readonly ILogger _logger;

        public IlrFileProviderService(IStreamableKeyValuePersistenceService blobStoragePersistenceService, IXmlSerializationService xmlSerializationService, ILogger logger)
        {
            _blobStoragePersistenceService = blobStoragePersistenceService;
            _xmlSerializationService = xmlSerializationService;
            _logger = logger;
        }

        public async Task<IMessage> Provide(string fileName)
        {
            Message message = null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await _blobStoragePersistenceService.GetAsync(fileName, ms).ConfigureAwait(false);
                    ms.Seek(0, SeekOrigin.Begin);
                    message = _xmlSerializationService.Deserialize<Message>(ms);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve and deserialise ILR file {fileName}", ex);
                throw;
            }

            return message;
        }
    }
}
