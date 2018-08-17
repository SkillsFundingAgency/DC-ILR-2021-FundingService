using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;

namespace ESFA.DC.ILR.FundingService.Data.Population.File
{
    public class FileDataRetrievalService : IFileDataRetrievalService
    {
        private readonly IFundingServiceDto _fundingServiceDto;

        public FileDataRetrievalService(IFundingServiceDto fundingServiceDto)
        {
            _fundingServiceDto = fundingServiceDto;
        }

        public IDictionary<string, IEnumerable<DPOutcome>> RetrieveDPOutcomes()
        {
            return _fundingServiceDto
                .Message?
                .LearnerDestinationAndProgressions?
                .ToDictionary(
                    l => l.LearnRefNumber,
                    l => l.DPOutcomes?
                        .Select(d => new DPOutcome()
                        {
                            OutCode = d.OutCode,
                            OutType = d.OutType,
                        }).ToList() ?? new List<DPOutcome>() as IEnumerable<DPOutcome>)
                ?? new Dictionary<string, IEnumerable<DPOutcome>>();
        }

        public int RetrieveUKPRN()
        {
            return _fundingServiceDto.Message.LearningProviderEntity.UKPRN;
        }
    }
}
