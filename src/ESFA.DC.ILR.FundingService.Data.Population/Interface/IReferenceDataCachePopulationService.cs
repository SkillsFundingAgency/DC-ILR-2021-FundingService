using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IReferenceDataCachePopulationService
    {
        void Populate(IEnumerable<string> learnAimRefs, IEnumerable<string> postcodes, IEnumerable<long> orgUkprns, IEnumerable<int> lEmpIDs);
    }
}
