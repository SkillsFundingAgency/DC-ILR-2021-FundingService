﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.Service.Constants;
using ESFA.DC.ILR.FundingService.ALB.Service.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ALBLearnerDto>
    {
        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
        }

        public IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<ALBLearnerDto> inputModels)
        {
            var global = BuildGlobal(ukprn);

            var entities = inputModels?
                .Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == Attributes.FundModel_99))
                .Select(l => BuildGlobalDataEntity(l, global)) ?? Enumerable.Empty<IDataEntity>();

            return entities.Any() ? entities : new List<IDataEntity> { BuildDefaultGlobalDataEntity(global) };
        }

        public IDataEntity BuildGlobalDataEntity(ALBLearnerDto learner, Global global)
        {
            var entity = new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = BuildGlobalAttributes(global)
            };

            if (learner != null)
            {
                entity.AddChild(BuildLearnerDataEntity(learner));
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

        public IDataEntity BuildLearnerDataEntity(ALBLearnerDto learner)
        {
            var learningDeliveryEntities = learner.LearningDeliveries?
                .Where(ld => ld.FundModel == Attributes.FundModel_99).Select(BuildLearningDeliveryDataEntity) ?? Enumerable.Empty<IDataEntity>();

            var entity = new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                }
            };

            entity.AddChildren(learningDeliveryEntities);

            return entity;
        }

        public IDataEntity BuildLearningDeliveryDataEntity(LearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var sfaPostCodeAreaCost = _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode);

            var learningDeliveryFamsEntities = learningDelivery?.LearningDeliveryFAMs?.Select(BuildLearningDeliveryFAM) ?? Enumerable.Empty<IDataEntity>();
            var larsFundingEntities = larsLearningDelivery?.LARSFundings?.Select(BuildLARSFunding) ?? Enumerable.Empty<IDataEntity>();
            var sfaAreaCostEntities = sfaPostCodeAreaCost?.Select(BuildSFAPostcodeAreaCost) ?? Enumerable.Empty<IDataEntity>();

            var entity = new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDate) },
                    { Attributes.LearnAimRefType, new AttributeData(larsLearningDelivery.LearnAimRefType) },
                    { Attributes.LearnDelFundModel, new AttributeData(learningDelivery.FundModel) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.NotionalNVQLevelv2, new AttributeData(larsLearningDelivery.NotionalNVQLevelv2) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDate) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdj) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.Outcome) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdj) },
                    { Attributes.RegulatedCreditValue, new AttributeData(larsLearningDelivery.RegulatedCreditValue) },
                }
            };

            entity.AddChildren(learningDeliveryFamsEntities);
            entity.AddChildren(larsFundingEntities);
            entity.AddChildren(sfaAreaCostEntities);

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
                    { Attributes.LARSFundWeightingFactor, new AttributeData(larsFunding.WeightingFactor) },
                }
            };
        }

        public IDataEntity BuildSFAPostcodeAreaCost(SfaAreaCost sfaAreaCost)
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
                PostcodesVersion = _postcodesReferenceDataService.PostcodesCurrentVersion(),
                UKPRN = ukprn
            };
        }

        private IDictionary<string, IAttributeData> BuildGlobalAttributes(Global global)
        {
            return new Dictionary<string, IAttributeData>
            {
                { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                { Attributes.PostcodeAreaCostVersion, new AttributeData(global.PostcodesVersion) },
                { Attributes.UKPRN, new AttributeData(global.UKPRN) }
            };
        }
    }
}