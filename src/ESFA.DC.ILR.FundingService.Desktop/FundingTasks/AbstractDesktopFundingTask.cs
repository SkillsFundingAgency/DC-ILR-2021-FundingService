using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop.FundingTasks
{
    public class AbstractDesktopFundingTask
    {
        public AbstractDesktopFundingTask(IJsonSerializationService jsonSerializationService)
        {
            JsonSerializationService = jsonSerializationService;
        }

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
    }
}
