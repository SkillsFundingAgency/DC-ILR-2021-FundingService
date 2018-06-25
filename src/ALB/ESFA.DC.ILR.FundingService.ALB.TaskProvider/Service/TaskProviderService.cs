using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.ALB.TaskProvider.Interface;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Dictionary;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.ALB.TaskProvider.Service
{
    public class TaskProviderService : ITaskProviderService
    {
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IFileDataCache _fileDataCache;
        private readonly IPreFundingALBOrchestrationService _preFundingALBOrchestrationService;
        private readonly IALBOrchestrationService _albOrchestrationService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IFileDataCache fileDataCache, IPreFundingALBOrchestrationService preFundingALBOrchestrationService, IALBOrchestrationService albOrchestrationService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _fileDataCache = fileDataCache;
            _preFundingALBOrchestrationService = preFundingALBOrchestrationService;
            _albOrchestrationService = albOrchestrationService;
        }

        public void ExecuteTasks(Message message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            // pre funding
            var learnersToProcess = _preFundingALBOrchestrationService.Execute();

            // process funding
            var fundingOutputs = ProcessFunding(learnersToProcess);

            // transform shards in to new object
            var fundingOutputsToPersist = TransformFundingOutput(fundingOutputs);

            // persist
            var dataPersister = new DataPersister();
            dataPersister.PersistData(fundingOutputsToPersist, @"C:\Code\temp\ALBFundingService\Json_Output.json");
        }

        private void BuildKeyValueDictionary(Message message)
        {
            var learners = message.Learner.ToList();

            var list = new DictionaryKeyValuePersistenceService();
            var serializer = new XmlSerializationService();

            _keyValuePersistenceService.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();
        }

        private IList<IFundingOutputs> ProcessFunding(IEnumerable<IList<ILearner>> learnersList)
        {
            IList<IFundingOutputs> fundingOutputsList = new List<IFundingOutputs>();

            foreach (var list in learnersList)
            {
                fundingOutputsList.Add(_albOrchestrationService.Execute(_fileDataCache.UKPRN, list));
            }

            return fundingOutputsList;
        }

        private IFundingOutputs TransformFundingOutput(IList<IFundingOutputs> fundingOutputsList)
        {
            var global = fundingOutputsList[0].Global;

            var learnerAttributes = fundingOutputsList.SelectMany(l => l.Learners).ToArray();

            return new FundingOutputs
            {
                Global = global,
                Learners = learnerAttributes,
            };
        }
    }
}
