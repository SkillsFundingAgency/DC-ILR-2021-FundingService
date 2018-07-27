using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;

namespace ESFA.DC.ILR.FundingService.Stubs
{
    public class DataPersister
    {
        public void PersistData<T>(T fundingOutputs, string filePath)
        {
            IJsonSerializationService serializationService = new JsonSerializationService();

            var serializedOutputs = serializationService.Serialize(fundingOutputs);
            
            System.IO.File.WriteAllText(filePath, serializedOutputs);
        }
    }
}
