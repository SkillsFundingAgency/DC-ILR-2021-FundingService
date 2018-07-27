using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

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

            var taskList = new List<Task<string>>();

            foreach (var fundingActorDto in fundingActorDtos)
            {
                var task = _fundingActorProvider.Provide().Process(fundingActorDto);
                taskList.Add(task);
            }

            await Task.WhenAll(taskList);

            var results = taskList.Select(t => _jsonSerializationService.Deserialize<TActorReturn>(t.Result));

            _logger.LogDebug($"Completed {taskList.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();

            await _keyValuePersistenceService.SaveAsync(outputKey, _jsonSerializationService.Serialize(Condense(results)));

            _logger.LogDebug($"Persisted {_actorName} results - {stopWatch.ElapsedMilliseconds}");
        }

        public abstract TActorReturn Condense(IEnumerable<TActorReturn> inputs);
    }
}
