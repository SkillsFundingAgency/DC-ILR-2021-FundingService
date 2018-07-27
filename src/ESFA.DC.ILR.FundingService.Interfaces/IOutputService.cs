using ESFA.DC.OPA.Model.Interface;
using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IOutputService<T>
    {
        T ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities);
    }
}
