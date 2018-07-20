using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Abstract
{
    public abstract class AbstractActorTask<TActor, TActorReturn>
        where TActor : IFundingActor
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly IActorProvider<TActor> _fundingActorProvider;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly string _actorName;

        public AbstractActorTask(
            IJsonSerializationService jsonSerializationService,
            IActorProvider<TActor> fundingActorProvider,
            IKeyValuePersistenceService keyValuePersistenceService,
            ILogger logger,
            string actorName)
        {
            _jsonSerializationService = jsonSerializationService;
            _fundingActorProvider = fundingActorProvider;
            _keyValuePersistenceService = keyValuePersistenceService;
            _logger = logger;
            _actorName = actorName;
        }

        public async Task Execute(IEnumerable<FundingActorDto> fundingActorDtos, string outputKey)
        {
            _logger.LogDebug($"Starting {_actorName} Actors");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var tasks = fundingActorDtos.Select(a => _fundingActorProvider.Provide().Process(a)).ToList();

            await Task.WhenAll(tasks);

            _logger.LogDebug($"Completed {tasks.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();

            var results = tasks.Select(t => t.Result).Select(r => _jsonSerializationService.Deserialize<TActorReturn>(r));

            await _keyValuePersistenceService.SaveAsync(outputKey, _jsonSerializationService.Serialize(Condense(results)));

            _logger.LogDebug($"Persisted {_actorName} results - {stopWatch.ElapsedMilliseconds}");
        }

        public abstract TActorReturn Condense(IEnumerable<TActorReturn> inputs);
    }
}
