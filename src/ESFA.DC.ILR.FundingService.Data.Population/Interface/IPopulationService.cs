using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IPopulationService
    {
        Task PopulateAsync(CancellationToken cancellationToken);
    }
}
