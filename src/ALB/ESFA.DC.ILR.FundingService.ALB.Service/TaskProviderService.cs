using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.ALB.Service
{
    public class TaskProviderService : ITaskProviderService
    {
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IFundingService<ILearner, ALBFundingOutputs> _fundingService;
        private readonly IPopulationService _populationService;
        private readonly IPagingService<ILearner> _learnerPerActorService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IFundingService<ILearner, ALBFundingOutputs> fundingService, IPopulationService populationService, IPagingService<ILearner> learnerPerActorService, IJsonSerializationService jsonSerializationService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _fundingService = fundingService;
            _populationService = populationService;
            _learnerPerActorService = learnerPerActorService;
            _jsonSerializationService = jsonSerializationService;
        }

        public void ExecuteTasks(IMessage message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            // pre funding
            _populationService.Populate();
            var learnersToProcess = _learnerPerActorService.BuildPages();

            // process funding
            var fundingOutputs = ProcessFunding(learnersToProcess);

            // transform shards in to new object
            var fundingOutputsToPersist = TransformFundingOutput(fundingOutputs);

            // persist
            var serializedOutputs = _jsonSerializationService.Serialize(fundingOutputsToPersist);

            System.IO.File.WriteAllText(@"C:\Code\temp\ALBFundingService\Json_Output.json", serializedOutputs);
        }

        private void BuildKeyValueDictionary(IMessage message)
        {
            var learners = message.Learners.ToList();

            var serializer = new XmlSerializationService();

            _keyValuePersistenceService.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();
        }

        private IEnumerable<ALBFundingOutputs> ProcessFunding(IEnumerable<IEnumerable<ILearner>> learnersList)
        {
            var fundingOutputsList = new List<ALBFundingOutputs>();

            foreach (var list in learnersList)
            {
                fundingOutputsList.Add(_fundingService.ProcessFunding(list));
            }

            return fundingOutputsList;
        }

        private ALBFundingOutputs TransformFundingOutput(IEnumerable<ALBFundingOutputs> fundingOutputsList)
        {
            var global = fundingOutputsList.First().Global;

            var learnerAttributes = fundingOutputsList.SelectMany(l => l.Learners).ToArray();

            return new ALBFundingOutputs
            {
                Global = global,
                Learners = learnerAttributes,
            };
        }
    }
}
