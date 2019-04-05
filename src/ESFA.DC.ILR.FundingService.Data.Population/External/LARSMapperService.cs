using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using LARSLearningDelivery = ESFA.DC.ILR.FundingService.Data.External.LARS.Model.LARSLearningDelivery;
using LARSLearningDeliveryInput = ESFA.DC.ILR.ReferenceDataService.Model.LARS.LARSLearningDelivery;
using LARSStandard = ESFA.DC.ILR.FundingService.Data.External.LARS.Model.LARSStandard;
using LARSStandardInput = ESFA.DC.ILR.ReferenceDataService.Model.LARS.LARSStandard;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class LARSMapperService : ILARSMapperService
    {
        public LARSMapperService()
        {
        }

        public IDictionary<string, LARSLearningDelivery> MapLARSLearningDeliveries(IReadOnlyCollection<LARSLearningDeliveryInput> larsLearningDeliveries)
        {
            return larsLearningDeliveries?
                 .ToDictionary(
                laf => laf.LearnAimRef,
                ld => new LARSLearningDelivery
                {
                    LearnAimRef = ld.LearnAimRef,
                    LearnAimRefTitle = ld.LearnAimRefTitle,
                    LearnAimRefType = ld.LearnAimRefType,
                    LearningDeliveryGenre = ld.LearningDeliveryGenre,
                    NotionalNVQLevelv2 = ld.NotionalNVQLevelv2,
                    RegulatedCreditValue = ld.RegulatedCreditValue,
                    EnglandFEHEStatus = ld.EnglandFEHEStatus,
                    EnglPrscID = ld.EnglPrscID,
                    FrameworkCommonComponent = ld.FrameworkCommonComponent,
                    AwardOrgCode = ld.AwardOrgCode,
                    EFACOFType = ld.EFACOFType,
                    SectorSubjectAreaTier2 = ld.SectorSubjectAreaTier2,
                    LARSAnnualValues = ld.LARSAnnualValues?.Select(LARSAnnualValueFromEntity).ToList(),
                    LARSCareerLearningPilots = ld.LARSCareerLearningPilots?.Select(LARSCareerLearningPilotFromEntity).ToList(),
                    LARSLearningDeliveryCategories = ld.LARSLearningDeliveryCategories?.Select(LARSLearningDeliveryCategoryFromEntity).ToList(),
                    LARSFrameworkAims = ld.LARSFrameworkAims?.Select(LARSFrameworkAimFromEntity).ToList(),
                    LARSFundings = ld.LARSFundings?.Select(LARSFundingFromEntity).ToList(),
                    LARSValidities = ld.LARSValidities?.Select(LARSValidityFromEntity).ToList()
                });
        }

        public IDictionary<int, LARSStandard> MapLARSStandards(IReadOnlyCollection<LARSStandardInput> larsStandards)
        {
            return larsStandards?
                .ToDictionary(
                s => s.StandardCode,
                ls => new LARSStandard
                {
                    StandardCode = ls.StandardCode,
                    StandardSectorCode = ls.StandardSectorCode,
                    NotionalEndLevel = ls.NotionalEndLevel,
                    LARSStandardFundings = ls.LARSStandardFundings?.Select(LARSStandardFundingFromEntity).ToList(),
                    LARSStandardApprenticeshipFundings = ls.LARSStandardApprenticeshipFundings?.Select(LARSStandardApprenticeshipFundingFromEntity).ToList(),
                    LARSStandardCommonComponents = ls.LARSStandardCommonComponents?.Select(LARSStandardCommonComponentFromEntity).ToList()
                });
        }

        private Data.External.LARS.Model.LARSAnnualValue LARSAnnualValueFromEntity(LARSAnnualValue entity)
        {
            return new Data.External.LARS.Model.LARSAnnualValue()
            {
                BasicSkillsType = entity.BasicSkillsType,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        private Data.External.LARS.Model.LARSCareerLearningPilot LARSCareerLearningPilotFromEntity(LARSCareerLearningPilot entity)
        {
            return new Data.External.LARS.Model.LARSCareerLearningPilot()
            {
                AreaCode = entity.AreaCode,
                SubsidyRate = entity.SubsidyRate,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        private Data.External.LARS.Model.LARSLearningDeliveryCategory LARSLearningDeliveryCategoryFromEntity(LARSLearningDeliveryCategory entity)
        {
            return new Data.External.LARS.Model.LARSLearningDeliveryCategory()
            {
                CategoryRef = entity.CategoryRef,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        private Data.External.LARS.Model.LARSFrameworkAim LARSFrameworkAimFromEntity(LARSFrameworkAim entity)
        {
            return new Data.External.LARS.Model.LARSFrameworkAim()
            {
                FworkCode = entity.FworkCode,
                ProgType = entity.ProgType,
                PwayCode = entity.PwayCode,
                FrameworkComponentType = entity.FrameworkComponentType,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                LARSFramework = LARSFrameworkFromEntity(entity.LARSFramework)
            };
        }

        private Data.External.LARS.Model.LARSFramework LARSFrameworkFromEntity(LARSFramework entity)
        {
            return new Data.External.LARS.Model.LARSFramework
            {
                EffectiveFromNullable = entity.EffectiveFromNullable,
                EffectiveTo = entity.EffectiveTo,
                LARSFrameworkApprenticeshipFundings = entity.LARSFrameworkApprenticeshipFundings?.Select(LARSFrameworkApprenticeshipFundingFromEntity).ToList(),
                LARSFrameworkCommonComponents = entity.LARSFrameworkCommonComponents?.Select(LARSFrameworkCommonComponentFromEntity).ToList(),
            };
        }

        private Data.External.LARS.Model.LARSFrameworkApprenticeshipFunding LARSFrameworkApprenticeshipFundingFromEntity(LARSFrameworkApprenticeshipFunding entity)
        {
            return new Data.External.LARS.Model.LARSFrameworkApprenticeshipFunding
            {
                BandNumber = entity.BandNumber,
                CareLeaverAdditionalPayment = entity.CareLeaverAdditionalPayment,
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                CoreGovContributionCap = entity.CoreGovContributionCap,
                SixteenToEighteenIncentive = entity.SixteenToEighteenIncentive,
                SixteenToEighteenProviderAdditionalPayment = entity.SixteenToEighteenProviderAdditionalPayment,
                SixteenToEighteenEmployerAdditionalPayment = entity.SixteenToEighteenEmployerAdditionalPayment,
                SixteenToEighteenFrameworkUplift = entity.SixteenToEighteenFrameworkUplift,
                Duration = entity.Duration,
                ReservedValue2 = entity.ReservedValue2,
                ReservedValue3 = entity.ReservedValue3,
                MaxEmployerLevyCap = entity.MaxEmployerLevyCap,
                FundableWithoutEmployer = entity.FundableWithoutEmployer
            };
        }

        private Data.External.LARS.Model.LARSFrameworkCommonComponent LARSFrameworkCommonComponentFromEntity(LARSFrameworkCommonComponent entity)
        {
            return new Data.External.LARS.Model.LARSFrameworkCommonComponent
            {
                CommonComponent = entity.CommonComponent,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo
            };
        }

        private Data.External.LARS.Model.LARSFunding LARSFundingFromEntity(LARSFunding entity)
        {
            return new Data.External.LARS.Model.LARSFunding
            {
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                RateWeighted = entity.RateWeighted,
                WeightingFactor = entity.WeightingFactor,
                RateUnWeighted = entity.RateUnWeighted
            };
        }

        private Data.External.LARS.Model.LARSValidity LARSValidityFromEntity(LARSValidity entity)
        {
            return new Data.External.LARS.Model.LARSValidity()
            {
                Category = entity.ValidityCategory,
                LastNewStartDate = entity.LastNewStartDate,
                StartDate = entity.StartDate,
            };
        }

        private Data.External.LARS.Model.LARSStandardFunding LARSStandardFundingFromEntity(LARSStandardFunding entity)
        {
            return new Data.External.LARS.Model.LARSStandardFunding()
            {
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                AchievementIncentive = entity.AchievementIncentive,
                BandNumber = entity.BandNumber,
                CoreGovContributionCap = entity.CoreGovContributionCap,
                FundableWithoutEmployer = entity.FundableWithoutEmployer,
                SixteenToEighteenIncentive = entity.SixteenToEighteenIncentive,
                SmallBusinessIncentive = entity.SmallBusinessIncentive
            };
        }

        private Data.External.LARS.Model.LARSStandardApprenticeshipFunding LARSStandardApprenticeshipFundingFromEntity(LARSStandardApprenticeshipFunding entity)
        {
            return new Data.External.LARS.Model.LARSStandardApprenticeshipFunding
            {
                BandNumber = entity.BandNumber,
                CareLeaverAdditionalPayment = entity.CareLeaverAdditionalPayment,
                ProgType = entity.ProgType,
                PwayCode = entity.PwayCode,
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                CoreGovContributionCap = entity.CoreGovContributionCap,
                SixteenToEighteenIncentive = entity.SixteenToEighteenIncentive,
                SixteenToEighteenProviderAdditionalPayment = entity.SixteenToEighteenProviderAdditionalPayment,
                SixteenToEighteenEmployerAdditionalPayment = entity.SixteenToEighteenEmployerAdditionalPayment,
                SixteenToEighteenFrameworkUplift = entity.SixteenToEighteenFrameworkUplift,
                Duration = entity.Duration,
                ReservedValue2 = entity.ReservedValue2,
                ReservedValue3 = entity.ReservedValue3,
                MaxEmployerLevyCap = entity.MaxEmployerLevyCap,
                FundableWithoutEmployer = entity.FundableWithoutEmployer
            };
        }

        private Data.External.LARS.Model.LARSStandardCommonComponent LARSStandardCommonComponentFromEntity(LARSStandardCommonComponent entity)
        {
            return new Data.External.LARS.Model.LARSStandardCommonComponent()
            {
                CommonComponent = entity.CommonComponent,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}
