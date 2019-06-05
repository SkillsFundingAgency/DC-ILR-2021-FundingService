using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.FundingService.Providers.Interfaces
{
    public interface IFilePersistanceService
    {
        Task PersistAsync(string fileReference, string container, string fileData, CancellationToken cancellationToken);
    }
}