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
        private readonly ILARSDataRetrievalService _larsDataRetrievalService;
        private readonly IFundingServiceDto _fundingServiceDto;
        private readonly IPostcodesMapperService _postcodesMapperService;
        private readonly IOrganisationsMapperService _organisationsMapperService;
        private readonly ILargeEmployersMapperService _largeEmployersMapperService;
        private readonly IAppsEarningsHistoryMapperService _appsEarningsHistoryMapperService;
        private readonly IFCSMapperService _fcsMapperService;

        public ExternalDataCachePopulationService(
            IExternalDataCache externalDataCache,
            ILARSDataRetrievalService larsDataRetrievalService,
            IFundingServiceDto fundingServiceDto,
            IPostcodesMapperService postcodesMapperService,
            IOrganisationsMapperService organisationsMapperService,
            ILargeEmployersMapperService largeEmployersMapperService,
             IAppsEarningsHistoryMapperService appsEarningsHistoryMapperService,
             IFCSMapperService fcsMapperService)
        {
            _externalDataCache = externalDataCache;
            _larsDataRetrievalService = larsDataRetrievalService;
            _fundingServiceDto = fundingServiceDto;
            _postcodesMapperService = postcodesMapperService;
            _organisationsMapperService = organisationsMapperService;
            _largeEmployersMapperService = largeEmployersMapperService;
            _appsEarningsHistoryMapperService = appsEarningsHistoryMapperService;
            _fcsMapperService = fcsMapperService;
        }

        public async Task PopulateAsync(CancellationToken cancellationToken)
        {
            var providerUKPRN = _fundingServiceDto.Message.LearningProviderEntity.UKPRN;

            var learnAimRefs = _larsDataRetrievalService.UniqueLearnAimRefs(_fundingServiceDto.Message).ToCaseInsensitiveHashSet();
            var standardCodes = _larsDataRetrievalService.UniqueStandardCodes(_fundingServiceDto.Message).ToList();
            var frameworks = _larsDataRetrievalService.UniqueFrameworkCommonComponents(_fundingServiceDto.Message);
            var apprenticeshipFundingStandards = _larsDataRetrievalService.UniqueApprenticeshipFundingStandards(_fundingServiceDto.Message);
            var apprenticeshipFundingFrameworks = _larsDataRetrievalService.UniqueApprenticeshipFundingFrameworks(_fundingServiceDto.Message);

            var referenceDataCache = (ExternalDataCache)_externalDataCache;

            referenceDataCache.LARSCurrentVersion = _larsDataRetrievalService.CurrentVersion();
            referenceDataCache.LARSAnnualValue = _larsDataRetrievalService.LARSAnnualValuesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSLearningDelivery = _larsDataRetrievalService.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSLearningDeliveryCategory = _larsDataRetrievalService.LARSLearningDeliveryCategoriesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSFrameworkAims = _larsDataRetrievalService.LARSFrameworkAimsForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSFunding = _larsDataRetrievalService.LARSFundingsForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSStandardCommonComponent = _larsDataRetrievalService.LARSStandardCommonComponentForStandardCode(standardCodes);
            referenceDataCache.LARSFrameworkCommonComponent = _larsDataRetrievalService.LARSFrameworkCommonComponentForLearnAimRefs(frameworks);
            referenceDataCache.LARSApprenticeshipFundingStandards = _larsDataRetrievalService.LARSApprenticeshipFundingStandards(apprenticeshipFundingStandards);
            referenceDataCache.LARSApprenticeshipFundingFrameworks = _larsDataRetrievalService.LARSApprenticeshipFundingFrameworks(apprenticeshipFundingFrameworks);
            referenceDataCache.LARSStandardFundings = _larsDataRetrievalService.LARSStandardFundingForStandardCodes(standardCodes);

            referenceDataCache.PostcodeRoots = _postcodesMapperService.MapPostcodes(_fundingServiceDto.ReferenceData.Postcodes);
            referenceDataCache.PostcodeCurrentVersion = _fundingServiceDto.ReferenceData.MetaDatas.ReferenceDataVersions.PostcodesVersion.Version;

            referenceDataCache.OrgVersion = _fundingServiceDto.ReferenceData.MetaDatas.ReferenceDataVersions.OrganisationsVersion.Version;
            referenceDataCache.OrgFunding = _organisationsMapperService.MapOrgFundings(_fundingServiceDto.ReferenceData.Organisations, providerUKPRN);

            referenceDataCache.LargeEmployers = _largeEmployersMapperService.MapLargeEmployers(_fundingServiceDto.ReferenceData.Employers);

            referenceDataCache.FCSContractAllocations = _fcsMapperService.MapFCSContractAllocations(_fundingServiceDto.ReferenceData.FCSContractAllocations);

            referenceDataCache.AECLatestInYearEarningHistory = _appsEarningsHistoryMapperService.MapAppsEarningsHistories(_fundingServiceDto.ReferenceData.AppsEarningsHistories);
        }
    }
}