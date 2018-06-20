using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Interface
{
    public interface ILargeEmployersReferenceDataService
    {
        IEnumerable<LargeEmployers> LargeEmployersforEmpID(int lEmpID);
    }
}
