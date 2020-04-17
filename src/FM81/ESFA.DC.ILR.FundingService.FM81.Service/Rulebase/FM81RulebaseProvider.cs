using System.IO;
using System.Linq;
using System.Reflection;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM81.Service.Rulebase
{
    public class FM81RulebaseProvider : IRulebaseStreamProvider<FM81LearnerDto>
    {
        private const string RulebaseName = @"Trailblazer Funding Calc 20_21";

        public Stream GetStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var rulebaseZipPath = manifestResourceNames.First(n => n.Contains(RulebaseName));

            return assembly.GetManifestResourceStream(rulebaseZipPath);
        }
    }
}
