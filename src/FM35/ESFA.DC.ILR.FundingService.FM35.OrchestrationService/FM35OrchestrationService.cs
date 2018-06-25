using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.OrchestrationService
{
    public class FM35OrchestrationService : IFM35OrchestrationService
    {
        private readonly IFundingService<IFM35FundingOutputs> _fundingService;

        public FM35OrchestrationService(IFundingService<IFM35FundingOutputs> fundingService)
        {
            _fundingService = fundingService;
        }

        public IFM35FundingOutputs Execute(int ukprn, IList<ILearner> fm35ValidLearners)
        {
            return _fundingService.ProcessFunding(ukprn, fm35ValidLearners);
        }
    }
}
