using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface
{
    public interface IOrganisationReferenceDataService
    {
        string OrganisationVersion();

        IEnumerable<OrgFunding> OrganisationFundingForUKPRN(int ukprn);

        IEnumerable<CampusIdentifierSpecResource> SpecialistResourcesForCampusIdentifier(string campId);

        IEnumerable<PostcodeSpecialistResource> PostcodeSpecialistResourcesForUkprn(int ukprn);
    }
}
