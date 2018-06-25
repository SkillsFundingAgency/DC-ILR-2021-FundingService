using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.FM35.TaskProvider.Interface;
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
        private readonly IFileDataCache _fileDataCache;
        private readonly IPreFundingFM35OrchestrationService _preFundingFM35OrchestrationService;
        private readonly IFM35OrchestrationService _fm35OrchestrationService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IFileDataCache fileDataCache, IPreFundingFM35OrchestrationService preFundingFM35OrchestrationService, IFM35OrchestrationService fm35OrchestrationService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _fileDataCache = fileDataCache;
            _preFundingFM35OrchestrationService = preFundingFM35OrchestrationService;
            _fm35OrchestrationService = fm35OrchestrationService;
        }

        public void ExecuteTasks(Message message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            // pre funding
            var learnersToProcess = _preFundingFM35OrchestrationService.Execute();

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
                fundingOutputsList.Add(_fm35OrchestrationService.Execute(ukprn, ll));
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
