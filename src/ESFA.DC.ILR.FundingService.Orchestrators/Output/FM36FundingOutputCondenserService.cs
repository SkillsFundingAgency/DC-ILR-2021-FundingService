using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class FM36FundingOutputCondenserService : IFundingOutputCondenserService<FM36FundingOutputs>
    {
        public FM36FundingOutputs Condense(IEnumerable<FM36FundingOutputs> fundingOutputs)
        {
            fundingOutputs = fundingOutputs.Where(f => f != null);

            if (fundingOutputs.Any())
            {
                return new FM36FundingOutputs()
                {
                    Global = fundingOutputs.FirstOrDefault()?.Global,
                    Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(r => r.Learners).ToArray()
                };
            }

            return new FM36FundingOutputs();
        }
    }
}
