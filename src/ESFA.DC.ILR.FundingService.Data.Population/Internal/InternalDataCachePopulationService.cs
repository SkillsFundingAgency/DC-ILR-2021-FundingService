using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Internal
{
    public class InternalDataCachePopulationService : IInternalDataCachePopulationService
    {
        private readonly IInternalDataCache _internalDataCache;

        public InternalDataCachePopulationService(IInternalDataCache internalDataCache)
        {
            _internalDataCache = internalDataCache;
        }

        private static Dictionary<int, DateTime> Periods => new Dictionary<int, DateTime>
        {
           { 1, new DateTime(2018, 08, 01) },
           { 2, new DateTime(2018, 09, 01) },
           { 3, new DateTime(2018, 10, 01) },
           { 4, new DateTime(2018, 11, 01) },
           { 5, new DateTime(2018, 12, 01) },
           { 6, new DateTime(2019, 01, 01) },
           { 7, new DateTime(2019, 02, 01) },
           { 8, new DateTime(2019, 03, 01) },
           { 9, new DateTime(2019, 04, 01) },
           { 10, new DateTime(2019, 05, 01) },
           { 11, new DateTime(2019, 06, 01) },
           { 12, new DateTime(2019, 07, 01) },
        };

        public void Populate()
        {
            var internalCache = (InternalDataCache)_internalDataCache;

            internalCache.Period1 = Periods[1];
            internalCache.Period2 = Periods[2];
            internalCache.Period3 = Periods[3];
            internalCache.Period4 = Periods[4];
            internalCache.Period5 = Periods[5];
            internalCache.Period6 = Periods[6];
            internalCache.Period7 = Periods[7];
            internalCache.Period8 = Periods[8];
            internalCache.Period9 = Periods[9];
            internalCache.Period10 = Periods[10];
            internalCache.Period11 = Periods[11];
            internalCache.Period12 = Periods[12];
        }
    }
}
