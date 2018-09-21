using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.ALB.Service.Constants;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service
{
    public class FundingOutputService : IOutputService<ALBFundingOutputs>
    {
        private readonly IInternalDataCache _internalDataCache;
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IInternalDataCache internalDataCache, IDataEntityAttributeService dataEntityAttributeService)
        {
            _internalDataCache = internalDataCache;
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public ALBFundingOutputs ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            if (dataEntities != null)
            {
                dataEntities = dataEntities.ToList();
                if (dataEntities.Any())
                {
                    return new ALBFundingOutputs
                    {
                        Global = GlobalOutput(dataEntities.First()),
                        Learners = dataEntities
                        .Where(g => g.Children != null)
                        .SelectMany(g => g.Children.Where(e => e.EntityName == Attributes.EntityLearner)
                        .Select(LearnerFromDataEntity))
                        .ToArray(),
                    };
                }
            }

            return new ALBFundingOutputs();
        }

        public GlobalAttribute GlobalOutput(IDataEntity dataEntity)
        {
            return new GlobalAttribute
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN).Value,
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LARSVersion),
                PostcodeAreaCostVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PostcodeAreaCostVersion),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
            };
        }

        public LearnerAttribute LearnerFromDataEntity(IDataEntity dataEntity)
        {
            return new LearnerAttribute()
            {
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnRefNumber),
                LearnerPeriodisedAttributes = LearnerPeriodisedAttributes(dataEntity).ToArray(),
                LearningDeliveryAttributes = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityLearningDelivery)
                        .Select(LearningDeliveryFromDataEntity).ToArray()
            };
        }

        public LearnerPeriodisedAttribute[] LearnerPeriodisedAttributes(IDataEntity learner)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.ALBSeqNum,
            };

            List<LearnerPeriodisedAttribute> learnerPeriodisedAttributesList = new List<LearnerPeriodisedAttribute>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learner.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = decimal.Parse(attributeValue.Value.ToString());

                    learnerPeriodisedAttributesList.Add(new LearnerPeriodisedAttribute
                    {
                        AttributeName = attribute,
                        Period1 = value,
                        Period2 = value,
                        Period3 = value,
                        Period4 = value,
                        Period5 = value,
                        Period6 = value,
                        Period7 = value,
                        Period8 = value,
                        Period9 = value,
                        Period10 = value,
                        Period11 = value,
                        Period12 = value,
                    });
                }

                if (changePoints.Any())
                {
                    learnerPeriodisedAttributesList.Add(new LearnerPeriodisedAttribute
                    {
                        AttributeName = attribute,
                        Period1 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period1),
                        Period2 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period2),
                        Period3 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period3),
                        Period4 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period4),
                        Period5 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period5),
                        Period6 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period6),
                        Period7 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period7),
                        Period8 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period8),
                        Period9 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period9),
                        Period10 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period10),
                        Period11 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period11),
                        Period12 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period12),
                    });
                }
            }

            return learnerPeriodisedAttributesList.ToArray();
        }

        public LearningDeliveryAttribute LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDeliveryAttribute
            {
                AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AimSeqNumber).Value,
                LearningDeliveryAttributeDatas = LearningDeliveryAttributeData(dataEntity),
                LearningDeliveryPeriodisedAttributes = LearningDeliveryPeriodisedAttributeData(dataEntity).ToArray(),
            };
        }

        public LearningDeliveryAttributeData LearningDeliveryAttributeData(IDataEntity dataEntity)
        {
            return new LearningDeliveryAttributeData
            {
                Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Achieved),
                ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualNumInstalm),
                AdvLoan = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AdvLoan),
                ApplicFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ApplicFactDate),
                ApplicProgWeightFact = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.ApplicProgWeightFact),
                AreaCostFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AreaCostFactAdj),
                AreaCostInstalment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AreaCostInstalment),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.FundLine),
                FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.FundStart),
                LearnDelApplicLARSCarPilFundSubRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelApplicLARSCarPilFundSubRate),
                LearnDelApplicSubsidyPilotAreaCode = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnDelApplicSubsidyPilotAreaCode),
                LearnDelCarLearnPilotAimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelCarLearnPilotAimValue),
                LearnDelCarLearnPilotInstalAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelCarLearnPilotInstalAmount),
                LearnDelEligCareerLearnPilot = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnDelEligCareerLearnPilot),
                LiabilityDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LiabilityDate),
                LoanBursAreaUplift = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LoanBursAreaUplift),
                LoanBursSupp = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LoanBursSupp),
                OutstndNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.OutstndNumOnProgInstalm),
                PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedNumOnProgInstalm),
                WeightedRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.WeightedRate),
            };
        }

        public LearningDeliveryPeriodisedAttribute[] LearningDeliveryPeriodisedAttributeData(IDataEntity learningDelivery)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.ALBCode,
                Attributes.ALBSupportPayment,
                Attributes.AreaUpliftBalPayment,
                Attributes.AreaUpliftOnProgPayment,
                Attributes.LearnDelCarLearnPilotOnProgPayment,
                Attributes.LearnDelCarLearnPilotBalPayment
            };

            List<LearningDeliveryPeriodisedAttribute> learningDeliveryPeriodisedAttributeList = new List<LearningDeliveryPeriodisedAttribute>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learningDelivery.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = decimal.Parse(attributeValue.Value.ToString());

                    learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedAttribute
                    {
                        AttributeName = attribute,
                        Period1 = value,
                        Period2 = value,
                        Period3 = value,
                        Period4 = value,
                        Period5 = value,
                        Period6 = value,
                        Period7 = value,
                        Period8 = value,
                        Period9 = value,
                        Period10 = value,
                        Period11 = value,
                        Period12 = value,
                    });
                }

                if (changePoints.Any())
                {
                    learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedAttribute
                    {
                        AttributeName = attribute,
                        Period1 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period1),
                        Period2 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period2),
                        Period3 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period3),
                        Period4 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period4),
                        Period5 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period5),
                        Period6 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period6),
                        Period7 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period7),
                        Period8 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period8),
                        Period9 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period9),
                        Period10 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period10),
                        Period11 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period11),
                        Period12 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period12),
                    });
                }
            }

            return learningDeliveryPeriodisedAttributeList.ToArray();
        }

        private decimal GetPeriodAttributeValue(IAttributeData attributes, DateTime periodDate)
        {
            return decimal.Parse(attributes.Changepoints.Where(cp => cp.ChangePoint == periodDate).Select(v => v.Value).SingleOrDefault().ToString());
        }
    }
}
