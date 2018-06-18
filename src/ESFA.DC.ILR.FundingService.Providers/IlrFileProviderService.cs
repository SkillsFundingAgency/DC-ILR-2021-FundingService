using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Providers
{
    public class IlrFileProviderService : IIlrFileProviderService
    {
        private readonly IKeyValuePersistenceService _blobStoragePersistenceService;
        private readonly IXmlSerializationService _xmlSerializationService;

        public IlrFileProviderService(IKeyValuePersistenceService blobStoragePersistenceService, IXmlSerializationService xmlSerializationService)
        {
            _blobStoragePersistenceService = blobStoragePersistenceService;
            _xmlSerializationService = xmlSerializationService;
        }

        public async Task<IMessage> Provide(string fileName)
        {
            var ilrMessageString = await _blobStoragePersistenceService.GetAsync(fileName);

            Message message = _xmlSerializationService.Deserialize<Message>(ilrMessageString);
            return message;
        }
    }
}
