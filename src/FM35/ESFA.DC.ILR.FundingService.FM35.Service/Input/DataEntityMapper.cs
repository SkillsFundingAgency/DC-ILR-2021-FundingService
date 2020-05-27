using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Extensions;
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
                .Select(l => BuildGlobalDataEntity(l, global)) ?? Enumerable.Empty<IDataEntity>();

            return entities.Any() ? entities : new List<IDataEntity> { BuildDefaultGlobalDataEntity(global) };
        }

        public IDataEntity BuildGlobalDataEntity(FM35LearnerDto learner, Global global)
        {
            var orgFunding = _organisationReferenceDataService.OrganisationFundingForUKPRN(global.UKPRN)
                .Where(f => f.OrgFundFactType.CaseInsensitiveEquals(Attributes.OrgFundFactorTypeAdultSkills));

            var postcodeSpecialistResources = _organisationReferenceDataService.PostcodeSpecialistResourcesForUkprn(global.UKPRN).ToList();

            var orgDataEntities = orgFunding.Any() ? orgFunding?.Select(BuildOrgFundingDataEntity).ToList() : new List<IDataEntity> { new DataEntity(Attributes.EntityOrgFunding) };
            var specialistResourceEntities = postcodeSpecialistResources?.Select(BuildPostcodeSpecialistResource) ?? Enumerable.Empty<IDataEntity>();

            var entity = new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = BuildGlobalAttributes(global)
            };

            if (learner != null)
            {
                entity.AddChild(BuildLearnerDataEntity(learner));
                entity.AddChildren(orgDataEntities);
                entity.AddChildren(specialistResourceEntities);
            }

            return entity;
        }

        public IDataEntity BuildDefaultGlobalDataEntity(Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = BuildGlobalAttributes(global)
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM35LearnerDto learner)
        {
            var sfaPostDisadvantage = _postcodesReferenceDataService.SFADisadvantagesForPostcode(learner.PostcodePrior);
            var specialistResources = _organisationReferenceDataService.SpecialistResourcesForCampusIdentifier(learner.CampId);

            var learnerEmploymentStatusEntities = learner.LearnerEmploymentStatuses?.Select(BuildLearnerEmploymentStatus) ?? Enumerable.Empty<IDataEntity>();
            var learningDeliveryEntities = learner.LearningDeliveries?.Where(ld => ld.FundModel == _fundModel).Select(BuildLearningDeliveryDataEntity) ?? Enumerable.Empty<IDataEntity>();
            var sfaPostDisadvantageEntities = sfaPostDisadvantage?.Select(BuildSFAPostcodeDisadvantage) ?? Enumerable.Empty<IDataEntity>();
            var specialistResourcesEntities = specialistResources?.Select(BuildSpecialistResources) ?? Enumerable.Empty<IDataEntity>();

            var entity = new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirth) },
                }
            };

            entity.AddChildren(learningDeliveryEntities);
            entity.AddChildren(learnerEmploymentStatusEntities);
            entity.AddChildren(sfaPostDisadvantageEntities);
            entity.AddChildren(specialistResourcesEntities);

            return entity;
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

            var learningDeliveryFamsEntities = learningDelivery?.LearningDeliveryFAMs?.Select(BuildLearningDeliveryFAM) ?? Enumerable.Empty<IDataEntity>();
            var larsAnnualValueEntities = larsLearningDelivery?.LARSAnnualValues?.Select(BuildLARSAnnualValue) ?? Enumerable.Empty<IDataEntity>();
            var larsLearningDeliveryCategoryEntities = larsLearningDelivery?.LARSLearningDeliveryCategories?.Select(BuildLARSLearningDeliveryCategories) ?? Enumerable.Empty<IDataEntity>();
            var sfaAreaCostEntities = sfaAreaCost?.Select(BuildSFAAreaCost) ?? Enumerable.Empty<IDataEntity>();
            var larsFundingEntities = larsLearningDelivery?.LARSFundings?.Select(BuildLARSFunding) ?? Enumerable.Empty<IDataEntity>();

            var entity = new DataEntity(Attributes.EntityLearningDelivery)
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
                    { Attributes.FundModel, new AttributeData(learningDelivery.FundModel) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDate) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDate) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdj) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.Outcome) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdj) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgType) },
                    { Attributes.PwayCode, new AttributeData(learningDelivery.PwayCode) }
                }
            };

            entity.AddChildren(learningDeliveryFamsEntities);
            entity.AddChildren(larsAnnualValueEntities);
            entity.AddChildren(larsLearningDeliveryCategoryEntities);
            entity.AddChildren(sfaAreaCostEntities);
            entity.AddChildren(larsFundingEntities);

            return entity;
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

        public IDataEntity BuildSpecialistResources(CampusIdentifierSpecResource campusIdentifierSpecResource)
        {
            return new DataEntity(Attributes.EntityCampusIdentifiers)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.CampIdSpecialistResources, new AttributeData(campusIdentifierSpecResource.SpecialistResources) },
                    { Attributes.CampIdEffectiveFrom, new AttributeData(campusIdentifierSpecResource.EffectiveFrom) },
                    { Attributes.CampIdEffectiveTo, new AttributeData(campusIdentifierSpecResource.EffectiveTo) }
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

        public IDataEntity BuildPostcodeSpecialistResource(PostcodeSpecialistResource specResource)
        {
            return new DataEntity(Attributes.EntityPostcodeSpecialistResources)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.PostcodeSpecResPostcode, new AttributeData(specResource.Postcode) },
                    { Attributes.PostcodeSpecResSpecialistResources, new AttributeData(specResource.SpecialistResources) },
                    { Attributes.PostcodeSpecResEffectiveFrom, new AttributeData(specResource.EffectiveFrom) },
                    { Attributes.PostcodeSpecResEffectiveTo, new AttributeData(specResource.EffectiveTo) },
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

        private IDictionary<string, IAttributeData> BuildGlobalAttributes(Global global)
        {
            return new Dictionary<string, IAttributeData>
            {
                 { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                 { Attributes.OrgVersion, new AttributeData(global.OrgVersion) },
                 { Attributes.PostcodeDisadvantageVersion, new AttributeData(global.PostcodeDisadvantageVersion) },
                 { Attributes.UKPRN, new AttributeData(global.UKPRN) }
            };
        }
    }
}