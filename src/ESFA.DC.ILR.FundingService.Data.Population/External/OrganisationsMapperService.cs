using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class OrganisationsMapperService : IOrganisationsMapperService
    {
        public OrganisationsMapperService()
        {
        }

        public IDictionary<int, IReadOnlyCollection<OrgFunding>> MapOrgFundings(IReadOnlyCollection<Organisation> organisations)
        {
            return organisations?
                    .ToDictionary(
                    o => o.UKPRN,
                    o => o.OrganisationFundings?.Select(of => OrgFundingFromEntity(of, o.UKPRN)).ToList() as IReadOnlyCollection<OrgFunding>);
        }

        public IDictionary<string, IReadOnlyCollection<CampusIdentifierSpecResource>> MapCampusIdentifiers(IReadOnlyCollection<Organisation> organisations)
        {
            return organisations.SelectMany(c => c.CampusIdentifers)
                .GroupBy(ci => ci.CampusIdentifier)
                .ToDictionary(
                c => c.Key,
                c => c.SelectMany(ci => ci.SpecialistResources
                .Select(sr => CampusIdentifierSpecResourceFromEntity(sr, ci.CampusIdentifier)))
                .ToList() as IReadOnlyCollection<CampusIdentifierSpecResource>, StringComparer.OrdinalIgnoreCase);
        }

        public OrgFunding OrgFundingFromEntity(OrganisationFunding entity, int ukprn)
        {
            return new OrgFunding()
            {
                UKPRN = ukprn,
                OrgFundFactor = entity.OrgFundFactor,
                OrgFundFactType = entity.OrgFundFactType,
                OrgFundFactValue = entity.OrgFundFactValue,
                OrgFundEffectiveFrom = entity.EffectiveFrom,
                OrgFundEffectiveTo = entity.EffectiveTo,
            };
        }

        public CampusIdentifierSpecResource CampusIdentifierSpecResourceFromEntity(SpecialistResource entity, string campusIdentifier)
        {
            return new CampusIdentifierSpecResource()
            {
                CampusIdentifier = campusIdentifier,
                SpecialistResources = entity.IsSpecialistResource == true ? "Y" : "N",
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}
