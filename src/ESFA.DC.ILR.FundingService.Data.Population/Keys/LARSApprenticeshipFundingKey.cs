namespace ESFA.DC.ILR.FundingService.Data.Population.Keys
{
    public struct LARSApprenticeshipFundingKey
    {
        public LARSApprenticeshipFundingKey(int code, int progType, int pwayCode)
        {
            Code = code;
            ProgType = progType;
            PwayCode = pwayCode;
        }

        public int Code { get; }

        public int ProgType { get; }

        public int PwayCode { get; }
    }
}
