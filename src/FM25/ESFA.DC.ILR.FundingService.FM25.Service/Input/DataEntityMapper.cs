using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.ILR.FundingService.FM25.Service.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<FM25LearnerDto>
    {
        private readonly int _fundModel = Attributes.FundModel_25;
        private readonly DateTime _orgFundingAppliesFrom = new DateTime(2019, 8, 1);

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IOrganisationReferenceDataService _organisationReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService, IOrganisationReferenceDataService organisationReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _organisationReferenceDataService = organisationReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
        }

        public IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<FM25LearnerDto> inputModels)
        {
            var global = BuildGlobal(ukprn);

            var entities = inputModels?
                .Where(l => l.LearningDeliveries
                .Any(ld => ld.FundModel == _fundModel))
                .Select(l => BuildGlobalDataEntity(l, global)) ?? new List<IDataEntity>();

            return entities.Any() ? entities : new List<IDataEntity> { BuildDefaultGlobalDataEntity(global) };
        }

        public IDataEntity BuildGlobalDataEntity(FM25LearnerDto learner, Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AreaCostFactor1618, new AttributeData(global.AreaCostFactor1618) },
                    { Attributes.DisadvantageProportion, new AttributeData(global.DisadvantageProportion) },
                    { Attributes.HistoricLargeProgrammeProportion, new AttributeData(global.HistoricLargeProgrammeProportion) },
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.OrgVersion, new AttributeData(global.OrgVersion) },
                    { Attributes.PostcodeDisadvantageVersion, new AttributeData(global.PostcodesVersion) },
                    { Attributes.ProgrammeWeighting, new AttributeData(global.ProgrammeWeighting) },
                    { Attributes.RetentionFactor, new AttributeData(global.RetentionFactor) },
                    { Attributes.SpecialistResources, new AttributeData(global.SpecialistResources) },
                    { Attributes.SpecialistCampIDPCW, new AttributeData(global.SpecialistCampIDPCW) },                    
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
                    { Attributes.AreaCostFactor1618, new AttributeData(global.AreaCostFactor1618) },
                    { Attributes.DisadvantageProportion, new AttributeData(global.DisadvantageProportion) },
                    { Attributes.HistoricLargeProgrammeProportion, new AttributeData(global.HistoricLargeProgrammeProportion) },
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.OrgVersion, new AttributeData(global.OrgVersion) },
                    { Attributes.PostcodeDisadvantageVersion, new AttributeData(global.PostcodesVersion) },
                    { Attributes.ProgrammeWeighting, new AttributeData(global.ProgrammeWeighting) },
                    { Attributes.RetentionFactor, new AttributeData(global.RetentionFactor) },
                    { Attributes.SpecialistResources, new AttributeData(global.SpecialistResources) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                }
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM25LearnerDto learner)
        {
            var efaDisadvantageUplift = _postcodesReferenceDataService.LatestEFADisadvantagesUpliftForPostcode(learner.Postcode);

            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirth) },
                    { Attributes.EngGrade, new AttributeData(learner.EngGrade) },
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.LrnFAM_ECF, new AttributeData(learner.LrnFAM_ECF) },
                    { Attributes.LrnFAM_EDF1, new AttributeData(learner.LrnFAM_EDF1) },
                    { Attributes.LrnFAM_EDF2, new AttributeData(learner.LrnFAM_EDF2) },
                    { Attributes.LrnFAM_EHC, new AttributeData(learner.LrnFAM_EHC) },
                    { Attributes.LrnFAM_HNS, new AttributeData(learner.LrnFAM_HNS) },
                    { Attributes.LrnFAM_MCF, new AttributeData(learner.LrnFAM_MCF) },
                    { Attributes.MathGrade, new AttributeData(learner.MathGrade) },
                    { Attributes.PlanEEPHours, new AttributeData(learner.PlanEEPHours) },
                    { Attributes.PlanLearnHours, new AttributeData(learner.PlanLearnHours) },
                    { Attributes.PostcodeDisadvantageUplift, new AttributeData(efaDisadvantageUplift) },
                    { Attributes.ULN, new AttributeData(learner.ULN) },
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            learner.DPOutcomes?
                                .Select(BuildDPOutcome) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(LearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.AwardOrgCode, new AttributeData(larsLearningDelivery.AwardOrgCode) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.EFACOFType, new AttributeData(larsLearningDelivery.EFACOFType) },
                    { Attributes.FundModel, new AttributeData(learningDelivery.FundModel) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDate) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnAimRefTitle, new AttributeData(larsLearningDelivery.LearnAimRefTitle) },
                    { Attributes.LearnAimRefType, new AttributeData(larsLearningDelivery.LearnAimRefType) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgType) },
                    { Attributes.SectorSubjectAreaTier2, new AttributeData(larsLearningDelivery.SectorSubjectAreaTier2) },
                    { Attributes.WithdrawReason, new AttributeData(learningDelivery.WithdrawReason) },
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                    larsLearningDelivery?
                                    .LARSValidities?
                                    .Select(BuildLearningDeliveryLARSValidity) ?? new List<IDataEntity>())
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

        public IDataEntity BuildDPOutcome(DPOutcome dpOutcome)
        {
            return new DataEntity(Attributes.EntityDPOutcome)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.OutCode, new AttributeData(dpOutcome.OutCode) },
                    { Attributes.OutType, new AttributeData(dpOutcome.OutType) },
                }
            };
        }

        public IDataEntity BuildLearningDeliveryLARSValidity(LARSValidity larsValidity)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARSValidity)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.ValidityCategory, new AttributeData(larsValidity.Category) },
                    { Attributes.ValidityLastNewStartDate, new AttributeData(larsValidity.LastNewStartDate) },
                    { Attributes.ValidityStartDate, new AttributeData(larsValidity.StartDate) },
                }
            };
        }

        public Global BuildGlobal(int ukprn)
        {
            var orgFundings = _organisationReferenceDataService.OrganisationFundingForUKPRN(ukprn)
                .Where(f => f.OrgFundFactType == Attributes.OrgFundFactorTypeEFA1619).ToList();

            return new Global()
            {
                AreaCostFactor1618 = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricAreaCost)?.OrgFundFactValue,
                DisadvantageProportion = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricDisadvantageFundingProportion)?.OrgFundFactValue,
                HistoricLargeProgrammeProportion = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricLargeProgProportion)?.OrgFundFactValue,
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                OrgVersion = _organisationReferenceDataService.OrganisationVersion(),
                PostcodesVersion = _postcodesReferenceDataService.PostcodesCurrentVersion(),
                ProgrammeWeighting = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoriProgCostWeigting)?.OrgFundFactValue,
                RetentionFactor = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricRetention)?.OrgFundFactValue,
                SpecialistResources = orgFundings.FirstOrDefault(f => f.OrgFundFactor == Attributes.OrgFundFactorSpecialistResources)?.OrgFundFactValue == "1" ? true : false,
                SpecialistCampIDPCW = "N",
                UKPRN = ukprn
            };
        }
    }
}