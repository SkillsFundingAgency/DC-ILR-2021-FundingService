using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Providers.Output
{
    public class FundingOutputPersistenceSfService<T> : IFundingOutputPersistenceService<T> where T : class
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;

        public FundingOutputPersistenceSfService(
            IJsonSerializationService jsonSerializationService,
            IKeyValuePersistenceService keyValuePersistenceService)
        {
            _jsonSerializationService = jsonSerializationService;
            _keyValuePersistenceService = keyValuePersistenceService;
        }
        public async Task Process(T fundingOutputs, string fundingOutputKey)
        {
            await _keyValuePersistenceService.SaveAsync(fundingOutputKey,
                _jsonSerializationService.Serialize(fundingOutputs));
        }
    }
}
