namespace ESFA.DC.ILR.FundingService.FundingActor
{
    public interface IFundingActorDto
    {
        long JobId { get; }

        string ValidLearners { get; }

        string ExternalDataCache { get; }

        string FileDataCache { get; }
    }
}
