using System.Threading;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IExternalDataCachePopulationService
    {
        IExternalDataCache PopulateAsync(ReferenceDataRoot referenceDataRoot, CancellationToken cancellationToken);
    }
}
