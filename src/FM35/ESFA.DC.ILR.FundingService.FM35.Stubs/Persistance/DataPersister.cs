using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;

namespace ESFA.DC.ILR.FundingService.FM35.Stubs.Persistance
{
    public class DataPersister
    {
        public void PersistData(IFM35FundingOutputs fundingOutputs)
        {
            IJsonSerializationService serializationService = new JsonSerializationService();

            var serializedOutputs = serializationService.Serialize(fundingOutputs);

            WriteToFile(serializedOutputs);
        }

        private static void WriteToFile(string stringOutput)
        {
           using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
            {
                System.IO.File.WriteAllText(@"C:\Code\temp\FM35FundingService\Json_Output.json", stringOutput);
            }
        }
    }
}
