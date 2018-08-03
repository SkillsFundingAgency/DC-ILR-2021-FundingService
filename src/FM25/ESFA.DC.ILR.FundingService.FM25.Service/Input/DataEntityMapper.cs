using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private const string EntityGlobal = "global";
        private const string EntityLearner = "Learner";
        private const string EntityLearningDelivery = "LearningDelivery";
        private const string EntityDPOutcome = "DPOutcome";
        private const string EntityLearningDeliveryLARSValidity = "LearningDeliveryLARSValidity";

        private const string GlobalAreaCostFactor1618 = "AreaCostFactor1618";
        private const string GlobalDisadvantageProportion = "DisadvantageProportion";
        private const string GlobalHistoricLargeProgrammeProportion = "HistoricLargeProgrammeProportion";
        private const string GlobalLARSVersion = "LARSVersion";
        private const string GlobalOrgVersion = "OrgVersion";
        private const string GlobalProgrammeWeighting = "ProgrammeWeighting";
        private const string GlobalRetentionFactor = "RetentionFactor";
        private const string GlobalSpecialistResources = "SpecialistResources";
        private const string GlobalUKPRN = "UKPRN";

        private const string LearnerDateOfBirth = "DateOfBirth";
        private const string LearnerEngGrade = "EngGrade";
        private const string LearnerLearnRefNumber = "LearnRefNumber";
        private const string LearnerLrnFAM_ECF = "LrnFAM_ECF";
        private const string LearnerLrnFAM_EDF1 = "LrnFAM_EDF1";
        private const string LearnerLrnFAM_EDF2 = "LrnFAM_EDF2";
        private const string LearnerLrnFAM_EHC = "LrnFAM_EHC";
        private const string LearnerLrnFAM_HNS = "LrnFAM_HNS";
        private const string LearnerLrnFAM_MCF = "LrnFAM_MCF";
        private const string LearnerMathGrade = "MathGrade";
        private const string LearnerPlanEEPHours = "PlanEEPHours";
        private const string LearnerPlanLearnHours = "PlanLearnHours";
        private const string LearnerPostcodeDisadvantageUplift = "PostcodeDisadvantageUplift";
        private const string LearnerULN = "ULN";

        private const string LearningDeliveryAimSeqNumber = "AimSeqNumber";
        private const string LearningDeliveryAimType = "AimType";
        private const string LearningDeliveryAwardOrgCode = "AwardOrgCode";
        private const string LearningDeliveryCompStatus = "CompStatus";
        private const string LearningDeliveryEFACOFType = "EFACOFType";
        private const string LearningDeliveryFundModel = "FundModel";
        private const string LearningDeliveryLearnActEndDate = "LearnActEndDate";
        private const string LearningDeliveryLearnAimRef = "LearnAimRef";
        private const string LearningDeliveryLearnAimRefTitle = "LearnAimRefTitle";
        private const string LearningDeliveryLearnAimRefType = "LearnAimRefType";
        private const string LearningDeliveryLearnPlanEndDate = "LearnPlanEndDate";
        private const string LearningDeliveryLearnStartDate = "LearnStartDate";
        private const string LearningDeliveryLrnDelFAM_SOF = "LrnDelFAM_SOF";
        private const string LearningDeliveryLearnDelFAM_LDM1 = "LearnDelFAM_LDM1";
        private const string LearningDeliveryLearnDelFAM_LDM2 = "LearnDelFAM_LDM2";
        private const string LearningDeliveryLearnDelFAM_LDM3 = "LearnDelFAM_LDM3";
        private const string LearningDeliveryLearnDelFAM_LDM4 = "LearnDelFAM_LDM4";
        private const string LearningDeliveryProgType = "ProgType";
        private const string LearningDeliverySectorSubjectAreaTier2 = "SectorSubjectAreaTier2";
        private const string LearningDeliveryWithdrawReason = "WithdrawReason";

        private const string DPOutcomeOutCode = "OutCode";
        private const string DPOutcomeOutType = "OutType";

        private const string LearningDeliveryLARSValidityValidityCategory = "ValidityCategory";
        private const string LearningDeliveryLARSValidityValidityLastNewStartDate = "ValidityLastNewStartDate";
        private const string LearningDeliveryLARSValidityValidityStartDate = "ValidityStartDate";

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IOrganisationReferenceDataService _organisationReferenceDataService;
        private readonly IFileDataService _fileDataService;

        private readonly IAttributeData todo = null;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService, IOrganisationReferenceDataService organisationReferenceDataService, IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _organisationReferenceDataService = organisationReferenceDataService;
            _fileDataService = fileDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            return inputModels.Select(BuildGlobalDataEntity);
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner)
        {
            return new DataEntity(EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { GlobalAreaCostFactor1618, todo },
                    { GlobalDisadvantageProportion, todo },
                    { GlobalHistoricLargeProgrammeProportion, todo },
                    { GlobalLARSVersion, new AttributeData(_larsReferenceDataService.LARSCurrentVersion()) },
                    { GlobalOrgVersion, new AttributeData(_organisationReferenceDataService.OrganisationVersion()) },
                    { GlobalProgrammeWeighting, todo },
                    { GlobalRetentionFactor, todo },
                    { GlobalSpecialistResources, todo },
                    { GlobalUKPRN, new AttributeData(_fileDataService.UKPRN()) }
                },
                Children = { BuildLearnerDataEntity(learner) }
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            return new DataEntity(EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearnerDateOfBirth, new AttributeData(learner.DateOfBirthNullable) },
                    { LearnerEngGrade, new AttributeData(learner.EngGrade) },
                    { LearnerLearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { LearnerLrnFAM_ECF, todo },
                    { LearnerLrnFAM_EDF1, todo },
                    { LearnerLrnFAM_EDF2, todo },
                    { LearnerLrnFAM_EHC, todo },
                    { LearnerLrnFAM_HNS, todo },
                    { LearnerLrnFAM_MCF, todo },
                    { LearnerMathGrade, new AttributeData(learner.MathGrade) },
                    { LearnerPlanEEPHours, new AttributeData(learner.PlanEEPHoursNullable) },
                    { LearnerPlanLearnHours, new AttributeData(learner.PlanLearnHoursNullable) },
                    { LearnerPostcodeDisadvantageUplift, todo },
                    { LearnerULN, new AttributeData(learner.ULN) },
                },
                Children =
                    learner
                        .LearningDeliveries
                        .Select(BuildLearningDeliveryDataEntity)
                        .Union(
                            _fileDataService.DPOutcomesForLearnRefNumber(learner.LearnRefNumber)
                                .Select(BuildDPOutcome))
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            return new DataEntity(EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearningDeliveryAimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { LearningDeliveryAimType, new AttributeData(learningDelivery.AimType) },
                    { LearningDeliveryAwardOrgCode, todo },
                    { LearningDeliveryCompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { LearningDeliveryEFACOFType, todo },
                    { LearningDeliveryFundModel, new AttributeData(learningDelivery.FundModel) },
                    { LearningDeliveryLearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { LearningDeliveryLearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { LearningDeliveryLearnAimRefTitle, todo },
                    { LearningDeliveryLearnAimRefType, todo },
                    { LearningDeliveryLearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { LearningDeliveryLearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { LearningDeliveryLrnDelFAM_SOF, todo },
                    { LearningDeliveryLearnDelFAM_LDM1, todo },
                    { LearningDeliveryLearnDelFAM_LDM2, todo },
                    { LearningDeliveryLearnDelFAM_LDM3, todo },
                    { LearningDeliveryLearnDelFAM_LDM4, todo },
                    { LearningDeliveryProgType, new AttributeData(learningDelivery.ProgTypeNullable) },
                    { LearningDeliverySectorSubjectAreaTier2, todo },
                    { LearningDeliveryWithdrawReason, new AttributeData(learningDelivery.WithdrawReasonNullable) },
                },
                Children =
                    _larsReferenceDataService
                        .LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)?
                        .LARSValidities
                        .Select(BuildLearningDeliveryLARSValidity)
                        .ToList()
                    ?? new List<IDataEntity>()
            };
        }

        public IDataEntity BuildDPOutcome(DPOutcome dpOutcome)
        {
            return new DataEntity(EntityDPOutcome)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { DPOutcomeOutCode, new AttributeData(dpOutcome.OutCode) },
                    { DPOutcomeOutType, new AttributeData(dpOutcome.OutType) },
                }
            };
        }

        public IDataEntity BuildLearningDeliveryLARSValidity(LARSValidity larsValidity)
        {
            return new DataEntity(EntityLearningDeliveryLARSValidity)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearningDeliveryLARSValidityValidityCategory,  todo },
                    { LearningDeliveryLARSValidityValidityLastNewStartDate, todo },
                    { LearningDeliveryLARSValidityValidityStartDate, todo },
                }
            };
        }
    }
}