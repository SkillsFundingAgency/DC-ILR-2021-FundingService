namespace ESFA.DC.ILR.FundingService.Data.Population.Keys
{
    public struct LearnRefNumberULNKey
    {
        public LearnRefNumberULNKey(string learnRefNumber, long uln)
        {
            LearnRefNumber = learnRefNumber;
            ULN = uln;
        }

        public string LearnRefNumber { get; }

        public long ULN { get; }
    }
}
