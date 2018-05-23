using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.IO.AzureCosmos.Config.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class AzureCosmosKeyValuePersistenceConfig : IAzureCosmosKeyValuePersistenceServiceConfig
    {
        public AzureCosmosKeyValuePersistenceConfig(string endpointUrl, string authKeyOrResourceToken)
        {
            EndpointUrl = endpointUrl;
            AuthKeyOrResourceToken = authKeyOrResourceToken;
        }
        public string EndpointUrl { get; }
        public string AuthKeyOrResourceToken { get; }
    }
}
