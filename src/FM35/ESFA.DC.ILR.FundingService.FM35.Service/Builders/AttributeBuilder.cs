using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface.Builders;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Builders
{
    public class AttributeBuilder : IAttributeBuilder<IAttributeData>
    {
        // Global
        private const string LARSVersion = "LARSVersion";
        private const string OrgVersion = "OrgVersion";
        private const string UKPRN = "UKPRN";
        private const string PostcodeDisadvantageVersion = "PostcodeDisadvantageVersion";

        // OrgFunding
        private const string OrgFundEffectiveTo = "OrgFundEffectiveTo";
        private const string OrgFundEffectiveFrom = "OrgFundEffectiveFrom";
        private const string OrgFundFactor = "OrgFundFactor";
        private const string OrgFundFactValue = "OrgFundFactValue";
        private const string OrgFundFactType = "OrgFundFactType";

        // Learner
        private const string LearnRefNumber = "LearnRefNumber";
        private const string DateOfBirth = "DateOfBirth";

        // Learner Employment Status
        private const string EmpId = "EmpId";
        private const string DateEmpStatApp = "DateEmpStatApp";

        // Large Employer
        private const string LargeEmpEffectiveFrom = "LargeEmpEffectiveFrom";
        private const string LargeEmpEffectiveTo = "LargeEmpEffectiveTo";

        // SFA Postcode Disadvantage
        private const string DisUplift = "DisUplift";
        private const string DisUpEffectiveFrom = "DisUpEffectiveFrom";
        private const string DisUpEffectiveTo = "DisUpEffectiveTo";

        // LearningDelivery
        private const string AchDate = "AchDate";
        private const string AddHours = "AddHours";
        private const string AimSeqNumber = "AimSeqNumber";
        private const string AimType = "AimType";
        private const string CompStatus = "CompStatus";
        private const string EmpOutcome = "EmpOutcome";
        private const string EnglandFEHEStatus = "EnglandFEHEStatus";
        private const string EnglPrscID = "EnglPrscID";
        private const string FworkCode = "FworkCode";
        private const string FrameworkCommonComponent = "FrameworkCommonComponent";
        private const string FrameworkComponentType = "FrameworkComponentType";
        private const string LearnActEndDate = "LearnActEndDate";
        private const string LearnPlanEndDate = "LearnPlanEndDate";
        private const string LearnStartDate = "LearnStartDate";
        private const string LrnDelFAM_EEF = "LrnDelFAM_EEF";
        private const string LrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        private const string LrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        private const string LrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        private const string LrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        private const string LrnDelFAM_FFI = "LrnDelFAM_FFI";
        private const string LrnDelFAM_RES = "LrnDelFAM_RES";
        private const string OrigLearnStartDate = "OrigLearnStartDate";
        private const string OtherFundAdj = "OtherFundAdj";
        private const string Outcome = "Outcome";
        private const string PriorLearnFundAdj = "PriorLearnFundAdj";
        private const string ProgType = "ProgType";
        private const string PwayCode = "PwayCode";

        // LearningDeliveryFAM
        private const string LearnDelFAMCode = "LearnDelFAMCode";
        private const string LearnDelFAMDateFrom = "LearnDelFAMDateFrom";
        private const string LearnDelFAMDateTo = "LearnDelFAMDateTo";
        private const string LearnDelFAMType = "LearnDelFAMType";

        // LearningDeliveryLARSCategory
        private const string LearnDelCatRef = "LearnDelCatRef";
        private const string LearnDelCatDateFrom = "LearnDelCatDateFrom";
        private const string LearnDelCatDateTo = "LearnDelCatDateTo";

        // LearningDeliveryLARSAnnualValue
        private const string LearnDelAnnValBasicSkillsTypeCode = "LearnDelAnnValBasicSkillsTypeCode";
        private const string LearnDelAnnValDateFrom = "LearnDelAnnValDateFrom";
        private const string LearnDelAnnValDateTo = "LearnDelAnnValDateTo";

        // LearningDeliveryLARSFunding
        private const string LARSFundCategory = "LARSFundCategory";
        private const string LARSFundEffectiveFrom = "LARSFundEffectiveFrom";
        private const string LARSFundEffectiveTo = "LARSFundEffectiveTo";
        private const string LARSFundUnweightedRate = "LARSFundUnweightedRate";
        private const string LARSFundWeightedRate = "LARSFundWeightedRate";
        private const string LARSFundWeightingFactor = "LARSFundWeightingFactor";

        // SFAPostcodeAreaCost
        private const string AreaCosEffectiveFrom = "AreaCosEffectiveFrom";
        private const string AreaCosEffectiveTo = "AreaCosEffectiveTo";
        private const string AreaCosFactor = "AreaCosFactor";

        public IDictionary<string, IAttributeData> BuildGlobalAttributes(int ukprn, string larsVersion, string orgVersion, string postcodeDisadvantageVersion)
        {
            return new Dictionary<string, IAttributeData>
            {
                { UKPRN,  new AttributeData(UKPRN, ukprn) },
                { LARSVersion, new AttributeData(LARSVersion, larsVersion) },
                { OrgVersion, new AttributeData(OrgVersion, orgVersion) },
                { PostcodeDisadvantageVersion,  new AttributeData(PostcodeDisadvantageVersion, postcodeDisadvantageVersion) },
            };
        }

        public IDictionary<string, IAttributeData> BuildOrgFundingAttributes(DateTime orgFundEffectiveFrom, DateTime? orgFundEffectiveTo, string orgFundFactor, string orgFundFactType, string orgFundFactValue)
        {
            return new Dictionary<string, IAttributeData>
            {
                { OrgFundEffectiveFrom,  new AttributeData(OrgFundEffectiveFrom, orgFundEffectiveFrom) },
                { OrgFundEffectiveTo, new AttributeData(OrgFundEffectiveTo, orgFundEffectiveTo) },
                { OrgFundFactor, new AttributeData(OrgFundFactor, orgFundFactor) },
                { OrgFundFactType,  new AttributeData(OrgFundFactType, orgFundFactType) },
                { OrgFundFactValue,  new AttributeData(OrgFundFactValue, orgFundFactValue) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearnerAttributes(string learnRefNumber, DateTime? dateOfBirth)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnRefNumber, new AttributeData(LearnRefNumber, learnRefNumber) },
                { DateOfBirth, new AttributeData(DateOfBirth, dateOfBirth) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearnerEmploymentStatusAttributes(long? empId, DateTime? dateEmpStatApp)
        {
            return new Dictionary<string, IAttributeData>
            {
                { EmpId, new AttributeData(EmpId, empId) },
                { DateEmpStatApp, new AttributeData(DateEmpStatApp, dateEmpStatApp) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLLargeEmployerReferenceDataAttributes(DateTime? largeEmpEffectiveFrom, DateTime? largeEmpEffectiveTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LargeEmpEffectiveFrom, new AttributeData(LargeEmpEffectiveFrom, largeEmpEffectiveFrom) },
                { LargeEmpEffectiveTo, new AttributeData(LargeEmpEffectiveTo, largeEmpEffectiveTo) },
            };
        }

        public IDictionary<string, IAttributeData> BuildSFAPostcodeDisadvantageAttributes(decimal? disUplift, DateTime? disUpEffectiveFrom, DateTime? disUpEffectiveTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { DisUplift, new AttributeData(DisUplift, disUplift) },
                { DisUpEffectiveFrom, new AttributeData(DisUpEffectiveFrom, disUpEffectiveFrom) },
                { DisUpEffectiveTo, new AttributeData(DisUpEffectiveTo, disUpEffectiveTo) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryAttributes(
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
            long? pwayCode)
        {
            return new Dictionary<string, IAttributeData>
            {
                { AchDate, new AttributeData(AchDate, achDate) },
                { AddHours, new AttributeData(AddHours, addHours) },
                { AimSeqNumber, new AttributeData(AimSeqNumber, aimSeqNumber) },
                { AimType, new AttributeData(AimType, aimType) },
                { CompStatus, new AttributeData(CompStatus, compStatus) },
                { EmpOutcome, new AttributeData(EmpOutcome, empOutcome) },
                { EnglandFEHEStatus, new AttributeData(EnglandFEHEStatus, englandFEHEStatus) },
                { EnglPrscID, new AttributeData(EnglPrscID, englPrscID) },
                { FworkCode, new AttributeData(FworkCode, fworkCode) },
                { FrameworkCommonComponent, new AttributeData(FrameworkCommonComponent, frameworkCommonComponent) },
                { FrameworkComponentType, new AttributeData(FrameworkComponentType, frameworkComponentType) },
                { LearnActEndDate, new AttributeData(LearnActEndDate, learnActEndDate) },
                { LearnPlanEndDate, new AttributeData(LearnPlanEndDate, learnPlanEndDate) },
                { LearnStartDate, new AttributeData(LearnStartDate, learnStartDate) },
                { LrnDelFAM_EEF, new AttributeData(LrnDelFAM_EEF, lrnDelFAM_EEF) },
                { LrnDelFAM_LDM1, new AttributeData(LrnDelFAM_LDM1, lrnDelFAM_LDM1) },
                { LrnDelFAM_LDM2, new AttributeData(LrnDelFAM_LDM2, lrnDelFAM_LDM2) },
                { LrnDelFAM_LDM3, new AttributeData(LrnDelFAM_LDM3, lrnDelFAM_LDM3) },
                { LrnDelFAM_LDM4, new AttributeData(LrnDelFAM_LDM4, lrnDelFAM_LDM4) },
                { LrnDelFAM_FFI, new AttributeData(LrnDelFAM_FFI, lrnDelFAM_FFI) },
                { LrnDelFAM_RES, new AttributeData(LrnDelFAM_RES, lrnDelFAM_RES) },
                { OrigLearnStartDate, new AttributeData(OrigLearnStartDate, origLearnStartDate) },
                { OtherFundAdj, new AttributeData(OtherFundAdj, otherFundAdj) },
                { Outcome, new AttributeData(Outcome, outcome) },
                { PriorLearnFundAdj, new AttributeData(PriorLearnFundAdj, priorLearnFundAdj) },
                { ProgType, new AttributeData(ProgType, progType) },
                { PwayCode, new AttributeData(PwayCode, pwayCode) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryFAMAttributes(string learnDelFAMCode, DateTime? learnDelFAMDateFrom, DateTime? learnDelFAMDateTo, string learnDelFAMType)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnDelFAMCode, new AttributeData(LearnDelFAMCode, learnDelFAMCode) },
                { LearnDelFAMDateFrom, new AttributeData(LearnDelFAMDateFrom, learnDelFAMDateFrom) },
                { LearnDelFAMDateTo, new AttributeData(LearnDelFAMDateTo, learnDelFAMDateTo) },
                { LearnDelFAMType, new AttributeData(LearnDelFAMType, learnDelFAMType) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryLARSAnnualValueAttributes(long? learnDelAnnValBasicSkillsTypeCode, DateTime learnDelAnnValDateFrom, DateTime? learnDelAnnValDateTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnDelAnnValBasicSkillsTypeCode, new AttributeData(LearnDelAnnValBasicSkillsTypeCode, learnDelAnnValBasicSkillsTypeCode) },
                { LearnDelAnnValDateFrom, new AttributeData(LearnDelAnnValDateFrom, learnDelAnnValDateFrom) },
                { LearnDelAnnValDateTo, new AttributeData(LearnDelAnnValDateTo, learnDelAnnValDateTo) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryLARSCategoryAttributes(long? learnDelCatRef, DateTime learnDelCatDateFrom, DateTime? learnDelCatDateTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnDelCatRef, new AttributeData(LearnDelCatRef, learnDelCatRef) },
                { LearnDelCatDateFrom, new AttributeData(LearnDelCatDateFrom, learnDelCatDateFrom) },
                { LearnDelCatDateTo, new AttributeData(LearnDelCatDateTo, learnDelCatDateTo) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryLarsFundingAttributes(string larsFundCategory, DateTime larsFundEffectiveFrom, DateTime? larsFundEffectiveTo, decimal? larsFundUnWeightedRate, decimal? larsFundWeightedRate, string larsFundWeightingFactor)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LARSFundCategory, new AttributeData(LARSFundCategory, larsFundCategory) },
                { LARSFundEffectiveFrom, new AttributeData(LARSFundEffectiveFrom, larsFundEffectiveFrom) },
                { LARSFundEffectiveTo, new AttributeData(LARSFundEffectiveTo, larsFundEffectiveTo) },
                { LARSFundUnweightedRate, new AttributeData(LARSFundUnweightedRate, larsFundUnWeightedRate) },
                { LARSFundWeightedRate, new AttributeData(LARSFundWeightedRate, larsFundWeightedRate) },
                { LARSFundWeightingFactor, new AttributeData(LARSFundWeightingFactor, larsFundWeightingFactor) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliverySfaAreaCostAttributes(DateTime? areaCosEffectiveFrom, DateTime? areaCosEffectiveTo, decimal areaCosFactor)
        {
            return new Dictionary<string, IAttributeData>
            {
                { AreaCosEffectiveFrom, new AttributeData(AreaCosEffectiveFrom, areaCosEffectiveFrom) },
                { AreaCosEffectiveTo, new AttributeData(AreaCosEffectiveTo, areaCosEffectiveTo) },
                { AreaCosFactor, new AttributeData(AreaCosFactor, areaCosFactor) },
            };
        }
    }
}
