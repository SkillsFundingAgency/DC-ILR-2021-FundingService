namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class FundingActorDto
    {
        public int JobId { get; set; }

        public int Ukprn { get; set; }

        public byte[] ValidLearners { get; set; }

        public byte[] ReferenceDataCache { get; set; }
    }
}
