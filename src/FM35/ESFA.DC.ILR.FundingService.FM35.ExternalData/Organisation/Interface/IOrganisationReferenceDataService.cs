using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Interface
{
    public interface IOrganisationReferenceDataService
    {
        string OrganisationVersion();

        IEnumerable<OrgFunding> OrganisationFundingForUKPRN(int ukprn);
    }
}
