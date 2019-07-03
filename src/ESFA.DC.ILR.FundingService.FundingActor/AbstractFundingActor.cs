using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
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
                LARSLearningDelivery = deserialzedCache.LARSLearningDelivery.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase),
                LARSStandards = deserialzedCache.LARSStandards,
                OrgFunding = deserialzedCache.OrgFunding,
                OrgVersion = deserialzedCache.OrgVersion,
                PostcodeCurrentVersion = deserialzedCache.PostcodeCurrentVersion,
                PostcodeRoots = deserialzedCache.PostcodeRoots.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase),
                Periods = deserialzedCache.Periods,
            };
        }

        public List<T> BuildLearners<T>(string serializedLearners)
        {
            return JsonSerializationService.Deserialize<List<T>>(serializedLearners);
        }

        public string BuildFundingOutput<T>(T model)
        {
            return JsonSerializationService.Serialize(model);
        }
    }
}
