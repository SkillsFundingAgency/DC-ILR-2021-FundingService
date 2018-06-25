using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.OrchestrationService
{
    public class PreFundingFM35OrchestrationService : IPreFundingFM35OrchestrationService
    {
        private readonly IPopulationService _populationService;
        private readonly ILearnerPerActorService<ILearner, IList<ILearner>> _learnerPerActorService;

        public PreFundingFM35OrchestrationService(IPopulationService populationService, ILearnerPerActorService<ILearner, IList<ILearner>> learnerPerActorService)
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
