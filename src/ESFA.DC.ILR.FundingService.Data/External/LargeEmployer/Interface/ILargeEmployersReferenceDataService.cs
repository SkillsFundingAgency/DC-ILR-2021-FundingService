using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface
{
    public interface ILargeEmployersReferenceDataService
    {
        IEnumerable<LargeEmployers> LargeEmployersforEmpID(int lEmpID);
    }
}
