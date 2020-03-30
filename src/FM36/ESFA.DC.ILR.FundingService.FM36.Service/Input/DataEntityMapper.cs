using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Interface;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM36.Service.Constants;
using ESFA.DC.ILR.FundingService.FM36.Service.Model;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<FM36LearnerDto>
    {
        private readonly int _fundModel = Attributes.FundModel_36;

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IAppsEarningsHistoryReferenceDataService _appsEarningsHistoryReferenceDataService;

        public DataEntityMapper(
            ILARSReferenceDataService larsReferenceDataService,
            IPostcodesReferenceDataService postcodesReferenceDataService,
            IAppsEarningsHistoryReferenceDataService appsEarningsHistoryReferenceDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
            _appsEarningsHistoryReferenceDataService = appsEarningsHistoryReferenceDataService;
        }

        public IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<FM36LearnerDto> inputModels)
        {
            var global = BuildGlobal(ukprn);

            var entities = inputModels?
                .Where(l => l.LearningDeliveries
                .Any(ld => ld.FundModel == _fundModel))
                .Select(l => BuildGlobalDataEntity(l, global)) ?? new List<IDataEntity>();

            return entities.Any() ? entities : new List<IDataEntity> { BuildDefaultGlobalDataEntity(global) };
        }

        public IDataEntity BuildGlobalDataEntity(FM36LearnerDto learner, Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.Year, new AttributeData(global.Year) },
                    { Attributes.CollectionPeriod, new AttributeData(global.CollectionPeriod) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildDefaultGlobalDataEntity(Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.Year, new AttributeData(global.Year) },
                    { Attributes.CollectionPeriod, new AttributeData(global.CollectionPeriod) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                }
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM36LearnerDto learner)
        {
            var dasPostDisadvantage = _postcodesReferenceDataService.DASDisadvantagesForPostcode(learner.PostcodePrior);
            var appsEarningsHistory = _appsEarningsHistoryReferenceDataService.AECEarningsHistory(learner.ULN);

            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirth) },
                    { Attributes.ULN, new AttributeData(learner.ULN) },
                    { Attributes.PrevUKPRN, new AttributeData(learner.PrevUKPRN) },
                    { Attributes.PMUKPRN, new AttributeData(learner.PMUKPRN) }
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Where(ld => ld.FundModel == _fundModel)
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            learner.LearnerEmploymentStatuses?
                            .Select(BuildLearnerEmploymentStatus) ?? new List<IDataEntity>())
                        .Union(
                            dasPostDisadvantage?
                            .Select(BuildDASPostcodeDisadvantage) ?? new List<IDataEntity>())
                        .Union(
                            appsEarningsHistory?
                            .Select(BuildApprenticeshipsEarningsHistory) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(LearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var larsStandard = _larsReferenceDataService.LARSStandardForStandardCode(learningDelivery.StdCode);

            var larsFramework = larsLearningDelivery.LARSFrameworks?
                .Where(lf => lf.FworkCode == learningDelivery.FworkCode
                && lf.ProgType == learningDelivery.ProgType
                && lf.PwayCode == learningDelivery.PwayCode).FirstOrDefault();

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AchDate, new AttributeData(learningDelivery.AchDate) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.FrameworkCommonComponent, new AttributeData(larsLearningDelivery.FrameworkCommonComponent) },
                    { Attributes.FworkCode, new AttributeData(learningDelivery.FworkCode) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDate) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDate) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdj) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdj) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgType) },
                    { Attributes.PwayCode, new AttributeData(learningDelivery.PwayCode) },
                    { Attributes.STDCode, new AttributeData(learningDelivery.StdCode) },
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                   learningDelivery?
                                    .AppFinRecords?
                                    .Select(BuildApprenticeshipFinancialRecord) ?? new List<IDataEntity>())
                            .Union(
                                    larsFramework?
                                    .LARSFrameworkApprenticeshipFundings?
                                    .Select(BuildLARSFrameworkApprenticeshipFunding) ?? new List<IDataEntity>())
                            .Union(
                                    larsFramework?
                                    .LARSFrameworkCommonComponents?
                                    .Select(BuildLARSFrameworkCommonComponent) ?? new List<IDataEntity>())
                             .Union(
                                   larsStandard?
                                   .LARSStandardCommonComponents?
                                    .Select(BuildLARSStandardCommonComponent) ?? new List<IDataEntity>())
                            .Union(
                                    larsStandard?
                                    .LARSStandardApprenticeshipFundings?
                                    .Select(BuildLARSStandardApprenticeshipFunding) ?? new List<IDataEntity>())
                            .ToList()
                            .Union(
                                    larsLearningDelivery?
                                    .LARSFundings?
                                    .Select(BuildLARSFunding) ?? new List<IDataEntity>())
                            .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryFAM(LearningDeliveryFAM learningDeliveryFAM)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryFAM)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelFAMCode, new AttributeData(learningDeliveryFAM.LearnDelFAMCode) },
                    { Attributes.LearnDelFAMDateTo, new AttributeData(learningDeliveryFAM.LearnDelFAMDateTo) },
                    { Attributes.LearnDelFAMDateFrom, new AttributeData(learningDeliveryFAM.LearnDelFAMDateFrom) },
                    { Attributes.LearnDelFAMType, new AttributeData(learningDeliveryFAM.LearnDelFAMType) },
                }
            };
        }

        public IDataEntity BuildLearnerEmploymentStatus(LearnerEmploymentStatus learnerEmploymentStatus)
        {
            return new DataEntity(Attributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AgreeId, new AttributeData("AgreeIdPlaceHolder") },
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpId, new AttributeData(learnerEmploymentStatus.EmpId) },
                    { Attributes.EMPStat, new AttributeData(learnerEmploymentStatus.EmpStat) },
                    { Attributes.EmpStatMon_SEM, new AttributeData(learnerEmploymentStatus.SEM) }
                }
            };
        }

        public IDataEntity BuildDASPostcodeDisadvantage(DasDisadvantage dasDisadvantage)
        {
            return new DataEntity(Attributes.EntitySFA_PostcodeDisadvantage)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DisApprenticeshipUplift, new AttributeData(dasDisadvantage.Uplift) },
                    { Attributes.DisUpEffectiveFrom, new AttributeData(dasDisadvantage.EffectiveFrom) },
                    { Attributes.DisUpEffectiveTo, new AttributeData(dasDisadvantage.EffectiveTo) }
                }
            };
        }

        public IDataEntity BuildApprenticeshipsEarningsHistory(AECEarningsHistory aecEarningsHistory)
        {
            return new DataEntity(Attributes.EntityHistoricEarningInput)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AppIdentifierInput, new AttributeData(aecEarningsHistory.AppIdentifier) },
                    { Attributes.AppProgCompletedInTheYearInput, new AttributeData(aecEarningsHistory.AppProgCompletedInTheYearInput) },
                    { Attributes.HistoricCollectionReturnInput, new AttributeData(aecEarningsHistory.CollectionReturnCode) },
                    { Attributes.HistoricCollectionYearInput, new AttributeData(aecEarningsHistory.CollectionYear) },
                    { Attributes.HistoricDaysInYearInput, new AttributeData(aecEarningsHistory.DaysInYear) },
                    { Attributes.HistoricEffectiveTNPStartDateInput, new AttributeData(aecEarningsHistory.HistoricEffectiveTNPStartDateInput) },
                    { Attributes.HistoricEmpIdStartWithinYearInput, new AttributeData(aecEarningsHistory.HistoricEmpIdStartWithinYear) },
                    { Attributes.HistoricEmpIdEndWithinYearInput, new AttributeData(aecEarningsHistory.HistoricEmpIdEndWithinYear) },
                    { Attributes.HistoricFworkCodeInput, new AttributeData(aecEarningsHistory.FworkCode) },
                    { Attributes.HistoricLearnDelProgEarliestACT2DateInput, new AttributeData(aecEarningsHistory.HistoricLearnDelProgEarliestACT2DateInput) },
                    { Attributes.HistoricLearner1618AtStartInput, new AttributeData(aecEarningsHistory.HistoricLearner1618StartInput) },
                    { Attributes.HistoricLearnRefNumberInput, new AttributeData(aecEarningsHistory.LearnRefNumber) },
                    { Attributes.HistoricPMRAmountInput, new AttributeData(aecEarningsHistory.HistoricPMRAmount) },
                    { Attributes.HistoricProgrammeStartDateIgnorePathwayInput, new AttributeData(aecEarningsHistory.ProgrammeStartDateIgnorePathway) },
                    { Attributes.HistoricProgrammeStartDateMatchPathwayInput, new AttributeData(aecEarningsHistory.ProgrammeStartDateMatchPathway) },
                    { Attributes.HistoricProgTypeInput, new AttributeData(aecEarningsHistory.ProgType) },
                    { Attributes.HistoricPwayCodeInput, new AttributeData(aecEarningsHistory.PwayCode) },
                    { Attributes.HistoricTotalProgAimPaymentsInTheYearInput, new AttributeData(aecEarningsHistory.TotalProgAimPaymentsInTheYear) },
                    { Attributes.HistoricTotal1618UpliftPaymentsInTheYearInput, new AttributeData(aecEarningsHistory.HistoricTotal1618UpliftPaymentsInTheYearInput) },
                    { Attributes.HistoricSTDCodeInput, new AttributeData(aecEarningsHistory.STDCode) },
                    { Attributes.HistoricTNP1Input, new AttributeData(aecEarningsHistory.HistoricTNP1Input) },
                    { Attributes.HistoricTNP2Input, new AttributeData(aecEarningsHistory.HistoricTNP2Input) },
                    { Attributes.HistoricTNP3Input, new AttributeData(aecEarningsHistory.HistoricTNP3Input) },
                    { Attributes.HistoricTNP4Input, new AttributeData(aecEarningsHistory.HistoricTNP4Input) },
                    { Attributes.HistoricUKPRNInput, new AttributeData(aecEarningsHistory.UKPRN) },
                    { Attributes.HistoricULNInput, new AttributeData(aecEarningsHistory.ULN) },
                    { Attributes.HistoricUptoEndDateInput, new AttributeData(aecEarningsHistory.UptoEndDate) },
                    { Attributes.HistoricVirtualTNP3EndofTheYearInput, new AttributeData(aecEarningsHistory.HistoricVirtualTNP3EndOfTheYearInput) },
                    { Attributes.HistoricVirtualTNP4EndofTheYearInput, new AttributeData(aecEarningsHistory.HistoricVirtualTNP4EndOfTheYearInput) }
                }
            };
        }

        public IDataEntity BuildApprenticeshipFinancialRecord(AppFinRecord appFinRecord)
        {
            return new DataEntity(Attributes.EntityApprenticeshipFinancialRecord)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AFinAmount, new AttributeData(appFinRecord.AFinAmount) },
                    { Attributes.AFinCode, new AttributeData(appFinRecord.AFinCode) },
                    { Attributes.AFinDate, new AttributeData(appFinRecord.AFinDate) },
                    { Attributes.AFinType, new AttributeData(appFinRecord.AFinType) }
                }
            };
        }

        public IDataEntity BuildLARSStandardApprenticeshipFunding(LARSStandardApprenticeshipFunding larsStandardApprenticeshipFunding)
        {
            return new DataEntity(Attributes.EntityStandardLARSApprenticshipFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.StandardAF1618EmployerAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenEmployerAdditionalPayment) },
                    { Attributes.StandardAF1618ProviderAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenProviderAdditionalPayment) },
                    { Attributes.StandardAF1618FrameworkUplift, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenFrameworkUplift) },
                    { Attributes.StandardAFCareLeaverAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.CareLeaverAdditionalPayment) },
                    { Attributes.StandardAFEffectiveFrom, new AttributeData(larsStandardApprenticeshipFunding.EffectiveFrom) },
                    { Attributes.StandardAFEffectiveTo, new AttributeData(larsStandardApprenticeshipFunding.EffectiveTo) },
                    { Attributes.StandardAFFundingCategory, new AttributeData(larsStandardApprenticeshipFunding.FundingCategory) },
                    { Attributes.StandardAFMaxEmployerLevyCap, new AttributeData(larsStandardApprenticeshipFunding.MaxEmployerLevyCap) },
                    { Attributes.StandardAFReservedValue2, new AttributeData(larsStandardApprenticeshipFunding.ReservedValue2) },
                    { Attributes.StandardAFReservedValue3, new AttributeData(larsStandardApprenticeshipFunding.ReservedValue3) }
                }
            };
        }

        public IDataEntity BuildLARSFrameworkApprenticeshipFunding(LARSFrameworkApprenticeshipFunding larsFrameworkApprenticeshipFunding)
        {
            return new DataEntity(Attributes.EntityFrameworkLARSApprenticshipFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.FrameworkAF1618EmployerAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenEmployerAdditionalPayment) },
                    { Attributes.FrameworkAF1618ProviderAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenProviderAdditionalPayment) },
                    { Attributes.FrameworkAF1618FrameworkUplift, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenFrameworkUplift) },
                    { Attributes.FrameworkAFCareLeaverAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.CareLeaverAdditionalPayment) },
                    { Attributes.FrameworkAFEffectiveFrom, new AttributeData(larsFrameworkApprenticeshipFunding.EffectiveFrom) },
                    { Attributes.FrameworkAFEffectiveTo, new AttributeData(larsFrameworkApprenticeshipFunding.EffectiveTo) },
                    { Attributes.FrameworkAFFundingCategory, new AttributeData(larsFrameworkApprenticeshipFunding.FundingCategory) },
                    { Attributes.FrameworkAFMaxEmployerLevyCap, new AttributeData(larsFrameworkApprenticeshipFunding.MaxEmployerLevyCap) },
                    { Attributes.FrameworkAFReservedValue2, new AttributeData(larsFrameworkApprenticeshipFunding.ReservedValue2) },
                    { Attributes.FrameworkAFReservedValue3, new AttributeData(larsFrameworkApprenticeshipFunding.ReservedValue3) }
                }
            };
        }

        public IDataEntity BuildLARSFrameworkCommonComponent(LARSFrameworkCommonComponent larsFrameworkCommonComponent)
        {
            return new DataEntity(Attributes.EntityLARSFrameworkCmnComp)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSFrameworkCommonComponentCode, new AttributeData(larsFrameworkCommonComponent.CommonComponent) },
                    { Attributes.LARSFrameworkCommonComponentEffectiveFrom, new AttributeData(larsFrameworkCommonComponent.EffectiveFrom) },
                    { Attributes.LARSFrameworkCommonComponentEffectiveTo, new AttributeData(larsFrameworkCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSStandardCommonComponent(LARSStandardCommonComponent larsStandardCommonComponent)
        {
            return new DataEntity(Attributes.EntityStandardCommonComponent)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSStandardCommonComponentCode, new AttributeData(larsStandardCommonComponent.CommonComponent) },
                    { Attributes.LARSStandardCommonComponentEffectiveFrom, new AttributeData(larsStandardCommonComponent.EffectiveFrom) },
                    { Attributes.LARSStandardCommonComponentEffectiveTo, new AttributeData(larsStandardCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSFunding(LARSFunding larsFunding)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_Funding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSFundCategory, new AttributeData(larsFunding.FundingCategory) },
                    { Attributes.LARSFundEffectiveFrom, new AttributeData(larsFunding.EffectiveFrom) },
                    { Attributes.LARSFundEffectiveTo, new AttributeData(larsFunding.EffectiveTo) },
                    { Attributes.LARSFundWeightedRate, new AttributeData(larsFunding.RateWeighted) },
                }
            };
        }

        public Global BuildGlobal(int ukprn)
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                Year = Attributes.YearValue,

                // ToDo: implement AcademicYear service over InternalCache for "CollectionPeriod" to calculate value.
                // This attribute is not used by rulebase at present 10/09/18.
                CollectionPeriod = Attributes.CollectionPeriodValue,
                UKPRN = ukprn
            };
        }
    }
}