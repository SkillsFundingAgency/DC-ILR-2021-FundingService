using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class ExternalDataCachePopulationService : IExternalDataCachePopulationService
    {
        private readonly IPostcodesMapperService _postcodesMapperService;
        private readonly IOrganisationsMapperService _organisationsMapperService;
        private readonly ILargeEmployersMapperService _largeEmployersMapperService;
        private readonly IAppsEarningsHistoryMapperService _appsEarningsHistoryMapperService;
        private readonly IFCSMapperService _fcsMapperService;
        private readonly ILARSMapperService _larsMapperService;

        public ExternalDataCachePopulationService(
            IPostcodesMapperService postcodesMapperService,
            IOrganisationsMapperService organisationsMapperService,
            ILargeEmployersMapperService largeEmployersMapperService,
            IAppsEarningsHistoryMapperService appsEarningsHistoryMapperService,
            IFCSMapperService fcsMapperService,
            ILARSMapperService larsMapperService)
        {
            _postcodesMapperService = postcodesMapperService;
            _organisationsMapperService = organisationsMapperService;
            _largeEmployersMapperService = largeEmployersMapperService;
            _appsEarningsHistoryMapperService = appsEarningsHistoryMapperService;
            _fcsMapperService = fcsMapperService;
            _larsMapperService = larsMapperService;
        }

        public IExternalDataCache PopulateAsync(ReferenceDataRoot referenceDataRoot, CancellationToken cancellationToken)
        {
            return new ExternalDataCache
            {
                LARSCurrentVersion = referenceDataRoot.MetaDatas.ReferenceDataVersions.LarsVersion.Version,
                LARSLearningDelivery = _larsMapperService.MapLARSLearningDeliveries(referenceDataRoot.LARSLearningDeliveries),
                LARSStandards = _larsMapperService.MapLARSStandards(referenceDataRoot.LARSStandards),

                PostcodeRoots = _postcodesMapperService.MapPostcodes(referenceDataRoot.Postcodes),
                PostcodeCurrentVersion = referenceDataRoot.MetaDatas.ReferenceDataVersions.PostcodesVersion.Version,

                OrgVersion = referenceDataRoot.MetaDatas.ReferenceDataVersions.OrganisationsVersion.Version,
                OrgFunding = _organisationsMapperService.MapOrgFundings(referenceDataRoot.Organisations),

                LargeEmployers = _largeEmployersMapperService.MapLargeEmployers(referenceDataRoot.Employers),

                FCSContractAllocations = _fcsMapperService.MapFCSContractAllocations(referenceDataRoot.FCSContractAllocations),

                AECLatestInYearEarningHistory = _appsEarningsHistoryMapperService.MapAppsEarningsHistories(referenceDataRoot.AppsEarningsHistories),

                Periods = new Periods(),
            };
        }
    }
}