using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.Interfaces
{
    public interface IIlrFileProviderService
    {
        Task<IMessage> Provide(string fileName);
    }
}