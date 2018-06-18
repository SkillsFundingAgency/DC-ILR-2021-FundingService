using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Interface
{
    public interface IFundingService
    {
        IFundingOutputs ProcessFunding(int ukprn, IList<ILearner> learnerList);
    }
}
