using System.IO;
using System.Linq;
using System.Reflection;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.OPA.Service.Rulebase
{
    public class RulebaseProvider : IRulebaseProvider
    {
        private readonly string _rulebaseName;

        public RulebaseProvider(string rulebaseName)
        {
            _rulebaseName = rulebaseName;
        }

        public Stream GetStream()
        {
            var assembly = Assembly.GetEntryAssembly();
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var rulebaseZipPath = manifestResourceNames.First(n => n.Contains(_rulebaseName));

            return assembly.GetManifestResourceStream(rulebaseZipPath);
        }
    }
}
