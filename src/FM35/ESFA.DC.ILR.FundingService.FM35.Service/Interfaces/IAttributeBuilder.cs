using System;
using System.Collections.Generic;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Interfaces
{
    public interface IAttributeBuilder<T>
    {
        IDictionary<string, IAttributeData> BuildGlobalAttributes(int ukprn, string larsVersion, string orgVersion, string postcodeDisadvantageVersion);

        IDictionary<string, IAttributeData> BuildOrgFundingAttributes(
            DateTime orgFundEffectiveFrom,
            DateTime? orgFundEffectiveTo,
            string orgFundFactor,
            string orgFundFactType,
            string orgFundFactValue);

        IDictionary<string, IAttributeData> BuildLearnerAttributes(string learnRefNumber, DateTime? dateOfBirth);

        IDictionary<string, IAttributeData> BuildLearnerEmploymentStatusAttributes(long? empId, DateTime? dateEmpStatApp);

        IDictionary<string, IAttributeData> BuildLLargeEmployerReferenceDataAttributes(DateTime? largeEmpEffectiveFrom, DateTime? largeEmpEffectiveTo);

        IDictionary<string, IAttributeData> BuildSFAPostcodeDisadvantageAttributes(decimal? disUplift, DateTime? disUpEffectiveFrom, DateTime? disUpEffectiveTo);

        IDictionary<string, IAttributeData> BuildLearningDeliveryAttributes(
            DateTime? achDate,
            long? addHours,
            long? aimSeqNumber,
            long? aimType,
            long? compStatus,
            long? empOutcome,
            string englandFEHEStatus,
            long? englPrscID,
            long? fworkCode,
            long? frameworkCommonComponent,
            long? frameworkComponentType,
            DateTime? learnActEndDate,
            DateTime? learnPlanEndDate,
            DateTime? learnStartDate,
            long? lrnDelFAM_EEF,
            long? lrnDelFAM_LDM1,
            long? lrnDelFAM_LDM2,
            long? lrnDelFAM_LDM3,
            long? lrnDelFAM_LDM4,
            long? lrnDelFAM_FFI,
            long? lrnDelFAM_RES,
            DateTime? origLearnStartDate,
            long? otherFundAdj,
            long? outcome,
            long? priorLearnFundAdj,
            long? progType,
            long? pwayCode);

        IDictionary<string, IAttributeData> BuildLearningDeliveryFAMAttributes(
            string learnDelFAMCode,
            DateTime? learnDelFAMDateFrom,
            DateTime? learnDelFAMDateTo,
            string learnDelFAMType);

        IDictionary<string, IAttributeData> BuildLearningDeliveryLARSCategoryAttributes(long? learnDelCatRef, DateTime learnDelCatDateFrom, DateTime? learnDelCatDateTo);

        IDictionary<string, IAttributeData> BuildLearningDeliveryLARSAnnualValueAttributes(long? learnDelAnnValBasicSkillsTypeCode, DateTime learnDelAnnValDateFrom, DateTime? learnDelAnnValDateTo);

        IDictionary<string, IAttributeData> BuildLearningDeliveryLarsFundingAttributes(
            string larsFundCategory,
            DateTime larsFundEffectiveFrom,
            DateTime? larsFundEffectiveTo,
            decimal? larsFundUnWeightedRate,
            decimal? larsFundWeightedRate,
            string larsFundWeightingFactor);

        IDictionary<string, IAttributeData> BuildLearningDeliverySfaAreaCostAttributes(
            DateTime? areaCosEffectiveFrom,
            DateTime? areaCosEffectiveTo,
            decimal areaCosFactor);
    }
}
