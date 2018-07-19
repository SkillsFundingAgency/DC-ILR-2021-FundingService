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

        public async Task Execute(IEnumerable<FundingActorDto> fundingActorDtos, string outputKey)
        {
            var stopWatch = new Stopwatch();

            var tasks = fundingActorDtos.Select(a => _fundingActorProvider.Provide().Process(a)).ToList();

            await Task.WhenAll(tasks);

            _logger.LogDebug("completed Actors FM35 service");

            // get results from actor tasks
            var collatedFundingOuputputLearners = new List<LearnerAttribute>();
            var globalFundingOutput = new GlobalAttribute();
            foreach (var actorTask in tasks)
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
            await _fundingOutputPersistenceService.Process(results, outputKey);
            _logger.LogDebug($"Persisted FM35 Funding results in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}
