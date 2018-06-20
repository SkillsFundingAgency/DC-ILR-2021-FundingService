using System;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute
{
    public interface ILearningDeliveryAttributeData
    {
        DateTime? AchApplicDate { get; }

        bool? Achieved { get; }

        decimal? AchieveElement { get; }

        bool? AchievePayElig { get; }

        decimal? AchievePayPctPreTrans { get; }

        decimal? AchPayTransHeldBack { get; }

        int? ActualDaysIL { get; }

        int? ActualNumInstalm { get; }

        int? ActualNumInstalmPreTrans { get; }

        int? ActualNumInstalmTrans { get; }

        DateTime? AdjLearnStartDate { get; }

        bool? AdltLearnResp { get; }

        int? AgeAimStart { get; }

        decimal? AimValue { get; }

        DateTime? AppAdjLearnStartDate { get; }

        decimal? AppAgeFact { get; }

        bool? AppATAGTA { get; }

        bool? AppCompetency { get; }

        bool? AppFuncSkill { get; }

        decimal? AppFuncSkill1618AdjFact { get; }

        bool? AppKnowl { get; }

        DateTime? AppLearnStartDate { get; }

        DateTime? ApplicEmpFactDate { get; }

        DateTime? ApplicFactDate { get; }

        DateTime? ApplicFundRateDate { get; }

        string ApplicProgWeightFact { get; }

        decimal? ApplicUnweightFundRate { get; }

        decimal? ApplicWeightFundRate { get; }

        bool? AppNonFund { get; }

        decimal? AreaCostFactAdj { get; }

        int? BalInstalmPreTrans { get; }

        decimal? BaseValueUnweight { get; }

        decimal? CapFactor { get; }

        decimal? DisUpFactAdj { get; }

        bool? EmpOutcomePayElig { get; }

        decimal? EmpOutcomePctHeldBackTrans { get; }

        decimal? EmpOutcomePctPreTrans { get; }

        bool? EmpRespOth { get; }

        bool? ESOL { get; }

        bool? FullyFund { get; }

        string FundLine { get; }

        bool? FundStart { get; }

        int? LargeEmployerID { get; }

        decimal? LargeEmployerFM35Fctr { get; }

        DateTime? LargeEmployerStatusDate { get; }

        decimal? LTRCUpliftFctr { get; }

        decimal? NonGovCont { get; }

        bool? OLASSCustody { get; }

        decimal? OnProgPayPctPreTrans { get; }

        int? OutstndNumOnProgInstalm { get; }

        int? OutstndNumOnProgInstalmTrans { get; }

        int? PlannedNumOnProgInstalm { get; }

        int? PlannedNumOnProgInstalmTrans { get; }

        int? PlannedTotalDaysIL { get; }

        int? PlannedTotalDaysILPreTrans { get; }

        decimal? PropFundRemain { get; }

        decimal? PropFundRemainAch { get; }

        bool? PrscHEAim { get; }

         bool? Residential { get; }

        bool? Restart { get; }

        decimal? SpecResUplift { get; }

        decimal? StartPropTrans { get; }

        int? ThresholdDays { get; }

        bool? Traineeship { get; }

        bool? Trans { get; }

        DateTime? TrnAdjLearnStartDate { get; }

        bool? TrnWorkPlaceAim { get; }

        bool? TrnWorkPrepAim { get; }

        decimal? UnWeightedRateFromESOL { get; }

        decimal? UnweightedRateFromLARS { get; }

        decimal? WeightedRateFromESOL { get; }

        decimal? WeightedRateFromLARS { get; }
    }
}