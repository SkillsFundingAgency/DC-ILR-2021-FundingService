using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;

namespace ESFA.DC.ILR.FundingService.Data.Population.File
{
    public class UKPRNDataRetrievalService : IUKPRNDataRetrievalService
    {
        private readonly IFundingServiceDto _fundingServiceDto;

        public UKPRNDataRetrievalService(IFundingServiceDto fundingServiceDto)
        {
            _fundingServiceDto = fundingServiceDto;
        }

        public int Retrieve()
        {
            return _fundingServiceDto.Message.LearningProviderEntity.UKPRN;
        }
    }
}
