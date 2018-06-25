using ESFA.DC.ILR.FundingService.Data.Context;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using System.Collections.Generic;
using System.Xml.Schema;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Context
{
    public class FundingContextPopulationService : IFundingContextPopulationService
    {
        private readonly IFundingContext _fundingContext;
        private readonly IUKPRNDataRetrievalService _ukprnDataRetrievalService;
        private readonly IValidLearnersDataRetrievalService _validLearnersDataRetrievalService;

        public FundingContextPopulationService(IFundingContext fundingContext, IUKPRNDataRetrievalService ukrpnDataRetrievalService, IValidLearnersDataRetrievalService validLearnersDataRetrievalService)
        {
            _fundingContext = fundingContext;
            _ukprnDataRetrievalService = ukrpnDataRetrievalService;
            _validLearnersDataRetrievalService = validLearnersDataRetrievalService;
        }

        public void Populate()
        {
            var fundingContext = (FundingContext)_fundingContext;

            fundingContext.UKPRN = _ukprnDataRetrievalService.Retrieve();
            fundingContext.ValidLearners = _validLearnersDataRetrievalService.Retrieve();
        }
    }
}
