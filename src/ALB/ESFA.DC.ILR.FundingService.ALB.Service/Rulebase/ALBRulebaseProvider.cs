using System.IO;
using System.Linq;
using System.Reflection;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Rulebase
{
    public class ALBRulebaseProvider : IRulebaseStreamProvider<ALBLearnerDto>
    {
        private const string RulebaseName = @"Loans Bursary 20_21";

        public Stream GetStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var rulebaseZipPath = manifestResourceNames.First(n => n.Contains(RulebaseName));

            return assembly.GetManifestResourceStream(rulebaseZipPath);
        }
    }
}
