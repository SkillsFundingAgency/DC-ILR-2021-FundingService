using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Interface;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM36.Service.Constants;
using ESFA.DC.ILR.FundingService.FM36.Service.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private readonly HashSet<int> _fundModels = new HashSet<int> { InputAttributes.FundModel_36 };

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IAppsEarningsHistoryReferenceDataService _appsEarningsHistoryReferenceDataService;
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(
            ILARSReferenceDataService larsReferenceDataService,
            IPostcodesReferenceDataService postcodesReferenceDataService,
            IAppsEarningsHistoryReferenceDataService appsEarningsHistoryReferenceDataService,
            IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
            _appsEarningsHistoryReferenceDataService = appsEarningsHistoryReferenceDataService;
            _fileDataService = fileDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            var global = BuildGlobal();

            return inputModels.Where(l => l.LearningDeliveries.Any(ld => _fundModels.Contains(ld.FundModel))).Select(l => BuildGlobalDataEntity(l, global));
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner, Global global)
        {
            return new DataEntity(InputAttributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { InputAttributes.Year, new AttributeData(global.Year) },
                    { InputAttributes.CollectionPeriod, new AttributeData(global.CollectionPeriod) },
                    { InputAttributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            var learnerEmploymentStatusDenormalized = BuildLearnerEmploymentStatusDenormalized(learner.LearnerEmploymentStatuses);
            var sfaPostDisadvantage = _postcodesReferenceDataService.SFADisadvantagesForPostcode(learner.PostcodePrior);
            var appsEarningsHistory = _appsEarningsHistoryReferenceDataService.AECEarningsHistory(learner.ULN);

            return new DataEntity(InputAttributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { InputAttributes.DateOfBirth, new AttributeData(learner.DateOfBirthNullable) },
                    { InputAttributes.ULN, new AttributeData(learner.ULN) },
                    { InputAttributes.PrevUKPRN, new AttributeData(learner.PrevUKPRNNullable) },
                    { InputAttributes.PMUKPRN, new AttributeData(learner.PMUKPRNNullable) }
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            learnerEmploymentStatusDenormalized?
                            .Select(BuildLearnerEmploymentStatus))
                        .Union(
                            sfaPostDisadvantage?
                            .Select(BuildSFAPostcodeDisadvantage))
                        .Union(
                            appsEarningsHistory?
                            .Select(BuildApprenticeshipsEarningsHistory))
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var larsStandardAppenticeshipFunding = _larsReferenceDataService.LARSStandardApprenticeshipFunding(learningDelivery.StdCodeNullable, learningDelivery.ProgTypeNullable);
            var larsFrameworkAppenticeshipFunding = _larsReferenceDataService.LARSFrameworkApprenticeshipFunding(learningDelivery.FworkCodeNullable, learningDelivery.ProgTypeNullable, learningDelivery.PwayCodeNullable);
            var larsFrameworkCommonComponent = _larsReferenceDataService.LARSFrameworkCommonComponent(learningDelivery.LearnAimRef, learningDelivery.FworkCodeNullable, learningDelivery.ProgTypeNullable, learningDelivery.PwayCodeNullable);
            var larsStandardCommonComponent = _larsReferenceDataService.LARSStandardCommonComponent(learningDelivery.StdCodeNullable);

            return new DataEntity(InputAttributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { InputAttributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { InputAttributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { InputAttributes.FrameworkCommonComponent, new AttributeData(larsLearningDelivery.FrameworkCommonComponent) },
                    { InputAttributes.FworkCode, new AttributeData(learningDelivery.FworkCodeNullable) },
                    { InputAttributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { InputAttributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { InputAttributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { InputAttributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { InputAttributes.LrnDelFAM_EEF, new AttributeData(learningDeliveryFAMDenormalized.EEF) },
                    { InputAttributes.LrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { InputAttributes.LrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { InputAttributes.LrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { InputAttributes.LrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { InputAttributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDateNullable) },
                    { InputAttributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdjNullable) },
                    { InputAttributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdjNullable) },
                    { InputAttributes.ProgType, new AttributeData(learningDelivery.ProgTypeNullable) },
                    { InputAttributes.PwayCode, new AttributeData(learningDelivery.PwayCodeNullable) },
                    { InputAttributes.STDCode, new AttributeData(learningDelivery.StdCodeNullable) },
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                   learningDelivery?
                                    .AppFinRecords?
                                    .Select(BuildApprenticeshipFinancialRecord))
                            .Union(
                                   larsStandardAppenticeshipFunding?
                                    .Select(BuildLARSStandardApprenticeshipFunding))
                            .Union(
                                   larsFrameworkAppenticeshipFunding?
                                    .Select(BuildLARSFrameworkApprenticeshipFunding))
                            .Union(
                                   larsFrameworkCommonComponent?
                                    .Select(BuildLARSFrameworkCommonComponent))
                            .Union(
                                   larsStandardCommonComponent?
                                    .Select(BuildLARSStandardCommonComponent))
                            .Union(
                                   larsLearningDelivery?
                                    .LARSFunding?
                                    .Select(BuildLARSFunding))
                            .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryFAM(ILearningDeliveryFAM learningDeliveryFAM)
        {
            return new DataEntity(InputAttributes.EntityLearningDeliveryFAM)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.LearnDelFAMCode, new AttributeData(learningDeliveryFAM.LearnDelFAMCode) },
                    { InputAttributes.LearnDelFAMDateTo, new AttributeData(learningDeliveryFAM.LearnDelFAMDateToNullable) },
                    { InputAttributes.LearnDelFAMDateFrom, new AttributeData(learningDeliveryFAM.LearnDelFAMDateFromNullable) },
                    { InputAttributes.LearnDelFAMType, new AttributeData(learningDeliveryFAM.LearnDelFAMType) },
                }
            };
        }

        public IDataEntity BuildLearnerEmploymentStatus(LearnerEmploymentStatusDenormalized learnerEmploymentStatus)
        {
            var l = learnerEmploymentStatus;

            return new DataEntity(InputAttributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.AgreeId, new AttributeData(learnerEmploymentStatus.AgreeId) },
                    { InputAttributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { InputAttributes.EmpId, new AttributeData(learnerEmploymentStatus.EmpId) },
                    { InputAttributes.EMPStat, new AttributeData(learnerEmploymentStatus.EMPStat) },
                    { InputAttributes.EmpStatMon_SEM, new AttributeData(learnerEmploymentStatus.SEM) }
                }
            };
        }

        public IDataEntity BuildSFAPostcodeDisadvantage(SfaDisadvantage sfaDisadvantage)
        {
            return new DataEntity(InputAttributes.EntitySFA_PostcodeDisadvantage)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.DisApprenticeshipUplift, new AttributeData(sfaDisadvantage.Uplift) },
                    { InputAttributes.DisUpEffectiveFrom, new AttributeData(sfaDisadvantage.EffectiveFrom) },
                    { InputAttributes.DisUpEffectiveTo, new AttributeData(sfaDisadvantage.EffectiveTo) }
                }
            };
        }

        public IDataEntity BuildApprenticeshipsEarningsHistory(AECEarningsHistory aecEarningsHistory)
        {
            return new DataEntity(InputAttributes.EntitySFA_PostcodeDisadvantage)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.AppProgCompletedInTheYearInput, new AttributeData(aecEarningsHistory.AppProgCompletedInTheYearInput) },
                    { InputAttributes.HistoricCollectionReturnInput, new AttributeData(aecEarningsHistory.CollectionReturnCode) },
                    { InputAttributes.HistoricCollectionYearInput, new AttributeData(aecEarningsHistory.CollectionYear) },
                    { InputAttributes.HistoricDaysInYearInput, new AttributeData(aecEarningsHistory.DaysInYear) },
                    { InputAttributes.HistoricEffectiveTNPStartDateInput, new AttributeData(aecEarningsHistory.HistoricEffectiveTNPStartDateInput) },
                    { InputAttributes.HistoricEmpIdStartWithinYearInput, new AttributeData(aecEarningsHistory.HistoricEmpIdEndWithinYear) },
                    { InputAttributes.HistoricEmpIdEndWithinYearInput, new AttributeData(aecEarningsHistory.HistoricEmpIdEndWithinYear) },
                    { InputAttributes.HistoricFworkCodeInput, new AttributeData(aecEarningsHistory.FworkCode) },
                    { InputAttributes.HistoricLearnDelProgEarliestACT2DateInput, new AttributeData(aecEarningsHistory.HistoricLearnDelProgEarliestACT2DateInput) },
                    { InputAttributes.HistoricLearner1618AtStartInput, new AttributeData(aecEarningsHistory.HistoricLearner1618StartInput) },
                    { InputAttributes.HistoricLearnRefNumberInput, new AttributeData(aecEarningsHistory.LearnRefNumber) },
                    { InputAttributes.HistoricPMRAmountInput, new AttributeData(aecEarningsHistory.HistoricPMRAmount) },
                    { InputAttributes.HistoricProgrammeStartDateIgnorePathwayInput, new AttributeData(aecEarningsHistory.ProgrammeStartDateIgnorePathway) },
                    { InputAttributes.HistoricProgrammeStartDateMatchPathwayInput, new AttributeData(aecEarningsHistory.ProgrammeStartDateMatchPathway) },
                    { InputAttributes.HistoricProgTypeInput, new AttributeData(aecEarningsHistory.ProgType) },
                    { InputAttributes.HistoricPwayCodeInput, new AttributeData(aecEarningsHistory.PwayCode) },
                    { InputAttributes.HistoricTotalProgAimPaymentsInTheYearInput, new AttributeData(aecEarningsHistory.TotalProgAimPaymentsInTheYear) },
                    { InputAttributes.HistoricTotal1618UpliftPaymentsInTheYearInput, new AttributeData(aecEarningsHistory.HistoricTotal1618UpliftPaymentsInTheYearInput) },
                    { InputAttributes.HistoricSTDCodeInput, new AttributeData(aecEarningsHistory.STDCode) },
                    { InputAttributes.HistoricTNP1Input, new AttributeData(aecEarningsHistory.HistoricTNP1Input) },
                    { InputAttributes.HistoricTNP2Input, new AttributeData(aecEarningsHistory.HistoricTNP2Input) },
                    { InputAttributes.HistoricTNP3Input, new AttributeData(aecEarningsHistory.HistoricTNP3Input) },
                    { InputAttributes.HistoricTNP4Input, new AttributeData(aecEarningsHistory.HistoricTNP4Input) },
                    { InputAttributes.HistoricUKPRNInput, new AttributeData(aecEarningsHistory.UKPRN) },
                    { InputAttributes.HistoricULNInput, new AttributeData(aecEarningsHistory.ULN) },
                    { InputAttributes.HistoricUptoEndDateInput, new AttributeData(aecEarningsHistory.UptoEndDate) },
                    { InputAttributes.HistoricVirtualTNP3EndofTheYearInput, new AttributeData(aecEarningsHistory.HistoricVirtualTNP3EndOfTheYearInput) },
                    { InputAttributes.HistoricVirtualTNP4EndofTheYearInput, new AttributeData(aecEarningsHistory.HistoricVirtualTNP4EndOfTheYearInput) }
                }
            };
        }

        public IDataEntity BuildApprenticeshipFinancialRecord(IAppFinRecord appFinRecord)
        {
            return new DataEntity(InputAttributes.EntityApprenticeshipFinancialRecord)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.AFinAmount, new AttributeData(appFinRecord.AFinAmount) },
                    { InputAttributes.AFinCode, new AttributeData(appFinRecord.AFinCode) },
                    { InputAttributes.AFinDate, new AttributeData(appFinRecord.AFinDate) },
                    { InputAttributes.AFinType, new AttributeData(appFinRecord.AFinType) }
                }
            };
        }

        public IDataEntity BuildLARSStandardApprenticeshipFunding(LARSStandardApprenticeshipFunding larsStandardApprenticeshipFunding)
        {
            return new DataEntity(InputAttributes.EntityStandardLARSApprenticshipFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.StandardAF1618EmployerAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenEmployerAdditionalPayment) },
                    { InputAttributes.StandardAF1618ProviderAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenProviderAdditionalPayment) },
                    { InputAttributes.StandardAF1618FrameworkUplift, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenFrameworkUplift) },
                    { InputAttributes.StandardAFCareLeaverAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.CareLeaverAdditionalPayment) },
                    { InputAttributes.StandardAFEffectiveFrom, new AttributeData(larsStandardApprenticeshipFunding.EffectiveFrom) },
                    { InputAttributes.StandardAFEffectiveTo, new AttributeData(larsStandardApprenticeshipFunding.EffectiveTo) },
                    { InputAttributes.StandardAFFundingCategory, new AttributeData(larsStandardApprenticeshipFunding.FundingCategory) },
                    { InputAttributes.StandardAFMaxEmployerLevyCap, new AttributeData(larsStandardApprenticeshipFunding.MaxEmployerLevyCap) },
                    { InputAttributes.StandardAFReservedValue2, new AttributeData(larsStandardApprenticeshipFunding.ReservedValue2) },
                    { InputAttributes.StandardAFReservedValue3, new AttributeData(larsStandardApprenticeshipFunding.ReservedValue3) }
                }
            };
        }

        public IDataEntity BuildLARSFrameworkApprenticeshipFunding(LARSFrameworkApprenticeshipFunding larsFrameworkApprenticeshipFunding)
        {
            return new DataEntity(InputAttributes.EntityFrameworkLARSApprenticshipFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.FrameworkAF1618EmployerAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenEmployerAdditionalPayment) },
                    { InputAttributes.FrameworkAF1618ProviderAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenProviderAdditionalPayment) },
                    { InputAttributes.FrameworkAF1618FrameworkUplift, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenFrameworkUplift) },
                    { InputAttributes.FrameworkAFCareLeaverAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.CareLeaverAdditionalPayment) },
                    { InputAttributes.FrameworkAFEffectiveFrom, new AttributeData(larsFrameworkApprenticeshipFunding.EffectiveFrom) },
                    { InputAttributes.FrameworkAFEffectiveTo, new AttributeData(larsFrameworkApprenticeshipFunding.EffectiveTo) },
                    { InputAttributes.FrameworkAFFundingCategory, new AttributeData(larsFrameworkApprenticeshipFunding.FundingCategory) },
                    { InputAttributes.FrameworkAFMaxEmployerLevyCap, new AttributeData(larsFrameworkApprenticeshipFunding.MaxEmployerLevyCap) },
                    { InputAttributes.FrameworkAFReservedValue2, new AttributeData(larsFrameworkApprenticeshipFunding.ReservedValue2) },
                    { InputAttributes.FrameworkAFReservedValue3, new AttributeData(larsFrameworkApprenticeshipFunding.ReservedValue3) }
                }
            };
        }

        public IDataEntity BuildLARSFrameworkCommonComponent(LARSFrameworkCommonComponent larsFrameworkCommonComponent)
        {
            return new DataEntity(InputAttributes.EntityLARSFrameworkCmnComp)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.LARSFrameworkCommonComponentCode, new AttributeData(larsFrameworkCommonComponent.CommonComponent) },
                    { InputAttributes.LARSFrameworkCommonComponentEffectiveFrom, new AttributeData(larsFrameworkCommonComponent.EffectiveFrom) },
                    { InputAttributes.LARSFrameworkCommonComponentEffectiveTo, new AttributeData(larsFrameworkCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSStandardCommonComponent(LARSStandardCommonComponent larsStandardCommonComponent)
        {
            return new DataEntity(InputAttributes.EntityStandardCommonComponent)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.LARSStandardCommonComponentCode, new AttributeData(larsStandardCommonComponent.CommonComponent) },
                    { InputAttributes.LARSStandardCommonComponentEffectiveFrom, new AttributeData(larsStandardCommonComponent.EffectiveFrom) },
                    { InputAttributes.LARSStandardCommonComponentEffectiveTo, new AttributeData(larsStandardCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSFunding(LARSFunding larsFunding)
        {
            return new DataEntity(InputAttributes.EntityLearningDeliveryLARS_Funding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { InputAttributes.LARSFundCategory, new AttributeData(larsFunding.FundingCategory) },
                    { InputAttributes.LARSFundEffectiveFrom, new AttributeData(larsFunding.EffectiveFrom) },
                    { InputAttributes.LARSFundEffectiveTo, new AttributeData(larsFunding.EffectiveTo) },
                    { InputAttributes.LARSFundWeightedRate, new AttributeData(larsFunding.RateWeighted) },
                }
            };
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                Year = InputAttributes.YearValue,
                // ToDo: implement AcademicYear service over InternalCache for "CollectionPeriod" to calculate value.
                // This attribute is not used by rulebase at present 10/09/18.
                CollectionPeriod = InputAttributes.CollectionPeriodValue,
                UKPRN = _fileDataService.UKPRN()
            };
        }

        public IEnumerable<LearnerEmploymentStatusDenormalized> BuildLearnerEmploymentStatusDenormalized(IEnumerable<ILearnerEmploymentStatus> learnerEmploymentStatuses)
        {
            return learnerEmploymentStatuses?.Select(les => new LearnerEmploymentStatusDenormalized
            {
                AgreeId = les.AgreeId,
                DateEmpStatApp = les.DateEmpStatApp,
                EmpId = les.EmpIdNullable,
                EMPStat = les.EmpStat,
                SEM = les.EmploymentStatusMonitorings.Where(e => e.ESMType == InputAttributes.EmpStatMon_SEM).Select(e => e.ESMCode).FirstOrDefault()
            });
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new Service.Model.LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == InputAttributes.LDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.EEF = learningDeliveryFams.Where(f => f.LearnDelFAMType == InputAttributes.EEF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}