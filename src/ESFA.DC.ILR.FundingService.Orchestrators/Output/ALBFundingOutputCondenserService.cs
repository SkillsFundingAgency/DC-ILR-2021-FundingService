using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class ALBFundingOutputCondenserService : IFundingOutputCondenserService<ALBFundingOutputs>
    {
        public ALBFundingOutputs Condense(IEnumerable<ALBFundingOutputs> fundingOutputs)
        {
            fundingOutputs = fundingOutputs.Where(f => f != null);

            if (fundingOutputs.Any())
            {
                return new ALBFundingOutputs()
                {
                    Global = fundingOutputs.FirstOrDefault()?.Global,
                    Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(r => r.Learners).ToArray()
                };
            }

            return new ALBFundingOutputs();
        }
    }
}
