using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM70.Service.Constants;
using ESFA.DC.ILR.FundingService.FM70.Service.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<FM70LearnerDto>
    {
        private readonly int _fundModel = Attributes.FundModel_70;

        private readonly IFCSReferenceDataService _fcsDataReferenceDataService;
        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;

        public DataEntityMapper(IFCSReferenceDataService fcsDataReferenceDataService, ILARSReferenceDataService larsReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService)
        {
            _fcsDataReferenceDataService = fcsDataReferenceDataService;
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
        }

        public IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<FM70LearnerDto> inputModels)
        {
            var global = BuildGlobal(ukprn);

            var entities = inputModels?
                .Where(l => l.LearningDeliveries
                .Any(ld => _fundModel == ld.FundModel))
                .Select(l => BuildGlobalDataEntity(l, global)) ?? new List<IDataEntity>();

            return entities.Any() ? entities : new List<IDataEntity> { BuildDefaultGlobalDataEntity(global) };
        }

        public IDataEntity BuildGlobalDataEntity(FM70LearnerDto learner, Global global)
        {
           return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children =
                    learner != null ? new List<IDataEntity> { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildDefaultGlobalDataEntity(Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM70LearnerDto learner)
        {
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
                            learner.DPOutcomes?
                            .Select(BuildDPOutcomes) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(LearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var sfaAreaCost = _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode);

            var fcsContractData = _fcsDataReferenceDataService.FcsContractsForConRef(learningDelivery.ConRefNumber);
            var esfData = BuildEsfDataFromContract(fcsContractData);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AchDate, new AttributeData(learningDelivery.AchDate) },
                    { Attributes.AddHours, new AttributeData(learningDelivery.AddHours) },
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.CalcMethod, new AttributeData(esfData.Select(c => c.CalcMethod).FirstOrDefault()) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.ConRefNumber, new AttributeData(learningDelivery.ConRefNumber) },
                    { Attributes.GenreCode, new AttributeData(larsLearningDelivery.LearningDeliveryGenre) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDate) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDelivery.LrnDelFAM_LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDelivery.LrnDelFAM_LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDelivery.LrnDelFAM_LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDelivery.LrnDelFAM_LDM4) },
                    { Attributes.LrnDelFAM_RES, new AttributeData(learningDelivery.LrnDelFAM_RES) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDate) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdj) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.Outcome) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdj) },
                },
                Children = (
                            larsLearningDelivery?
                            .LARSAnnualValues?
                                   .Select(BuildLARSAnnualValue) ?? new List<IDataEntity>())
                            .Union(
                                   sfaAreaCost?
                                   .Select(BuildSFAAreaCost) ?? new List<IDataEntity>())
                            .Union(
                                    larsLearningDelivery?
                                    .LARSFundings?
                                   .Select(BuildLARSFunding) ?? new List<IDataEntity>())
                             .Union(
                                    esfData?
                                    .Select(BuildEsfDataEntity) ?? new List<IDataEntity>())
                            .ToList()
            };
        }

        public IDataEntity BuildLearnerEmploymentStatus(LearnerEmploymentStatus learnerEmploymentStatus)
        {
            return new DataEntity(Attributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpStat, new AttributeData(learnerEmploymentStatus.EmpStat) }
                },
            };
        }

        public IDataEntity BuildDPOutcomes(DPOutcome dpOutcome)
        {
            return new DataEntity(Attributes.EntityDPOutcome)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.OutCode,  new AttributeData(dpOutcome.OutCode) },
                    { Attributes.OutType, new AttributeData(dpOutcome.OutType) },
                    { Attributes.OutCollDate, new AttributeData(dpOutcome.OutCollDate) },
                    { Attributes.OutStartDate,  new AttributeData(dpOutcome.OutStartDate) },
                    { Attributes.OutEndDate,  new AttributeData(dpOutcome.OutEndDate) },
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
            return new DataEntity(Attributes.EntityLearningDeliveryLARSFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSFundingCategory, new AttributeData(larsFunding.FundingCategory) },
                    { Attributes.LARSFundingEffectiveFrom, new AttributeData(larsFunding.EffectiveFrom) },
                    { Attributes.LARSFundingEffectiveTo, new AttributeData(larsFunding.EffectiveTo) },
                    { Attributes.LARSFundingWeightedRate, new AttributeData(larsFunding.RateWeighted) },
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

        public IDataEntity BuildEsfDataEntity(EsfData esfData)
        {
            return new DataEntity(Attributes.EntityESFData)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.UnitCost, new AttributeData(esfData.UnitCost) },
                    { Attributes.ESFDeliverableCode, new AttributeData(esfData.ESFDeliverableCode) },
                    { Attributes.ESFDataPremiumFactor, new AttributeData(esfData.ESFDataPremiumFactor) },
                    { Attributes.EffectiveContractStartDate, new AttributeData(esfData.EffectiveContractStartDate) },
                    { Attributes.EffectiveContractEndDate, new AttributeData(esfData.EffectiveContractEndDate) }
                }
            };
        }

        public Global BuildGlobal(int ukprn)
        {
            return new Global()
            {
                UKPRN = ukprn
            };
        }

        public IEnumerable<EsfData> BuildEsfDataFromContract(IEnumerable<FCSContractAllocation> fcsContractAllocations)
        {
            return fcsContractAllocations.SelectMany(e => e.FCSContractDeliverables.Select(cd => new EsfData
            {
                ESFDataPremiumFactor = e.LearningRatePremiumFactor,
                EffectiveContractStartDate = e.ContractStartDate,
                EffectiveContractEndDate = e.ContractEndDate,
                UnitCost = cd.UnitCost,
                CalcMethod = e.CalcMethod,
                ESFDeliverableCode = cd.ExternalDeliverableCode,
            }));
        }
    }
}