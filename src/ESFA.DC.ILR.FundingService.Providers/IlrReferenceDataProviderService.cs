using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Providers
{
    public class IlrReferenceDataProviderService : IFileProviderService<ReferenceDataRoot>
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public IlrReferenceDataProviderService(IFileService fileService, IJsonSerializationService jsonSerializationService)
        {
            _fileService = fileService;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<ReferenceDataRoot> ProvideAsync(IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken)
        {
            ReferenceDataRoot referenceData;

            using (var fileStream = await _fileService.OpenReadStreamAsync(fundingServiceContext.IlrReferenceDataKey, fundingServiceContext.Container, cancellationToken))
            {
                referenceData = _jsonSerializationService.Deserialize<ReferenceDataRoot>(fileStream);
            }

            return referenceData;
        }
    }
}
