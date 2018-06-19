using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.ALB.ExternalData.Interface
{
    public interface IReferenceDataCachePopulationService
    {
        void Populate(IEnumerable<string> learnAimRefs, IEnumerable<string> postcodes);
    }
}
