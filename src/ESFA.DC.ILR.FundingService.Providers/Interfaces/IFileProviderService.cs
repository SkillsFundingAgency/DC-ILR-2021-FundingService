using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Providers.Interfaces
{
    public interface IFileProviderService<T>
    {
        Task<T> ProvideAsync(IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken);
    }
}