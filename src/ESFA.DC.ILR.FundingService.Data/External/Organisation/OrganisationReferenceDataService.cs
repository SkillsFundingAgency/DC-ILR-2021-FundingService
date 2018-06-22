using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.Organisation
{
    public class OrganisationReferenceDataService : IOrganisationReferenceDataService
    {
        private readonly IReferenceDataCache _referenceDataCache;

        public OrganisationReferenceDataService(IReferenceDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public IEnumerable<OrgFunding> OrganisationFundingForUKPRN(int ukprn)
        {
            _referenceDataCache.OrgFunding.TryGetValue(ukprn, out IEnumerable<OrgFunding> orgFunding);

            return orgFunding;
        }

        public string OrganisationVersion()
        {
            return _referenceDataCache.OrgVersion;
        }
    }
}
