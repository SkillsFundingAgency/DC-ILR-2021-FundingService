using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.FM25.Service.Model;
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

        private const string ECF = "ECF";
        private const string EHC = "EHC";
        private const string HNS = "HNS";
        private const string MCF = "MCF";
        private const string EDF = "EDF";
        private const string LDM = "LDM";
        private const string SOF = "SOF";

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IOrganisationReferenceDataService _organisationReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IFileDataService _fileDataService;

        private readonly IAttributeData todo = null;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService, IOrganisationReferenceDataService organisationReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService, IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _organisationReferenceDataService = organisationReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
            _fileDataService = fileDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            var global = BuildGlobal();

            return inputModels.Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == 25)).Select(l => BuildGlobalDataEntity(l, global));
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner, Global global)
        {
            return new DataEntity(EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { GlobalAreaCostFactor1618, new AttributeData(global.AreaCostFactor1618) },
                    { GlobalDisadvantageProportion, new AttributeData(global.DisadvantageProportion) },
                    { GlobalHistoricLargeProgrammeProportion, new AttributeData(global.HistoricLargeProgrammeProportion) },
                    { GlobalLARSVersion, new AttributeData(global.LARSVersion) },
                    { GlobalOrgVersion, new AttributeData(global.OrgVersion) },
                    { GlobalProgrammeWeighting, new AttributeData(global.ProgrammeWeighting) },
                    { GlobalRetentionFactor, new AttributeData(global.RetentionFactor) },
                    { GlobalSpecialistResources, new AttributeData(global.SpecialistResources) },
                    { GlobalUKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            var learnerFamDenormalized = BuildLearnerFAMDenormalized(learner.LearnerFAMs);
            var efaDisadvantage = _postcodesReferenceDataService.EFADisadvantagesForPostcode(learner.Postcode).FirstOrDefault();

            return new DataEntity(EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearnerDateOfBirth, new AttributeData(learner.DateOfBirthNullable) },
                    { LearnerEngGrade, new AttributeData(learner.EngGrade) },
                    { LearnerLearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { LearnerLrnFAM_ECF, new AttributeData(learnerFamDenormalized.ECF) },
                    { LearnerLrnFAM_EDF1, new AttributeData(learnerFamDenormalized.EDF1) },
                    { LearnerLrnFAM_EDF2, new AttributeData(learnerFamDenormalized.EDF2) },
                    { LearnerLrnFAM_EHC, new AttributeData(learnerFamDenormalized.EHC) },
                    { LearnerLrnFAM_HNS, new AttributeData(learnerFamDenormalized.HNS) },
                    { LearnerLrnFAM_MCF, new AttributeData(learnerFamDenormalized.MCF) },
                    { LearnerMathGrade, new AttributeData(learner.MathGrade) },
                    { LearnerPlanEEPHours, new AttributeData(learner.PlanEEPHoursNullable) },
                    { LearnerPlanLearnHours, new AttributeData(learner.PlanLearnHoursNullable) },
                    { LearnerPostcodeDisadvantageUplift, new AttributeData(efaDisadvantage?.Uplift) },
                    { LearnerULN, new AttributeData(learner.ULN) },
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            _fileDataService.DPOutcomesForLearnRefNumber(learner.LearnRefNumber)?
                                .Select(BuildDPOutcome) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);

            return new DataEntity(EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearningDeliveryAimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { LearningDeliveryAimType, new AttributeData(learningDelivery.AimType) },
                    { LearningDeliveryAwardOrgCode, new AttributeData(larsLearningDelivery.AwardOrgCode) },
                    { LearningDeliveryCompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { LearningDeliveryEFACOFType, new AttributeData(larsLearningDelivery.EFACOFType) },
                    { LearningDeliveryFundModel, new AttributeData(learningDelivery.FundModel) },
                    { LearningDeliveryLearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { LearningDeliveryLearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { LearningDeliveryLearnAimRefTitle, new AttributeData(larsLearningDelivery.LearnAimRefTitle) },
                    { LearningDeliveryLearnAimRefType, new AttributeData(larsLearningDelivery.LearnAimRefType) },
                    { LearningDeliveryLearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { LearningDeliveryLearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { LearningDeliveryLrnDelFAM_SOF, new AttributeData(learningDeliveryFAMDenormalized.SOF) },
                    { LearningDeliveryLearnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { LearningDeliveryLearnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { LearningDeliveryLearnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { LearningDeliveryLearnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { LearningDeliveryProgType, new AttributeData(learningDelivery.ProgTypeNullable) },
                    { LearningDeliverySectorSubjectAreaTier2, new AttributeData(larsLearningDelivery.SectorSubjectAreaTier2) },
                    { LearningDeliveryWithdrawReason, new AttributeData(learningDelivery.WithdrawReasonNullable) },
                },
                Children = larsLearningDelivery?
                        .LARSValidities?
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
                    { LearningDeliveryLARSValidityValidityCategory, new AttributeData(larsValidity.Category) },
                    { LearningDeliveryLARSValidityValidityLastNewStartDate, new AttributeData(larsValidity.LastNewStartDate) },
                    { LearningDeliveryLARSValidityValidityStartDate, new AttributeData(larsValidity.StartDate) },
                }
            };
        }

        public Global BuildGlobal()
        {
            var ukprn = _fileDataService.UKPRN();
            var orgFundings = _organisationReferenceDataService.OrganisationFundingForUKPRN(ukprn).Where(f => f.OrgFundFactType == "EFA 16-19").ToList();

            var effectiveFrom = new DateTime(2018, 8, 1); // TODO : Create Academic Year Service

            return new Global()
            {
                AreaCostFactor1618 = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == effectiveFrom && f.OrgFundFactor == "HISTORIC AREA COST FACTOR")?.OrgFundFactValue,
                DisadvantageProportion = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == effectiveFrom && f.OrgFundFactor == "HISTORIC DISADVANTAGE FUNDING PROPORTION")?.OrgFundFactValue,
                HistoricLargeProgrammeProportion = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == effectiveFrom && f.OrgFundFactor == "HISTORIC LARGE PROGRAMME PROPORTION")?.OrgFundFactValue,
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                OrgVersion = _organisationReferenceDataService.OrganisationVersion(),
                ProgrammeWeighting = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == effectiveFrom && f.OrgFundFactor == "HISTORIC PROGRAMME COST WEIGHTING FACTOR")?.OrgFundFactValue,
                RetentionFactor = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == effectiveFrom && f.OrgFundFactor == "HISTORIC RETENTION FACTOR")?.OrgFundFactValue,
                SpecialistResources = orgFundings.FirstOrDefault(f => f.OrgFundFactor == "SPECIALIST RESOURCES")?.OrgFundFactValue,
                UKPRN = ukprn,
            };
        }

        public LearnerFAMDenormalized BuildLearnerFAMDenormalized(IEnumerable<ILearnerFAM> learnerFams)
        {
            var learnerFam = new LearnerFAMDenormalized();

            if (learnerFams != null)
            {
                learnerFams = learnerFams.ToList();

                var edfArray = learnerFams.Where(f => f.LearnFAMType == EDF).Select(f => (int?)f.LearnFAMCode).ToArray();

                Array.Resize(ref edfArray, 2);

                learnerFam.ECF = learnerFams.Where(f => f.LearnFAMType == ECF).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.EDF1 = edfArray[0];
                learnerFam.EDF2 = edfArray[1];
                learnerFam.EHC = learnerFams.Where(f => f.LearnFAMType == EHC).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.HNS = learnerFams.Where(f => f.LearnFAMType == HNS).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.MCF = learnerFams.Where(f => f.LearnFAMType == MCF).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
            }

            return learnerFam;
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == LDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.SOF = learningDeliveryFams.Where(f => f.LearnDelFAMType == SOF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}