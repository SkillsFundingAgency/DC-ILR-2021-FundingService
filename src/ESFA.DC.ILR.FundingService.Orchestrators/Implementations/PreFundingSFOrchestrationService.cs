using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.ILR.FundingService.ALB.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    enum SerializationTypes
    {
        Json,
        Xml
    }

    public class PreFundingSFOrchestrationService : IPreFundingSFOrchestrationService
    {
        private readonly IPreFundingALBOrchestrationService _preFundingALBOrchestrationService;
        private readonly IReferenceDataCache _referenceDataCache;
        private readonly ISerializationService _serializationService;
        private readonly ILogger _logger;

        public PreFundingSFOrchestrationService(
            IPreFundingALBOrchestrationService preFundingALBOrchestrationService,
            IReferenceDataCache referenceDataCache,
            ISerializationService serializationService,
            ILogger logger)
        {
            _preFundingALBOrchestrationService = preFundingALBOrchestrationService;
            _referenceDataCache = referenceDataCache;
            _serializationService = serializationService;
            _logger = logger;
        }

        public void Execute(IJobContextMessage jobContextMessage)
        {
            var tasks = jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks;

            // TODO: read the valid learners from Azurecosmos. Or is this already done by fundingcontext

            // loop through list of all the tasks and execute them.
//            foreach (var taskItem in tasks.Where(x => x.SupportsParallelExecution))
//            {
            // populate data
            var albValidLearnersShards = _preFundingALBOrchestrationService.Execute();
            _logger.LogDebug("completed prefunding ALB service");

            // create actors for processing
            var actorTasks = new List<Task<IEnumerable<IFundingOutputs>>>();
            var ukprn = Convert.ToInt32(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn]);
            var jobId = Convert.ToInt32(jobContextMessage.JobId);

            // loop through the shards to create actors for processing
            foreach (var albValidLearnersShard in albValidLearnersShards)
            {
                var actor = GetFundingServiceActor();

                var referenceDataInBytes =
                    Encoding.UTF8.GetBytes(_serializationService.Serialize(_referenceDataCache));
                var albValidLearnersShardInBytes =
                    Encoding.UTF8.GetBytes(_serializationService.Serialize(albValidLearnersShard));
                var albActorModel = new ALBActorModel()
                {
                    ReferenceDataCache = referenceDataInBytes,
                    Ukprn = ukprn,
                    JobId = jobId,
                    AlbValidLearners = albValidLearnersShardInBytes
                };

                actorTasks.Add(Task.Run(() => actor.Process(albActorModel)));
            }

            Task.WaitAll(actorTasks.ToArray());
            _logger.LogDebug("completed Actors ALB service");

            // get results from actor tasks
            var results = new List<IFundingOutputs>();
            foreach (var actorTask in actorTasks)
            {
                results.AddRange(actorTask.Result);
            }

            // TODO: do something with results

            // }
        }

        private IALBActor GetFundingServiceActor()
        {
            return ActorProxy.Create<IALBActor>(
                ActorId.CreateRandom(),
                new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/ALBActorService"));
        }
    }
}
