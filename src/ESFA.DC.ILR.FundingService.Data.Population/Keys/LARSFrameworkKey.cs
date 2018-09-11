namespace ESFA.DC.ILR.FundingService.Data.Population.Keys
{
    public struct LARSFrameworkKey
    {
        public LARSFrameworkKey(string learnAimRef, int fworkCode, int progType, int pwayCode)
        {
            LearnAimRef = learnAimRef;
            FworkCode = fworkCode;
            ProgType = progType;
            PwayCode = pwayCode;
        }

        public string LearnAimRef { get; }

        public int FworkCode { get; }

        public int ProgType { get; }

        public int PwayCode { get; }
    }
}
