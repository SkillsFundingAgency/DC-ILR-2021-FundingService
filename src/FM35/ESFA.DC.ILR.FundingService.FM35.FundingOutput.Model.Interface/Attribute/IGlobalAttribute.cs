namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute
{
    public interface IGlobalAttribute
    {
        int UKPRN { get; }

        string CurFundYr { get; }

        string LARSVersion { get; }

        string OrgVersion { get; }

        string PostcodeDisadvantageVersion { get; }

        string RulebaseVersion { get; }
    }
}