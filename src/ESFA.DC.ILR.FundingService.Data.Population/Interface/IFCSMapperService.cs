using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IFCSMapperService
    {
        IReadOnlyCollection<FCSContractAllocation> MapFCSContractAllocations(IReadOnlyCollection<FcsContractAllocation> fcsContractAllocations);
    }
}
