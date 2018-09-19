using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.Service.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private const string EntityGlobal = "global";
        private const string EntityLearner = "Learner";
        private const string EntityLearningDelivery = "LearningDelivery";
        private const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        private const string EntityLearningDeliverySFA_PostcodeAreaCost = "SFA_PostcodeAreaCost";
        private const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";
        private const string EntityLearningDeliverySubsidyPilotPostcodeArea = "SubsidyPilotPostcodeArea";
        private const string EntityLearningDeliveryLARS_CareerLearningPilot = "LARS_CareerLearningPilot";

        private const string GlobalLARSVersion = "LARSVersion";
        private const string GlobalPostcodeAreaCostVersion = "PostcodeAreaCostVersion";
        private const string GlobalUKPRN = "UKPRN";

        private const string LearnerLearnRefNumber = "LearnRefNumber";

        private const string LearningDeliveryAimSeqNumber = "AimSeqNumber";
        private const string LearningDeliveryCompStatus = "CompStatus";
        private const string LearningDeliveryLearnActEndDate = "LearnActEndDate";
        private const string LearningDeliveryLearnAimRefType = "LearnAimRefType";
        private const string LearningDeliveryLearnDelFundModel = "LearnDelFundModel";
        private const string LearningDeliveryLearnPlanEndDate = "LearnPlanEndDate";
        private const string LearningDeliveryLearnStartDate = "LearnStartDate";
        private const string LearningDeliveryLrnDelFAM_ADL = "LrnDelFAM_ADL";
        private const string LearningDeliveryLrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        private const string LearningDeliveryLrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        private const string LearningDeliveryLrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        private const string LearningDeliveryLrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        private const string LearningDeliveryLrnDelFAM_RES = "LrnDelFAM_RES";
        private const string LearningDeliveryNotionalNVQLevelv2 = "NotionalNVQLevelv2";
        private const string LearningDeliveryOrigLearnStartDate = "OrigLearnStartDate";
        private const string LearningDeliveryOtherFundAdj = "OtherFundAdj";
        private const string LearningDeliveryOutcome = "Outcome";
        private const string LearningDeliveryPriorLearnFundAdj = "PriorLearnFundAdj";
        private const string LearningDeliveryRegulatedCreditValue = "RegulatedCreditValue";

        private const string LearnDelFAMCode = "LearnDelFAMCode";
        private const string LearnDelFAMDateFrom = "LearnDelFAMDateFrom";
        private const string LearnDelFAMDateTo = "LearnDelFAMDateTo";
        private const string LearnDelFAMType = "LearnDelFAMType";

        private const string AreaCosFactor = "AreaCosFactor";
        private const string AreaCosEffectiveFrom = "AreaCosEffectiveFrom";
        private const string AreaCosEffectiveTo = "AreaCosEffectiveTo";

        private const string SubsidyPilotAreaCode = "SubsidyPilotAreaCode";
        private const string SubsidyPilotEffectiveFrom = "SubsidyPilotEffectiveFrom";
        private const string SubsidyPilotEffectiveTo = "SubsidyPilotEffectiveTo";

        private const string LARSFundCategory = "LARSFundCategory";
        private const string LARSFundEffectiveFrom = "LARSFundEffectiveFrom";
        private const string LARSFundEffectiveTo = "LARSFundEffectiveTo";
        private const string LARSFundWeightedRate = "LARSFundWeightedRate";
        private const string LARSFundWeightingFactor = "LARSFundWeightingFactor";

        private const string LearnDelLARSCarPilFundAreaCode = "LearnDelLARSCarPilFundAreaCode";
        private const string LearnDelLARSCarPilFundEffToDate = "LearnDelLARSCarPilFundEffToDate";
        private const string LearnDelLARSCarPilFundEffFromDate = "LearnDelLARSCarPilFundEffFromDate";
        private const string LearnDelLARSCarPilFundSubsidyRate = "LearnDelLARSCarPilFundSubsidyRate";

        private const string ADL = "ADL";
        private const string LDM = "LDM";
        private const string RES = "RES";

        private readonly HashSet<int> _fundModels = new HashSet<int> { 81, 99 };

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService, IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
            _fileDataService = fileDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            var global = BuildGlobal();

            return inputModels.Where(l => l.LearningDeliveries.Any(ld => _fundModels.Contains(ld.FundModel))).Select(l => BuildGlobalDataEntity(l, global));
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner, Global global)
        {
            return new DataEntity(EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { GlobalLARSVersion, new AttributeData(global.LARSVersion) },
                    { GlobalPostcodeAreaCostVersion, new AttributeData(global.PostcodesVersion) },
                    { GlobalUKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            return new DataEntity(EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearnerLearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var sfaPostCodeAreaCost = _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode);
            var subsidyPilotPostcodeArea = _postcodesReferenceDataService.CareerLearningPilotsForPostcode(learningDelivery.DelLocPostCode);
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);

            return new DataEntity(EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearningDeliveryAimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { LearningDeliveryCompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { LearningDeliveryLearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { LearningDeliveryLearnAimRefType, new AttributeData(larsLearningDelivery.LearnAimRefType) },
                    { LearningDeliveryLearnDelFundModel, new AttributeData(learningDelivery.FundModel) },
                    { LearningDeliveryLearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { LearningDeliveryLearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { LearningDeliveryLrnDelFAM_ADL, new AttributeData(learningDeliveryFAMDenormalized.ADL) },
                    { LearningDeliveryLrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { LearningDeliveryLrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { LearningDeliveryLrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { LearningDeliveryLrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { LearningDeliveryLrnDelFAM_RES, new AttributeData(learningDeliveryFAMDenormalized.RES) },
                    { LearningDeliveryNotionalNVQLevelv2, new AttributeData(larsLearningDelivery.NotionalNVQLevelv2) },
                    { LearningDeliveryOrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDateNullable) },
                    { LearningDeliveryOtherFundAdj, new AttributeData(learningDelivery.OtherFundAdjNullable) },
                    { LearningDeliveryOutcome, new AttributeData(learningDelivery.OutcomeNullable) },
                    { LearningDeliveryPriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdjNullable) },
                    { LearningDeliveryRegulatedCreditValue, new AttributeData(larsLearningDelivery.RegulatedCreditValue) },
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                   larsLearningDelivery?
                                    .LARSCareerLearningPilot?
                                    .Select(BuildLARSCareerLearningPilot))
                            .Union(
                                   larsLearningDelivery?
                                    .LARSFunding?
                                    .Select(BuildLARSFunding))
                             .Union(
                                   _postcodesReferenceDataService
                                    .SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)
                                    .Select(BuildSFAPostcodeAreaCost))
                             .Union(
                                   _postcodesReferenceDataService
                                    .CareerLearningPilotsForPostcode(learningDelivery.DelLocPostCode)
                                    .Select(BuildSubsidyPilotPostcodeArea))
                            .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryFAM(ILearningDeliveryFAM learningDeliveryFAM)
        {
            return new DataEntity(EntityLearningDeliveryFAM)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearnDelFAMCode, new AttributeData(learningDeliveryFAM.LearnDelFAMCode) },
                    { LearnDelFAMDateTo, new AttributeData(learningDeliveryFAM.LearnDelFAMDateToNullable) },
                    { LearnDelFAMDateFrom, new AttributeData(learningDeliveryFAM.LearnDelFAMDateFromNullable) },
                    { LearnDelFAMType, new AttributeData(learningDeliveryFAM.LearnDelFAMType) },
                }
            };
        }

        public IDataEntity BuildLARSCareerLearningPilot(LARSCareerLearningPilot larsCareerLearningPilot)
        {
            return new DataEntity(EntityLearningDeliveryLARS_CareerLearningPilot)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearnDelLARSCarPilFundAreaCode, new AttributeData(larsCareerLearningPilot.AreaCode) },
                    { LearnDelLARSCarPilFundEffFromDate, new AttributeData(larsCareerLearningPilot.EffectiveFrom) },
                    { LearnDelLARSCarPilFundEffToDate, new AttributeData(larsCareerLearningPilot.EffectiveTo) },
                    { LearnDelLARSCarPilFundSubsidyRate, new AttributeData(larsCareerLearningPilot.SubsidyRate) },
                }
            };
        }

        public IDataEntity BuildLARSFunding(LARSFunding larsFunding)
        {
            return new DataEntity(EntityLearningDeliveryLARS_Funding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LARSFundCategory, new AttributeData(larsFunding.FundingCategory) },
                    { LARSFundEffectiveFrom, new AttributeData(larsFunding.EffectiveFrom) },
                    { LARSFundEffectiveTo, new AttributeData(larsFunding.EffectiveTo) },
                    { LARSFundWeightedRate, new AttributeData(larsFunding.RateWeighted) },
                    { LARSFundWeightingFactor, new AttributeData(larsFunding.WeightingFactor) },
                }
            };
        }

        public IDataEntity BuildSFAPostcodeAreaCost(SfaAreaCost sfaAreaCost)
        {
            return new DataEntity(EntityLearningDeliverySFA_PostcodeAreaCost)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { AreaCosFactor, new AttributeData(sfaAreaCost.AreaCostFactor) },
                    { AreaCosEffectiveFrom, new AttributeData(sfaAreaCost.EffectiveFrom) },
                    { AreaCosEffectiveTo, new AttributeData(sfaAreaCost.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildSubsidyPilotPostcodeArea(CareerLearningPilot larsCareerLearningPilot)
        {
            return new DataEntity(EntityLearningDeliverySubsidyPilotPostcodeArea)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { SubsidyPilotAreaCode, new AttributeData(larsCareerLearningPilot.AreaCode) },
                    { SubsidyPilotEffectiveFrom, new AttributeData(larsCareerLearningPilot.EffectiveFrom) },
                    { SubsidyPilotEffectiveTo, new AttributeData(larsCareerLearningPilot.EffectiveTo) },
                }
            };
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                PostcodesVersion = _postcodesReferenceDataService.PostcodesCurrentVersion(),
                UKPRN = _fileDataService.UKPRN()
            };
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new Service.Model.LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == LDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.ADL = learningDeliveryFams.Where(f => f.LearnDelFAMType == ADL).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.RES = learningDeliveryFams.Where(f => f.LearnDelFAMType == RES).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}