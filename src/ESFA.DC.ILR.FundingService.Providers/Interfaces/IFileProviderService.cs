using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.Interfaces
{
    public interface IFileProviderService<T>
    {
        Task<T> Provide(IFundingServiceContext fundingServiceContext, CancellationToken cancellationToken);
    }
}