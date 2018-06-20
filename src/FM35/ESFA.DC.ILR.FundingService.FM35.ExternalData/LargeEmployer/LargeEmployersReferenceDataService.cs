using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer
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
