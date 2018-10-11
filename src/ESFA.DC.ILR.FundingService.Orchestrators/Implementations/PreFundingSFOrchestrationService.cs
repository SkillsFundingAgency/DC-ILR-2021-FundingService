using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Config.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Model.Interface;
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
        private readonly IActorTask<IALBActor, ALBGlobal> _albActorTask;
        private readonly IActorTask<IFM70Actor, FM70Global> _fm70ActorTask;
        private readonly IActorTask<IFM35Actor, FM35Global> _fm35ActorTask;
        private readonly IActorTask<IFM36Actor, FM36Global> _fm36ActorTask;
        private readonly IActorTask<IFM25Actor, FM25Global> _fm25ActorTask;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IPagingService<ILearner> _learnerPagingService;
        private readonly IExternalDataCache _externalDataCache;
        private readonly IInternalDataCache _internalDataCache;
        private readonly IFileDataCache _fileDataCache;
        private readonly ITopicAndTaskSectionConfig _topicAndTaskSectionConfig;
        private readonly ILogger _logger;

        public PreFundingSFOrchestrationService(
            IJsonSerializationService jsonSerializationService,
            IIlrFileProviderService ilrFileProviderService,
            IFundingServiceDto fundingServiceDto,
            IPopulationService populationService,
            IActorTask<IALBActor, ALBGlobal> albActorTask,
            IActorTask<IFM70Actor, FM70Global> fm70ActorTask,
            IActorTask<IFM35Actor, FM35Global> fm35ActorTask,
            IActorTask<IFM36Actor, FM36Global> fm36ActorTask,
            IActorTask<IFM25Actor, FM25Global> fm25ActorTask,
            IKeyValuePersistenceService keyValuePersistenceService,
            IPagingService<ILearner> learnerPagingService,
            IExternalDataCache externalDataCache,
            IInternalDataCache internalDataCache,
            IFileDataCache fileDataCache,
            ITopicAndTaskSectionConfig topicAndTaskSectionConfig,
            ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _fundingServiceDto = fundingServiceDto;
            _populationService = populationService;
            _albActorTask = albActorTask;
            _fm70ActorTask = fm70ActorTask;
            _fm35ActorTask = fm35ActorTask;
            _fm36ActorTask = fm36ActorTask;
            _fm25ActorTask = fm25ActorTask;
            _keyValuePersistenceService = keyValuePersistenceService;
            _externalDataCache = externalDataCache;
            _internalDataCache = internalDataCache;
            _fileDataCache = fileDataCache;
            _learnerPagingService = learnerPagingService;
            _topicAndTaskSectionConfig = topicAndTaskSectionConfig;
            _logger = logger;
        }

        public async Task ExecuteAsync(IJobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            FundingServiceDto fundingServiceDto = (FundingServiceDto)_fundingServiceDto;
            fundingServiceDto.Message = await _ilrFileProviderService.Provide(jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString()).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            // get valid and invalid learners from intermediate storage and store it in the dto for rulebases
            fundingServiceDto.ValidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.ValidLearnRefNumbers].ToString(), cancellationToken));

            fundingServiceDto.InvalidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.InvalidLearnRefNumbers].ToString(), cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();

            await _populationService.PopulateAsync(cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            string externalDataCache = _jsonSerializationService.Serialize(_externalDataCache);
            string internalDataCache = _jsonSerializationService.Serialize(_internalDataCache);
            string fileDataCache = _jsonSerializationService.Serialize(_fileDataCache);

            List<FundingActorDto> fundingActorDtos = _learnerPagingService
                .BuildPages()
                .Select(p =>
                    new FundingActorDto
                    {
                        JobId = jobContextMessage.JobId,
                        ExternalDataCache = externalDataCache,
                        InternalDataCache = internalDataCache,
                        FileDataCache = fileDataCache,
                        ValidLearners = _jsonSerializationService.Serialize(p)
                    }).ToList();

            List<string> taskNames = jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks.SelectMany(t => t.Tasks).ToList();

            List<Task> fundingTasks = new List<Task>();

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformFM70Calculation))
            {
                fundingTasks.Add(_fm70ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm70Output].ToString(), cancellationToken));
            }

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformFM35Calculation))
            {
                fundingTasks.Add(_fm35ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm35Output].ToString(), cancellationToken));
            }

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformFM36Calculation))
            {
                fundingTasks.Add(_fm36ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm36Output].ToString(), cancellationToken));
            }

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformFM25Calculation))
            {
                fundingTasks.Add(_fm25ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm25Output].ToString(), cancellationToken));
            }

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformALBCalculation))
            {
                fundingTasks.Add(_albActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput].ToString(), cancellationToken));
            }

            await Task.WhenAll(fundingTasks).ConfigureAwait(false);

            _logger.LogDebug($"Completed Funding Service for given rule bases in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}
