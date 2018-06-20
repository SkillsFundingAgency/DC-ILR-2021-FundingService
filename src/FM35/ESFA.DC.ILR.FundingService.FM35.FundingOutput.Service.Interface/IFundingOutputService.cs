using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service.Interface
{
    public interface IFundingOutputService
    {
        IFM35FundingOutputs ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities);
    }
}
