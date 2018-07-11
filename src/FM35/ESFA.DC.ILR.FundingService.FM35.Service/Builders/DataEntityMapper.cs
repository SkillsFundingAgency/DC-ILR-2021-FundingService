using System;
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
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Builders
{
    public class DataEntityMapper : IDataEntityMapper<ILearner, FM35FundingOutputs>
    {
        private const string Entityglobal = "global";
        private const string EntityOrgFunding = "OrgFunding";
        private const string EntityLearner = "Learner";
        private const string EntityLearnerEmploymentStatus = "LearnerEmploymentStatus";
        private const string EntityLargeEmployerReferenceData = "LargeEmployerReferenceData";
        private const string EntitySFAPostcodeDisadvantageEntity = "SFA_PostcodeDisadvantage";
        private const string EntityLearningDelivery = "LearningDelivery";
        private const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        private const string EntityLearningDeliverySFA_PostcodeAreaCost = "SFA_PostcodeAreaCost";
        private const string EntityLearningDeliveryLARS_AnnualValue = "LearningDeliveryAnnualValue";
        private const string EntityLearningDeliveryLARS_Category = "LearningDeliveryLARSCategory";
        private const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";
        private const string LearningDeliveryFAMTypeEEF = "EEF";
        private const string LearningDeliveryFAMTypeFFI = "FFI";
        private const string LearningDeliveryFAMTypeRES = "RES";
        private const string LearningDeliveryFAMTypeLDM1 = "LDM1";
        private const string LearningDeliveryFAMTypeLDM2 = "LDM2";
        private const string LearningDeliveryFAMTypeLDM3 = "LDM3";
        private const string LearningDeliveryFAMTypeLDM4 = "LDM4";

        private readonly IFundingContext _fundingContext;
        private readonly IFileDataCache _fileDataCache;
        private readonly ILargeEmployersReferenceDataService _largeEmployersReferenceDataService;
        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IOrganisationReferenceDataService _organisationReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IAttributeBuilder<IAttributeData> _attributeBuilder;

        public DataEntityMapper(IFundingContext fundingContext, IFileDataCache fileDataCache, ILargeEmployersReferenceDataService largeEmployersReferenceDataService, ILARSReferenceDataService larsReferenceDataService, IOrganisationReferenceDataService organisationReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService, IAttributeBuilder<IAttributeData> attributeBuilder)
        {
            _fundingContext = fundingContext;
            _fileDataCache = fileDataCache;
            _largeEmployersReferenceDataService = largeEmployersReferenceDataService;
            _larsReferenceDataService = larsReferenceDataService;
            _organisationReferenceDataService = organisationReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
            _attributeBuilder = attributeBuilder;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> learners)
        {
            var globalEntities = learners.Select(learner =>
            {
                // Global Entity
                IDataEntity globalEntity = GlobalEntity(_fileDataCache.UKPRN);

                // Learner Entity
                IDataEntity learnerEntity = LearnerEntity(learner);

                foreach (var learningDelivery in learner.LearningDeliveries)
                {
                    var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
                    var larsFrameworkAims = _larsReferenceDataService.LARSFFrameworkAimsForLearnAimRef(learningDelivery.LearnAimRef);

                    var larsFwkAims = larsFrameworkAims?.ToList();
                    IDataEntity learningDeliveryEntity = LearningDeliveryEntity(learningDelivery, larsLearningDelivery, larsFwkAims);

                    learnerEntity.AddChild(learningDeliveryEntity);
                }

                globalEntity.AddChild(learnerEntity);

                return globalEntity;
            });

            return globalEntities;
        }

        public FM35FundingOutputs MapFrom(IEnumerable<IDataEntity> inputModels)
        {
            throw new NotImplementedException();
        }

        protected internal IDataEntity GlobalEntity(int ukprn)
        {
            IDataEntity globalDataEntity = new DataEntity(Entityglobal)
            {
                Attributes =
                    _attributeBuilder.BuildGlobalAttributes(ukprn, _larsReferenceDataService.LARSCurrentVersion(), _organisationReferenceDataService.OrganisationVersion(), _postcodesReferenceDataService.PostcodesCurrentVersion()),
            };

            var orgFunding = _organisationReferenceDataService.OrganisationFundingForUKPRN(ukprn);

            foreach (var o in orgFunding)
            {
                globalDataEntity.AddChild(OrgFundingEntity(o));
            }

            return globalDataEntity;
        }

        protected internal IDataEntity OrgFundingEntity(OrgFunding orgFunding)
        {
            IDataEntity orgFundingDataEnity = new DataEntity(EntityOrgFunding)
            {
                Attributes =
                    _attributeBuilder.BuildOrgFundingAttributes(
                        orgFunding.OrgFundEffectiveFrom,
                        orgFunding.OrgFundEffectiveTo,
                        orgFunding.OrgFundFactor,
                        orgFunding.OrgFundFactType,
                        orgFunding.OrgFundFactValue),
            };

            return orgFundingDataEnity;
        }

        protected internal IDataEntity LearnerEntity(ILearner learner)
        {
            IDataEntity learnerDataEntity = new DataEntity(EntityLearner)
            {
                Attributes =
                    _attributeBuilder.BuildLearnerAttributes(learner.LearnRefNumber, learner.DateOfBirthNullable),
            };

            foreach (var employmentStatus in learner.LearnerEmploymentStatuses)
            {
                IDataEntity learnerEmploymentStatusDataEntity = LearnerEmploymentStatusEntity(employmentStatus);

                learnerDataEntity.AddChild(learnerEmploymentStatusDataEntity);
            }

            var sfaPostcodeDisadvantageData = _postcodesReferenceDataService.SFADisadvantagesForPostcode(learner.PostcodePrior);

            foreach (var postcode in sfaPostcodeDisadvantageData)
            {
                learnerDataEntity.AddChild(SFAPostcodeDisadvantageEntity(postcode));
            }

            return learnerDataEntity;
        }

        protected internal IDataEntity LearnerEmploymentStatusEntity(ILearnerEmploymentStatus employmentStatus)
        {
            IDataEntity learnerEmploymentStatusDataEntity = new DataEntity(EntityLearnerEmploymentStatus)
            {
                Attributes =
                   _attributeBuilder.BuildLearnerEmploymentStatusAttributes(employmentStatus.EmpIdNullable, employmentStatus.DateEmpStatApp),
            };

            var largeEmployers = employmentStatus.EmpIdNullable != null ?
                _largeEmployersReferenceDataService.LargeEmployersforEmpID((int)employmentStatus.EmpIdNullable) : null;

            if (largeEmployers != null)
            {
                foreach (var lemp in largeEmployers)
                {
                    IDataEntity largeEmployersDataEntity = LargeEmployersEntity(lemp);

                    learnerEmploymentStatusDataEntity.AddChild(largeEmployersDataEntity);
                }
            }

            return learnerEmploymentStatusDataEntity;
        }

        protected internal IDataEntity LargeEmployersEntity(LargeEmployers largeEmployer)
        {
            IDataEntity largeEmployersDataEntity = new DataEntity(EntityLargeEmployerReferenceData)
            {
                Attributes =
                        _attributeBuilder.BuildLLargeEmployerReferenceDataAttributes(largeEmployer.EffectiveFrom, largeEmployer.EffectiveTo),
            };

            return largeEmployersDataEntity;
        }

        protected internal IDataEntity SFAPostcodeDisadvantageEntity(SfaDisadvantage postcodeData)
        {
            IDataEntity sfaPostcodeDisadvantgeEntity = new DataEntity(EntitySFAPostcodeDisadvantageEntity)
            {
                Attributes =
                        _attributeBuilder.BuildSFAPostcodeDisadvantageAttributes(postcodeData.Uplift, postcodeData.EffectiveFrom, postcodeData.EffectiveTo),
            };

            return sfaPostcodeDisadvantgeEntity;
        }

        protected internal IDataEntity LearningDeliveryEntity(ILearningDelivery learningDelivery, LARSLearningDelivery larsLearningDelivery, IList<LARSFrameworkAims> larsFrameworkAims)
        {
            var learningDeliveryFAMS = PivotLearningDeliveryFAMS(learningDelivery);

            int? frameworkComponentType = null;

            if (larsFrameworkAims != null
                && larsFrameworkAims.Select(l => l.LearnAimRef).Contains(learningDelivery.LearnAimRef)
                && learningDelivery.FworkCodeNullable != null
                && learningDelivery.ProgTypeNullable != null
                && learningDelivery.PwayCodeNullable != null)
            {
                frameworkComponentType = larsFrameworkAims
                .Where(fwa =>
                        learningDelivery.LearnAimRef == fwa.LearnAimRef
                    && learningDelivery.FworkCodeNullable == fwa.FworkCode
                    && learningDelivery.ProgTypeNullable == fwa.ProgType
                    && learningDelivery.PwayCodeNullable == fwa.PwayCode)
                    //&& fwa.EffectiveTo == null)
                .Select(fwct => fwct.FrameworkComponentType).FirstOrDefault();
            }

            IDataEntity learningDeliveryEntity = new DataEntity(EntityLearningDelivery)
            {
                Attributes =
                    _attributeBuilder.BuildLearningDeliveryAttributes(
                        learningDelivery.AchDateNullable,
                        learningDelivery.AddHoursNullable,
                        learningDelivery.AimSeqNumber,
                        learningDelivery.AimType,
                        learningDelivery.CompStatus,
                        learningDelivery.EmpOutcomeNullable,
                        larsLearningDelivery.EnglandFEHEStatus,
                        larsLearningDelivery.EnglPrscID,
                        learningDelivery.FworkCodeNullable,
                        larsLearningDelivery.FrameworkCommonComponent,
                        frameworkComponentType,
                        learningDelivery.LearnActEndDateNullable,
                        learningDelivery.LearnPlanEndDate,
                        learningDelivery.LearnStartDate,
                        learningDeliveryFAMS.EEF,
                        learningDeliveryFAMS.LDM1,
                        learningDeliveryFAMS.LDM2,
                        learningDeliveryFAMS.LDM3,
                        learningDeliveryFAMS.LDM4,
                        learningDeliveryFAMS.FFI,
                        learningDeliveryFAMS.RES,
                        learningDelivery.OrigLearnStartDateNullable,
                        learningDelivery.OtherFundAdjNullable,
                        learningDelivery.OutcomeNullable,
                        learningDelivery.PriorLearnFundAdjNullable,
                        learningDelivery.ProgTypeNullable,
                        learningDelivery.PwayCodeNullable)
            };

            if (learningDelivery.LearningDeliveryFAMs != null)
            {
                foreach (var learningDeliveryFAM in learningDelivery.LearningDeliveryFAMs)
                {
                    IDataEntity learningDeliveryFAMEntity = LearningDeliveryFAMEntity(learningDeliveryFAM);

                    learningDeliveryEntity.AddChild(learningDeliveryFAMEntity);
                }
            }

            if (_larsReferenceDataService.LARSAnnualValuesForLearnAimRef(learningDelivery.LearnAimRef) != null)
            {
                learningDeliveryEntity.AddChildren(
                    _larsReferenceDataService.LARSAnnualValuesForLearnAimRef(learningDelivery.LearnAimRef)
                        .Select(larsAnnualValue => LARSAnnualValueEntity(larsAnnualValue)));
            }

            if (_larsReferenceDataService.LARSFundingsForLearnAimRef(learningDelivery.LearnAimRef) != null)
            {
                learningDeliveryEntity.AddChildren(
                    _larsReferenceDataService.LARSFundingsForLearnAimRef(learningDelivery.LearnAimRef)
                        .Select(larsFunding => LARSFundingEntity(larsFunding)));
            }

            if (_larsReferenceDataService.LARSLearningDeliveryCategoriesForLearnAimRef(learningDelivery.LearnAimRef) != null)
            {
                learningDeliveryEntity.AddChildren(
                    _larsReferenceDataService.LARSLearningDeliveryCategoriesForLearnAimRef(learningDelivery.LearnAimRef)
                        .Select(larsCategory => LARSLearningDeliveryCategoryEntity(larsCategory)));
            }

            if (_postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode) != null)
            {
                learningDeliveryEntity.AddChildren(
                    _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)
                        .Select(sfaAreaCost => SFAAreaCostEntity(sfaAreaCost)));
            }

            return learningDeliveryEntity;
        }

        protected internal IDataEntity LearningDeliveryFAMEntity(ILearningDeliveryFAM learningDeliveryFam)
        {
            IDataEntity learningDeliveryFAMDataEntity = new DataEntity(EntityLearningDeliveryFAM)
            {
                Attributes =
                    _attributeBuilder.BuildLearningDeliveryFAMAttributes(
                        learningDeliveryFam.LearnDelFAMCode,
                        learningDeliveryFam.LearnDelFAMDateFromNullable,
                        learningDeliveryFam.LearnDelFAMDateToNullable,
                        learningDeliveryFam.LearnDelFAMType),
            };

            return learningDeliveryFAMDataEntity;
        }

        protected internal IDataEntity LARSAnnualValueEntity(LARSAnnualValue larsAnnualValue)
        {
            var larsAnnualValueEntity = new DataEntity(EntityLearningDeliveryLARS_AnnualValue)
            {
                Attributes = _attributeBuilder.BuildLearningDeliveryLARSAnnualValueAttributes(
                    larsAnnualValue.BasicSkillsType,
                    larsAnnualValue.EffectiveFrom,
                    larsAnnualValue?.EffectiveTo)
            };

            return larsAnnualValueEntity;
        }

        protected internal IDataEntity LARSFundingEntity(LARSFunding larsFunding)
        {
            var larsFundingDataEntity = new DataEntity(EntityLearningDeliveryLARS_Funding)
            {
                Attributes = _attributeBuilder.BuildLearningDeliveryLarsFundingAttributes(
                    larsFunding.FundingCategory,
                    larsFunding.EffectiveFrom,
                    larsFunding?.EffectiveTo,
                    larsFunding.RateUnWeighted,
                    larsFunding.RateWeighted,
                    larsFunding.WeightingFactor),
            };

            return larsFundingDataEntity;
        }

        protected internal IDataEntity LARSLearningDeliveryCategoryEntity(LARSLearningDeliveryCategory larsLearningDeliveryCategory)
        {
            var larsLearningDeliveryCategoryEntity = new DataEntity(EntityLearningDeliveryLARS_Category)
            {
                Attributes = _attributeBuilder.BuildLearningDeliveryLARSCategoryAttributes(
                    larsLearningDeliveryCategory.CategoryRef,
                    larsLearningDeliveryCategory.EffectiveFrom,
                    larsLearningDeliveryCategory?.EffectiveTo)
            };

            return larsLearningDeliveryCategoryEntity;
        }

        protected internal IDataEntity SFAAreaCostEntity(SfaAreaCost sfaAreaCost)
        {
            var sfaAreaCostDataEntity = new DataEntity(EntityLearningDeliverySFA_PostcodeAreaCost)
            {
                Attributes =
                _attributeBuilder.BuildLearningDeliverySfaAreaCostAttributes(
                        sfaAreaCost?.EffectiveFrom,
                        sfaAreaCost?.EffectiveTo,
                        sfaAreaCost.AreaCostFactor),
            };

            return sfaAreaCostDataEntity;
        }

        protected internal LearningDeliveryFAMPivot PivotLearningDeliveryFAMS(ILearningDelivery learningDelivery)
        {
            if (learningDelivery.LearningDeliveryFAMs != null)
            {
                var ldmArray = learningDelivery.LearningDeliveryFAMs?
                    .Where(w => w.LearnDelFAMType.Contains("LDM"))
                    .Select(ldf => ToNullableInt(ldf.LearnDelFAMCode)).ToArray();

                Array.Resize(ref ldmArray, 4);

                return new LearningDeliveryFAMPivot
                {
                    EEF = ToNullableInt(learningDelivery.LearningDeliveryFAMs.Where(w => w.LearnDelFAMType == LearningDeliveryFAMTypeEEF).Select(ldf => ldf.LearnDelFAMCode).SingleOrDefault()),
                    FFI = ToNullableInt(learningDelivery.LearningDeliveryFAMs.Where(w => w.LearnDelFAMType == LearningDeliveryFAMTypeFFI).Select(ldf => ldf.LearnDelFAMCode).SingleOrDefault()),
                    RES = ToNullableInt(learningDelivery.LearningDeliveryFAMs.Where(w => w.LearnDelFAMType == LearningDeliveryFAMTypeRES).Select(ldf => ldf.LearnDelFAMCode).SingleOrDefault()),
                    LDM1 = ldmArray[0],
                    LDM2 = ldmArray[1],
                    LDM3 = ldmArray[2],
                    LDM4 = ldmArray[3],
                };
            }

            return new LearningDeliveryFAMPivot
            {
                EEF = null,
                FFI = null,
                RES = null,
                LDM1 = null,
                LDM2 = null,
                LDM3 = null,
                LDM4 = null,
            };
        }

        private static int? ToNullableInt(string stringValue)
        {
            if (int.TryParse(stringValue, out int i))
            {
                return i;
            }

            return null;
        }
    }
}
