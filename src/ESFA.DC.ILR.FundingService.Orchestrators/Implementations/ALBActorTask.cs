using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Abstract;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.RuleBaseTasks
{
    public class ALBActorTask : AbstractActorTask<IALBActor, FundingOutputs>, IALBActorTask
    {
        public ALBActorTask(
            IJsonSerializationService jsonSerializationService,
            IActorProvider<IALBActor> fundingActorProvider,
            IKeyValuePersistenceService keyValuePersistenceService,
            ILogger logger)
            : base(jsonSerializationService, fundingActorProvider, keyValuePersistenceService, logger, ActorServiceNameConstants.ALB)
        {
        }

        public override FundingOutputs Condense(IEnumerable<FundingOutputs> inputs)
        {
            return new FundingOutputs()
            {
                Global = inputs.First().Global,
                Learners = inputs.SelectMany(r => r.Learners).ToArray()
            };
        }
    }
}
