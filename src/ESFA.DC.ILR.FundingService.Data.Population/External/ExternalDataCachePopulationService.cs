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
        private readonly ILARS _LARSContext;
        private readonly IOrganisations _organisationContext;
        private readonly IFundingServiceDto _fundingServiceDto;

        public ExternalDataCachePopulationService(
            IExternalDataCache externalDataCache,
            IPostcodesDataRetrievalService postcodesDataRetrievalService,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService,
            ILARS LARSContext,
            IOrganisations organisationContext,
            IFundingServiceDto fundingServiceDto)
        {
            _externalDataCache = externalDataCache;
            _postcodesDataRetrievalService = postcodesDataRetrievalService;
            _largeEmployersDataRetrievalService = largeEmployersDataRetrievalService;
            _LARSContext = LARSContext;
            _organisationContext = organisationContext;
            _fundingServiceDto = fundingServiceDto;
        }
        
        public void Populate()
        {
            var learners = _fundingServiceDto.Message.Learners;

            var uniquePostcodes = _postcodesDataRetrievalService.UniquePostcodes(_fundingServiceDto.Message).ToList();
            var learnAimRefs = learners.SelectMany(l => l.LearningDeliveries).Select(ld => ld.LearnAimRef).Distinct().ToList();
            var uniqueEmployerIds = _largeEmployersDataRetrievalService.UniqueEmployerIds(_fundingServiceDto.Message).ToList();
            var orgUkprns = new List<long>() { _fundingServiceDto.Message.LearningProviderEntity.UKPRN };

            var referenceDataCache = (ExternalDataCache)_externalDataCache;

            referenceDataCache.LARSCurrentVersion = LARSCurrentVersion();
            referenceDataCache.LARSAnnualValue = LARSAnnualValue(learnAimRefs);
            referenceDataCache.LARSLearningDelivery = LARSLearningDelivery(learnAimRefs);
            referenceDataCache.LARSLearningDeliveryCatgeory = LARSLearningDeliveryCategory(learnAimRefs);
            referenceDataCache.LARSFrameworkAims = LARSFrameworkAims(learnAimRefs);
            referenceDataCache.LARSFunding = LARSFunding(learnAimRefs);

            referenceDataCache.PostcodeCurrentVersion = _postcodesDataRetrievalService.VersionNumber();
            referenceDataCache.SfaAreaCost = _postcodesDataRetrievalService.SfaAreaCostsForPostcodes(uniquePostcodes);
            referenceDataCache.SfaDisadvantage = _postcodesDataRetrievalService.SfaDisadvantagesForPostcodes(uniquePostcodes);

            referenceDataCache.OrgVersion = OrgVersion();
            referenceDataCache.OrgFunding = OrgFunding(orgUkprns);

            referenceDataCache.LargeEmployers = _largeEmployersDataRetrievalService.LargeEmployersForEmployerIds(uniqueEmployerIds);
        }

        #region LARS

        private string LARSCurrentVersion()
        {
            return _LARSContext.LARS_Version.Select(lv => lv.MainDataSchemaName).Max();
        }

        private IDictionary<string, LARSLearningDelivery> LARSLearningDelivery(IEnumerable<string> learnAimRefs)
        {
            return
                _LARSContext.LARS_LearningDelivery
                .Where(ld => learnAimRefs.Contains(ld.LearnAimRef))
                .ToDictionary(a => a.LearnAimRef, ld => new LARSLearningDelivery
                {
                    LearnAimRef = ld.LearnAimRef,
                    LearnAimRefType = ld.LearnAimRefType,
                    NotionalNVQLevelv2 = ld.NotionalNVQLevelv2,
                    RegulatedCreditValue = ld.RegulatedCreditValue,
                    EnglandFEHEStatus = ld.EnglandFEHEStatus,
                    EnglPrscID = ld.EnglPrscID,
                    FrameworkCommonComponent = ld.FrameworkCommonComponent,
                });
        }

        private IDictionary<string, IEnumerable<LARSFunding>> LARSFunding(IEnumerable<string> learnAimRefs)
        {
            return
                _LARSContext.LARS_Funding
                .Where(lf => learnAimRefs.Contains(lf.LearnAimRef)).GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(lf => new LARSFunding
                {
                    LearnAimRef = lf.LearnAimRef,
                    FundingCategory = lf.FundingCategory,
                    EffectiveFrom = lf.EffectiveFrom,
                    EffectiveTo = lf.EffectiveTo,
                    RateWeighted = lf.RateWeighted,
                    WeightingFactor = lf.WeightingFactor,
                    RateUnWeighted = lf.RateUnWeighted,
                }).ToList() as IEnumerable<LARSFunding>);
        }

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

        private string OrgVersion()
        {
            return _organisationContext.Org_Version.Select(lv => lv.MainDataSchemaName).Max();
        }

        private IDictionary<long, IEnumerable<OrgFunding>> OrgFunding(IEnumerable<long> orgUkprns)
        {
            return
                _organisationContext.Org_Funding
                .Where(o => orgUkprns.Contains(o.UKPRN) && o.FundingFactorType == "Adult Skills").GroupBy(u => u.UKPRN)
                .ToDictionary(a => a.Key, a => a.Select(of => new OrgFunding
                {
                    UKPRN = of.UKPRN,
                    OrgFundFactor = of.FundingFactor,
                    OrgFundFactType = of.FundingFactorType,
                    OrgFundFactValue = of.FundingFactorValue,
                    OrgFundEffectiveFrom = of.EffectiveFrom,
                    OrgFundEffectiveTo = of.EffectiveTo,
                }).ToList() as IEnumerable<OrgFunding>);
        }
    }

}