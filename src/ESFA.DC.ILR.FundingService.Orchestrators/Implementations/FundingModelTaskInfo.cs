using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class FundingModelTaskInfo
    {
        public string TaskPerformSwitch { get; set; }

        public IEnumerable<int> FundingModel { get; set; }

        public string OutputKey { get; set; }
    }
}
