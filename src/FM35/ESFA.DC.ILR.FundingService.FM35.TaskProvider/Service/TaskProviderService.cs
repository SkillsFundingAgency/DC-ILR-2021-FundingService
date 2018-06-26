using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.FM35.TaskProvider.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Dictionary;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.FM35.TaskProvider.Service
{
    public class TaskProviderService : ITaskProviderService
    {
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IPopulationService _populationService;
        private readonly IFileDataCache _fileDataCache;
        private readonly ILearnerPerActorService<ILearner, IList<ILearner>> _learnerPerActorService;
        private readonly IFundingService<IFM35FundingOutputs> _fundingService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IPopulationService populationService, IFileDataCache fileDataCache, ILearnerPerActorService<ILearner, IList<ILearner>> learnerPerActorService, IFundingService<IFM35FundingOutputs> fundingService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _populationService = populationService;
            _fileDataCache = fileDataCache;
            _learnerPerActorService = learnerPerActorService;
            _fundingService = fundingService;
        }

        public void ExecuteTasks(Message message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            _populationService.Populate();

            // pre funding
            var learnersToProcess = _learnerPerActorService.Process();

            // process funding
            var fundingOutputs = ProcessFunding(learnersToProcess);

            // persist
            var dataPersister = new DataPersister();
            dataPersister.PersistData(fundingOutputs, @"C:\Code\temp\FM35FundingService\Json_Output.json");
        }

        private void BuildKeyValueDictionary(Message message)
        {
            var learners = message.Learner.ToList();

            var list = new DictionaryKeyValuePersistenceService();
            var serializer = new XmlSerializationService();

            _keyValuePersistenceService.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();
        }

        private IFM35FundingOutputs ProcessFunding(IEnumerable<IList<ILearner>> learnersList)
        {
            int ukprn = _fileDataCache.UKPRN;
            ConcurrentBag<IFM35FundingOutputs> fundingOutputsList = new ConcurrentBag<IFM35FundingOutputs>();

            Parallel.ForEach(learnersList, ll =>
            {
                fundingOutputsList.Add(_fundingService.ProcessFunding(ukprn, ll));
            });

            //foreach (var list in learnersList)
            //{
            //    fundingOutputsList.Add(_fm35OrchestrationService.Execute(ukprn, list));
            //}

            return TransformFundingOutput(fundingOutputsList.ToList());
        }

        private IFM35FundingOutputs TransformFundingOutput(IList<IFM35FundingOutputs> fundingOutputs)
        {
            var global = fundingOutputs.Select(g => g.Global).FirstOrDefault();

            var learnerAttributes = fundingOutputs.SelectMany(l => l.Learners).ToArray();
                // fundingOutputs.SelectMany(f => f.SelectMany(l => l.Learners)).ToArray();

            return new FM35FundingOutputs
            {
                Global = global,
                Learners = learnerAttributes,
            };
        }
    }
}
