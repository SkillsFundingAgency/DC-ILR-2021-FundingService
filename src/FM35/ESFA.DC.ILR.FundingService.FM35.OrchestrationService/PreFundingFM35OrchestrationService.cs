using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.OrchestrationService
{
    public class PreFundingFM35OrchestrationService : IPreFundingFM35OrchestrationService
    {
        private readonly IPreFundingFM35PopulationService _preFundingOrchestrationService;
        private readonly ILearnerPerActorService<ILearner, IList<ILearner>> _learnerPerActorService;

        public PreFundingFM35OrchestrationService(IPreFundingFM35PopulationService preFundingOrchestrationService, ILearnerPerActorService<ILearner, IList<ILearner>> learnerPerActorService)
        {
            _preFundingOrchestrationService = preFundingOrchestrationService;
            _learnerPerActorService = learnerPerActorService;
        }

        public IEnumerable<IList<ILearner>> Execute()
        {
            _preFundingOrchestrationService.PopulateData();

            return _learnerPerActorService.Process();
        }
    }
}
