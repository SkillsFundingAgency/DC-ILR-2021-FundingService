namespace ESFA.DC.ILR.FundingService.FM36.Service.Constants
{
    public static class InputAttributes
    {
        // Entity
        private const string EntityGlobal = "global";
        private const string EntityLearner = "Learner";
        private const string EntityLearningDelivery = "LearningDelivery";
        private const string EntityApprenticeshipFinancialRecord = "ApprenticeshipFinancialRecord";
        private const string EntityLearnerEmploymentStatus = "LearnerEmploymentStatus";
        private const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        private const string EntityStandardLARSApprenticshipFunding = "Standard_LARS_ApprenticshipFunding";
        private const string EntityFrameworkLARSApprenticshipFunding = "Framework_LARS_ApprenticshipFunding";
        private const string EntityLearningDeliverySFA_PostcodeDisadvantage = "SFA_PostcodeDisadvantage";
        private const string EntityHistoricEarningInput = "HistoricEarningInput";
        private const string EntityLARSFrameworkCmnComp = "LARS_FrameworkCmnComp";
        private const string EntityStandardCommonComponent = "LARS_StandardCommonComponent";
        private const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";

        // Global Names
        private const string LARSVersion = "LARSVersion";
        private const string UKPRN = "UKPRN";
        private const string CollectionPeriod = "CollectionPeriod";
        private const string Year = "Year";

        // Global Values
        private const string YearValue = "1819";
        private const string Period1 = "R01";
        private const string Period2 = "R02";
        private const string Period3 = "R03";
        private const string Period4 = "R04";
        private const string Period5 = "R05";
        private const string Period6 = "R06";
        private const string Period7 = "R07";
        private const string Period8 = "R08";
        private const string Period9 = "R09";
        private const string Period10 = "R10";
        private const string Period11 = "R11";
        private const string Period12 = "R12";

        // Learner
        private const string LearnRefNumber = "LearnRefNumber";
        private const string DateOfBirth = "DateOfBirth";
        private const string ULN = "ULN";
        private const string PrevUKPRN = "PrevUKPRN";

        // LearningDelivery
        private const string AimSeqNumber = "AimSeqNumber";
        private const string AimType = "AimType";
        private const string FrameworkCommonComponent = "FrameworkCommonComponent";
        private const string FworkCode = "FworkCode";
        private const string LearnAimRef = "LearnAimRef";
        private const string LearnPlanEndDate = "LearnPlanEndDate";
        private const string LearnStartDate = "LearnStartDate";
        private const string LrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        private const string LrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        private const string LrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        private const string LrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        private const string OrigLearnStartDate = "OrigLearnStartDate";
        private const string PriorLearnFundAdj = "PriorLearnFundAdj";
        private const string PwayCode = "PwayCode";
        private const string STDCode = "STDCode";

        // ApprenticeshipFinancialRecord
        private const string AFinAmount = "AFinAmount";
        private const string AFinCode = "AFinCode";
        private const string AFinDate = "AFinDate";
        private const string AFinType = "AFinType";

        // LearningDeliveryFAM
        private const string LearnDelFAMCode = "LearnDelFAMCode";
        private const string LearnDelFAMDateFrom = "LearnDelFAMDateFrom";
        private const string LearnDelFAMDateTo = "LearnDelFAMDateTo";
        private const string LearnDelFAMType = "LearnDelFAMType";

        // LearnerEmploymentStatus
        private const string AgreeId = "AgreeId";
        private const string DateEmpStatApp = "DateEmpStatApp";
        private const string EmpId = "EmpId";
        private const string EMPStat = "EMPStat";
        private const string EmpStatMon_SEM = "EmpStatMon_SEM";

        // Standard_LARS_ApprenticshipFunding
        private const string StandardAF1618EmployerAdditionalPayment = "StandardAF1618EmployerAdditionalPayment";
        private const string StandardAF1618ProviderAdditionalPayment = "StandardAF1618ProviderAdditionalPayment";
        private const string StandardAF1618FrameworkUplift = "StandardAF1618FrameworkUplift";
        private const string StandardAFCareLeaverAdditionalPayment = "StandardAFCareLeaverAdditionalPayment";
        private const string StandardAFEffectiveFrom = "StandardAFEffectiveFrom";
        private const string StandardAFEffectiveTo = "StandardAFEffectiveTo";
        private const string StandardAFFundingCategory = "StandardAFFundingCategory";
        private const string StandardAFMaxEmployerLevyCap = "StandardAFMaxEmployerLevyCap";
        private const string StandardAFReservedValue2 = "StandardAFReservedValue2";
        private const string StandardAFReservedValue3 = "StandardAFReservedValue3";

        // Framework_LARS_ApprenticshipFunding
        private const string FrameworkAF1618EmployerAdditionalPayment = "FrameworkAF1618EmployerAdditionalPayment";
        private const string FrameworkAF1618ProviderAdditionalPayment = "FrameworkAF1618ProviderAdditionalPayment";
        private const string FrameworkAF1618FrameworkUplift = "FrameworkAF1618FrameworkUplift";
        private const string FrameworkAFCareLeaverAdditionalPayment = "FrameworkAFCareLeaverAdditionalPayment";
        private const string FrameworkAFEffectiveFrom = "FrameworkAFEffectiveFrom";
        private const string FrameworkAFEffectiveTo = "FrameworkAFEffectiveTo";
        private const string FrameworkAFFundingCategory = "FrameworkAFFundingCategory";
        private const string FrameworkAFMaxEmployerLevyCap = "FrameworkAFMaxEmployerLevyCap";
        private const string FrameworkAFReservedValue2 = "FrameworkAFReservedValue2";
        private const string FrameworkAFReservedValue3 = "FrameworkAFReservedValue3";

        // SFA_PostcodeDisadvantage
        private const string DisApprenticeshipUplift = "DisApprenticeshipUplift";
        private const string DisUpEffectiveFrom = "DisUpEffectiveFrom";
        private const string DisUpEffectiveTo = "DisUpEffectiveTo";

        // HistoricEarningInput
        private const string AppIdentifierInput = "AppIdentifierInput";
        private const string AppProgCompletedInTheYearInput = "AppProgCompletedInTheYearInput";
        private const string HistoricCollectionReturnInput = "HistoricCollectionReturnInput";
        private const string HistoricCollectionYearInput = "HistoricCollectionYearInput";
        private const string HistoricDaysInYearInput = "HistoricDaysInYearInput";
        private const string HistoricEffectiveTNPStartDateInput = "HistoricEffectiveTNPStartDateInput";
        private const string HistoricEmpIdStartWithinYearInput = "HistoricEmpIdStartWithinYearInput";
        private const string HistoricEmpIdEndWithinYearInput = "HistoricEmpIdEndWithinYearInput";
        private const string HistoricFworkCodeInput = "HistoricFworkCodeInput";
        private const string HistoricLearnDelProgEarliestACT2DateInput = "HistoricLearnDelProgEarliestACT2DateInput";
        private const string HistoricLearner1618AtStartInput = "HistoricLearner1618AtStartInput";
        private const string HistoricLearnRefNumberInput = "HistoricLearnRefNumberInput";
        private const string HistoricPMRAmountInput = "HistoricPMRAmountInput";
        private const string HistoricProgrammeStartDateIgnorePathwayInput = "HistoricProgrammeStartDateIgnorePathwayInput";
        private const string HistoricProgrammeStartDateMatchPathwayInput = "HistoricProgrammeStartDateMatchPathwayInput";
        private const string HistoricProgTypeInput = "HistoricProgTypeInput";
        private const string HistoricPwayCodeInput = "HistoricPwayCodeInput";
        private const string HistoricTotalProgAimPaymentsInTheYearInput = "HistoricTotalProgAimPaymentsInTheYearInput";
        private const string HistoricTotal1618UpliftPaymentsInTheYearInput = "HistoricTotal1618UpliftPaymentsInTheYearInput";
        private const string HistoricSTDCodeInput = "HistoricSTDCodeInput";
        private const string HistoricTNP1Input = "HistoricTNP1Input";
        private const string HistoricTNP2Input = "HistoricTNP2Input";
        private const string HistoricTNP3Input = "HistoricTNP3Input";
        private const string HistoricTNP4Input = "HistoricTNP4Input";
        private const string HistoricUKPRNInput = "HistoricUKPRNInput";
        private const string HistoricULNInput = "HistoricULNInput";
        private const string HistoricUptoEndDateInput = "HistoricUptoEndDateInput";
        private const string HistoricVirtualTNP3EndofTheYearInput = "HistoricVirtualTNP3EndofTheYearInput";
        private const string HistoricVirtualTNP4EndofTheYearInput = "HistoricVirtualTNP4EndofTheYearInput";

        // LARS_FrameworkCmnComp
        private const string LARSFrameworkCommonComponentCode = "LARSFrameworkCommonComponentCode";
        private const string LARSFrameworkCommonComponentEffectiveTo = "LARSFrameworkCommonComponentEffectiveTo";
        private const string LARSFrameworkCommonComponentEffectiveFrom = "LARSFrameworkCommonComponentEffectiveFrom";

        // LARS_StandardCommonComponent
        private const string LARSStandardCommonComponentCode = "LARSStandardCommonComponentCode";
        private const string LARSStandardCommonComponentEffectiveFrom = "LARSStandardCommonComponentEffectiveFrom";
        private const string LARSStandardCommonComponentEffectiveTo = "LARSStandardCommonComponentEffectiveTo";

        // LearningDeliveryLARSFunding
        private const string LARSFundCategory = "LARSFundCategory";
        private const string LARSFundEffectiveFrom = "LARSFundEffectiveFrom";
        private const string LARSFundEffectiveTo = "LARSFundEffectiveTo";
        private const string LARSFundWeightedRate = "LARSFundWeightedRate";
    }
}
