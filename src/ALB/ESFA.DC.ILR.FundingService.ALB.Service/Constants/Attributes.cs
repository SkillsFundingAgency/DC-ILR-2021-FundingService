namespace ESFA.DC.ILR.FundingService.ALB.Service.Constants
{
    public static class Attributes
    {
        // Entity
        public const string EntityGlobal = "global";
        public const string EntityLearner = "Learner";
        public const string EntityLearningDelivery = "LearningDelivery";
        public const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        public const string EntityLearningDeliverySFA_PostcodeAreaCost = "SFA_PostcodeAreaCost";
        public const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";
        public const string EntityLearningDeliverySubsidyPilotPostcodeArea = "SubsidyPilotPostcodeArea";
        public const string EntityLearningDeliveryLARS_CareerLearningPilot = "LARS_CareerLearningPilot";

        // Global
        public const string LARSVersion = "LARSVersion";
        public const string UKPRN = "UKPRN";
        public const string PostcodeAreaCostVersion = "PostcodeAreaCostVersion";

        // Learner
        public const string LearnRefNumber = "LearnRefNumber";

        // LearningDelivery
        public const string AimSeqNumber = "AimSeqNumber";
        public const string CompStatus = "CompStatus";
        public const string LearnActEndDate = "LearnActEndDate";
        public const string LearnAimRefType = "LearnAimRefType";
        public const string LearnDelFundModel = "LearnDelFundModel";
        public const string LearnPlanEndDate = "LearnPlanEndDate";
        public const string LearnStartDate = "LearnStartDate";
        public const string LrnDelFAM_ADL = "LrnDelFAM_ADL";
        public const string LrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        public const string LrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        public const string LrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        public const string LrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        public const string LrnDelFAM_RES = "LrnDelFAM_RES";
        public const string NotionalNVQLevelv2 = "NotionalNVQLevelv2";
        public const string OrigLearnStartDate = "OrigLearnStartDate";
        public const string OtherFundAdj = "OtherFundAdj";
        public const string Outcome = "Outcome";
        public const string PriorLearnFundAdj = "PriorLearnFundAdj";
        public const string RegulatedCreditValue = "RegulatedCreditValue";

        // LearningDeliveryFAM
        public const string LearnDelFAMCode = "LearnDelFAMCode";
        public const string LearnDelFAMDateFrom = "LearnDelFAMDateFrom";
        public const string LearnDelFAMDateTo = "LearnDelFAMDateTo";
        public const string LearnDelFAMType = "LearnDelFAMType";

        public const string ADL = "ADL";
        public const string LDM = "LDM";
        public const string RES = "RES";

        // SFAPostcodeAreaCost
        public const string AreaCosEffectiveFrom = "AreaCosEffectiveFrom";
        public const string AreaCosEffectiveTo = "AreaCosEffectiveTo";
        public const string AreaCosFactor = "AreaCosFactor";

        // LearningDeliveryLARSFunding
        public const string LARSFundCategory = "LARSFundCategory";
        public const string LARSFundEffectiveFrom = "LARSFundEffectiveFrom";
        public const string LARSFundEffectiveTo = "LARSFundEffectiveTo";
        public const string LARSFundWeightedRate = "LARSFundWeightedRate";
        public const string LARSFundWeightingFactor = "LARSFundWeightingFactor";

        // SubsidyPilotPostcodeArea
        public const string SubsidyPilotAreaCode = "SubsidyPilotAreaCode";
        public const string SubsidyPilotEffectiveFrom = "SubsidyPilotEffectiveFrom";
        public const string SubsidyPilotEffectiveTo = "SubsidyPilotEffectiveTo";

        // LARS_CareerLearningPilot
        public const string LearnDelLARSCarPilFundAreaCode = "LearnDelLARSCarPilFundAreaCode";
        public const string LearnDelLARSCarPilFundEffToDate = "LearnDelLARSCarPilFundEffToDate";
        public const string LearnDelLARSCarPilFundEffFromDate = "LearnDelLARSCarPilFundEffFromDate";
        public const string LearnDelLARSCarPilFundSubsidyRate = "LearnDelLARSCarPilFundSubsidyRate";
    }
}
