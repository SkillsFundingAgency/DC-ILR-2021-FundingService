using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class ExternalDataCachePopulationService : IExternalDataCachePopulationService
    {
        private readonly IExternalDataCache _externalDataCache;
        private readonly IPostcodesDataRetrievalService _postcodesDataRetrievalService;
        private readonly ILargeEmployersDataRetrievalService _largeEmployersDataRetrievalService;
        private readonly ILARSDataRetrievalService _larsDataRetrievalService;
        private readonly IOrganisationDataRetrievalService _organisationDataRetrievalService;
        private readonly IFundingServiceDto _fundingServiceDto;

        public ExternalDataCachePopulationService(
            IExternalDataCache externalDataCache,
            IPostcodesDataRetrievalService postcodesDataRetrievalService,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService,
            ILARSDataRetrievalService larsDataRetrievalService,
            IOrganisationDataRetrievalService organisationDataRetrievalService,
            IFundingServiceDto fundingServiceDto)
        {
            _externalDataCache = externalDataCache;
            _postcodesDataRetrievalService = postcodesDataRetrievalService;
            _largeEmployersDataRetrievalService = largeEmployersDataRetrievalService;
            _larsDataRetrievalService = larsDataRetrievalService;
            _organisationDataRetrievalService = organisationDataRetrievalService;
            _fundingServiceDto = fundingServiceDto;
        }
        
        public void Populate()
        {
            var uniquePostcodes = _postcodesDataRetrievalService.UniquePostcodes(_fundingServiceDto.Message).ToList();
            var learnAimRefs = _larsDataRetrievalService.UniqueLearnAimRefs(_fundingServiceDto.Message).ToList();
            var uniqueEmployerIds = _largeEmployersDataRetrievalService.UniqueEmployerIds(_fundingServiceDto.Message).ToList();
            var ukprns = new List<long>() { _fundingServiceDto.Message.LearningProviderEntity.UKPRN };

            var referenceDataCache = (ExternalDataCache)_externalDataCache;

            referenceDataCache.LARSCurrentVersion = _larsDataRetrievalService.CurrentVersion();
            referenceDataCache.LARSAnnualValue = _larsDataRetrievalService.LARSAnnualValuesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSLearningDelivery = _larsDataRetrievalService.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSLearningDeliveryCategory = _larsDataRetrievalService.LARSLearningDeliveryCategoriesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSFrameworkAims = _larsDataRetrievalService.LARSFrameworkAimsForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSFunding = _larsDataRetrievalService.LARSFundingsForLearnAimRefs(learnAimRefs);

            referenceDataCache.PostcodeCurrentVersion = _postcodesDataRetrievalService.CurrentVersion();
            referenceDataCache.SfaAreaCost = _postcodesDataRetrievalService.SfaAreaCostsForPostcodes(uniquePostcodes);
            referenceDataCache.SfaDisadvantage = _postcodesDataRetrievalService.SfaDisadvantagesForPostcodes(uniquePostcodes);

            referenceDataCache.OrgVersion = _organisationDataRetrievalService.CurrentVersion();
            referenceDataCache.OrgFunding = _organisationDataRetrievalService.OrgFundingsForUkprns(ukprns);

            referenceDataCache.LargeEmployers = _largeEmployersDataRetrievalService.LargeEmployersForEmployerIds(uniqueEmployerIds);
        }
    }
}