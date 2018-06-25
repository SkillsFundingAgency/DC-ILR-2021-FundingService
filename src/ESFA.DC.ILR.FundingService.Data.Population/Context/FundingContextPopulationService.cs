using ESFA.DC.ILR.FundingService.Data.Context;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Context
{
    public class FundingContextPopulationService : IFundingContextPopulationService
    {
        private readonly IFundingContext _fundingContext;
        private readonly IValidLearnersDataRetrievalService _validLearnersDataRetrievalService;

        public FundingContextPopulationService(IFundingContext fundingContext, IValidLearnersDataRetrievalService validLearnersDataRetrievalService)
        {
            _fundingContext = fundingContext;
            _validLearnersDataRetrievalService = validLearnersDataRetrievalService;
        }

        public void Populate()
        {
            var fundingContext = (FundingContext)_fundingContext;

            fundingContext.ValidLearners = _validLearnersDataRetrievalService.Retrieve();
        }
    }
}
