using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation
{
    public class OrganisationReferenceDataService : IOrganisationReferenceDataService
    {
        private readonly IReferenceDataCache _referenceDataCache;

        public OrganisationReferenceDataService(IReferenceDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        IEnumerable<OrgFunding> IOrganisationReferenceDataService.OrganisationFundingForUKPRN(int ukprn)
        {
            _referenceDataCache.OrgFunding.TryGetValue(ukprn, out IEnumerable<OrgFunding> orgFunding);

            return orgFunding;
        }

        string IOrganisationReferenceDataService.OrganisationVersion()
        {
            return _referenceDataCache.OrgVersion;
        }
    }
}
