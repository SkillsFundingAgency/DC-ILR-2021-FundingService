using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.OrchestrationService
{
    public class PreFundingALBOrchestrationService : IPreFundingALBOrchestrationService
    {
        private readonly IPopulationService _populationService;
        private readonly ILearnerPerActorService<ILearner, IList<ILearner>> _learnerPerActorService;

        public PreFundingALBOrchestrationService(IPopulationService populationService, ILearnerPerActorService<ILearner, IList<ILearner>> learnerPerActorService)
        {
            _populationService = populationService;
            _learnerPerActorService = learnerPerActorService;
        }

        public IEnumerable<IList<ILearner>> Execute()
        {
            _populationService.Populate();

            return _learnerPerActorService.Process();
        }
    }
}
