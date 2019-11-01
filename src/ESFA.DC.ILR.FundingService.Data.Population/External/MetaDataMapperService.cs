using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class MetaDataMapperService : IMetaDataMapperService
    {
        public ReferenceDataVersion GetReferenceDataVersions(MetaData metaDatas)
        {
            return metaDatas.ReferenceDataVersions;
        }

        public Periods BuildPeriods(MetaData metaDatas)
        {
            var censusDates = metaDatas.CollectionDates.CensusDates;

            return new Periods
            {
                Period1 = censusDates.First(p => p.Period == 1).Start,
                Period2 = censusDates.First(p => p.Period == 2).Start,
                Period3 = censusDates.First(p => p.Period == 3).Start,
                Period4 = censusDates.First(p => p.Period == 4).Start,
                Period5 = censusDates.First(p => p.Period == 5).Start,
                Period6 = censusDates.First(p => p.Period == 6).Start,
                Period7 = censusDates.First(p => p.Period == 7).Start,
                Period8 = censusDates.First(p => p.Period == 8).Start,
                Period9 = censusDates.First(p => p.Period == 9).Start,
                Period10 = censusDates.First(p => p.Period == 10).Start,
                Period11 = censusDates.First(p => p.Period == 11).Start,
                Period12 = censusDates.First(p => p.Period == 12).Start,
            };
        }
    }
}
