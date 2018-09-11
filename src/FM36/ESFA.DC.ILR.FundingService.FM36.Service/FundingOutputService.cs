using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM36.Service
{
    public class FundingOutputService : IOutputService<FM36FundingOutputs>
    {
        private readonly IInternalDataCache _internalDataCache;
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IInternalDataCache internalDataCache, IDataEntityAttributeService dataEntityAttributeService)
        {
            _internalDataCache = internalDataCache;
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public FM36FundingOutputs ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            if (dataEntities != null)
            {
                dataEntities = dataEntities.ToList();
                if (dataEntities.Any())
                {
                    return new FM36FundingOutputs
                    {
                        Global = GlobalOutput(dataEntities.First()),
                        Learners = dataEntities
                        .Where(g => g.Children != null)
                        .SelectMany(g => g.Children.Where(e => e.EntityName == "Learner")
                        .Select(LearnerFromDataEntity))
                        .ToArray(),
                    };
                }
            }

            return new FM36FundingOutputs();
        }

        public GlobalAttribute GlobalOutput(IDataEntity dataEntity)
        {
            return new GlobalAttribute
            {
                //UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "UKPRN").Value,
                //LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "LARSVersion"),
                //PostcodeAreaCostVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "PostcodeAreaCostVersion"),
                //RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "RulebaseVersion"),
            };
        }

        public LearnerAttribute LearnerFromDataEntity(IDataEntity dataEntity)
        {
            return new LearnerAttribute()
            {
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "LearnRefNumber"),
                //LearnerPeriodisedAttributes = LearnerPeriodisedAttributes(dataEntity).ToArray(),
                //LearningDeliveryAttributes = dataEntity
                //        .Children
                //        .Where(e => e.EntityName == "LearningDelivery")
                //        .Select(LearningDeliveryFromDataEntity).ToArray()
            };
        }

        //public LearnerPeriodisedAttribute[] LearnerPeriodisedAttributes(IDataEntity learner)
        //{
        //    List<string> attributeList = new List<string>()
        //    {
        //        "ALBSeqNum",
        //    };

        //    List<LearnerPeriodisedAttribute> learnerPeriodisedAttributesList = new List<LearnerPeriodisedAttribute>();

        //    foreach (var attribute in attributeList)
        //    {
        //        var attributeValue = learner.Attributes[attribute];

        //        var changePoints = attributeValue.Changepoints;

        //        if (!changePoints.Any())
        //        {
        //            var value = decimal.Parse(attributeValue.Value.ToString());

        //            learnerPeriodisedAttributesList.Add(new LearnerPeriodisedAttribute
        //            {
        //                AttributeName = attribute,
        //                Period1 = value,
        //                Period2 = value,
        //                Period3 = value,
        //                Period4 = value,
        //                Period5 = value,
        //                Period6 = value,
        //                Period7 = value,
        //                Period8 = value,
        //                Period9 = value,
        //                Period10 = value,
        //                Period11 = value,
        //                Period12 = value,
        //            });
        //        }

        //        if (changePoints.Any())
        //        {
        //            learnerPeriodisedAttributesList.Add(new LearnerPeriodisedAttribute
        //            {
        //                AttributeName = attribute,
        //                Period1 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period1),
        //                Period2 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period2),
        //                Period3 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period3),
        //                Period4 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period4),
        //                Period5 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period5),
        //                Period6 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period6),
        //                Period7 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period7),
        //                Period8 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period8),
        //                Period9 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period9),
        //                Period10 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period10),
        //                Period11 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period11),
        //                Period12 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period12),
        //            });
        //        }
        //    }

        //    return learnerPeriodisedAttributesList.ToArray();
        //}

        //public LearningDeliveryAttribute LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        //{
        //    return new LearningDeliveryAttribute
        //    {
        //        AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "AimSeqNumber").Value,
        //        LearningDeliveryAttributeDatas = LearningDeliveryAttributeData(dataEntity),
        //        LearningDeliveryPeriodisedAttributes = LearningDeliveryPeriodisedAttributeData(dataEntity).ToArray(),
        //    };
        //}

        //public LearningDeliveryAttributeData LearningDeliveryAttributeData(IDataEntity dataEntity)
        //{
        //    return new LearningDeliveryAttributeData
        //    {
        //        Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "Achieved"),
        //        ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "ActualNumInstalm"),
        //        AdvLoan = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AdvLoan"),
        //        ApplicFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "ApplicFactDate"),
        //        ApplicProgWeightFact = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "ApplicProgWeightFact"),
        //        AreaCostFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AreaCostFactAdj"),
        //        AreaCostInstalment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AreaCostInstalment"),
        //        FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "FundLine"),
        //        FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "FundStart"),
        //        LearnDelApplicLARSCarPilFundSubRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "LearnDelApplicLARSCarPilFundSubRate"),
        //        LearnDelApplicSubsidyPilotAreaCode = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "LearnDelApplicSubsidyPilotAreaCode"),
        //        LearnDelCarLearnPilotAimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "LearnDelCarLearnPilotAimValue"),
        //        LearnDelCarLearnPilotInstalAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "LearnDelCarLearnPilotInstalAmount"),
        //        LearnDelEligCareerLearnPilot = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "LearnDelEligCareerLearnPilot"),
        //        LiabilityDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "LiabilityDate"),
        //        LoanBursAreaUplift = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "LoanBursAreaUplift"),
        //        LoanBursSupp = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "LoanBursSupp"),
        //        OutstndNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalm"),
        //        PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm"),
        //        WeightedRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "WeightedRate"),
        //    };
        //}

        //public LearningDeliveryPeriodisedAttribute[] LearningDeliveryPeriodisedAttributeData(IDataEntity learningDelivery)
        //{
        //    List<string> attributeList = new List<string>()
        //    {
        //        "ALBCode",
        //        "ALBSupportPayment",
        //        "AreaUpliftBalPayment",
        //        "AreaUpliftOnProgPayment",
        //        "LearnDelCarLearnPilotOnProgPayment",
        //        "LearnDelCarLearnPilotBalPayment"
        //    };

        //    List<LearningDeliveryPeriodisedAttribute> learningDeliveryPeriodisedAttributeList = new List<LearningDeliveryPeriodisedAttribute>();

        //    foreach (var attribute in attributeList)
        //    {
        //        var attributeValue = learningDelivery.Attributes[attribute];

        //        var changePoints = attributeValue.Changepoints;

        //        if (!changePoints.Any())
        //        {
        //            var value = decimal.Parse(attributeValue.Value.ToString());

        //            learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedAttribute
        //            {
        //                AttributeName = attribute,
        //                Period1 = value,
        //                Period2 = value,
        //                Period3 = value,
        //                Period4 = value,
        //                Period5 = value,
        //                Period6 = value,
        //                Period7 = value,
        //                Period8 = value,
        //                Period9 = value,
        //                Period10 = value,
        //                Period11 = value,
        //                Period12 = value,
        //            });
        //        }

        //        if (changePoints.Any())
        //        {
        //            learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedAttribute
        //            {
        //                AttributeName = attribute,
        //                Period1 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period1),
        //                Period2 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period2),
        //                Period3 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period3),
        //                Period4 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period4),
        //                Period5 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period5),
        //                Period6 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period6),
        //                Period7 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period7),
        //                Period8 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period8),
        //                Period9 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period9),
        //                Period10 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period10),
        //                Period11 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period11),
        //                Period12 = GetPeriodAttributeValue(attributeValue, _internalDataCache.Period12),
        //            });
        //        }
        //    }

        //    return learningDeliveryPeriodisedAttributeList.ToArray();
        //}

        private decimal GetPeriodAttributeValue(IAttributeData attributes, DateTime periodDate)
        {
            return decimal.Parse(attributes.Changepoints.Where(cp => cp.ChangePoint == periodDate).Select(v => v.Value).SingleOrDefault().ToString());
        }
    }
}
