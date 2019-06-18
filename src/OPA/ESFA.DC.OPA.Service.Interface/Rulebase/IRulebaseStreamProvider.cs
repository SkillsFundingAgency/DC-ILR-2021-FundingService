using System.IO;

namespace ESFA.DC.OPA.Service.Interface.Rulebase
{
    public interface IRulebaseStreamProvider<T>
    {
        Stream GetStream();
    }
}
