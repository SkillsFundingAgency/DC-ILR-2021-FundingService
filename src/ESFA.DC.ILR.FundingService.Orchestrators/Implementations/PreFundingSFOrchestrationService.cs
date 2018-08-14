using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class PreFundingSFOrchestrationService : IPreFundingSFOrchestrationService
    {
        private readonly ISerializationService _jsonSerializationService;
        private readonly IIlrFileProviderService _ilrFileProviderService;
        private readonly IFundingServiceDto _fundingServiceDto;
        private readonly IPopulationService _populationService;
        private readonly IActorTask<IALBActor, FundingOutputs> _albActorTask;
        private readonly IActorTask<IFM35Actor, FM35FundingOutputs> _fm35ActorTask;
        private readonly IActorTask<IFM25Actor, Global> _fm25ActorTask;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IPagingService<ILearner> _learnerPagingService;
        private readonly IExternalDataCache _externalDataCache;
        private readonly IFileDataCache _fileDataCache;
        private readonly ILogger _logger;

        public PreFundingSFOrchestrationService(
            IJsonSerializationService jsonSerializationService,
            IIlrFileProviderService ilrFileProviderService,
            IFundingServiceDto fundingServiceDto,
            IPopulationService populationService,
            IActorTask<IALBActor, FundingOutputs> albActorTask,
            IActorTask<IFM35Actor, FM35FundingOutputs> fm35ActorTask,
            IActorTask<IFM25Actor, Global> fm25ActorTask,
            IKeyValuePersistenceService keyValuePersistenceService,
            IPagingService<ILearner> learnerPagingService,
            IExternalDataCache externalDataCache,
            IFileDataCache fileDataCache,
            ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _fundingServiceDto = fundingServiceDto;
            _populationService = populationService;
            _albActorTask = albActorTask;
            _fm35ActorTask = fm35ActorTask;
            _fm25ActorTask = fm25ActorTask;
            _keyValuePersistenceService = keyValuePersistenceService;
            _externalDataCache = externalDataCache;
            _fileDataCache = fileDataCache;
            _learnerPagingService = learnerPagingService;
            _logger = logger;
        }

        public async Task Execute(IJobContextMessage jobContextMessage)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get the ilr object from file
            var ilrMessage = await _ilrFileProviderService.Provide(jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString());
            var fundingServiceDto = (FundingServiceDto)_fundingServiceDto;
            fundingServiceDto.Message = ilrMessage;

            // get valid learners from intermediate storage and store it in the dto for rulebases
            fundingServiceDto.ValidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.ValidLearnRefNumbers].ToString()));

            _populationService.Populate();

            var ukprn = Convert.ToInt32(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn]);
            var jobId = Convert.ToInt32(jobContextMessage.JobId);

            var externalDataCache = _jsonSerializationService.Serialize(_externalDataCache);
            var fileDataCache = _jsonSerializationService.Serialize(_fileDataCache);

            var fundingActorDtos = _learnerPagingService
                .BuildPages()
                .Select(p =>
                    new FundingActorDto()
                    {
                        JobId = jobId,
                        Ukprn = ukprn,
                        ExternalDataCache = externalDataCache,
                        FileDataCache = fileDataCache,
                        ValidLearners = _jsonSerializationService.Serialize(p)
                    }).ToList();

            var fundingTasks = new List<Task>()
            {
                _albActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput].ToString()),
                _fm35ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm35Output].ToString()),
                _fm25ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm25Output].ToString()),
            };

            // execute all fundingtasks
            await Task.WhenAll(fundingTasks);

            _logger.LogDebug($"Completed Funding Service for given Rulebases in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}
