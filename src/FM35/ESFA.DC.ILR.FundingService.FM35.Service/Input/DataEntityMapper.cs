using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM35.Service.Constants;
using ESFA.DC.ILR.FundingService.FM35.Service.Models;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<FM35LearnerDto>
    {
        private readonly int _fundModel = Attributes.FundModel_35;

        private readonly ILargeEmployersReferenceDataService _largeEmployersReferenceDataService;
        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IOrganisationReferenceDataService _organisationReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;

        public DataEntityMapper(
                    ILargeEmployersReferenceDataService largeEmployersReferenceDataService,
                    ILARSReferenceDataService larsReferenceDataService,
                    IOrganisationReferenceDataService organisationReferenceDataService,
                    IPostcodesReferenceDataService postcodesReferenceDataService)
        {
            _largeEmployersReferenceDataService = largeEmployersReferenceDataService;
            _larsReferenceDataService = larsReferenceDataService;
            _organisationReferenceDataService = organisationReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
        }

        public IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<FM35LearnerDto> inputModels)
        {
            var global = BuildGlobal(ukprn);

            var entities = inputModels?
                .Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == _fundModel))
                .Select(l => BuildGlobalDataEntity(l, global)) ?? new List<IDataEntity>();

            return entities.Any() ? entities : new List<IDataEntity> { BuildDefaultGlobalDataEntity(global) };
        }

        public IDataEntity BuildGlobalDataEntity(FM35LearnerDto learner, Global global)
        {
            var orgFunding = _organisationReferenceDataService.OrganisationFundingForUKPRN(global.UKPRN).Where(f => f.OrgFundFactType == Attributes.OrgFundFactorTypeAdultSkills).ToList();

            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.OrgVersion, new AttributeData(global.OrgVersion) },
                    { Attributes.PostcodeDisadvantageVersion, new AttributeData(global.PostcodeDisadvantageVersion) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children =
                    learner != null ?
                     new List<IDataEntity> { BuildLearnerDataEntity(learner) }
                     .Union(
                         orgFunding?
                         .Select(BuildOrgFundingDataEntity) ?? new List<IDataEntity>())
                         .ToList()
                     : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildDefaultGlobalDataEntity(Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.OrgVersion, new AttributeData(global.OrgVersion) },
                    { Attributes.PostcodeDisadvantageVersion, new AttributeData(global.PostcodeDisadvantageVersion) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                }
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM35LearnerDto learner)
        {
            var sfaPostDisadvantage = _postcodesReferenceDataService.SFADisadvantagesForPostcode(learner.PostcodePrior);

            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirth) },
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
                            sfaPostDisadvantage?
                            .Select(BuildSFAPostcodeDisadvantage) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildOrgFundingDataEntity(OrgFunding orgFunding)
        {
            return new DataEntity(Attributes.EntityOrgFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                    {
                        { Attributes.OrgFundEffectiveFrom,  new AttributeData(orgFunding.OrgFundEffectiveFrom) },
                        { Attributes.OrgFundEffectiveTo, new AttributeData(orgFunding.OrgFundEffectiveTo) },
                        { Attributes.OrgFundFactor, new AttributeData(orgFunding.OrgFundFactor) },
                        { Attributes.OrgFundFactType,  new AttributeData(orgFunding.OrgFundFactType) },
                        { Attributes.OrgFundFactValue,  new AttributeData(orgFunding.OrgFundFactValue) },
                    }
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(LearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);

            var larsFramework = larsLearningDelivery.LARSFrameworks?
                .Where(lf => lf.FworkCode == learningDelivery.FworkCode
                && lf.ProgType == learningDelivery.ProgType
                && lf.PwayCode == learningDelivery.PwayCode).FirstOrDefault();

            var sfaAreaCost = _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AchDate, new AttributeData(learningDelivery.AchDate) },
                    { Attributes.AddHours, new AttributeData(learningDelivery.AddHours) },
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.EmpOutcome, new AttributeData(learningDelivery.EmpOutcome) },
                    { Attributes.EnglandFEHEStatus, new AttributeData(larsLearningDelivery.EnglandFEHEStatus) },
                    { Attributes.EnglPrscID, new AttributeData(larsLearningDelivery.EnglPrscID) },
                    { Attributes.FworkCode, new AttributeData(learningDelivery.FworkCode) },
                    { Attributes.FrameworkCommonComponent, new AttributeData(larsLearningDelivery.FrameworkCommonComponent) },
                    { Attributes.FrameworkComponentType, new AttributeData(larsFramework?.LARSFrameworkAim?.FrameworkComponentType) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDate) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_EEF, new AttributeData(learningDelivery.LrnDelFAM_EEF) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDelivery.LrnDelFAM_LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDelivery.LrnDelFAM_LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDelivery.LrnDelFAM_LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDelivery.LrnDelFAM_LDM4) },
                    { Attributes.LrnDelFAM_FFI, new AttributeData(learningDelivery.LrnDelFAM_FFI) },
                    { Attributes.LrnDelFAM_RES, new AttributeData(learningDelivery.LrnDelFAM_RES) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDate) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdj) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.Outcome) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdj) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgType) },
                    { Attributes.PwayCode, new AttributeData(learningDelivery.PwayCode) }
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                   larsLearningDelivery?
                                   .LARSAnnualValues?
                                   .Select(BuildLARSAnnualValue) ?? new List<IDataEntity>())
                            .Union(
                                   larsLearningDelivery?
                                   .LARSLearningDeliveryCategories?
                                   .Select(BuildLARSLearningDeliveryCategories) ?? new List<IDataEntity>())
                            .Union(
                                   sfaAreaCost?
                                   .Select(BuildSFAAreaCost) ?? new List<IDataEntity>())
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
            var largeEmployer = _largeEmployersReferenceDataService.LargeEmployersforEmpID(learnerEmploymentStatus.EmpId);

            return new DataEntity(Attributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpId, new AttributeData(learnerEmploymentStatus.EmpId) }
                },
                Children = (
                            largeEmployer?
                            .Select(BuildLargeEmployer) ?? new List<IDataEntity>())
                            .ToList()
            };
        }

        public IDataEntity BuildLargeEmployer(LargeEmployers largeEmployers)
        {
            return new DataEntity(Attributes.EntityLargeEmployerReferenceData)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LargeEmpEffectiveFrom, new AttributeData(largeEmployers.EffectiveFrom) },
                    { Attributes.LargeEmpEffectiveTo, new AttributeData(largeEmployers.EffectiveTo) }
                }
            };
        }

        public IDataEntity BuildSFAPostcodeDisadvantage(SfaDisadvantage sfaDisadvantage)
        {
            return new DataEntity(Attributes.EntitySFA_PostcodeDisadvantage)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DisUplift, new AttributeData(sfaDisadvantage.Uplift) },
                    { Attributes.DisUpEffectiveFrom, new AttributeData(sfaDisadvantage.EffectiveFrom) },
                    { Attributes.DisUpEffectiveTo, new AttributeData(sfaDisadvantage.EffectiveTo) }
                }
            };
        }

        public IDataEntity BuildLARSAnnualValue(LARSAnnualValue larsAnnualValue)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_AnnualValue)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelAnnValBasicSkillsTypeCode, new AttributeData(larsAnnualValue.BasicSkillsType) },
                    { Attributes.LearnDelAnnValDateFrom, new AttributeData(larsAnnualValue.EffectiveFrom) },
                    { Attributes.LearnDelAnnValDateTo, new AttributeData(larsAnnualValue.EffectiveTo) }
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
                    { Attributes.LARSFundUnweightedRate, new AttributeData(larsFunding.RateUnWeighted) },
                    { Attributes.LARSFundWeightingFactor, new AttributeData(larsFunding.WeightingFactor) },
                }
            };
        }

        public IDataEntity BuildLARSLearningDeliveryCategories(LARSLearningDeliveryCategory larsLearningDeliveryCategory)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_Category)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelCatRef, new AttributeData(larsLearningDeliveryCategory.CategoryRef) },
                    { Attributes.LearnDelCatDateFrom, new AttributeData(larsLearningDeliveryCategory.EffectiveFrom) },
                    { Attributes.LearnDelCatDateTo, new AttributeData(larsLearningDeliveryCategory.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildSFAAreaCost(SfaAreaCost sfaAreaCost)
        {
            return new DataEntity(Attributes.EntityLearningDeliverySFA_PostcodeAreaCost)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AreaCosFactor, new AttributeData(sfaAreaCost.AreaCostFactor) },
                    { Attributes.AreaCosEffectiveFrom, new AttributeData(sfaAreaCost.EffectiveFrom) },
                    { Attributes.AreaCosEffectiveTo, new AttributeData(sfaAreaCost.EffectiveTo) },
                }
            };
        }

        public Global BuildGlobal(int ukprn)
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                OrgVersion = _organisationReferenceDataService.OrganisationVersion(),
                PostcodeDisadvantageVersion = _postcodesReferenceDataService.PostcodesCurrentVersion(),
                UKPRN = ukprn
            };
        }
    }
}