using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.Interfaces
{
    public interface IFundingOutputPersistenceService<T>
        where T : class
    {
        Task Process(T fundingOutputs, string fundingOutputKey);
    }
}