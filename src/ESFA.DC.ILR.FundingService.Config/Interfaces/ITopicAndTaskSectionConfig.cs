namespace ESFA.DC.ILR.FundingService.Config.Interfaces
{
    public interface ITopicAndTaskSectionConfig
    {
        string TopicFunding_TaskPerformALBCalculation { get; set; }

        string TopicFunding_TaskPerformFM25Calculation { get; set; }

        string TopicFunding_TaskPerformFM35Calculation { get; set; }

        string TopicFunding_TaskPerformFM36Calculation { get; set; }
    }
}
