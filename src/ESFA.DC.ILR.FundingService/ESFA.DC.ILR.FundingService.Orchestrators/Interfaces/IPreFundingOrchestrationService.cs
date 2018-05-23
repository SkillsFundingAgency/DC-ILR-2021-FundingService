using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IPreFundingOrchestrationService<T, out U> where T : class
    {
        IEnumerable<U> Execute();
    }
}
