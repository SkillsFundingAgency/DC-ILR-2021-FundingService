using ESFA.DC.IO.AzureStorage.Config.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stateless.Configuration
{
    internal class AzureStorageKeyValuePersistenceServiceConfig : IAzureStorageKeyValuePersistenceServiceConfig
    {
        public AzureStorageKeyValuePersistenceServiceConfig(string connectionString, string containerName)
        {
            ConnectionString = connectionString;
            ContainerName = containerName;
        }

        public string ConnectionString { get; }

        public string ContainerName { get; }
    }
}
