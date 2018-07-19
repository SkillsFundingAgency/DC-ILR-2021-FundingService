using System;
using System.Linq;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.Providers.Factory
{
    public class RulebaseProviderFactory : IRulebaseProviderFactory
    {
        public IRulebaseProvider Build()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName.Contains("Actor"));
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var rulebaseZipPath = manifestResourceNames.First(n => n.Contains("Rulebase"));

            return new RulebaseProvider(rulebaseZipPath);
        }
    }
}
