using System.IO;
using System.Linq;
using System.Reflection;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Rulebase
{
    public class FM70RulebaseProvider : IRulebaseStreamProvider<FM70LearnerDto>
    {
        private const string RulebaseName = @"ESF 1920 Funding Calc";

        public Stream GetStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var rulebaseZipPath = manifestResourceNames.First(n => n.Contains(RulebaseName));

            return assembly.GetManifestResourceStream(rulebaseZipPath);
        }
    }
}