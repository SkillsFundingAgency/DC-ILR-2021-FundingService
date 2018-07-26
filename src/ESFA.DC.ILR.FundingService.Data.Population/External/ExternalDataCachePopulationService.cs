using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
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
        private readonly ILARS _LARSContext;
        private readonly IFundingServiceDto _fundingServiceDto;

        public ExternalDataCachePopulationService(
            IExternalDataCache externalDataCache,
            IPostcodesDataRetrievalService postcodesDataRetrievalService,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService,
            ILARSDataRetrievalService larsDataRetrievalService,
            IOrganisationDataRetrievalService organisationDataRetrievalService,
            ILARS LARSContext,
            IFundingServiceDto fundingServiceDto)
        {
            _externalDataCache = externalDataCache;
            _postcodesDataRetrievalService = postcodesDataRetrievalService;
            _largeEmployersDataRetrievalService = largeEmployersDataRetrievalService;
            _larsDataRetrievalService = larsDataRetrievalService;
            _organisationDataRetrievalService = organisationDataRetrievalService;
            _LARSContext = LARSContext;
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
            referenceDataCache.LARSAnnualValue = LARSAnnualValue(learnAimRefs);
            referenceDataCache.LARSLearningDelivery = _larsDataRetrievalService.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSLearningDeliveryCatgeory = LARSLearningDeliveryCategory(learnAimRefs);
            referenceDataCache.LARSFrameworkAims = LARSFrameworkAims(learnAimRefs);
            referenceDataCache.LARSFunding = _larsDataRetrievalService.LARSFundingsForLearnAimRefs(learnAimRefs);

            referenceDataCache.PostcodeCurrentVersion = _postcodesDataRetrievalService.CurrentVersion();
            referenceDataCache.SfaAreaCost = _postcodesDataRetrievalService.SfaAreaCostsForPostcodes(uniquePostcodes);
            referenceDataCache.SfaDisadvantage = _postcodesDataRetrievalService.SfaDisadvantagesForPostcodes(uniquePostcodes);

            referenceDataCache.OrgVersion = _organisationDataRetrievalService.CurrentVersion();
            referenceDataCache.OrgFunding = _organisationDataRetrievalService.OrgFundingsForUkprns(ukprns);

            referenceDataCache.LargeEmployers = _largeEmployersDataRetrievalService.LargeEmployersForEmployerIds(uniqueEmployerIds);
        }

        #region LARS
        
        private IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValue(IEnumerable<string> learnAimRefs)
        {
            return
                _LARSContext.LARS_AnnualValue
                .Where(la => learnAimRefs.Contains(la.LearnAimRef)).GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(la => new LARSAnnualValue
                {
                    LearnAimRef = la.LearnAimRef,
                    BasicSkillsType = la.BasicSkillsType,
                    EffectiveFrom = la.EffectiveFrom,
                    EffectiveTo = la.EffectiveTo,
                }).ToList() as IEnumerable<LARSAnnualValue>);
        }

        private IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCategory(IEnumerable<string> learnAimRefs)
        {
            return
                _LARSContext.LARS_LearningDeliveryCategory
                .Where(ld => learnAimRefs.Contains(ld.LearnAimRef)).GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(ld => new LARSLearningDeliveryCategory
                {
                    LearnAimRef = ld.LearnAimRef,
                    CategoryRef = ld.CategoryRef,
                    EffectiveFrom = ld.EffectiveFrom,
                    EffectiveTo = ld.EffectiveTo,
                }).ToList() as IEnumerable<LARSLearningDeliveryCategory>);
        }

        private IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAims(IEnumerable<string> learnAimRefs)
        {
            return
                _LARSContext.LARS_FrameworkAims
                .Where(lf => learnAimRefs.Contains(lf.LearnAimRef)).GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(lf => new LARSFrameworkAims
                {
                    LearnAimRef = lf.LearnAimRef,
                    FworkCode = lf.FworkCode,
                    ProgType = lf.ProgType,
                    PwayCode = lf.PwayCode,
                    FrameworkComponentType = lf.FrameworkComponentType,
                    EffectiveFrom = lf.EffectiveFrom,
                    EffectiveTo = lf.EffectiveTo,
                }).ToList() as IEnumerable<LARSFrameworkAims>);
        }

        #endregion
    }
}