using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.FundingActor.Tasks
{
    public class FM25ActorTask : IFundingTask
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly IActorProvider<IFM25Actor> _fundingActorProvider;
        private readonly IFilePersistanceService _filePersistanceService;
        private readonly IFundingOutputCondenserService<FM25Global> _fundingOutputCondenserService;
        private readonly string _actorName;

        public FM25ActorTask(
            IJsonSerializationService jsonSerializationService,
            IActorProvider<IFM25Actor> fundingActorProvider,
            IFilePersistanceService filePersistanceService,
            IFundingOutputCondenserService<FM25Global> fundingOutputCondenserService,
            ILogger logger,
            string actorName)
        {
            _jsonSerializationService = jsonSerializationService;
            _fundingActorProvider = fundingActorProvider;
            _filePersistanceService = filePersistanceService;
            _fundingOutputCondenserService = fundingOutputCondenserService;
            _logger = logger;
            _actorName = actorName;
        }

        public async Task Execute(IEnumerable<FundingDto> fundingActorDtos, IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting {_actorName} Actors");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Task<string>> taskList = new List<Task<string>>();
            List<IFM25Actor> actors = new List<IFM25Actor>();

            foreach (FundingDto fundingActorDto in fundingActorDtos)
            {
                IFM25Actor actor = _fundingActorProvider.Provide();
                actors.Add(actor);
                taskList.Add(actor.Process(fundingActorDto, cancellationToken));
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);

            IEnumerable<FM25Global> results = taskList.Select(t => _jsonSerializationService.Deserialize<FM25Global>(t.Result)) ?? Enumerable.Empty<FM25Global>();

            _logger.LogDebug($"Completed {taskList.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            List<Task> tasksDestroy = new List<Task>();
            foreach (IFM25Actor actor in actors)
            {
                tasksDestroy.Add(_fundingActorProvider.DestroyAsync(actor, cancellationToken));
            }

            await Task.WhenAll(tasksDestroy).ConfigureAwait(false);

            _logger.LogDebug($"Destroyed {taskList.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();

            var output = _fundingOutputCondenserService.Condense(results, fundingServiceContext.Ukprn, fundingServiceContext.Year);

            await _filePersistanceService.PersistAsync(fundingServiceContext.FundingFm25OutputKey, fundingServiceContext.Container, output, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Persisted {_actorName} results - {stopWatch.ElapsedMilliseconds}");
        }
    }
}
