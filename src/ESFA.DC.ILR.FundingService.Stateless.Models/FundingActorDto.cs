namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class FundingActorDto
    {
        public long JobId { get; set; }

        public string ValidLearners { get; set; }

        public string ExternalDataCache { get; set; }

        public string InternalDataCache { get; set; }

        public string FileDataCache { get; set; }
    }
}
