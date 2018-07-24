using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Abstract;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class FM35ActorTask : AbstractActorTask<IFM35Actor, FM35FundingOutputs>, IFM35ActorTask
    {
        public FM35ActorTask(
            IJsonSerializationService jsonSerializationService,
            IActorProvider<IFM35Actor> fundingActorProvider,
            IKeyValuePersistenceService keyValuePersistenceService,
            ILogger logger)
            : base(jsonSerializationService, fundingActorProvider, keyValuePersistenceService, logger, ActorServiceNameConstants.FM35)
        {
        }

        public override FM35FundingOutputs Condense(IEnumerable<FM35FundingOutputs> inputs)
        {
            return new FM35FundingOutputs()
            {
                Global = inputs.First().Global,
                Learners = inputs.SelectMany(r => r.Learners).ToArray()
            };
        }
    }
}
