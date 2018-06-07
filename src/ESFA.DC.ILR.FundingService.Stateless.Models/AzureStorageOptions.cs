using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class AzureStorageOptions
    {
        public string AzureBlobConnectionString { get; set; }
        public string AzureBlobContainerName { get; set; }
    }
}
