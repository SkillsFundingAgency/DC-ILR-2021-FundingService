namespace ESFA.DC.ILR.FundingService.FM36.Service.Constants
{
    public static class InputAttributes
    {
        // FundModels
        public const int FundModel_36 = 36;

        // Entity
        public const string EntityGlobal = "global";
        public const string EntityLearner = "Learner";
        public const string EntityLearningDelivery = "LearningDelivery";
        public const string EntityApprenticeshipFinancialRecord = "ApprenticeshipFinancialRecord";
        public const string EntityLearnerEmploymentStatus = "LearnerEmploymentStatus";
        public const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        public const string EntityStandardLARSApprenticshipFunding = "Standard_LARS_ApprenticshipFunding";
        public const string EntityFrameworkLARSApprenticshipFunding = "Framework_LARS_ApprenticshipFunding";
        public const string EntitySFA_PostcodeDisadvantage = "SFA_PostcodeDisadvantage";
        public const string EntityHistoricEarningInput = "HistoricEarningInput";
        public const string EntityLARSFrameworkCmnComp = "LARS_FrameworkCmnComp";
        public const string EntityStandardCommonComponent = "LARS_StandardCommonComponent";
        public const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";

        // Global Names
        public const string LARSVersion = "LARSVersion";
        public const string UKPRN = "UKPRN";
        public const string CollectionPeriod = "CollectionPeriod";
        public const string Year = "Year";

        // Global Values
        public const string YearValue = "1819";
        public const string CollectionPeriodValue = "DefaultPeriod";
        public const string Period1 = "R01";
        public const string Period2 = "R02";
        public const string Period3 = "R03";
        public const string Period4 = "R04";
        public const string Period5 = "R05";
        public const string Period6 = "R06";
        public const string Period7 = "R07";
        public const string Period8 = "R08";
        public const string Period9 = "R09";
        public const string Period10 = "R10";
        public const string Period11 = "R11";
        public const string Period12 = "R12";

        // Learner
        public const string LearnRefNumber = "LearnRefNumber";
        public const string DateOfBirth = "DateOfBirth";
        public const string ULN = "ULN";
        public const string PrevUKPRN = "PrevUKPRN";
        public const string PMUKPRN = "PMUKPRN";

        // LearningDelivery
        public const string AimSeqNumber = "AimSeqNumber";
        public const string AimType = "AimType";
        public const string CompStatus = "CompStatus";
        public const string FrameworkCommonComponent = "FrameworkCommonComponent";
        public const string FworkCode = "FworkCode";
        public const string LearnAimRef = "LearnAimRef";
        public const string LearnActEndDate = "LearnActEndDate";
        public const string LearnPlanEndDate = "LearnPlanEndDate";
        public const string LearnStartDate = "LearnStartDate";
        public const string LrnDelFAM_EEF = "LrnDelFAM_EEF";
        public const string LrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        public const string LrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        public const string LrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        public const string LrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        public const string OrigLearnStartDate = "OrigLearnStartDate";
        public const string OtherFundAdj = "OtherFundAdj";
        public const string PriorLearnFundAdj = "PriorLearnFundAdj";
        public const string ProgType = "ProgTpe";
        public const string PwayCode = "PwayCode";
        public const string STDCode = "STDCode";

        // LearningDeliveryFAM
        public const string EEF = "EEF";
        public const string LDM = "LDM";

        // ApprenticeshipFinancialRecord
        public const string AFinAmount = "AFinAmount";
        public const string AFinCode = "AFinCode";
        public const string AFinDate = "AFinDate";
        public const string AFinType = "AFinType";

        // LearningDeliveryFAM
        public const string LearnDelFAMCode = "LearnDelFAMCode";
        public const string LearnDelFAMDateFrom = "LearnDelFAMDateFrom";
        public const string LearnDelFAMDateTo = "LearnDelFAMDateTo";
        public const string LearnDelFAMType = "LearnDelFAMType";

        // LearnerEmploymentStatus
        public const string AgreeId = "AgreeId";
        public const string DateEmpStatApp = "DateEmpStatApp";
        public const string EmpId = "EmpId";
        public const string EMPStat = "EMPStat";
        public const string EmpStatMon_SEM = "EmpStatMon_SEM";

        // Standard_LARS_ApprenticshipFunding
        public const string StandardAF1618EmployerAdditionalPayment = "StandardAF1618EmployerAdditionalPayment";
        public const string StandardAF1618ProviderAdditionalPayment = "StandardAF1618ProviderAdditionalPayment";
        public const string StandardAF1618FrameworkUplift = "StandardAF1618FrameworkUplift";
        public const string StandardAFCareLeaverAdditionalPayment = "StandardAFCareLeaverAdditionalPayment";
        public const string StandardAFEffectiveFrom = "StandardAFEffectiveFrom";
        public const string StandardAFEffectiveTo = "StandardAFEffectiveTo";
        public const string StandardAFFundingCategory = "StandardAFFundingCategory";
        public const string StandardAFMaxEmployerLevyCap = "StandardAFMaxEmployerLevyCap";
        public const string StandardAFReservedValue2 = "StandardAFReservedValue2";
        public const string StandardAFReservedValue3 = "StandardAFReservedValue3";

        // Framework_LARS_ApprenticshipFunding
        public const string FrameworkAF1618EmployerAdditionalPayment = "FrameworkAF1618EmployerAdditionalPayment";
        public const string FrameworkAF1618ProviderAdditionalPayment = "FrameworkAF1618ProviderAdditionalPayment";
        public const string FrameworkAF1618FrameworkUplift = "FrameworkAF1618FrameworkUplift";
        public const string FrameworkAFCareLeaverAdditionalPayment = "FrameworkAFCareLeaverAdditionalPayment";
        public const string FrameworkAFEffectiveFrom = "FrameworkAFEffectiveFrom";
        public const string FrameworkAFEffectiveTo = "FrameworkAFEffectiveTo";
        public const string FrameworkAFFundingCategory = "FrameworkAFFundingCategory";
        public const string FrameworkAFMaxEmployerLevyCap = "FrameworkAFMaxEmployerLevyCap";
        public const string FrameworkAFReservedValue2 = "FrameworkAFReservedValue2";
        public const string FrameworkAFReservedValue3 = "FrameworkAFReservedValue3";

        // SFA_PostcodeDisadvantage
        public const string DisApprenticeshipUplift = "DisApprenticeshipUplift";
        public const string DisUpEffectiveFrom = "DisUpEffectiveFrom";
        public const string DisUpEffectiveTo = "DisUpEffectiveTo";

        // HistoricEarningInput
        public const string AppIdentifierInput = "AppIdentifierInput";
        public const string AppProgCompletedInTheYearInput = "AppProgCompletedInTheYearInput";
        public const string HistoricCollectionReturnInput = "HistoricCollectionReturnInput";
        public const string HistoricCollectionYearInput = "HistoricCollectionYearInput";
        public const string HistoricDaysInYearInput = "HistoricDaysInYearInput";
        public const string HistoricEffectiveTNPStartDateInput = "HistoricEffectiveTNPStartDateInput";
        public const string HistoricEmpIdStartWithinYearInput = "HistoricEmpIdStartWithinYearInput";
        public const string HistoricEmpIdEndWithinYearInput = "HistoricEmpIdEndWithinYearInput";
        public const string HistoricFworkCodeInput = "HistoricFworkCodeInput";
        public const string HistoricLearnDelProgEarliestACT2DateInput = "HistoricLearnDelProgEarliestACT2DateInput";
        public const string HistoricLearner1618AtStartInput = "HistoricLearner1618AtStartInput";
        public const string HistoricLearnRefNumberInput = "HistoricLearnRefNumberInput";
        public const string HistoricPMRAmountInput = "HistoricPMRAmountInput";
        public const string HistoricProgrammeStartDateIgnorePathwayInput = "HistoricProgrammeStartDateIgnorePathwayInput";
        public const string HistoricProgrammeStartDateMatchPathwayInput = "HistoricProgrammeStartDateMatchPathwayInput";
        public const string HistoricProgTypeInput = "HistoricProgTypeInput";
        public const string HistoricPwayCodeInput = "HistoricPwayCodeInput";
        public const string HistoricTotalProgAimPaymentsInTheYearInput = "HistoricTotalProgAimPaymentsInTheYearInput";
        public const string HistoricTotal1618UpliftPaymentsInTheYearInput = "HistoricTotal1618UpliftPaymentsInTheYearInput";
        public const string HistoricSTDCodeInput = "HistoricSTDCodeInput";
        public const string HistoricTNP1Input = "HistoricTNP1Input";
        public const string HistoricTNP2Input = "HistoricTNP2Input";
        public const string HistoricTNP3Input = "HistoricTNP3Input";
        public const string HistoricTNP4Input = "HistoricTNP4Input";
        public const string HistoricUKPRNInput = "HistoricUKPRNInput";
        public const string HistoricULNInput = "HistoricULNInput";
        public const string HistoricUptoEndDateInput = "HistoricUptoEndDateInput";
        public const string HistoricVirtualTNP3EndofTheYearInput = "HistoricVirtualTNP3EndofTheYearInput";
        public const string HistoricVirtualTNP4EndofTheYearInput = "HistoricVirtualTNP4EndofTheYearInput";

        // LARS_FrameworkCmnComp
        public const string LARSFrameworkCommonComponentCode = "LARSFrameworkCommonComponentCode";
        public const string LARSFrameworkCommonComponentEffectiveTo = "LARSFrameworkCommonComponentEffectiveTo";
        public const string LARSFrameworkCommonComponentEffectiveFrom = "LARSFrameworkCommonComponentEffectiveFrom";

        // LARS_StandardCommonComponent
        public const string LARSStandardCommonComponentCode = "LARSStandardCommonComponentCode";
        public const string LARSStandardCommonComponentEffectiveFrom = "LARSStandardCommonComponentEffectiveFrom";
        public const string LARSStandardCommonComponentEffectiveTo = "LARSStandardCommonComponentEffectiveTo";

        // LearningDeliveryLARSFunding
        public const string LARSFundCategory = "LARSFundCategory";
        public const string LARSFundEffectiveFrom = "LARSFundEffectiveFrom";
        public const string LARSFundEffectiveTo = "LARSFundEffectiveTo";
        public const string LARSFundWeightedRate = "LARSFundWeightedRate";
    }
}
