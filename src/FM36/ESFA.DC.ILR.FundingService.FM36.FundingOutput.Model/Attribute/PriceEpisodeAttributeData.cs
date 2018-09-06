using System;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Attribute
{
    public class PriceEpisodeAttributeData
    {
        public DateTime? EpisodeStartDate { get; set; }

        public decimal? TNP1 { get; set; }

        public decimal? TNP2 { get; set; }

        public decimal? TNP3 { get; set; }

        public decimal? TNP4 { get; set; }

        public decimal? PriceEpisodeUpperBandLimit { get; set; }

        public DateTime? PriceEpisodePlannedEndDate { get; set; }

        public DateTime? PriceEpisodeActualEndDate { get; set; }

        public decimal? PriceEpisodeTotalTNPPrice { get; set; }

        public decimal? PriceEpisodeUpperLimitAdjustment { get; set; }

        public int? PriceEpisodePlannedInstalments { get; set; }

        public int? PriceEpisodeActualInstalments { get; set; }

        public int? PriceEpisodeInstalmentsThisPeriod { get; set; }

        public decimal? PriceEpisodeCompletionElement { get; set; }

        public decimal? PriceEpisodePreviousEarnings { get; set; }

        public decimal? PriceEpisodeInstalmentValue { get; set; }

        public decimal? PriceEpisodeOnProgPayment { get; set; }

        public decimal? PriceEpisodeTotalEarnings { get; set; }

        public decimal? PriceEpisodeBalanceValue { get; set; }

        public decimal? PriceEpisodeBalancePayment { get; set; }

        public bool? PriceEpisodeCompleted { get; set; }

        public decimal? PriceEpisodeCompletionPayment { get; set; }

        public decimal? PriceEpisodeRemainingTNPAmount { get; set; }

        public decimal? PriceEpisodeRemainingAmountWithinUpperLimit { get; set; }

        public decimal? PriceEpisodeCappedRemainingTNPAmount { get; set; }

        public decimal? PriceEpisodeExpectedTotalMonthlyValue { get; set; }

        public long? PriceEpisodeAimSeqNumber { get; set; }

        public decimal? PriceEpisodeFirstDisadvantagePayment { get; set; }

        public decimal? PriceEpisodeSecondDisadvantagePayment { get; set; }

        public decimal? PriceEpisodeApplic1618FrameworkUpliftBalancing { get; set; }

        public decimal? PriceEpisodeApplic1618FrameworkUpliftCompletionPayment { get; set; }

        public decimal? PriceEpisodeApplic1618FrameworkUpliftOnProgPayment { get; set; }

        public decimal? PriceEpisodeSecondProv1618Pay { get; set; }

        public decimal? PriceEpisodeFirstEmp1618Pay { get; set; }

        public decimal? PriceEpisodeSecondEmp1618Pay { get; set; }

        public decimal? PriceEpisodeFirstProv1618Pay { get; set; }

        public decimal? PriceEpisodeLSFCash { get; set; }

        public string PriceEpisodeFundLineType { get; set; }

        public decimal? PriceEpisodeSFAContribPct { get; set; }

        public int? PriceEpisodeLevyNonPayInd { get; set; }

        public DateTime? EpisodeEffectiveTNPStartDate { get; set; }

        public DateTime? PriceEpisodeFirstAdditionalPaymentThresholdDate { get; set; }

        public DateTime? PriceEpisodeSecondAdditionalPaymentThresholdDate { get; set; }

        public string PriceEpisodeContractType { get; set; }

        public decimal? PriceEpisodePreviousEarningsSameProvider { get; set; }

        public decimal? PriceEpisodeTotProgFunding { get; set; }

        public decimal? PriceEpisodeProgFundIndMinCoInvest { get; set; }

        public decimal? PriceEpisodeProgFundIndMaxEmpCont { get; set; }

        public decimal? PriceEpisodeTotalPMRs { get; set; }

        public decimal? PriceEpisodeCumulativePMRs { get; set; }

        public int? PriceEpisodeCompExemCode { get; set; }

        public DateTime? PriceEpisodeLearnerAdditionalPaymentThresholdDate { get; set; }

        public string PriceEpisodeAgreeId { get; set; }

        public DateTime? PriceEpisodeRedStartDate { get; set; }

        public int? PriceEpisodeRedStatusCode { get; set; }
    }
}
