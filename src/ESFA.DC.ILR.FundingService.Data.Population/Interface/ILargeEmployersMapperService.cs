using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface ILargeEmployersMapperService
    {
        IDictionary<int, IReadOnlyCollection<LargeEmployers>> MapLargeEmployers(IReadOnlyCollection<Employer> employers);
    }
}
