﻿using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.Organisation
{
    public class OrganisationReferenceDataService : IOrganisationReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public OrganisationReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public IEnumerable<OrgFunding> OrganisationFundingForUKPRN(int ukprn)
        {
            _referenceDataCache.OrgFunding.TryGetValue(ukprn, out IReadOnlyCollection<OrgFunding> orgFunding);

            return orgFunding?? new List<OrgFunding>();
        }

        public string OrganisationVersion()
        {
            return _referenceDataCache.OrgVersion;
        }

        public IEnumerable<CampusIdentifierSpecResource> SpecialistResourcesForCampusIdentifier(string campId)
        {
            if (campId != null)
            {
                _referenceDataCache.CampusIdentifierSpecResources.TryGetValue(campId, out IReadOnlyCollection<CampusIdentifierSpecResource> campusIdentifierSpecResources);

                return campusIdentifierSpecResources ?? new List<CampusIdentifierSpecResource>();
            }

            return new List<CampusIdentifierSpecResource>();
        }
    }
}
