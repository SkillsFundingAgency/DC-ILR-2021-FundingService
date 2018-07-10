using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.ALB.Service
{
    public class TaskProviderService : ITaskProviderService
    {
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IFundingService<IFundingOutputs> _fundingService;
        private readonly IPopulationService _populationService;
        private readonly ILearnerPerActorService<ILearner, IList<ILearner>> _learnerPerActorService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IFundingService<IFundingOutputs> fundingService, IPopulationService populationService, ILearnerPerActorService<ILearner, IList<ILearner>> learnerPerActorService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _fundingService = fundingService;
            _populationService = populationService;
            _learnerPerActorService = learnerPerActorService;
        }

        public void ExecuteTasks(IMessage message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            // pre funding
            _populationService.Populate();
            var learnersToProcess = _learnerPerActorService.Process();

            // process funding
            var fundingOutputs = ProcessFunding(learnersToProcess);

            // transform shards in to new object
            var fundingOutputsToPersist = TransformFundingOutput(fundingOutputs);

            // persist
            var dataPersister = new DataPersister();
            dataPersister.PersistData(fundingOutputsToPersist, @"C:\Code\temp\ALBFundingService\Json_Output.json");
        }

        private void BuildKeyValueDictionary(IMessage message)
        {
            var learners = message.Learners.ToList();

            var serializer = new XmlSerializationService();

            _keyValuePersistenceService.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();
        }

        private IList<IFundingOutputs> ProcessFunding(IEnumerable<IList<ILearner>> learnersList)
        {
            IList<IFundingOutputs> fundingOutputsList = new List<IFundingOutputs>();

            foreach (var list in learnersList)
            {
                fundingOutputsList.Add(_fundingService.ProcessFunding(list));
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
