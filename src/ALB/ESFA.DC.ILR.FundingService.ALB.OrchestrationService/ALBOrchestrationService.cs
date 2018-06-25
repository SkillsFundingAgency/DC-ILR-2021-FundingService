using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.OrchestrationService
{
    public class ALBOrchestrationService : IALBOrchestrationService
    {
        private readonly IFundingService<IFundingOutputs> _fundingService;

        public ALBOrchestrationService(IFundingService<IFundingOutputs> fundingService)
        {
            _fundingService = fundingService;
        }

        public IFundingOutputs Execute(int ukprn, IList<ILearner> albValidLearners)
        {
            return _fundingService.ProcessFunding(ukprn, albValidLearners);
        }
    }
}