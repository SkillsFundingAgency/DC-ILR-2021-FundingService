using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.ALB.Service.Builders.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Builders
{
    public class DataEntityMapper : IDataEntityMapper<ILearner, IFundingOutputs>
    {
        private const string EntityGlobal = "global";
        private const string EntityLearner = "Learner";
        private const string EntityLearningDelivery = "LearningDelivery";
        private const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        private const string EntityLearningDeliverySFA_PostcodeAreaCost = "SFA_PostcodeAreaCost";
        private const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";
        private const string LearningDeliveryFAMTypeADL = "ADL";
        private const string LearningDeliveryFAMTypeRES = "RES";

        private readonly IExternalDataCache _referenceDataCache;
        private readonly IFileDataCache _fileDataCache;
        private readonly IALBAttributeBuilder _attributeBuilder;

        public DataEntityMapper(IExternalDataCache referenceDataCache, IFileDataCache fileDataCache, IALBAttributeBuilder attributeBuilder)
        {
            _referenceDataCache = referenceDataCache;
            _fileDataCache = fileDataCache;
            _attributeBuilder = attributeBuilder;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> learners)
        {
            var globalEntities = learners.Select(learner =>
            {
                // Global Entity
                IDataEntity globalEntity = GlobalEntity(_fileDataCache.UKPRN);

                // Learner Entity
                IDataEntity learnerEntity = LearnerEntity(learner.LearnRefNumber);

                // LearningDelivery Entities
                foreach (var learningDelivery in learner.LearningDeliveries)
                {
                    _referenceDataCache.LARSLearningDelivery.TryGetValue(learningDelivery.LearnAimRef, out LARSLearningDelivery larsLearningDelivery);
                    IDataEntity learningDeliveryEntity = LearningDeliveryEntity(learningDelivery, larsLearningDelivery);

                    learnerEntity.AddChild(learningDeliveryEntity);

                    // LearningDeliveryFAM Entities
                    if (learningDelivery.LearningDeliveryFAMs != null)
                    {
                        foreach (var learningDeliveryFAM in learningDelivery.LearningDeliveryFAMs)
                        {
                            IDataEntity learningDeliveryFAMEntity = LearningDeliveryFAMEntity(learningDeliveryFAM);

                            learningDeliveryEntity.AddChild(learningDeliveryFAMEntity);
                        }
                    }

                    // SFA Postcode Area Cost Entities
                    if (_referenceDataCache.SfaAreaCost.ContainsKey(learningDelivery.DelLocPostCode))
                    {
                        learningDeliveryEntity.AddChildren(
                            _referenceDataCache.SfaAreaCost[learningDelivery.DelLocPostCode]
                                .Select(sfaAreaCost => SFAAreaCostEntity(sfaAreaCost)));
                    }

                    // LARS Funding Entities
                    if (_referenceDataCache.LARSFunding.ContainsKey(learningDelivery.LearnAimRef))
                    {
                        learningDeliveryEntity.AddChildren(
                            _referenceDataCache.LARSFunding[learningDelivery.LearnAimRef]
                                .Select(larsFunding => LARSFundingEntity(larsFunding)));
                    }
                }

                globalEntity.AddChild(learnerEntity);

                return globalEntity;
            }).AsParallel();

            return globalEntities;
        }

        public IFundingOutputs MapFrom(IEnumerable<IDataEntity> inputModels)
        {
            throw new System.NotImplementedException();
        }

        #region Entity Builders

        protected internal IDataEntity GlobalEntity(int ukprn)
        {
            IDataEntity globalDataEntity = new DataEntity(EntityGlobal)
            {
                Attributes =
                    _attributeBuilder.BuildGlobalAttributes(ukprn, _referenceDataCache.LARSCurrentVersion, _referenceDataCache.PostcodeCurrentVersion),
            };

            return globalDataEntity;
        }

        protected internal IDataEntity LearnerEntity(string learnRefNumber)
        {
            IDataEntity learnerDataEntity = new DataEntity(EntityLearner)
            {
                Attributes =
                    _attributeBuilder.BuildLearnerAttributes(learnRefNumber),
            };

            return learnerDataEntity;
        }

        protected internal IDataEntity LearningDeliveryEntity(ILearningDelivery learningDelivery, LARSLearningDelivery larsLearningDelivery)
        {
            IDataEntity learningDeliveryDataEntity = new DataEntity(EntityLearningDelivery)
            {
                Attributes =
                    _attributeBuilder.BuildLearningDeliveryAttributes(
                        learningDelivery.AimSeqNumber,
                        learningDelivery.CompStatus,
                        learningDelivery.LearnActEndDateNullable,
                        larsLearningDelivery.LearnAimRefType,
                        learningDelivery.LearnPlanEndDate,
                        learningDelivery.LearnStartDate,
                        GetLDFAM(learningDelivery, LearningDeliveryFAMTypeADL),
                        GetLDFAM(learningDelivery, LearningDeliveryFAMTypeRES),
                        larsLearningDelivery.NotionalNVQLevelv2,
                        learningDelivery.OrigLearnStartDateNullable,
                        learningDelivery.OtherFundAdjNullable,
                        learningDelivery.OutcomeNullable,
                        learningDelivery.PriorLearnFundAdjNullable,
                        larsLearningDelivery?.RegulatedCreditValue),
            };

            return learningDeliveryDataEntity;
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

        protected internal IDataEntity LARSFundingEntity(LARSFunding larsFunding)
        {
            var larsFundingDataEntity = new DataEntity(EntityLearningDeliveryLARS_Funding)
            {
                Attributes = _attributeBuilder.BuildLearningDeliveryLarsFundingAttributes(
                    larsFunding.FundingCategory,
                    larsFunding.EffectiveFrom,
                    larsFunding?.EffectiveTo,
                    larsFunding.RateWeighted,
                    larsFunding.WeightingFactor),
            };

            return larsFundingDataEntity;
        }

        #endregion

        private static string GetLDFAM(ILearningDelivery learningDelivery, string famType)
        {
            string famCodeValue = learningDelivery.LearningDeliveryFAMs?.Where(w => w.LearnDelFAMType.Contains(famType))
                    .Select(ldf => ldf.LearnDelFAMCode).SingleOrDefault();

            return famCodeValue;
        }
    }
}