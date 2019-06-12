using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.FundingService.Providers.Interfaces
{
    public interface IFilePersistanceService
    {
        Task PersistAsync<T>(string fileReference, string container, T output, CancellationToken cancellationToken);
    }
}