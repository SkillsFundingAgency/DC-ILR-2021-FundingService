using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36.HistoricEarningsOutput.Model;
using ESFA.DC.ILR.FundingService.FM36.Service.Constants;
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
                        .SelectMany(g => g.Children.Where(e => e.EntityName == Attributes.EntityLearner)
                        .Select(LearnerFromDataEntity))
                        .ToArray(),
                    };
                }
            }

            return new FM36FundingOutputs();
        }

        public FM36Global GlobalOutput(IDataEntity dataEntity)
        {
            return new FM36Global
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN).Value,
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LARSVersion),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
                Year = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.Year),
            };
        }

        public Learner LearnerFromDataEntity(IDataEntity dataEntity)
        {
            return new Learner()
            {
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnRefNumber),
                LearningDeliveryAttributes = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityLearningDelivery)
                        .Select(LearningDeliveryFromDataEntity).ToArray(),
                PriceEpisodeAttributes = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityApprenticeshipPriceEpisode)
                        .Select(PriceEpisodeFromDataEntity).ToArray(),
                HistoricEarningOutputAttributeDatas = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityHistoricEarningOutput)
                        .Select(HistoricEarningOutputDataFromDataEntity).ToArray()
            };
        }

        public LearningDelivery LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDelivery
            {
                AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AimSeqNumber).Value,
                LearningDeliveryAttributeDatas = LearningDeliveryAttributeData(dataEntity),
                LearningDeliveryPeriodisedAttributes = LearningDeliveryPeriodisedAttributeData(dataEntity).ToArray(),
            };
        }

        public LearningDeliveryValues LearningDeliveryAttributeData(IDataEntity dataEntity)
        {
            return new LearningDeliveryValues
            {
                ActualDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualDaysIL),
                ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualNumInstalm),
                AdjStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AdjStartDate),
                AgeAtProgStart = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AgeAtProgStart),
                AppAdjLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AppAdjLearnStartDate),
                AppAdjLearnStartDateMatchPathway = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AppAdjLearnStartDateMatchPathway),
                ApplicCompDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ApplicCompDate),
                CombinedAdjProp = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.CombinedAdjProp),
                Completed = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Completed),
                FirstIncentiveThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.FirstIncentiveThresholdDate),
                FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.FundStart),
                LDApplic1618FrameworkUpliftBalancingValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FrameworkUpliftBalancingValue),
                LDApplic1618FrameworkUpliftCompElement = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FrameworkUpliftCompElement),
                LDApplic1618FRameworkUpliftCompletionValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FRameworkUpliftCompletionValue),
                LDApplic1618FrameworkUpliftMonthInstalVal = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FrameworkUpliftMonthInstalVal),
                LDApplic1618FrameworkUpliftPrevEarnings = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FrameworkUpliftPrevEarnings),
                LDApplic1618FrameworkUpliftPrevEarningsStage1 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FrameworkUpliftPrevEarningsStage1),
                LDApplic1618FrameworkUpliftRemainingAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FrameworkUpliftRemainingAmount),
                LDApplic1618FrameworkUpliftTotalActEarnings = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LDApplic1618FrameworkUpliftTotalActEarnings),
                LearnAimRef = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnAimRef),
                LearnDel1618AtStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnDel1618AtStart),
                LearnDelAppAccDaysIL = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelAppAccDaysIL),
                LearnDelApplicDisadvAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelApplicDisadvAmount),
                LearnDelApplicEmp1618Incentive = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelApplicEmp1618Incentive),
                LearnDelApplicEmpDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LearnDelApplicEmpDate),
                LearnDelApplicProv1618FrameworkUplift = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelApplicProv1618FrameworkUplift),
                LearnDelApplicProv1618Incentive = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelApplicProv1618Incentive),
                LearnDelAppPrevAccDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelAppPrevAccDaysIL),
                LearnDelDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelDaysIL),
                LearnDelDisadAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelDisadAmount),
                LearnDelEligDisadvPayment = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnDelEligDisadvPayment),
                LearnDelEmpIdFirstAdditionalPaymentThreshold = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelEmpIdFirstAdditionalPaymentThreshold),
                LearnDelEmpIdSecondAdditionalPaymentThreshold = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelEmpIdSecondAdditionalPaymentThreshold),
                LearnDelHistDaysThisApp = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelHistDaysThisApp),
                LearnDelHistProgEarnings = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelHistProgEarnings),
                LearnDelInitialFundLineType = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnDelInitialFundLineType),
                LearnDelMathEng = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnDelMathEng),
                LearnDelProgEarliestACT2Date = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LearnDelProgEarliestACT2Date),
                LearnDelNonLevyProcured = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnDelNonLevyProcured),
                LearnDelApplicCareLeaverIncentive = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelApplicCareLeaverIncentive),
                LearnDelHistDaysCareLeavers = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelHistDaysCareLeavers),
                LearnDelAccDaysILCareLeavers = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelAccDaysILCareLeavers),
                LearnDelPrevAccDaysILCareLeavers = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelPrevAccDaysILCareLeavers),
                LearnDelLearnerAddPayThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LearnDelLearnerAddPayThresholdDate),
                LearnDelRedCode = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelRedCode),
                LearnDelRedStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LearnDelRedStartDate),
                MathEngAimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.MathEngAimValue),
                OutstandNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.OutstandNumOnProgInstalm),
                PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedNumOnProgInstalm),
                PlannedTotalDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedTotalDaysIL),
                SecondIncentiveThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.SecondIncentiveThresholdDate),
                ThresholdDays = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ThresholdDays)
            };
        }

        public LearningDeliveryPeriodisedValues[] LearningDeliveryPeriodisedAttributeData(IDataEntity learningDelivery)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.DisadvFirstPayment,
                Attributes.DisadvSecondPayment,
                Attributes.InstPerPeriod,
                Attributes.LDApplic1618FrameworkUpliftBalancingPayment,
                Attributes.LDApplic1618FrameworkUpliftCompletionPayment,
                Attributes.LDApplic1618FrameworkUpliftOnProgPayment,
                Attributes.LearnDelFirstEmp1618Pay,
                Attributes.LearnDelFirstProv1618Pay,
                Attributes.LearnDelLearnAddPayment,
                Attributes.LearnDelLevyNonPayInd,
                Attributes.LearnDelSecondEmp1618Pay,
                Attributes.LearnDelSecondProv1618Pay,
                Attributes.LearnDelSEMContWaiver,
                Attributes.LearnDelSFAContribPct,
                Attributes.LearnSuppFund,
                Attributes.LearnSuppFundCash,
                Attributes.MathEngBalPayment,
                Attributes.MathEngBalPct,
                Attributes.MathEngOnProgPayment,
                Attributes.MathEngOnProgPct,
                Attributes.ProgrammeAimBalPayment,
                Attributes.ProgrammeAimCompletionPayment,
                Attributes.ProgrammeAimOnProgPayment,
                Attributes.ProgrammeAimProgFundIndMaxEmpCont,
                Attributes.ProgrammeAimProgFundIndMinCoInvest,
                Attributes.ProgrammeAimTotProgFund
            };

            List<LearningDeliveryPeriodisedValues> learningDeliveryPeriodisedAttributeList = new List<LearningDeliveryPeriodisedValues>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learningDelivery.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = ConvertValue(attributeValue.Value);

                    learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedValues
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
                    learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedValues
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

        public PriceEpisodes PriceEpisodeFromDataEntity(IDataEntity dataEntity)
        {
            return new PriceEpisodes
            {
                PriceEpisodeIdentifier = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PriceEpisodeIdentifier),
                PriceEpisodeAttributeDatas = PriceEpisodeAttributeData(dataEntity),
                PriceEpisodePeriodisedAttributes = PriceEpisodePeriodisedAttributeData(dataEntity).ToArray(),
            };
        }

        public PriceEpisodeData PriceEpisodeAttributeData(IDataEntity dataEntity)
        {
            return new PriceEpisodeData
            {
                EpisodeStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.EpisodeStartDate),
                TNP1 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.TNP1),
                TNP2 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.TNP2),
                TNP3 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.TNP3),
                TNP4 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.TNP4),
                PriceEpisodeUpperBandLimit = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeUpperBandLimit),
                PriceEpisodePlannedEndDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.PriceEpisodePlannedEndDate),
                PriceEpisodeActualEndDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.PriceEpisodeActualEndDate),
                PriceEpisodeTotalTNPPrice = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeTotalTNPPrice),
                PriceEpisodeUpperLimitAdjustment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeUpperLimitAdjustment),
                PriceEpisodePlannedInstalments = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PriceEpisodePlannedInstalments),
                PriceEpisodeActualInstalments = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PriceEpisodeActualInstalments),
                PriceEpisodeInstalmentsThisPeriod = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PriceEpisodeInstalmentsThisPeriod),
                PriceEpisodeCompletionElement = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeCompletionElement),
                PriceEpisodePreviousEarnings = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodePreviousEarnings),
                PriceEpisodeInstalmentValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeInstalmentValue),
                PriceEpisodeOnProgPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeOnProgPayment),
                PriceEpisodeTotalEarnings = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeTotalEarnings),
                PriceEpisodeBalanceValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeBalanceValue),
                PriceEpisodeBalancePayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeBalancePayment),
                PriceEpisodeCompleted = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.PriceEpisodeCompleted),
                PriceEpisodeCompletionPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeCompletionPayment),
                PriceEpisodeRemainingTNPAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeRemainingTNPAmount),
                PriceEpisodeRemainingAmountWithinUpperLimit = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeRemainingAmountWithinUpperLimit),
                PriceEpisodeCappedRemainingTNPAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeCappedRemainingTNPAmount),
                PriceEpisodeExpectedTotalMonthlyValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeExpectedTotalMonthlyValue),
                PriceEpisodeAimSeqNumber = _dataEntityAttributeService.GetLongAttributeValue(dataEntity, Attributes.PriceEpisodeAimSeqNumber),
                PriceEpisodeFirstDisadvantagePayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeFirstDisadvantagePayment),
                PriceEpisodeSecondDisadvantagePayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeSecondDisadvantagePayment),
                PriceEpisodeApplic1618FrameworkUpliftBalancing = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeApplic1618FrameworkUpliftBalancing),
                PriceEpisodeApplic1618FrameworkUpliftCompletionPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment),
                PriceEpisodeApplic1618FrameworkUpliftOnProgPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment),
                PriceEpisodeSecondProv1618Pay = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeSecondProv1618Pay),
                PriceEpisodeFirstEmp1618Pay = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeFirstEmp1618Pay),
                PriceEpisodeSecondEmp1618Pay = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeSecondEmp1618Pay),
                PriceEpisodeFirstProv1618Pay = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeFirstProv1618Pay),
                PriceEpisodeLSFCash = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeLSFCash),
                PriceEpisodeFundLineType = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PriceEpisodeFundLineType),
                PriceEpisodeSFAContribPct = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeSFAContribPct),
                PriceEpisodeLevyNonPayInd = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PriceEpisodeLevyNonPayInd),
                EpisodeEffectiveTNPStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.EpisodeEffectiveTNPStartDate),
                PriceEpisodeFirstAdditionalPaymentThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.PriceEpisodeFirstAdditionalPaymentThresholdDate),
                PriceEpisodeSecondAdditionalPaymentThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.PriceEpisodeSecondAdditionalPaymentThresholdDate),
                PriceEpisodeContractType = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PriceEpisodeContractType),
                PriceEpisodePreviousEarningsSameProvider = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodePreviousEarningsSameProvider),
                PriceEpisodeTotProgFunding = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeTotProgFunding),
                PriceEpisodeProgFundIndMinCoInvest = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeProgFundIndMinCoInvest),
                PriceEpisodeProgFundIndMaxEmpCont = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeProgFundIndMaxEmpCont),
                PriceEpisodeTotalPMRs = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeTotalPMRs),
                PriceEpisodeCumulativePMRs = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PriceEpisodeCumulativePMRs),
                PriceEpisodeCompExemCode = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PriceEpisodeCompExemCode),
                PriceEpisodeLearnerAdditionalPaymentThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.PriceEpisodeLearnerAdditionalPaymentThresholdDate),
                PriceEpisodeAgreeId = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PriceEpisodeAgreeId),
                PriceEpisodeRedStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.PriceEpisodeRedStartDate),
                PriceEpisodeRedStatusCode = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PriceEpisodeRedStatusCode)
            };
        }

        public PriceEpisodePeriodisedValues[] PriceEpisodePeriodisedAttributeData(IDataEntity priceEpisode)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.PriceEpisodeApplic1618FrameworkUpliftBalancing,
                Attributes.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,
                Attributes.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,
                Attributes.PriceEpisodeBalancePayment,
                Attributes.PriceEpisodeBalanceValue,
                Attributes.PriceEpisodeCompletionPayment,
                Attributes.PriceEpisodeFirstDisadvantagePayment,
                Attributes.PriceEpisodeFirstEmp1618Pay,
                Attributes.PriceEpisodeFirstProv1618Pay,
                Attributes.PriceEpisodeInstalmentsThisPeriod,
                Attributes.PriceEpisodeLearnerAdditionalPayment,
                Attributes.PriceEpisodeLevyNonPayInd,
                Attributes.PriceEpisodeLSFCash,
                Attributes.PriceEpisodeOnProgPayment,
                Attributes.PriceEpisodeProgFundIndMaxEmpCont,
                Attributes.PriceEpisodeProgFundIndMinCoInvest,
                Attributes.PriceEpisodeSecondDisadvantagePayment,
                Attributes.PriceEpisodeSecondEmp1618Pay,
                Attributes.PriceEpisodeSecondProv1618Pay,
                Attributes.PriceEpisodeSFAContribPct,
                Attributes.PriceEpisodeTotProgFunding
            };

            List<PriceEpisodePeriodisedValues> priceEpisodePeriodisedAttributeList = new List<PriceEpisodePeriodisedValues>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = priceEpisode.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = ConvertValue(attributeValue.Value);

                    priceEpisodePeriodisedAttributeList.Add(new PriceEpisodePeriodisedValues
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
                    priceEpisodePeriodisedAttributeList.Add(new PriceEpisodePeriodisedValues
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

            return priceEpisodePeriodisedAttributeList.ToArray();
        }

        public HistoricEarningOutputAttributeData HistoricEarningOutputDataFromDataEntity(IDataEntity dataEntity)
        {
            return new HistoricEarningOutputAttributeData
            {
                AppIdentifierOutput = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.AppIdentifierOutput),
                AppProgCompletedInTheYearOutput = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AppProgCompletedInTheYearOutput),
                HistoricDaysInYearOutput = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.HistoricDaysInYearOutput),
                HistoricEffectiveTNPStartDateOutput = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.HistoricEffectiveTNPStartDateOutput),
                HistoricEmpIdEndWithinYearOutput = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.HistoricEmpIdEndWithinYearOutput),
                HistoricEmpIdStartWithinYearOutput = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.HistoricEmpIdStartWithinYearOutput),
                HistoricFworkCodeOutput = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.HistoricFworkCodeOutput),
                HistoricLearner1618AtStartOutput = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.HistoricLearner1618AtStartOutput),
                HistoricPMRAmountOutput = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricPMRAmountOutput),
                HistoricProgrammeStartDateIgnorePathwayOutput = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.HistoricProgrammeStartDateIgnorePathwayOutput),
                HistoricProgrammeStartDateMatchPathwayOutput = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.HistoricProgrammeStartDateMatchPathwayOutput),
                HistoricProgTypeOutput = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.HistoricProgTypeOutput),
                HistoricPwayCodeOutput = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.HistoricPwayCodeOutput),
                HistoricSTDCodeOutput = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.HistoricSTDCodeOutput),
                HistoricTNP1Output = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricTNP1Output),
                HistoricTNP2Output = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricTNP2Output),
                HistoricTNP3Output = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricTNP3Output),
                HistoricTNP4Output = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricTNP4Output),
                HistoricTotal1618UpliftPaymentsInTheYear = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricTotal1618UpliftPaymentsInTheYear),
                HistoricTotalProgAimPaymentsInTheYear = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricTotalProgAimPaymentsInTheYear),
                HistoricULNOutput = _dataEntityAttributeService.GetLongAttributeValue(dataEntity, Attributes.HistoricULNOutput),
                HistoricUptoEndDateOutput = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.HistoricUptoEndDateOutput),
                HistoricVirtualTNP3EndofThisYearOutput = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricVirtualTNP3EndofThisYearOutput),
                HistoricVirtualTNP4EndofThisYearOutput = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.HistoricVirtualTNP4EndofThisYearOutput),
                HistoricLearnDelProgEarliestACT2DateOutput = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.HistoricLearnDelProgEarliestACT2DateOutput)
            };
        }

        private decimal? GetPeriodAttributeValue(IAttributeData attributes, DateTime periodDate)
        {
            var value = ConvertValue(attributes.Changepoints.Where(cp => cp.ChangePoint == periodDate).Select(v => v.Value).SingleOrDefault());

            return value != null ? decimal.Parse(value.ToString()) : value;
        }

        private decimal? ConvertValue(object value)
        {
            if (value != null && value.ToString() != "uncertain")
            {
                var stringValue = value.ToString();

                if (stringValue == "true" || stringValue == "false")
                {
                    return stringValue == "true" ? 1.0m : 0.0m;
                }

                return decimal.Parse(stringValue);
            }

            return null;
        }
    }
}
