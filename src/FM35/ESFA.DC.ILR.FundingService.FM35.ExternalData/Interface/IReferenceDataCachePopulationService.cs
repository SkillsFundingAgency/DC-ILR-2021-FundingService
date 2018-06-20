using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface
{
    public interface IReferenceDataCachePopulationService
    {
        void Populate(IList<string> learnAimRefs, IList<string> postcodes, IList<long> orgUkprns, IList<int> lEmpIDs);
    }
}
