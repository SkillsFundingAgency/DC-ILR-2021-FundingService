using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class FM35FundingOutputCondenserService : IFundingOutputCondenserService<FM35FundingOutputs>
    {
        public FM35FundingOutputs Condense(IEnumerable<FM35FundingOutputs> fundingOutputs)
        {
            fundingOutputs = fundingOutputs.Where(f => f != null);

            if (fundingOutputs.Any())
            {
                return new FM35FundingOutputs()
                {
                    Global = fundingOutputs.FirstOrDefault()?.Global,
                    Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(r => r.Learners).ToArray()
                };
            }

            return new FM35FundingOutputs();
        }
    }
}
