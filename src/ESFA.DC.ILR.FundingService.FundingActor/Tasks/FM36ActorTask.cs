using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.FundingActor.Tasks
{
    public class FM36ActorTask : IFundingTask
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly IActorProvider<IFM36Actor> _fundingActorProvider;
        private readonly IFilePersistanceService _filePersistanceService;
        private readonly IFundingOutputCondenserService<FM36Global> _fundingOutputCondenserService;
        private readonly string _actorName;

        public FM36ActorTask(
            IJsonSerializationService jsonSerializationService,
            IActorProvider<IFM36Actor> fundingActorProvider,
            IFilePersistanceService filePersistanceService,
            IFundingOutputCondenserService<FM36Global> fundingOutputCondenserService,
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
            List<IFM36Actor> actors = new List<IFM36Actor>();

            foreach (FundingDto fundingActorDto in fundingActorDtos)
            {
                IFM36Actor actor = _fundingActorProvider.Provide();
                actors.Add(actor);
                taskList.Add(actor.Process(fundingActorDto, cancellationToken));
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);

            IEnumerable<FM36Global> results = taskList.Select(t => _jsonSerializationService.Deserialize<FM36Global>(t.Result));

            _logger.LogDebug($"Completed {taskList.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            List<Task> tasksDestroy = new List<Task>();
            foreach (IFM36Actor actor in actors)
            {
                tasksDestroy.Add(_fundingActorProvider.DestroyAsync(actor, cancellationToken));
            }

            await Task.WhenAll(tasksDestroy).ConfigureAwait(false);

            _logger.LogDebug($"Destroyed {taskList.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();

            var output = _fundingOutputCondenserService.Condense(results, fundingServiceContext.Ukprn, fundingServiceContext.Year);

            await _filePersistanceService.PersistAsync(fundingServiceContext.FundingFm36OutputKey, fundingServiceContext.Container, output, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Persisted {_actorName} results - {stopWatch.ElapsedMilliseconds}");
        }
    }
}
