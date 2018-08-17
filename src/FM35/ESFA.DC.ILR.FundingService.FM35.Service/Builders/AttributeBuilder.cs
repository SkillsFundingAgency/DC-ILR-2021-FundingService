using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.Service.Interfaces;
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
                { UKPRN,  new AttributeData(ukprn) },
                { LARSVersion, new AttributeData(larsVersion) },
                { OrgVersion, new AttributeData(orgVersion) },
                { PostcodeDisadvantageVersion,  new AttributeData(postcodeDisadvantageVersion) },
            };
        }

        public IDictionary<string, IAttributeData> BuildOrgFundingAttributes(DateTime orgFundEffectiveFrom, DateTime? orgFundEffectiveTo, string orgFundFactor, string orgFundFactType, string orgFundFactValue)
        {
            return new Dictionary<string, IAttributeData>
            {
                { OrgFundEffectiveFrom,  new AttributeData(orgFundEffectiveFrom) },
                { OrgFundEffectiveTo, new AttributeData(orgFundEffectiveTo) },
                { OrgFundFactor, new AttributeData(orgFundFactor) },
                { OrgFundFactType,  new AttributeData(orgFundFactType) },
                { OrgFundFactValue,  new AttributeData(orgFundFactValue) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearnerAttributes(string learnRefNumber, DateTime? dateOfBirth)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnRefNumber, new AttributeData(learnRefNumber) },
                { DateOfBirth, new AttributeData(dateOfBirth) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearnerEmploymentStatusAttributes(long? empId, DateTime? dateEmpStatApp)
        {
            return new Dictionary<string, IAttributeData>
            {
                { EmpId, new AttributeData(empId) },
                { DateEmpStatApp, new AttributeData(dateEmpStatApp) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLLargeEmployerReferenceDataAttributes(DateTime? largeEmpEffectiveFrom, DateTime? largeEmpEffectiveTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LargeEmpEffectiveFrom, new AttributeData(largeEmpEffectiveFrom) },
                { LargeEmpEffectiveTo, new AttributeData(largeEmpEffectiveTo) },
            };
        }

        public IDictionary<string, IAttributeData> BuildSFAPostcodeDisadvantageAttributes(decimal? disUplift, DateTime? disUpEffectiveFrom, DateTime? disUpEffectiveTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { DisUplift, new AttributeData(disUplift) },
                { DisUpEffectiveFrom, new AttributeData(disUpEffectiveFrom) },
                { DisUpEffectiveTo, new AttributeData(disUpEffectiveTo) },
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
                { AchDate, new AttributeData(achDate) },
                { AddHours, new AttributeData(addHours) },
                { AimSeqNumber, new AttributeData(aimSeqNumber) },
                { AimType, new AttributeData(aimType) },
                { CompStatus, new AttributeData(compStatus) },
                { EmpOutcome, new AttributeData(empOutcome) },
                { EnglandFEHEStatus, new AttributeData(englandFEHEStatus) },
                { EnglPrscID, new AttributeData(englPrscID) },
                { FworkCode, new AttributeData(fworkCode) },
                { FrameworkCommonComponent, new AttributeData(frameworkCommonComponent) },
                { FrameworkComponentType, new AttributeData(frameworkComponentType) },
                { LearnActEndDate, new AttributeData(learnActEndDate) },
                { LearnPlanEndDate, new AttributeData(learnPlanEndDate) },
                { LearnStartDate, new AttributeData(learnStartDate) },
                { LrnDelFAM_EEF, new AttributeData(lrnDelFAM_EEF) },
                { LrnDelFAM_LDM1, new AttributeData(lrnDelFAM_LDM1) },
                { LrnDelFAM_LDM2, new AttributeData(lrnDelFAM_LDM2) },
                { LrnDelFAM_LDM3, new AttributeData(lrnDelFAM_LDM3) },
                { LrnDelFAM_LDM4, new AttributeData(lrnDelFAM_LDM4) },
                { LrnDelFAM_FFI, new AttributeData(lrnDelFAM_FFI) },
                { LrnDelFAM_RES, new AttributeData(lrnDelFAM_RES) },
                { OrigLearnStartDate, new AttributeData(origLearnStartDate) },
                { OtherFundAdj, new AttributeData(otherFundAdj) },
                { Outcome, new AttributeData(outcome) },
                { PriorLearnFundAdj, new AttributeData(priorLearnFundAdj) },
                { ProgType, new AttributeData(progType) },
                { PwayCode, new AttributeData(pwayCode) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryFAMAttributes(string learnDelFAMCode, DateTime? learnDelFAMDateFrom, DateTime? learnDelFAMDateTo, string learnDelFAMType)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnDelFAMCode, new AttributeData(learnDelFAMCode) },
                { LearnDelFAMDateFrom, new AttributeData(learnDelFAMDateFrom) },
                { LearnDelFAMDateTo, new AttributeData(learnDelFAMDateTo) },
                { LearnDelFAMType, new AttributeData(learnDelFAMType) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryLARSAnnualValueAttributes(long? learnDelAnnValBasicSkillsTypeCode, DateTime learnDelAnnValDateFrom, DateTime? learnDelAnnValDateTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnDelAnnValBasicSkillsTypeCode, new AttributeData(learnDelAnnValBasicSkillsTypeCode) },
                { LearnDelAnnValDateFrom, new AttributeData(learnDelAnnValDateFrom) },
                { LearnDelAnnValDateTo, new AttributeData(learnDelAnnValDateTo) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryLARSCategoryAttributes(long? learnDelCatRef, DateTime learnDelCatDateFrom, DateTime? learnDelCatDateTo)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LearnDelCatRef, new AttributeData(learnDelCatRef) },
                { LearnDelCatDateFrom, new AttributeData(learnDelCatDateFrom) },
                { LearnDelCatDateTo, new AttributeData(learnDelCatDateTo) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliveryLarsFundingAttributes(string larsFundCategory, DateTime larsFundEffectiveFrom, DateTime? larsFundEffectiveTo, decimal? larsFundUnWeightedRate, decimal? larsFundWeightedRate, string larsFundWeightingFactor)
        {
            return new Dictionary<string, IAttributeData>
            {
                { LARSFundCategory, new AttributeData(larsFundCategory) },
                { LARSFundEffectiveFrom, new AttributeData(larsFundEffectiveFrom) },
                { LARSFundEffectiveTo, new AttributeData(larsFundEffectiveTo) },
                { LARSFundUnweightedRate, new AttributeData(larsFundUnWeightedRate) },
                { LARSFundWeightedRate, new AttributeData(larsFundWeightedRate) },
                { LARSFundWeightingFactor, new AttributeData(larsFundWeightingFactor) },
            };
        }

        public IDictionary<string, IAttributeData> BuildLearningDeliverySfaAreaCostAttributes(DateTime? areaCosEffectiveFrom, DateTime? areaCosEffectiveTo, decimal areaCosFactor)
        {
            return new Dictionary<string, IAttributeData>
            {
                { AreaCosEffectiveFrom, new AttributeData(areaCosEffectiveFrom) },
                { AreaCosEffectiveTo, new AttributeData(areaCosEffectiveTo) },
                { AreaCosFactor, new AttributeData(areaCosFactor) },
            };
        }
    }
}
