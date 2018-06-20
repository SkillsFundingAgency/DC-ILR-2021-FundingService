using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface
{
    public interface IPreFundingFM35OrchestrationService
    {
        IEnumerable<IList<ILearner>> Execute();
    }
}
