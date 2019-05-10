using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IOrganisationsMapperService
    {
        IDictionary<int, IReadOnlyCollection<OrgFunding>> MapOrgFundings(IReadOnlyCollection<Organisation> organisations, int providerUKPRN);
    }
}
