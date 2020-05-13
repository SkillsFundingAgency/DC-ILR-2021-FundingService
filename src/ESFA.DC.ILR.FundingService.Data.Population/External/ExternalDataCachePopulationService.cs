using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class ExternalDataCachePopulationService : IExternalDataCachePopulationService
    {
        private readonly IMetaDataMapperService _metaDataMapperService;
        private readonly IPostcodesMapperService _postcodesMapperService;
        private readonly IOrganisationsMapperService _organisationsMapperService;
        private readonly ILargeEmployersMapperService _largeEmployersMapperService;
        private readonly IAppsEarningsHistoryMapperService _appsEarningsHistoryMapperService;
        private readonly IFCSMapperService _fcsMapperService;
        private readonly ILARSMapperService _larsMapperService;

        public ExternalDataCachePopulationService(
            IMetaDataMapperService metaDataMapperService,
            IPostcodesMapperService postcodesMapperService,
            IOrganisationsMapperService organisationsMapperService,
            ILargeEmployersMapperService largeEmployersMapperService,
            IAppsEarningsHistoryMapperService appsEarningsHistoryMapperService,
            IFCSMapperService fcsMapperService,
            ILARSMapperService larsMapperService)
        {
            _metaDataMapperService = metaDataMapperService;
            _postcodesMapperService = postcodesMapperService;
            _organisationsMapperService = organisationsMapperService;
            _largeEmployersMapperService = largeEmployersMapperService;
            _appsEarningsHistoryMapperService = appsEarningsHistoryMapperService;
            _fcsMapperService = fcsMapperService;
            _larsMapperService = larsMapperService;
        }

        public IExternalDataCache PopulateAsync(ReferenceDataRoot referenceDataRoot, CancellationToken cancellationToken)
        {
            var referenceDataVersions = _metaDataMapperService.GetReferenceDataVersions(referenceDataRoot.MetaDatas);

            return new ExternalDataCache
            {
                LARSCurrentVersion = referenceDataVersions.LarsVersion.Version,
                LARSLearningDelivery = _larsMapperService.MapLARSLearningDeliveries(referenceDataRoot.LARSLearningDeliveries),
                LARSStandards = _larsMapperService.MapLARSStandards(referenceDataRoot.LARSStandards),

                PostcodeRoots = _postcodesMapperService.MapPostcodes(referenceDataRoot.Postcodes),
                PostcodeCurrentVersion = referenceDataVersions.PostcodesVersion.Version,

                OrgVersion = referenceDataVersions.OrganisationsVersion.Version,
                OrgFunding = _organisationsMapperService.MapOrgFundings(referenceDataRoot.Organisations),
                CampusIdentifierSpecResources = _organisationsMapperService.MapCampusIdentifiers(referenceDataRoot.Organisations),

                LargeEmployers = _largeEmployersMapperService.MapLargeEmployers(referenceDataRoot.Employers),

                FCSContractAllocations = _fcsMapperService.MapFCSContractAllocations(referenceDataRoot.FCSContractAllocations),

                AECLatestInYearEarningHistory = _appsEarningsHistoryMapperService.MapAppsEarningsHistories(referenceDataRoot.AppsEarningsHistories)
                ?? new Dictionary<long, IReadOnlyCollection<AECEarningsHistory>>(),

                Periods = _metaDataMapperService.BuildPeriods(referenceDataRoot.MetaDatas)
            };
        }
    }
}