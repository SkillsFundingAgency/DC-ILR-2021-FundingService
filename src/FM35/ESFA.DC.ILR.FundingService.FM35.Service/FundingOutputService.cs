using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using Oracle.Determinations.Masquerade.Util;

namespace ESFA.DC.ILR.FundingService.FM35.Service
{
    public class FundingOutputService : IOutputService<FM35FundingOutputs>
    {
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IDataEntityAttributeService dataEntityAttributeService)
        {
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        private static Dictionary<int, System.DateTime> Periods => new Dictionary<int, System.DateTime>
        {
           { 1, new System.DateTime(2018, 08, 01) },
           { 2, new System.DateTime(2018, 09, 01) },
           { 3, new System.DateTime(2018, 10, 01) },
           { 4, new System.DateTime(2018, 11, 01) },
           { 5, new System.DateTime(2018, 12, 01) },
           { 6, new System.DateTime(2019, 01, 01) },
           { 7, new System.DateTime(2019, 02, 01) },
           { 8, new System.DateTime(2019, 03, 01) },
           { 9, new System.DateTime(2019, 04, 01) },
           { 10, new System.DateTime(2019, 05, 01) },
           { 11, new System.DateTime(2019, 06, 01) },
           { 12, new System.DateTime(2019, 07, 01) },
        };

        public FM35FundingOutputs ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            if (dataEntities.Any())
            {
                return new FM35FundingOutputs
                {
                    Global = GlobalOutput(dataEntities.First()),
                    Learners = dataEntities
                        .Where(g => g.Children != null)
                        .SelectMany(g => g.Children.Where(e => e.EntityName == "Learner")
                        .Select(LearnerFromDataEntity))
                        .ToArray(),
                };
            }

            return new FM35FundingOutputs();
        }

        public GlobalAttribute GlobalOutput(IDataEntity dataEntity)
        {
            return new GlobalAttribute
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "UKPRN").Value,
                CurFundYr = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "CurFundYr"),
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "LARSVersion"),
                OrgVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "OrgVersion"),
                PostcodeDisadvantageVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "PostcodeDisadvantageVersion"),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "RulebaseVersion"),
            };
        }

        public LearnerAttribute LearnerFromDataEntity(IDataEntity dataEntity)
        {
            return new LearnerAttribute()
                {
                    LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "LearnRefNumber"),
                    LearningDeliveryAttributes = dataEntity
                        .Children
                        .Where(e => e.EntityName == "LearningDelivery")
                        .Select(LearningDeliveryFromDataEntity).ToArray()
                };
        }

        public LearningDeliveryAttribute LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDeliveryAttribute
                {
                    AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "AimSeqNumber"),
                    LearningDeliveryAttributeDatas = LearningDeliveryAttributeData(dataEntity),
                    LearningDeliveryPeriodisedAttributes = LearningDeliveryPeriodisedAttributeData(dataEntity),
                };
        }

        public LearningDeliveryAttributeData LearningDeliveryAttributeData(IDataEntity dataEntity)
        {
            return new LearningDeliveryAttributeData
            {
                AchApplicDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "AchApplicDate"),
                Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "Achieved"),
                AchieveElement = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AchieveElement"),
                AchievePayElig = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AchievePayElig"),
                AchievePayPctPreTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AchievePayPctPreTrans"),
                AchPayTransHeldBack = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AchPayTransHeldBack"),
                ActualDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "ActualDaysIL"),
                ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "ActualNumInstalm"),
                ActualNumInstalmPreTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "ActualNumInstalmPreTrans"),
                ActualNumInstalmTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "ActualNumInstalmTrans"),
                AdjLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "AdjLearnStartDate"),
                AdltLearnResp = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AdltLearnResp"),
                AgeAimStart = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "AgeAimStart"),
                AimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AimValue"),
                AppAdjLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "AppAdjLearnStartDate"),
                AppAgeFact = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "AppAgeFact"),
                AppATAGTA = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AppATAGTA"),
                AppCompetency = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AppCompetency"),
                AppFuncSkill = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AppFuncSkill"),
                AppFuncSkill1618AdjFact = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "AppFuncSkill1618AdjFact"),
                AppKnowl = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AppKnowl"),
                AppLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "AppLearnStartDate"),
                ApplicEmpFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "ApplicEmpFactDate"),
                ApplicFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "ApplicFactDate"),
                ApplicFundRateDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "ApplicFundRateDate"),
                ApplicProgWeightFact = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "ApplicProgWeightFact"),
                ApplicUnweightFundRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "ApplicUnweightFundRate"),
                ApplicWeightFundRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "ApplicWeightFundRate"),
                AppNonFund = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AppNonFund"),
                AreaCostFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AreaCostFactAdj"),
                BalInstalmPreTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "BalInstalmPreTrans"),
                BaseValueUnweight = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "BaseValueUnweight"),
                CapFactor = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "CapFactor"),
                DisUpFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "DisUpFactAdj"),
                EmpOutcomePayElig = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "EmpOutcomePayElig"),
                EmpOutcomePctHeldBackTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "EmpOutcomePctHeldBackTrans"),
                EmpOutcomePctPreTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "EmpOutcomePctPreTrans"),
                EmpRespOth = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "EmpRespOth"),
                ESOL = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "ESOL"),
                FullyFund = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "FullyFund"),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "FundLine"),
                FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "FundStart"),
                LargeEmployerFM35Fctr = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "LargeEmployerFM35Fctr"),
                LargeEmployerID = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "LargeEmployerID"),
                LargeEmployerStatusDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "LargeEmployerStatusDate"),
                LTRCUpliftFctr = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "LTRCUpliftFctr"),
                NonGovCont = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "NonGovCont"),
                OLASSCustody = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "OLASSCustody"),
                OnProgPayPctPreTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "OnProgPayPctPreTrans"),
                OutstndNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalm"),
                OutstndNumOnProgInstalmTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalmTrans"),
                PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm"),
                PlannedNumOnProgInstalmTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalmTrans"),
                PlannedTotalDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "PlannedTotalDaysIL"),
                PlannedTotalDaysILPreTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "PlannedTotalDaysILPreTrans"),
                PropFundRemain = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "PropFundRemain"),
                PropFundRemainAch = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "PropFundRemainAch"),
                PrscHEAim = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "PrscHEAim"),
                Residential = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "Residential"),
                Restart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "Restart"),
                SpecResUplift = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "SpecResUplift"),
                StartPropTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "StartPropTrans"),
                ThresholdDays = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "ThresholdDays"),
                Traineeship = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "Traineeship"),
                Trans = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "Trans"),
                TrnAdjLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "TrnAdjLearnStartDate"),
                TrnWorkPlaceAim = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "TrnWorkPlaceAim"),
                TrnWorkPrepAim = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "TrnWorkPrepAim"),
                UnWeightedRateFromESOL = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "UnWeightedRateFromESOL"),
                UnweightedRateFromLARS = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "UnweightedRateFromLARS"),
                WeightedRateFromESOL = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "WeightedRateFromESOL"),
                WeightedRateFromLARS = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "StartPropTrans"),
            };
        }

        protected internal LearningDeliveryPeriodisedAttribute[] LearningDeliveryPeriodisedAttributeData(IDataEntity learningDelivery)
        {
            List<string> attributeList = new List<string>()
            {
                "AchievePayment",
                "AchievePayPct",
                "AchievePayPctTrans",
                "BalancePayment",
                "BalancePaymentUncapped",
                "BalancePct",
                "BalancePctTrans",
                "EmpOutcomePay",
                "EmpOutcomePct",
                "EmpOutcomePctTrans",
                "InstPerPeriod",
                "LearnSuppFund",
                "LearnSuppFundCash",
                "OnProgPayment",
                "OnProgPaymentUncapped",
                "OnProgPayPct",
                "OnProgPayPctTrans",
                "TransInstPerPeriod"
            };

            List<LearningDeliveryPeriodisedAttribute> learningDeliveryPeriodisedAttributesList = new List<LearningDeliveryPeriodisedAttribute>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learningDelivery.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = attribute == "LearnSuppFund" ? ConvertLearnSuppFund(attributeValue.Value.ToString()) : decimal.Parse(attributeValue.Value.ToString());

                    learningDeliveryPeriodisedAttributesList.Add(new LearningDeliveryPeriodisedAttribute
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
                    learningDeliveryPeriodisedAttributesList.Add(new LearningDeliveryPeriodisedAttribute
                    {
                        AttributeName = attribute,
                        Period1 = GetPeriodAttributeValue(attributeValue, 1, attribute),
                        Period2 = GetPeriodAttributeValue(attributeValue, 2, attribute),
                        Period3 = GetPeriodAttributeValue(attributeValue, 3, attribute),
                        Period4 = GetPeriodAttributeValue(attributeValue, 4, attribute),
                        Period5 = GetPeriodAttributeValue(attributeValue, 5, attribute),
                        Period6 = GetPeriodAttributeValue(attributeValue, 6, attribute),
                        Period7 = GetPeriodAttributeValue(attributeValue, 7, attribute),
                        Period8 = GetPeriodAttributeValue(attributeValue, 8, attribute),
                        Period9 = GetPeriodAttributeValue(attributeValue, 9, attribute),
                        Period10 = GetPeriodAttributeValue(attributeValue, 10, attribute),
                        Period11 = GetPeriodAttributeValue(attributeValue, 11, attribute),
                        Period12 = GetPeriodAttributeValue(attributeValue, 12, attribute),
                    });
                }
            }

            return learningDeliveryPeriodisedAttributesList.ToArray();
        }

        private decimal? GetPeriodAttributeValue(IAttributeData attribute, int period, string attributeName)
        {
            var value = attribute.Changepoints.Where(cp => cp.ChangePoint == Periods[period]).Select(v => v.Value).SingleOrDefault();

            return attributeName == "LearnSuppFund" ? ConvertLearnSuppFund(value.ToString()) : decimal.Parse(value.ToString());
        }

        private decimal? ConvertLearnSuppFund(string value)
        {
            if (value != null && value != "uncertain")
            {
                return value == "true" ? 1.0m : 0.0m;
            }

            return null;
        }
    }
}
