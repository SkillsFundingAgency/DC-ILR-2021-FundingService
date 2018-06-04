using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class AzureCosmosOptions
    {
        public string CosmosEndpointUrl { get; set; }
        public string CosmosAuthKeyOrResourceToken { get; set; }
    }
}
