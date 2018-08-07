using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.FM35.Service
{
    public class FundingOutputCondenserService : IFundingOutputCondenserService<FM35FundingOutputs>
    {
        public FM35FundingOutputs Condense(IEnumerable<FM35FundingOutputs> fundingOutputs)
        {
            return new FM35FundingOutputs()
            {
                Global = fundingOutputs.First().Global,
                Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(r => r.Learners).ToArray()
            };
        }
    }
}
