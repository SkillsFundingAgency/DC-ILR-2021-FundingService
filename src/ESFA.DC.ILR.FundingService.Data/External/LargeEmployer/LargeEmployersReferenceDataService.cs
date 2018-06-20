using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.LargeEmployer
{
    public class LargeEmployersReferenceDataService : ILargeEmployersReferenceDataService
    {
        private readonly IReferenceDataCache _referenceDataCache;

        public LargeEmployersReferenceDataService(IReferenceDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public IEnumerable<LargeEmployers> LargeEmployersforEmpID(int lEmpID)
        {
            _referenceDataCache.LargeEmployers.TryGetValue(lEmpID, out IEnumerable<LargeEmployers> largeEmployers);

            return largeEmployers;
        }
    }
}
