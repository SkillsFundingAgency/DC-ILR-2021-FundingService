using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.RuleBaseTasks
{
    public class FM35OrchestrationSFTask : IFM35OrchestrationSFTask
    {
        private readonly IFundingOutputPersistenceService<FM35FundingOutputs> _fundingOutputPersistenceService;
        private readonly IExternalDataCache _referenceDataCache;
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IActorProvider<IFM35Actor> _fundingActorProvider;
        private readonly ILogger _logger;
        private readonly IPagingService<ILearner> _learnerPerActorService;

        public FM35OrchestrationSFTask(
            IFundingOutputPersistenceService<FM35FundingOutputs> fundingOutputPersistenceService,
            IExternalDataCache referenceDataCache,
            IJsonSerializationService jsonSerializationService,
            IPagingService<ILearner> learnerPerActorService,
            IActorProvider<IFM35Actor> fundingActorProvider,
            ILogger logger)
        {
            _fundingOutputPersistenceService = fundingOutputPersistenceService;
            _referenceDataCache = referenceDataCache;
            _jsonSerializationService = jsonSerializationService;
            _learnerPerActorService = learnerPerActorService;
            _fundingActorProvider = fundingActorProvider;
            _logger = logger;
        }

        public async Task Execute(IJobContextMessage jobContextMessage)
        {
            var stopWatch = new Stopwatch();

            var learnersShards = _learnerPerActorService.BuildPages().ToList();
            _logger.LogDebug("completed prefunding FM35 service");

            // create actors for processing
            var actorTasks = new List<Task<string>>();
            var ukprn = Convert.ToInt32(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn]);
            var jobId = Convert.ToInt32(jobContextMessage.JobId);

            var referenceDataInBytes = Encoding.UTF8.GetBytes(_jsonSerializationService.Serialize(_referenceDataCache));

            // loop through the shards to create actors for processing
            foreach (var learnersShard in learnersShards)
            {
                var albValidLearnersShardInBytes = Encoding.UTF8.GetBytes(_jsonSerializationService.Serialize(learnersShard));
                var actorModel = new FundingActorDto()
                {
                    ReferenceDataCache = referenceDataInBytes,
                    Ukprn = ukprn,
                    JobId = jobId,
                    ValidLearners = albValidLearnersShardInBytes
                };

                actorTasks.Add(Task.Run(() => _fundingActorProvider.Provide().Process(actorModel)));
            }

            await Task.WhenAll(actorTasks.ToArray());
            _logger.LogDebug("completed Actors FM35 service");

            // get results from actor tasks
            var collatedFundingOuputputLearners = new List<LearnerAttribute>();
            var globalFundingOutput = new GlobalAttribute();
            foreach (var actorTask in actorTasks)
            {
                var fundingOutputs = _jsonSerializationService.Deserialize<FM35FundingOutputs>(actorTask.Result);
                collatedFundingOuputputLearners.AddRange(fundingOutputs.Learners);
            }

            var results = new FM35FundingOutputs()
            {
                Global = globalFundingOutput,
                Learners = collatedFundingOuputputLearners.ToArray(),
            };

            stopWatch.Start();

            // persis results
            await _fundingOutputPersistenceService.Process(
                results,
                jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput].ToString());
            _logger.LogDebug($"Persisted FM35 Funding results in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}
