namespace ESFA.DC.ILR.FundingService.FundingActor
{
    public class FundingActorDto : IFundingActorDto
    {
        public long JobId { get; set; }

        public string ValidLearners { get; set; }

        public string ExternalDataCache { get; set; }

        public string InternalDataCache { get; set; }

        public string FileDataCache { get; set; }
    }
}
