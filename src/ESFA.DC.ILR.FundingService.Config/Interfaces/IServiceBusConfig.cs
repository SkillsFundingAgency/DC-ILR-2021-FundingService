namespace ESFA.DC.ILR.FundingService.Config.Interfaces
{
    public interface IServiceBusConfig
    {
        string AuditQueueName { get; }

        string ServiceBusConnectionString { get; }

        string TopicName { get; }

        string FundingCalcSubscriptionName { get; }

        string DataStoreSubscriptionName { get; }
    }
}
