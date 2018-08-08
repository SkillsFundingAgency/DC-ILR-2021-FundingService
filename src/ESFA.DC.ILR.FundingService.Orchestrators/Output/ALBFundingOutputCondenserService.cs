using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class ALBFundingOutputCondenserService : IFundingOutputCondenserService<FundingOutputs>
    {
        public FundingOutputs Condense(IEnumerable<FundingOutputs> fundingOutputs)
        {
            return new FundingOutputs()
            {
                Global = fundingOutputs.First().Global,
                Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(r => r.Learners).ToArray()
            };
        }
    }
}
