using System.Threading;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IFileDataCachePopulationService
    {
        IFileDataCache PopulateAsync(IMessage message, CancellationToken cancellationToken);
    }
}
