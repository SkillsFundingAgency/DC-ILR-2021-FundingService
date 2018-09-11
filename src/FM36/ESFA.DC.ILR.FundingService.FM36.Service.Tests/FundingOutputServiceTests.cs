using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Attribute;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Tests
{
    public class FundingOutputServiceTests
    {
        //[Fact]
        //public void FundingOutputModel()
        //{
        //    var model = new FM36FundingOutputs
        //    {
        //        Global = new GlobalAttribute
        //        {
        //            UKPRN = 12345678,
        //            LARSVersion = "LARS_Version",
        //            RulebaseVersion = "Rulebase_Version",
        //            Year = "1819"
        //        },
        //        Learners = new LearnerAttribute[]
        //        {
        //            new LearnerAttribute
        //            {
        //                LearnRefNumber = "Learner1",
        //                LearningDeliveryAttributes = new LearningDeliveryAttribute[]
        //                {
        //                    new LearningDeliveryAttribute
        //                    {
        //                        AimSeqNumber = 1,
        //                        LearningDeliveryAttributeDatas = new LearningDeliveryAttributeData
        //                        {
        //                             ActualDaysIL = 1,
        //                             ActualNumInstalm = 2,
        //                             AdjStartDate = new DateTime(2018, 8, 1),
        //                             AgeAtProgStart = 3,
        //                             AppAdjLearnStartDate = new DateTime(2018, 8, 1),
        //                             AppAdjLearnStartDateMatchPathway = new DateTime(2018, 8, 1),
        //                             ApplicCompDate = new DateTime(2018, 8, 1),
        //                             CombinedAdjProp = 1.0m,
        //                             Completed = false,
        //                             FirstIncentiveThresholdDate = new DateTime(2018, 8, 1),
        //                             FundStart = false,
        //                             LDApplic1618FrameworkUpliftBalancingValue = 1.0m,
        //                             LDApplic1618FrameworkUpliftCompElement = 1.0m,
        //                             LDApplic1618FRameworkUpliftCompletionValue = 1.0m,
        //                             LDApplic1618FrameworkUpliftMonthInstalVal = 1.0m,
        //                             LDApplic1618FrameworkUpliftPrevEarnings = 1.0m,
        //                             LDApplic1618FrameworkUpliftPrevEarningsStage1 = 1.0m,
        //                             LDApplic1618FrameworkUpliftRemainingAmount = 1.0m,
        //                             LDApplic1618FrameworkUpliftTotalActEarnings = 1.0m,
        //                             LearnAimRef = "AimRef",
        //                             LearnDel1618AtStart = false,
        //                             LearnDelAppAccDaysIL = 1.0m,
        //                             LearnDelApplicDisadvAmount = 1.0m,
        //                             LearnDelApplicEmp1618Incentive = 1.0m,
        //                             LearnDelApplicEmpDate = new DateTime(2018, 8, 1),
        //                             LearnDelApplicProv1618FrameworkUplift = 1.0m,
        //                             LearnDelApplicProv1618Incentive = 1.0m,
        //                             LearnDelAppPrevAccDaysIL = 4,
        //                             LearnDelDaysIL = 5,
        //                             LearnDelDisadAmount = 1.0m,
        //                             LearnDelEligDisadvPayment = false,
        //                             LearnDelEmpIdFirstAdditionalPaymentThreshold = 1.0m,
        //                             LearnDelEmpIdSecondAdditionalPaymentThreshold = 1.0m,
        //                             LearnDelHistDaysThisApp = 6,
        //                             LearnDelHistProgEarnings = 1.0m,
        //                             LearnDelInitialFundLineType = "Type",
        //                             LearnDelMathEng = false,
        //                             LearnDelProgEarliestACT2Date = new DateTime(2018, 8, 1),
        //                             LearnDelNonLevyProcured = false,
        //                             MathEngAimValue = 1.0m,
        //                             OutstandNumOnProgInstalm = 7,
        //                             PlannedNumOnProgInstalm = 8,
        //                             PlannedTotalDaysIL = 9,
        //                             SecondIncentiveThresholdDate = new DateTime(2018, 8, 1),
        //                             ThresholdDays = 10,
        //                             LearnDelApplicCareLeaverIncentive = 1.0m,
        //                             LearnDelHistDaysCareLeavers = 11,
        //                             LearnDelAccDaysILCareLeavers = 12,
        //                             LearnDelPrevAccDaysILCareLeavers = 13,
        //                             LearnDelLearnerAddPayThresholdDate = new DateTime(2018, 8, 1),
        //                             LearnDelRedCode = 14,
        //                             LearnDelRedStartDate = new DateTime(2018, 8, 1)
        //                        },
        //                        LearningDeliveryPeriodisedAttributes = new LearningDeliveryPeriodisedAttribute[]
        //                        {
        //                             new LearningDeliveryPeriodisedAttribute
        //                            {
        //                                AttributeName = "Attribute1",
        //                                Period1 = 1.00m,
        //                                Period2 = 1.00m,
        //                                Period3 = 1.00m,
        //                                Period4 = 1.00m,
        //                                Period5 = 1.00m,
        //                                Period6 = 1.00m,
        //                                Period7 = 1.00m,
        //                                Period8 = 1.00m,
        //                                Period9 = 1.00m,
        //                                Period10 = 1.00m,
        //                                Period11 = 1.00m,
        //                                Period12 = 1.00m,
        //                            },
        //                            new LearningDeliveryPeriodisedAttribute
        //                            {
        //                                AttributeName = "Attribute2",
        //                                Period1 = 2.00m,
        //                                Period2 = 2.00m,
        //                                Period3 = 2.00m,
        //                                Period4 = 2.00m,
        //                                Period5 = 2.00m,
        //                                Period6 = 2.00m,
        //                                Period7 = 2.00m,
        //                                Period8 = 2.00m,
        //                                Period9 = 2.00m,
        //                                Period10 = 2.00m,
        //                                Period11 = 2.00m,
        //                                Period12 = 2.00m,
        //                            }
        //                        }
        //                    },
        //                    new LearningDeliveryAttribute
        //                    {
        //                        AimSeqNumber = 2,
        //                        LearningDeliveryAttributeDatas = new LearningDeliveryAttributeData
        //                        {
        //                             ActualDaysIL = 1,
        //                             ActualNumInstalm = 2,
        //                             AdjStartDate = new DateTime(2018, 8, 1),
        //                             AgeAtProgStart = 3,
        //                             AppAdjLearnStartDate = new DateTime(2018, 8, 1),
        //                             AppAdjLearnStartDateMatchPathway = new DateTime(2018, 8, 1),
        //                             ApplicCompDate = new DateTime(2018, 8, 1),
        //                             CombinedAdjProp = 1.0m,
        //                             Completed = false,
        //                             FirstIncentiveThresholdDate = new DateTime(2018, 8, 1),
        //                             FundStart = false,
        //                             LDApplic1618FrameworkUpliftBalancingValue = 1.0m,
        //                             LDApplic1618FrameworkUpliftCompElement = 1.0m,
        //                             LDApplic1618FRameworkUpliftCompletionValue = 1.0m,
        //                             LDApplic1618FrameworkUpliftMonthInstalVal = 1.0m,
        //                             LDApplic1618FrameworkUpliftPrevEarnings = 1.0m,
        //                             LDApplic1618FrameworkUpliftPrevEarningsStage1 = 1.0m,
        //                             LDApplic1618FrameworkUpliftRemainingAmount = 1.0m,
        //                             LDApplic1618FrameworkUpliftTotalActEarnings = 1.0m,
        //                             LearnAimRef = "AimRef",
        //                             LearnDel1618AtStart = false,
        //                             LearnDelAppAccDaysIL = 1.0m,
        //                             LearnDelApplicDisadvAmount = 1.0m,
        //                             LearnDelApplicEmp1618Incentive = 1.0m,
        //                             LearnDelApplicEmpDate = new DateTime(2018, 8, 1),
        //                             LearnDelApplicProv1618FrameworkUplift = 1.0m,
        //                             LearnDelApplicProv1618Incentive = 1.0m,
        //                             LearnDelAppPrevAccDaysIL = 4,
        //                             LearnDelDaysIL = 5,
        //                             LearnDelDisadAmount = 1.0m,
        //                             LearnDelEligDisadvPayment = false,
        //                             LearnDelEmpIdFirstAdditionalPaymentThreshold = 1.0m,
        //                             LearnDelEmpIdSecondAdditionalPaymentThreshold = 1.0m,
        //                             LearnDelHistDaysThisApp = 6,
        //                             LearnDelHistProgEarnings = 1.0m,
        //                             LearnDelInitialFundLineType = "Type",
        //                             LearnDelMathEng = false,
        //                             LearnDelProgEarliestACT2Date = new DateTime(2018, 8, 1),
        //                             LearnDelNonLevyProcured = false,
        //                             MathEngAimValue = 1.0m,
        //                             OutstandNumOnProgInstalm = 7,
        //                             PlannedNumOnProgInstalm = 8,
        //                             PlannedTotalDaysIL = 9,
        //                             SecondIncentiveThresholdDate = new DateTime(2018, 8, 1),
        //                             ThresholdDays = 10,
        //                             LearnDelApplicCareLeaverIncentive = 1.0m,
        //                             LearnDelHistDaysCareLeavers = 11,
        //                             LearnDelAccDaysILCareLeavers = 12,
        //                             LearnDelPrevAccDaysILCareLeavers = 13,
        //                             LearnDelLearnerAddPayThresholdDate = new DateTime(2018, 8, 1),
        //                             LearnDelRedCode = 14,
        //                             LearnDelRedStartDate = new DateTime(2018, 8, 1)
        //                        },
        //                        LearningDeliveryPeriodisedAttributes = new LearningDeliveryPeriodisedAttribute[]
        //                        {
        //                             new LearningDeliveryPeriodisedAttribute
        //                            {
        //                                AttributeName = "Attribute1",
        //                                Period1 = 1.00m,
        //                                Period2 = 1.00m,
        //                                Period3 = 1.00m,
        //                                Period4 = 1.00m,
        //                                Period5 = 1.00m,
        //                                Period6 = 1.00m,
        //                                Period7 = 1.00m,
        //                                Period8 = 1.00m,
        //                                Period9 = 1.00m,
        //                                Period10 = 1.00m,
        //                                Period11 = 1.00m,
        //                                Period12 = 1.00m,
        //                            },
        //                            new LearningDeliveryPeriodisedAttribute
        //                            {
        //                                AttributeName = "Attribute2",
        //                                Period1 = 2.00m,
        //                                Period2 = 2.00m,
        //                                Period3 = 2.00m,
        //                                Period4 = 2.00m,
        //                                Period5 = 2.00m,
        //                                Period6 = 2.00m,
        //                                Period7 = 2.00m,
        //                                Period8 = 2.00m,
        //                                Period9 = 2.00m,
        //                                Period10 = 2.00m,
        //                                Period11 = 2.00m,
        //                                Period12 = 2.00m,
        //                            }
        //                        }
        //                    }
        //                },
        //                PriceEpisodeAttributes = new PriceEpisodeAttribute[]
        //                {
        //                    new PriceEpisodeAttribute
        //                    {
        //                        PriceEpisodeIdentifier = "P_E_Identifier",
        //                        PriceEpisodeAttributeDatas = new PriceEpisodeAttributeData
        //                        {
        //                            EpisodeStartDate = new DateTime(2018, 8, 1),
        //                            TNP1 = 1.0m,
        //                            TNP2 = 1.0m,
        //                            TNP3 = 1.0m,
        //                            TNP4 = 1.0m,
        //                            PriceEpisodeUpperBandLimit = 1.0m,
        //                            PriceEpisodePlannedEndDate = new DateTime(2018, 8, 1),
        //                            PriceEpisodeActualEndDate = new DateTime(2018, 8, 1),
        //                            PriceEpisodeTotalTNPPrice = 1.0m,
        //                            PriceEpisodeUpperLimitAdjustment = 1.0m,
        //                            PriceEpisodePlannedInstalments = 1,
        //                            PriceEpisodeActualInstalments = 2,
        //                            PriceEpisodeInstalmentsThisPeriod = 3,
        //                            PriceEpisodeCompletionElement = 1.0m,
        //                            PriceEpisodePreviousEarnings = 1.0m,
        //                            PriceEpisodeInstalmentValue = 1.0m,
        //                            PriceEpisodeOnProgPayment = 1.0m,
        //                            PriceEpisodeTotalEarnings = 1.0m,
        //                            PriceEpisodeBalanceValue = 1.0m,
        //                            PriceEpisodeBalancePayment = 1.0m,
        //                            PriceEpisodeCompleted = false,
        //                            PriceEpisodeCompletionPayment = 1.0m,
        //                            PriceEpisodeRemainingTNPAmount = 1.0m,
        //                            PriceEpisodeRemainingAmountWithinUpperLimit = 1.0m,
        //                            PriceEpisodeCappedRemainingTNPAmount = 1.0m,
        //                            PriceEpisodeExpectedTotalMonthlyValue = 1.0m,
        //                            PriceEpisodeAimSeqNumber = 100000,
        //                            PriceEpisodeFirstDisadvantagePayment = 1.0m,
        //                            PriceEpisodeSecondDisadvantagePayment = 1.0m,
        //                            PriceEpisodeApplic1618FrameworkUpliftBalancing = 1.0m,
        //                            PriceEpisodeApplic1618FrameworkUpliftCompletionPayment = 1.0m,
        //                            PriceEpisodeApplic1618FrameworkUpliftOnProgPayment = 1.0m,
        //                            PriceEpisodeSecondProv1618Pay = 1.0m,
        //                            PriceEpisodeFirstEmp1618Pay = 1.0m,
        //                            PriceEpisodeSecondEmp1618Pay = 1.0m,
        //                            PriceEpisodeFirstProv1618Pay = 1.0m,
        //                            PriceEpisodeLSFCash = 1.0m,
        //                            PriceEpisodeFundLineType = "Type",
        //                            PriceEpisodeSFAContribPct = 1.0m,
        //                            PriceEpisodeLevyNonPayInd = 3,
        //                            EpisodeEffectiveTNPStartDate = new DateTime(2018, 8, 1),
        //                            PriceEpisodeFirstAdditionalPaymentThresholdDate = new DateTime(2018, 8, 1),
        //                            PriceEpisodeSecondAdditionalPaymentThresholdDate = new DateTime(2018, 8, 1),
        //                            PriceEpisodeContractType = "Type",
        //                            PriceEpisodePreviousEarningsSameProvider = 1.0m,
        //                            PriceEpisodeTotProgFunding = 1.0m,
        //                            PriceEpisodeProgFundIndMinCoInvest = 1.0m,
        //                            PriceEpisodeProgFundIndMaxEmpCont = 1.0m,
        //                            PriceEpisodeTotalPMRs = 1.0m,
        //                            PriceEpisodeCumulativePMRs = 1.0m,
        //                            PriceEpisodeCompExemCode = 4,
        //                            PriceEpisodeLearnerAdditionalPaymentThresholdDate = new DateTime(2018, 8, 1),
        //                            PriceEpisodeAgreeId = "AgreedId",
        //                            PriceEpisodeRedStartDate = new DateTime(2018, 8, 1),
        //                            PriceEpisodeRedStatusCode = 5
        //                        },
        //                        PriceEpisodePeriodisedAttributes = new PriceEpisodePeriodisedAttribute[]
        //                        {
        //                            new PriceEpisodePeriodisedAttribute
        //                            {
        //                                AttributeName = "Attribute1",
        //                                Period1 = 1.00m,
        //                                Period2 = 1.00m,
        //                                Period3 = 1.00m,
        //                                Period4 = 1.00m,
        //                                Period5 = 1.00m,
        //                                Period6 = 1.00m,
        //                                Period7 = 1.00m,
        //                                Period8 = 1.00m,
        //                                Period9 = 1.00m,
        //                                Period10 = 1.00m,
        //                                Period11 = 1.00m,
        //                                Period12 = 1.00m,
        //                            },
        //                            new PriceEpisodePeriodisedAttribute
        //                            {
        //                                AttributeName = "Attribute2",
        //                                Period1 = 2.00m,
        //                                Period2 = 2.00m,
        //                                Period3 = 2.00m,
        //                                Period4 = 2.00m,
        //                                Period5 = 2.00m,
        //                                Period6 = 2.00m,
        //                                Period7 = 2.00m,
        //                                Period8 = 2.00m,
        //                                Period9 = 2.00m,
        //                                Period10 = 2.00m,
        //                                Period11 = 2.00m,
        //                                Period12 = 2.00m,
        //                            }
        //                        }
        //                    }
        //                },
        //                HistoricEarningOutputAttributeDatas = new HistoricEarningOutputAttributeData[]
        //                {
        //                    new HistoricEarningOutputAttributeData
        //                    {
        //                        AppIdentifierOutput = "AppIdentifierOutput",
        //                        AppProgCompletedInTheYearOutput = false,
        //                        HistoricDaysInYearOutput = 1,
        //                        HistoricEffectiveTNPStartDateOutput = new DateTime(2018, 8, 1),
        //                        HistoricEmpIdEndWithinYearOutput = 2,
        //                        HistoricEmpIdStartWithinYearOutput = 3,
        //                        HistoricFworkCodeOutput = 4,
        //                        HistoricLearner1618AtStartOutput = false,
        //                        HistoricPMRAmountOutput = 1.0m,
        //                        HistoricProgrammeStartDateIgnorePathwayOutput = new DateTime(2018, 8, 1),
        //                        HistoricProgrammeStartDateMatchPathwayOutput = new DateTime(2018, 8, 1),
        //                        HistoricProgTypeOutput = 5,
        //                        HistoricPwayCodeOutput = 6,
        //                        HistoricSTDCodeOutput = 7,
        //                        HistoricTNP1Output = 1.0m,
        //                        HistoricTNP2Output = 1.0m,
        //                        HistoricTNP3Output = 1.0m,
        //                        HistoricTNP4Output = 1.0m,
        //                        HistoricTotal1618UpliftPaymentsInTheYear = 1.0m,
        //                        HistoricTotalProgAimPaymentsInTheYear = 1.0m,
        //                        HistoricULNOutput = 1234567890,
        //                        HistoricUptoEndDateOutput = new DateTime(2018, 8, 1),
        //                        HistoricVirtualTNP3EndofThisYearOutput = 1.0m,
        //                        HistoricVirtualTNP4EndofThisYearOutput = 1.0m,
        //                        HistoricLearnDelProgEarliestACT2DateOutput = new DateTime(2018, 8, 1),
        //                    }
        //                }
        //            }
        //        }
        //    };

        //    var json = new ESFA.DC.Serialization.Json.JsonSerializationService();

        //    var jsonString = json.Serialize(model);
        //}

        ////private FundingOutputService NewService(IInternalDataCache internalDataCache = null, IDataEntityAttributeService dataEntityAttributeService = null)
        ////{
        ////    return new FundingOutputService(internalDataCache, dataEntityAttributeService);
        ////}
    }
}
