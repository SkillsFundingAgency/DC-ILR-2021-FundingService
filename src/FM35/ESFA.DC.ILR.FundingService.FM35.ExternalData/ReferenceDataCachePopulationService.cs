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
using IReferenceDataCache = ESFA.DC.ILR.FundingService.Data.Interface.IReferenceDataCache;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData
{
    public class ReferenceDataCachePopulationService : IReferenceDataCachePopulationService
    {
        private readonly IReferenceDataCache _referenceDataCache;
        private readonly ILARS _LARSContext;
        private readonly IPostcodes _postcodesContext;
        private readonly IOrganisations _organisationContext;
        private readonly ILargeEmployer _largeEmployerContext;

        public ReferenceDataCachePopulationService(IReferenceDataCache referenceDataCache, ILARS LARSContext, IPostcodes postcodesContext, IOrganisations organisationContext, ILargeEmployer largeEmployerContext)
        {
            _referenceDataCache = referenceDataCache;
            _LARSContext = LARSContext;
            _postcodesContext = postcodesContext;
            _organisationContext = organisationContext;
            _largeEmployerContext = largeEmployerContext;
        }

        public void Populate(IList<string> learnAimRefs, IList<string> postcodes, IList<long> orgUkprns, IList<int> lEmpIDs)
        {
            var referenceDataCache = (ReferenceDataCache)_referenceDataCache;

            referenceDataCache.LARSCurrentVersion = LARSCurrentVersion();
            referenceDataCache.LARSAnnualValue = LARSAnnualValue(learnAimRefs);
            referenceDataCache.LARSLearningDelivery = LARSLearningDelivery(learnAimRefs);
            referenceDataCache.LARSLearningDeliveryCatgeory = LARSLearningDeliveryCategory(learnAimRefs);
            referenceDataCache.LARSFrameworkAims = LARSFrameworkAims(learnAimRefs);
            referenceDataCache.LARSFunding = LARSFunding(learnAimRefs);

            referenceDataCache.PostcodeCurrentVersion = PostcodesVersion();
            referenceDataCache.SfaAreaCost = SFAAreaCost(postcodes);
            referenceDataCache.SfaDisadvantage = SfaDisadvantage(postcodes);

            referenceDataCache.OrgVersion = OrgVersion();
            referenceDataCache.OrgFunding = OrgFunding(orgUkprns);

            referenceDataCache.LargeEmployers = LargeEmployers(lEmpIDs);
        }

        private string LARSCurrentVersion()
        {
            return _LARSContext.LARS_Version.Select(lv => lv.MainDataSchemaName).Max();
        }

        private IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValue(IList<string> learnAimRefs)
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

        private IDictionary<string, LARSLearningDelivery> LARSLearningDelivery(IList<string> learnAimRefs)
        {
            return
                _LARSContext.LARS_LearningDelivery
                .Where(ld => learnAimRefs.Contains(ld.LearnAimRef))
                .ToDictionary(a => a.LearnAimRef, ld => new LARSLearningDelivery
                {
                    LearnAimRef = ld.LearnAimRef,
                    EnglandFEHEStatus = ld.EnglandFEHEStatus,
                    EnglPrscID = ld.EnglPrscID,
                    FrameworkCommonComponent = ld.FrameworkCommonComponent,
                });
        }

        private IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCategory(IList<string> learnAimRefs)
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

        private IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAims(IList<string> learnAimRefs)
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

        private IDictionary<string, IEnumerable<LARSFunding>> LARSFunding(IList<string> learnAimRefs)
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

        private string PostcodesVersion()
        {
            return _postcodesContext.VersionInfos.Select(p => p.VersionNumber).Max();
        }

        private IDictionary<string, IEnumerable<SfaAreaCost>> SFAAreaCost(IList<string> postcodes)
        {
            return
                _postcodesContext.SFA_PostcodeAreaCost
                .Where(p => postcodes.Contains(p.Postcode)).GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(sac => new SfaAreaCost
                {
                    Postcode = sac.Postcode,
                    AreaCostFactor = sac.AreaCostFactor,
                    EffectiveFrom = sac.EffectiveFrom,
                    EffectiveTo = sac.EffectiveTo,
                }).ToList() as IEnumerable<SfaAreaCost>);
        }

        private IDictionary<string, IEnumerable<SfaDisadvantage>> SfaDisadvantage(IList<string> postcodes)
        {
            return
                _postcodesContext.SFA_PostcodeDisadvantage
                .Where(p => postcodes.Contains(p.Postcode)).GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(sd => new SfaDisadvantage
                {
                    Postcode = sd.Postcode,
                    Uplift = sd.Uplift,
                    EffectiveFrom = sd.EffectiveFrom,
                    EffectiveTo = sd.EffectiveTo,
                }).ToList() as IEnumerable<SfaDisadvantage>);
        }

        private string OrgVersion()
        {
            return _organisationContext.Org_Version.Select(lv => lv.MainDataSchemaName).Max();
        }

        private IDictionary<long, IEnumerable<OrgFunding>> OrgFunding(IList<long> orgUkprns)
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

        private IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployers(IList<int> lEmpIDs)
        {
            return
                _largeEmployerContext.LEMP_Employers
                .Where(l => lEmpIDs.Contains(l.ERN)).GroupBy(e => e.ERN)
                .ToDictionary(a => a.Key, a => a.Select(le => new LargeEmployers
                {
                    ERN = le.ERN,
                    EffectiveFrom = le.EffectiveFrom,
                    EffectiveTo = le.EffectiveTo,
                }).ToList() as IEnumerable<LargeEmployers>);
        }
    }
}
