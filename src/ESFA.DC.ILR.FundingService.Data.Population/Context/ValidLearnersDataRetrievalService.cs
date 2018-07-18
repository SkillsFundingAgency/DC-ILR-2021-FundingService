using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Context
{
    public class ValidLearnersDataRetrievalService : IValidLearnersDataRetrievalService
    {
        private readonly IFundingServiceDto _fundingServiceDto;

        public ValidLearnersDataRetrievalService(IFundingServiceDto fundingServiceDto)
        {
            _fundingServiceDto = fundingServiceDto;
        }

        public IEnumerable<ILearner> Retrieve()
        {
            return _fundingServiceDto.Message.Learners;//.Where(learner => _fundingServiceDto.ValidLearners.Contains(learner.LearnRefNumber));
        }
    }
}
