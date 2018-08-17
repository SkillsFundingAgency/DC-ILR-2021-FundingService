using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class LARSDataRetrievalService : ILARSDataRetrievalService
    {
        private readonly ILARS _lars;

        public LARSDataRetrievalService()
        {
        }

        public LARSDataRetrievalService(ILARS lars)
        {
            _lars = lars;
        }

        public virtual IQueryable<LARS_Funding> LARSFundings => _lars.LARS_Funding;

        public virtual IQueryable<LARS_LearningDelivery> LARSLearningDeliveries => _lars.LARS_LearningDelivery;

        public virtual IQueryable<LARS_Version> LARSVersions => _lars.LARS_Version;

        public virtual IQueryable<LARS_AnnualValue> LARSAnnualValues => _lars.LARS_AnnualValue;

        public virtual IQueryable<LARS_LearningDeliveryCategory> LARSLearningDeliveryCategories => _lars.LARS_LearningDeliveryCategory;

        public virtual IQueryable<LARS_FrameworkAims> LARSFrameworkAims => _lars.LARS_FrameworkAims;

        public string CurrentVersion()
        {
            return LARSVersions.OrderByDescending(v => v.MainDataSchemaName).Select(lv => lv.MainDataSchemaName).FirstOrDefault();
        }

        public IEnumerable<string> UniqueLearnAimRefs(IMessage message)
        {
            return message
                .Learners
                .Where(l => l.LearningDeliveries != null)
                .SelectMany(l => l.LearningDeliveries)
                .Select(ld => ld.LearnAimRef)
                .Distinct();
        }

        public IDictionary<string, IEnumerable<LARSFunding>> LARSFundingsForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSFundings
                .Where(lf => learnAimRefs.Contains(lf.LearnAimRef))
                .GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(LARSFundingFromEntity).ToList() as IEnumerable<LARSFunding>);
        }

        public LARSFunding LARSFundingFromEntity(LARS_Funding entity)
        {
            return new LARSFunding
            {
                LearnAimRef = entity.LearnAimRef,
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                RateWeighted = entity.RateWeighted,
                WeightingFactor = entity.WeightingFactor,
                RateUnWeighted = entity.RateUnWeighted,
            };
        }

        public IDictionary<string, LARSLearningDelivery> LARSLearningDeliveriesForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSLearningDeliveries
                    .Where(ld => learnAimRefs.Contains(ld.LearnAimRef))
                    .ToDictionary(a => a.LearnAimRef, LARSLearningDeliveryFromEntity);
        }

        public LARSLearningDelivery LARSLearningDeliveryFromEntity(LARS_LearningDelivery entity)
        {
            return new LARSLearningDelivery
            {
                LearnAimRef = entity.LearnAimRef,
                LearnAimRefTitle = entity.LearnAimRefTitle,
                LearnAimRefType = entity.LearnAimRefType,
                NotionalNVQLevelv2 = entity.NotionalNVQLevelv2,
                RegulatedCreditValue = entity.RegulatedCreditValue,
                EnglandFEHEStatus = entity.EnglandFEHEStatus,
                EnglPrscID = entity.EnglPrscID,
                FrameworkCommonComponent = entity.FrameworkCommonComponent,
                AwardOrgCode = entity.AwardOrgCode,
                EFACOFType = entity.EFACOFType,
                SectorSubjectAreaTier2 = entity.SectorSubjectAreaTier2,
                LARSValidities = entity.LARS_Validity.Select(LARSValidityFromEntity).ToList()
            };
        }

        public LARSValidity LARSValidityFromEntity(LARS_Validity entity)
        {
            return new LARSValidity()
            {
                Category = entity.ValidityCategory,
                LastNewStartDate = entity.LastNewStartDate,
                StartDate = entity.StartDate,
            };
        }

        public IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValuesForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSAnnualValues
                .Where(la => learnAimRefs.Contains(la.LearnAimRef))
                .GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(LARSAnnualValueFromEntity).ToList() as IEnumerable<LARSAnnualValue>);
        }

        public LARSAnnualValue LARSAnnualValueFromEntity(LARS_AnnualValue entity)
        {
            return new LARSAnnualValue()
            {
                LearnAimRef = entity.LearnAimRef,
                BasicSkillsType = entity.BasicSkillsType,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCategoriesForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSLearningDeliveryCategories
                .Where(ld => learnAimRefs.Contains(ld.LearnAimRef))
                .GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(LARSLearningDeliveryCategoryFromEntity).ToList() as IEnumerable<LARSLearningDeliveryCategory>);
        }

        public LARSLearningDeliveryCategory LARSLearningDeliveryCategoryFromEntity(LARS_LearningDeliveryCategory entity)
        {
            return new LARSLearningDeliveryCategory()
            {
                LearnAimRef = entity.LearnAimRef,
                CategoryRef = entity.CategoryRef,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAimsForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSFrameworkAims
                .Where(lf => learnAimRefs.Contains(lf.LearnAimRef))
                .GroupBy(a => a.LearnAimRef)
                .ToDictionary(a => a.Key, a => a.Select(LARSFrameworkAimsFromEntity).ToList() as IEnumerable<LARSFrameworkAims>);
        }

        public LARSFrameworkAims LARSFrameworkAimsFromEntity(LARS_FrameworkAims entity)
        {
            return new LARSFrameworkAims()
            {
                LearnAimRef = entity.LearnAimRef,
                FworkCode = entity.FworkCode,
                ProgType = entity.ProgType,
                PwayCode = entity.PwayCode,
                FrameworkComponentType = entity.FrameworkComponentType,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}
