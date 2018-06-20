using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface
{
    public interface IFM35OrchestrationService
    {
        IFM35FundingOutputs Execute(int ukprn, IList<ILearner> fm35ValidLearners);
    }
}
