using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.ILR.FundingService.ALB.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface.Attribute;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.ALB.Stubs.Persistance.Model.Output;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class PreFundingSFOrchestrationService : IPreFundingSFOrchestrationService
    {
        private readonly IPreFundingALBOrchestrationService _preFundingALBOrchestrationService;
        private readonly IReferenceDataCache _referenceDataCache;
        private readonly ISerializationService _jsonSerializationService;
        private readonly IIlrFileProviderService _ilrFileProviderService;
        private readonly IFundingServiceDto _fundingServiceDto;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IFundingOutputPersistenceService<IFundingOutputs> _fundingOutputPersistenceService;
        private readonly ILogger _logger;

        public PreFundingSFOrchestrationService(
            IPreFundingALBOrchestrationService preFundingALBOrchestrationService,
            IReferenceDataCache referenceDataCache,
            IJsonSerializationService jsonSerializationService,
            IIlrFileProviderService ilrFileProviderService,
            IFundingServiceDto fundingServiceDto,
            IKeyValuePersistenceService keyValuePersistenceService,
            IFundingOutputPersistenceService<IFundingOutputs> fundingOutputPersistenceService,
            ILogger logger)
        {
            _preFundingALBOrchestrationService = preFundingALBOrchestrationService;
            _referenceDataCache = referenceDataCache;
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _fundingServiceDto = fundingServiceDto;
            _keyValuePersistenceService = keyValuePersistenceService;
            _fundingOutputPersistenceService = fundingOutputPersistenceService;
            _logger = logger;
        }

        public async Task Execute(IJobContextMessage jobContextMessage)
        {
            var stopWatch = new Stopwatch();
            var tasks = jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks;

            // Get the ilr object from file
            var ilrMessage = await _ilrFileProviderService.Provide(jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString());
            var fundingServiceDto = (FundingServiceDto)_fundingServiceDto;
            fundingServiceDto.Message = ilrMessage;

            // get valid learners from intermediate storage and store it in the dto for rulebases
            fundingServiceDto.ValidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.ValidLearnRefNumbers].ToString()));

            // loop through list of all the tasks and execute them.
//            foreach (var taskItem in tasks.Where(x => x.SupportsParallelExecution))
//            {
            // populate data
            var albValidLearnersShards = _preFundingALBOrchestrationService.Execute();
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
            var collatedFundingOuputputLearners = new List<ILearnerAttribute>();
            var globalFundingOutput = new GlobalAttribute();
            foreach (var actorTask in actorTasks)
            {
                IFundingOutputs fundingOutputs =
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

            // }
            _logger.LogDebug($"Persisted Funding results in: {stopWatch.ElapsedMilliseconds}");
        }

        private IALBActor GetFundingServiceActor()
        {
            return ActorProxy.Create<IALBActor>(
                ActorId.CreateRandom(),
                new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/ALBActorService"));
        }
    }
}
