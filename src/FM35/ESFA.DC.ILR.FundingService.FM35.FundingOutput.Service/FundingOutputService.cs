using System.Collections.Generic;
using System.Linq;
using ESFA.DC.DateTime.Provider.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service
{
    public class FundingOutputService : IOutputService<FM35FundingOutputs>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public FundingOutputService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
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
            return new FM35FundingOutputs
            {
                Global = GlobalOutput(dataEntities.Select(g => g.Attributes).First()),
                Learners = LearnerOutput(dataEntities.SelectMany(g => g.Children.Where(e => e.EntityName == "Learner"))),
            };
        }

        protected internal GlobalAttribute GlobalOutput(IDictionary<string, IAttributeData> attributes)
        {
            return new GlobalAttribute
            {
                UKPRN = int.Parse(GetAttributeValue(attributes, "UKPRN")),
                CurFundYr = GetAttributeValue(attributes, "CurFundYr"),
                LARSVersion = GetAttributeValue(attributes, "LARSVersion"),
                OrgVersion = GetAttributeValue(attributes, "OrgVersion"),
                PostcodeDisadvantageVersion = GetAttributeValue(attributes, "PostcodeDisadvantageVersion"),
                RulebaseVersion = GetAttributeValue(attributes, "RulebaseVersion"),
            };
        }

        protected internal LearnerAttribute[] LearnerOutput(IEnumerable<IDataEntity> learnerEntities)
        {
            var learners = new List<LearnerAttribute>();

            foreach (var learner in learnerEntities)
            {
                learners.Add(new LearnerAttribute
                {
                    LearnRefNumber = learner.LearnRefNumber,
                    LearningDeliveryAttributes = LearningDeliveryAttributes(learner),
                });
            }

            return learners.ToArray();
        }

        protected internal LearningDeliveryAttribute[] LearningDeliveryAttributes(IDataEntity learner)
        {
            var list = new List<LearningDeliveryAttribute>();
            string aimSeqNumber = "AimSeqNumber";

            var learningdeliveries = learner.Children.Where(e => e.EntityName == "LearningDelivery").ToList();

            foreach (var delivery in learningdeliveries)
            {
                list.Add(new LearningDeliveryAttribute
                {
                    AimSeqNumber = (int)DecimalStrToInt(delivery.Attributes[aimSeqNumber].Value.ToString()),
                    LearningDeliveryAttributeDatas = LearningDeliveryAttributeData(delivery),
                    LearningDeliveryPeriodisedAttributes = LearningDeliveryPeriodisedAttributeData(delivery),
                });
            }

            return list.ToArray();
        }

        protected internal LearningDeliveryAttributeData LearningDeliveryAttributeData(IDataEntity learningDelivery)
        {
            var attributes = learningDelivery.Attributes;

            return new LearningDeliveryAttributeData
            {
                AchApplicDate = GetAttributeValueDate(attributes, "AchApplicDate"),
                Achieved = ConvertToBit(GetAttributeValue(attributes, "Achieved")),
                AchieveElement = decimal.Parse(GetAttributeValue(attributes, "AchieveElement")),
                AchievePayElig = ConvertToBit(GetAttributeValue(attributes, "AchievePayElig")),
                AchievePayPctPreTrans = decimal.Parse(GetAttributeValue(attributes, "AchievePayPctPreTrans")),
                AchPayTransHeldBack = decimal.Parse(GetAttributeValue(attributes, "AchPayTransHeldBack")),
                ActualDaysIL = DecimalStrToInt(GetAttributeValue(attributes, "ActualDaysIL")),
                ActualNumInstalm = DecimalStrToInt(GetAttributeValue(attributes, "ActualNumInstalm")),
                ActualNumInstalmPreTrans = DecimalStrToInt(GetAttributeValue(attributes, "ActualNumInstalmPreTrans")),
                ActualNumInstalmTrans = DecimalStrToInt(GetAttributeValue(attributes, "ActualNumInstalmTrans")),
                AdjLearnStartDate = GetAttributeValueDate(attributes, "AdjLearnStartDate"),
                AdltLearnResp = ConvertToBit(GetAttributeValue(attributes, "AdltLearnResp")),
                AgeAimStart = DecimalStrToInt(GetAttributeValue(attributes, "AgeAimStart")),
                AimValue = DecimalStrToInt(GetAttributeValue(attributes, "AimValue")),
                AppAdjLearnStartDate = GetAttributeValueDate(attributes, "AppAdjLearnStartDate"),
                AppAgeFact = DecimalStrToInt(GetAttributeValue(attributes, "AppAgeFact")),
                AppATAGTA = ConvertToBit(GetAttributeValue(attributes, "AppATAGTA")),
                AppCompetency = ConvertToBit(GetAttributeValue(attributes, "AppCompetency")),
                AppFuncSkill = ConvertToBit(GetAttributeValue(attributes, "AppFuncSkill")),
                AppFuncSkill1618AdjFact = DecimalStrToInt(GetAttributeValue(attributes, "AppFuncSkill1618AdjFact")),
                AppKnowl = ConvertToBit(GetAttributeValue(attributes, "AppKnowl")),
                AppLearnStartDate = GetAttributeValueDate(attributes, "AppLearnStartDate"),
                ApplicEmpFactDate = GetAttributeValueDate(attributes, "ApplicEmpFactDate"),
                ApplicFactDate = GetAttributeValueDate(attributes, "ApplicFactDate"),
                ApplicFundRateDate = GetAttributeValueDate(attributes, "ApplicFundRateDate"),
                ApplicProgWeightFact = GetAttributeValue(attributes, "ApplicProgWeightFact"),
                ApplicUnweightFundRate = decimal.Parse(GetAttributeValue(attributes, "ApplicUnweightFundRate")),
                ApplicWeightFundRate = decimal.Parse(GetAttributeValue(attributes, "ApplicWeightFundRate")),
                AppNonFund = ConvertToBit(GetAttributeValue(attributes, "AppNonFund")),
                AreaCostFactAdj = decimal.Parse(GetAttributeValue(attributes, "AreaCostFactAdj")),
                BalInstalmPreTrans = DecimalStrToInt(GetAttributeValue(attributes, "BalInstalmPreTrans")),
                BaseValueUnweight = decimal.Parse(GetAttributeValue(attributes, "BaseValueUnweight")),
                CapFactor = decimal.Parse(GetAttributeValue(attributes, "CapFactor")),
                DisUpFactAdj = decimal.Parse(GetAttributeValue(attributes, "DisUpFactAdj")),
                EmpOutcomePayElig = ConvertToBit(GetAttributeValue(attributes, "EmpOutcomePayElig")),
                EmpOutcomePctHeldBackTrans = decimal.Parse(GetAttributeValue(attributes, "EmpOutcomePctHeldBackTrans")),
                EmpOutcomePctPreTrans = decimal.Parse(GetAttributeValue(attributes, "EmpOutcomePctPreTrans")),
                EmpRespOth = ConvertToBit(GetAttributeValue(attributes, "EmpRespOth")),
                ESOL = ConvertToBit(GetAttributeValue(attributes, "ESOL")),
                FullyFund = ConvertToBit(GetAttributeValue(attributes, "FullyFund")),
                FundLine = GetAttributeValue(attributes, "FundLine"),
                FundStart = ConvertToBit(GetAttributeValue(attributes, "FundStart")),
                LargeEmployerFM35Fctr = decimal.Parse(GetAttributeValue(attributes, "LargeEmployerFM35Fctr")),
                LargeEmployerID = DecimalStrToInt(GetAttributeValue(attributes, "LargeEmployerID")),
                LargeEmployerStatusDate = GetAttributeValueDate(attributes, "LargeEmployerStatusDate"),
                LTRCUpliftFctr = decimal.Parse(GetAttributeValue(attributes, "LTRCUpliftFctr")),
                NonGovCont = decimal.Parse(GetAttributeValue(attributes, "NonGovCont")),
                OLASSCustody = ConvertToBit(GetAttributeValue(attributes, "OLASSCustody")),
                OnProgPayPctPreTrans = DecimalStrToInt(GetAttributeValue(attributes, "OnProgPayPctPreTrans")),
                OutstndNumOnProgInstalm = DecimalStrToInt(GetAttributeValue(attributes, "OutstndNumOnProgInstalm")),
                OutstndNumOnProgInstalmTrans = DecimalStrToInt(GetAttributeValue(attributes, "OutstndNumOnProgInstalmTrans")),
                PlannedNumOnProgInstalm = DecimalStrToInt(GetAttributeValue(attributes, "PlannedNumOnProgInstalm")),
                PlannedNumOnProgInstalmTrans = DecimalStrToInt(GetAttributeValue(attributes, "PlannedNumOnProgInstalmTrans")),
                PlannedTotalDaysIL = DecimalStrToInt(GetAttributeValue(attributes, "PlannedTotalDaysIL")),
                PlannedTotalDaysILPreTrans = DecimalStrToInt(GetAttributeValue(attributes, "PlannedTotalDaysILPreTrans")),
                PropFundRemain = decimal.Parse(GetAttributeValue(attributes, "PropFundRemain")),
                PropFundRemainAch = decimal.Parse(GetAttributeValue(attributes, "PropFundRemainAch")),
                PrscHEAim = ConvertToBit(GetAttributeValue(attributes, "PrscHEAim")),
                Residential = ConvertToBit(GetAttributeValue(attributes, "Residential")),
                Restart = ConvertToBit(GetAttributeValue(attributes, "Restart")),
                SpecResUplift = decimal.Parse(GetAttributeValue(attributes, "SpecResUplift")),
                StartPropTrans = decimal.Parse(GetAttributeValue(attributes, "StartPropTrans")),
                ThresholdDays = DecimalStrToInt(GetAttributeValue(attributes, "ThresholdDays")),
                Traineeship = ConvertToBit(GetAttributeValue(attributes, "Traineeship")),
                Trans = ConvertToBit(GetAttributeValue(attributes, "Trans")),
                TrnAdjLearnStartDate = GetAttributeValueDate(attributes, "TrnAdjLearnStartDate"),
                TrnWorkPlaceAim = ConvertToBit(GetAttributeValue(attributes, "TrnWorkPlaceAim")),
                TrnWorkPrepAim = ConvertToBit(GetAttributeValue(attributes, "TrnWorkPrepAim")),
                UnWeightedRateFromESOL = decimal.Parse(GetAttributeValue(attributes, "UnWeightedRateFromESOL")),
                UnweightedRateFromLARS = decimal.Parse(GetAttributeValue(attributes, "UnweightedRateFromLARS")),
                WeightedRateFromESOL = decimal.Parse(GetAttributeValue(attributes, "WeightedRateFromESOL")),
                WeightedRateFromLARS = decimal.Parse(GetAttributeValue(attributes, "StartPropTrans")),
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
                var attributeValue = (AttributeData)learningDelivery.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = attributeValue.Name == "LearnSuppFund" ? ConvertLearnSuppFund(attributeValue.Value.ToString()) : decimal.Parse(attributeValue.Value.ToString());

                    learningDeliveryPeriodisedAttributesList.Add(new LearningDeliveryPeriodisedAttribute
                    {
                        AttributeName = attributeValue.Name,
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
                        AttributeName = attributeValue.Name,
                        Period1 = GetPeriodAttributeValue(attributeValue, 1),
                        Period2 = GetPeriodAttributeValue(attributeValue, 2),
                        Period3 = GetPeriodAttributeValue(attributeValue, 3),
                        Period4 = GetPeriodAttributeValue(attributeValue, 4),
                        Period5 = GetPeriodAttributeValue(attributeValue, 5),
                        Period6 = GetPeriodAttributeValue(attributeValue, 6),
                        Period7 = GetPeriodAttributeValue(attributeValue, 7),
                        Period8 = GetPeriodAttributeValue(attributeValue, 8),
                        Period9 = GetPeriodAttributeValue(attributeValue, 9),
                        Period10 = GetPeriodAttributeValue(attributeValue, 10),
                        Period11 = GetPeriodAttributeValue(attributeValue, 11),
                        Period12 = GetPeriodAttributeValue(attributeValue, 12),
                    });
                }
            }

            return learningDeliveryPeriodisedAttributesList.ToArray();
        }

        private decimal? GetPeriodAttributeValue(AttributeData attributes, int period)
        {
            var value = attributes.Changepoints.Where(cp => cp.ChangePoint == GetPeriodDate(period)).Select(v => v.Value).SingleOrDefault();

            return attributes.Name == "LearnSuppFund" ? ConvertLearnSuppFund(value.ToString()) : decimal.Parse(value.ToString());
        }

        private string GetAttributeValue(IDictionary<string, IAttributeData> attributes, string attributeName)
        {
            var attribute = attributes[attributeName].Value;

            if (attribute != null && attribute.ToString() != "uncertain")
            {
                return attribute.ToString();
            }

            return null;
        }

        private System.DateTime? GetAttributeValueDate(IDictionary<string, IAttributeData> attributes, string attributeName)
        {
            var attributeValue = attributes[attributeName].Value;

            if (attributeValue != null && attributeValue.ToString() != "uncertain")
            {
                System.DateTime attributeDateValue = _dateTimeProvider.ConvertOpaToDateTime(attributeValue.ToString());

                return attributeDateValue;
            }

            return null;
        }

        private int? DecimalStrToInt(string value)
        {
            if (value != null && value != "uncertain")
            {
                return int.Parse(value.Substring(0, value.IndexOf('.', 0)));
            }

            return null;
        }

        private bool? ConvertToBit(string value)
        {
            if (value != null && value != "uncertain")
            {
                return value == "true" ? true : false;
            }

            return null;
        }

        private decimal? ConvertLearnSuppFund(string value)
        {
            if (value != null && value != "uncertain")
            {
                return value == "true" ? 1.0m : 0.0m;
            }

            return null;
        }

        private System.DateTime GetPeriodDate(int periodNumber)
        {
            return Periods[periodNumber];
        }
    }
}
