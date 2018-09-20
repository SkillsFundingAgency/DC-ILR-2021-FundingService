using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class ExternalDataCachePopulationService : IExternalDataCachePopulationService
    {
        private readonly IExternalDataCache _externalDataCache;
        private readonly IAppsEarningsHistoryDataRetrievalService _appsEarningsHistoryDataRetrievalService;
        private readonly IPostcodesDataRetrievalService _postcodesDataRetrievalService;
        private readonly ILargeEmployersDataRetrievalService _largeEmployersDataRetrievalService;
        private readonly ILARSDataRetrievalService _larsDataRetrievalService;
        private readonly IOrganisationDataRetrievalService _organisationDataRetrievalService;
        private readonly IFundingServiceDto _fundingServiceDto;

        public ExternalDataCachePopulationService(
            IExternalDataCache externalDataCache,
            IAppsEarningsHistoryDataRetrievalService appsEarningsHistoryDataRetrievalService,
            IPostcodesDataRetrievalService postcodesDataRetrievalService,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService,
            ILARSDataRetrievalService larsDataRetrievalService,
            IOrganisationDataRetrievalService organisationDataRetrievalService,
            IFundingServiceDto fundingServiceDto)
        {
            _externalDataCache = externalDataCache;
            _appsEarningsHistoryDataRetrievalService = appsEarningsHistoryDataRetrievalService;
            _postcodesDataRetrievalService = postcodesDataRetrievalService;
            _largeEmployersDataRetrievalService = largeEmployersDataRetrievalService;
            _larsDataRetrievalService = larsDataRetrievalService;
            _organisationDataRetrievalService = organisationDataRetrievalService;
            _fundingServiceDto = fundingServiceDto;
        }

        public async Task PopulateAsync(CancellationToken cancellationToken)
        {
            var providerUKPRN = _fundingServiceDto.Message.LearningProviderEntity.UKPRN;

            var uniquePostcodes = _postcodesDataRetrievalService.UniquePostcodes(_fundingServiceDto.Message).ToList();
            var learnAimRefs = _larsDataRetrievalService.UniqueLearnAimRefs(_fundingServiceDto.Message).ToList();
            var standardCodes = _larsDataRetrievalService.UniqueStandardCodes(_fundingServiceDto.Message).ToList();
            var frameworks = _larsDataRetrievalService.UniqueFrameworkCommonComponents(_fundingServiceDto.Message);
            var apprenticeshipFundingStandards = _larsDataRetrievalService.UniqueApprenticeshipFundingStandards(_fundingServiceDto.Message);
            var apprenticeshipFundingFrameworks = _larsDataRetrievalService.UniqueApprenticeshipFundingFrameworks(_fundingServiceDto.Message);

            var uniqueEmployerIds = _largeEmployersDataRetrievalService.UniqueEmployerIds(_fundingServiceDto.Message).ToList();
            var ukprns = new List<long>() { providerUKPRN };
            var learnRefNumberAndULN = _fundingServiceDto.Message.Learners.Select(l => new LearnRefNumberULNKey(l.LearnRefNumber, l.ULN));

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

            referenceDataCache.PostcodeCurrentVersion = _postcodesDataRetrievalService.CurrentVersion();
            referenceDataCache.DasDisadvantage = _postcodesDataRetrievalService.DasDisadvantagesForPostcodes(uniquePostcodes);
            referenceDataCache.SfaAreaCost = _postcodesDataRetrievalService.SfaAreaCostsForPostcodes(uniquePostcodes);
            referenceDataCache.SfaDisadvantage = _postcodesDataRetrievalService.SfaDisadvantagesForPostcodes(uniquePostcodes);
            referenceDataCache.EfaDisadvantage = _postcodesDataRetrievalService.EfaDisadvantagesForPostcodes(uniquePostcodes);
            referenceDataCache.CareerLearningPilot = _postcodesDataRetrievalService.CareerLearningPilotsForPostcodes(uniquePostcodes);

            referenceDataCache.OrgVersion = _organisationDataRetrievalService.CurrentVersion();
            referenceDataCache.OrgFunding = _organisationDataRetrievalService.OrgFundingsForUkprns(ukprns);

            referenceDataCache.LargeEmployers = _largeEmployersDataRetrievalService.LargeEmployersForEmployerIds(uniqueEmployerIds);

            referenceDataCache.AECLatestInYearEarningHistory = _appsEarningsHistoryDataRetrievalService.AppsEarningsHistoryForLearners(providerUKPRN, learnRefNumberAndULN);
        }
    }
}