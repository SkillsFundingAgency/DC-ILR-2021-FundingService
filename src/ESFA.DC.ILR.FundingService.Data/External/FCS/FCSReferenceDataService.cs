using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.FCS
{
    public class FCSReferenceDataService : IFCSReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public FCSReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public IEnumerable<FCSContractAllocation> FcsContractsForConRef(string conRefNumber)
        {
            if (conRefNumber == null)
            {
                return null;
            }

            _referenceDataCache.FCSContractAllocations.TryGetValue(conRefNumber, out IEnumerable<FCSContractAllocation> fcsContracts);

            return fcsContracts;
        }
    }
}
