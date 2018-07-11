using ESFA.DC.ILR.FundingService.Config.Interfaces;

namespace ESFA.DC.ILR.FundingService.Config
{
    public class ServiceBusConfig : IServiceBusConfig
    {
        public string AuditQueueName { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string TopicName { get; set; }

        public string FundingCalcSubscriptionName { get; set; }

        public string DataStoreSubscriptionName { get; set; }
    }
}
