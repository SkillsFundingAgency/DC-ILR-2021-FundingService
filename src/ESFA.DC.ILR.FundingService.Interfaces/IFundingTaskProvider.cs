using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFundingTaskProvider
    {
        Task ProvideAsync(IFundingServiceContext fundingServiceContext, IMessage message, string externalDataCache, string fileDataCache, CancellationToken cancellationToken);
    }
}
