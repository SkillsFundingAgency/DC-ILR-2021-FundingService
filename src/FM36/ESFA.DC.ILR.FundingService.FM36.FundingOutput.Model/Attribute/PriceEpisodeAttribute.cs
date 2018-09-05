namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Attribute
{
    public class PriceEpisodeAttribute
    {
        public string PriceEpisodeIdentifier { get; set; }

        public PriceEpisodeAttributeData PriceEpisodeAttributeDatas { get; set; }

        public PriceEpisodePeriodisedAttribute[] PriceEpisodePeriodisedAttributes { get; set; }
    }
}
