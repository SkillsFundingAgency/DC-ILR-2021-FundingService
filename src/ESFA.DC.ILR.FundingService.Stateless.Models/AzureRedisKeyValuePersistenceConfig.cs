using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.IO.Redis.Config.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class AzureRedisKeyValuePersistenceConfig : IRedisKeyValuePersistenceServiceConfig
    {
        public AzureRedisKeyValuePersistenceConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}
