using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class ExternalDataCachePopulationService : IExternalDataCachePopulationService
    {
        private readonly IExternalDataCache _externalDataCache;
        private readonly IFundingServiceDto _fundingServiceDto;
        private readonly IPostcodesMapperService _postcodesMapperService;
        private readonly IOrganisationsMapperService _organisationsMapperService;
        private readonly ILargeEmployersMapperService _largeEmployersMapperService;
        private readonly IAppsEarningsHistoryMapperService _appsEarningsHistoryMapperService;
        private readonly IFCSMapperService _fcsMapperService;
        private readonly ILARSMapperService _larsMapperService;

        public ExternalDataCachePopulationService(
            IExternalDataCache externalDataCache,
            IFundingServiceDto fundingServiceDto,
            IPostcodesMapperService postcodesMapperService,
            IOrganisationsMapperService organisationsMapperService,
            ILargeEmployersMapperService largeEmployersMapperService,
            IAppsEarningsHistoryMapperService appsEarningsHistoryMapperService,
            IFCSMapperService fcsMapperService,
            ILARSMapperService larsMapperService)
        {
            _externalDataCache = externalDataCache;
            _fundingServiceDto = fundingServiceDto;
            _postcodesMapperService = postcodesMapperService;
            _organisationsMapperService = organisationsMapperService;
            _largeEmployersMapperService = largeEmployersMapperService;
            _appsEarningsHistoryMapperService = appsEarningsHistoryMapperService;
            _fcsMapperService = fcsMapperService;
            _larsMapperService = larsMapperService;
        }

        public async Task PopulateAsync(CancellationToken cancellationToken)
        {
            var referenceDataCache = (ExternalDataCache)_externalDataCache;

            referenceDataCache.LARSCurrentVersion = _fundingServiceDto.ReferenceData.MetaDatas.ReferenceDataVersions.LarsVersion.Version;
            referenceDataCache.LARSLearningDelivery = _larsMapperService.MapLARSLearningDeliveries(_fundingServiceDto.ReferenceData.LARSLearningDeliveries);
            referenceDataCache.LARSStandards = _larsMapperService.MapLARSStandards(_fundingServiceDto.ReferenceData.LARSStandards);

            referenceDataCache.PostcodeRoots = _postcodesMapperService.MapPostcodes(_fundingServiceDto.ReferenceData.Postcodes);
            referenceDataCache.PostcodeCurrentVersion = _fundingServiceDto.ReferenceData.MetaDatas.ReferenceDataVersions.PostcodesVersion.Version;

            referenceDataCache.OrgVersion = _fundingServiceDto.ReferenceData.MetaDatas.ReferenceDataVersions.OrganisationsVersion.Version;
            referenceDataCache.OrgFunding = _organisationsMapperService.MapOrgFundings(_fundingServiceDto.ReferenceData.Organisations, _fundingServiceDto.Message.LearningProviderEntity.UKPRN);

            referenceDataCache.LargeEmployers = _largeEmployersMapperService.MapLargeEmployers(_fundingServiceDto.ReferenceData.Employers);

            referenceDataCache.FCSContractAllocations = _fcsMapperService.MapFCSContractAllocations(_fundingServiceDto.ReferenceData.FCSContractAllocations);

            referenceDataCache.AECLatestInYearEarningHistory = _appsEarningsHistoryMapperService.MapAppsEarningsHistories(_fundingServiceDto.ReferenceData.AppsEarningsHistories);
        }
    }
}