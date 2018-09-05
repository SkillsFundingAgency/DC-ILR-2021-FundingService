namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Attribute
{
    public class LearnerAttribute
    {
        public string LearnRefNumber { get; set; }

        public PriceEpisodeAttribute[] ProceEpisodeAttributes { get; set; }

        public LearningDeliveryAttribute[] LearningDeliveryAttributes { get; set; }

        public HistoricEarningOutputAttributeData[] HistoricEarningOutputAttributeDatas { get; set; }
    }
}
