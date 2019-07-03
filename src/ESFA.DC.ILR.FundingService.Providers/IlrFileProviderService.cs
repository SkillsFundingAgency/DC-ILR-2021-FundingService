using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Providers
{
    public class IlrFileProviderService : IFileProviderService<IMessage>
    {
        private readonly IFileService _fileService;
        private readonly IXmlSerializationService _xmlSerializationService;

        public IlrFileProviderService(IFileService fileService, IXmlSerializationService xmlSerializationService)
        {
            _fileService = fileService;
            _xmlSerializationService = xmlSerializationService;
        }

        public async Task<IMessage> ProvideAsync(IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken)
        {
            Message message;

            using (var fileStream = await _fileService.OpenReadStreamAsync(fundingServiceContext.FileReference, fundingServiceContext.Container, cancellationToken))
            {
                message = _xmlSerializationService.Deserialize<Message>(fileStream);
            }

            return message;
        }
    }
}
