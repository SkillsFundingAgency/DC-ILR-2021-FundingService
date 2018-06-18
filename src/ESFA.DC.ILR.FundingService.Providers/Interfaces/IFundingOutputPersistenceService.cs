using System.Threading.Tasks;

namespace ESFA.DC.ILR.FundingService.Providers.Interfaces
{
    public interface IFundingOutputPersistenceService<T>
        where T : class
    {
        Task Process(T fundingOutputs, string fundingOutputKey);
    }
}