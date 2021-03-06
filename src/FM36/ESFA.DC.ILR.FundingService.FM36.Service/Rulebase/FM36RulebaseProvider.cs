﻿using System.IO;
using System.Linq;
using System.Reflection;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Rulebase
{
    public class FM36RulebaseProvider : IRulebaseStreamProvider<FM36LearnerDto>
    {
        private const string RulebaseName = @"Apprenticeships Earnings Calc 20_21";

        public Stream GetStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var rulebaseZipPath = manifestResourceNames.First(n => n.Contains(RulebaseName));

            return assembly.GetManifestResourceStream(rulebaseZipPath);
        }
    }
}
