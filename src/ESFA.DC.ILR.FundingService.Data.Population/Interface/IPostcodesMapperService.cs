using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IPostcodesMapperService
    {
        IDictionary<string, PostcodeRoot> MapPostcodes(IReadOnlyCollection<Postcode> postcodes);
    }
}
