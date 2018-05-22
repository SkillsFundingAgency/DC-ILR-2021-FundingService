using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class ServiceBusOptions
    {
        public string AuditQueueName { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string TopicName { get; set; }

        public string FundingCalcSubscriptionName { get; set; }
       
    }
}
