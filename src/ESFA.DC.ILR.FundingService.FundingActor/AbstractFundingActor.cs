using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.Model;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.FundingActor
{
    public abstract class AbstractFundingActor : Actor
    {
        protected AbstractFundingActor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId)
        {
            LifetimeScope = lifetimeScope;
            ExecutionContext = executionContext;
            JsonSerializationService = jsonSerializationService;
            ActorId = actorId;
        }

        public IExecutionContext ExecutionContext { get; }

        protected ActorId ActorId { get; }

        protected ILifetimeScope LifetimeScope { get; }

        private IJsonSerializationService JsonSerializationService { get; }

        public ExternalDataCache BuildExternalDataCache(string serialzedCache)
        {
            var deserialzedCache = JsonSerializationService.Deserialize<ExternalDataCache>(serialzedCache);

            return new ExternalDataCache
            {
                AECLatestInYearEarningHistory = deserialzedCache.AECLatestInYearEarningHistory,
                FCSContractAllocations = deserialzedCache.FCSContractAllocations,
                LargeEmployers = deserialzedCache.LargeEmployers,
                LARSCurrentVersion = deserialzedCache.LARSCurrentVersion,
                LARSLearningDelivery = deserialzedCache.LARSLearningDelivery.ToCaseInsensitiveDictionary(),
                LARSStandards = deserialzedCache.LARSStandards,
                OrgFunding = deserialzedCache.OrgFunding,
                OrgVersion = deserialzedCache.OrgVersion,
                PostcodeCurrentVersion = deserialzedCache.PostcodeCurrentVersion,
                PostcodeRoots = deserialzedCache.PostcodeRoots.ToCaseInsensitiveDictionary(),
                Periods = deserialzedCache.Periods,
            };
        }

        public FileDataCache BuildFileDataCache(string serialzedCache)
        {
            var deserialzedCache = JsonSerializationService.Deserialize<FileDataCache>(serialzedCache);

            return new FileDataCache
            {
                UKPRN = deserialzedCache.UKPRN,
                DPOutcomes = deserialzedCache.DPOutcomes.ToCaseInsensitiveDictionary(),
            };
        }

        public List<MessageLearner> BuildLearners(string serializedLearners)
        {
            return JsonSerializationService.Deserialize<List<MessageLearner>>(serializedLearners);
        }

        public string BuildFundingOutput<T>(T model)
        {
            return JsonSerializationService.Serialize(model);
        }
    }
}
