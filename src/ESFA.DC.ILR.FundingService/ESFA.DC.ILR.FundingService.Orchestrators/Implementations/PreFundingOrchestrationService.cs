using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class PreFundingOrchestrationService
    {
        private readonly IPreFundingALBOrchestrationService _preFundingALBOrchestrationService;
        private readonly ILogger _logger;

        public PreFundingOrchestrationService(
            IPreFundingALBOrchestrationService preFundingALBOrchestrationService,
            ILogger logger)
        {
            _preFundingALBOrchestrationService = preFundingALBOrchestrationService;
            _logger = logger;
        }

        public void Execute(IJobContextMessage jobContextMessage)
        {
            var tasks = jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks;

            // loop through list of all the tasks and execute them.
            foreach (var taskItem in tasks.Where(x => x.SupportsParallelExecution))
            {
                // populate data
                var albValidLearnersShards = _preFundingALBOrchestrationService.Execute();

                // create actors for processing
                var actorTasks = new List<Task<string>>();
                foreach (var albValidLearnersShard in albValidLearnersShards)
                {
                    var actor = GetFundingServiceActor();
                }

                // get results

                // do something with results
            }
        }

        private object GetFundingServiceActor()
        {
            return null;
        }
    }
}
