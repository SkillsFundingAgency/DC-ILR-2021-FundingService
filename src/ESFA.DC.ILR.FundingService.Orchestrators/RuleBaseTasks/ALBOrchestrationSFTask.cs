using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ESFA.DC.ILR.FundingService.Orchestrators.RuleBaseTasks
{
    public class ALBOrchestrationSFTask : IALBOrchestrationSFTask
    {
        private readonly IFundingOutputPersistenceService<FundingOutputs> _fundingOutputPersistenceService;
        private readonly IExternalDataCache _referenceDataCache;
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly ILearnerPerActorService<ILearner, IList<ILearner>> _learnerPerActorService;
        private readonly IPopulationService _populationService;

        public ALBOrchestrationSFTask(
            IFundingOutputPersistenceService<FundingOutputs> fundingOutputPersistenceService,
            IExternalDataCache referenceDataCache,
            IJsonSerializationService jsonSerializationService,
            ILearnerPerActorService<ILearner, IList<ILearner>> learnerPerActorService,
            IPopulationService populationService,
            ILogger logger)
        {
            _fundingOutputPersistenceService = fundingOutputPersistenceService;
            _referenceDataCache = referenceDataCache;
            _jsonSerializationService = jsonSerializationService;
            _populationService = populationService;
            _learnerPerActorService = learnerPerActorService;
            _logger = logger;
        }

        public async Task Execute(IJobContextMessage jobContextMessage)
        {
            var stopWatch = new Stopwatch();

            _populationService.Populate();

            var albValidLearnersShards = _learnerPerActorService.Process();
            _logger.LogDebug("completed prefunding ALB service");

            // create actors for processing
            var actorTasks = new List<Task<string>>();
            var ukprn = Convert.ToInt32(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn]);
            var jobId = Convert.ToInt32(jobContextMessage.JobId);

            // loop through the shards to create actors for processing
            foreach (var albValidLearnersShard in albValidLearnersShards)
            {
                var actor = GetFundingServiceActor();

                var referenceDataInBytes =
                    Encoding.UTF8.GetBytes(_jsonSerializationService.Serialize(_referenceDataCache));
                var albValidLearnersShardInBytes =
                    Encoding.UTF8.GetBytes(_jsonSerializationService.Serialize(albValidLearnersShard));
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
            var collatedFundingOuputputLearners = new List<LearnerAttribute>();
            var globalFundingOutput = new GlobalAttribute();
            foreach (var actorTask in actorTasks)
            {
                FundingOutputs fundingOutputs =
                    _jsonSerializationService.Deserialize<FundingOutputs>(actorTask.Result);
                collatedFundingOuputputLearners.AddRange(fundingOutputs.Learners);
            }

            var results = new FundingOutputs()
            {
                Global = globalFundingOutput,
                Learners = collatedFundingOuputputLearners.ToArray(),
            };

            stopWatch.Start();

            // persis results
            await _fundingOutputPersistenceService.Process(
                results,
                jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput].ToString());
            _logger.LogDebug($"Persisted ALB Funding results in: {stopWatch.ElapsedMilliseconds}");
        }

        private IALBActor GetFundingServiceActor()
        {
            return ActorProxy.Create<IALBActor>(
                ActorId.CreateRandom(),
                new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/ALBActorService"));
        }
    }
}
