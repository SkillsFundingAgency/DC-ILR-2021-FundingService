using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.LargeEmployer
{
    public class LargeEmployersReferenceDataService : ILargeEmployersReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public LargeEmployersReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public IEnumerable<LargeEmployers> LargeEmployersforEmpID(int? lEmpID)
        {
            if (lEmpID == null)
            {
                return null;
            }
            _referenceDataCache.LargeEmployers.TryGetValue((int)lEmpID, out IEnumerable<LargeEmployers> largeEmployers);

            return largeEmployers;
        }
    }
}
