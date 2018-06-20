using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Interface
{
    public interface IFundingService
    {
        IFM35FundingOutputs ProcessFunding(int ukprn, IList<ILearner> learnerList);
    }
}
