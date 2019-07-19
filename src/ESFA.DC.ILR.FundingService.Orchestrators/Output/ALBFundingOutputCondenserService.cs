using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class ALBFundingOutputCondenserService : IFundingOutputCondenserService<ALBGlobal>
    {
        public ALBGlobal Condense(IEnumerable<ALBGlobal> fundingOutputs, int ukprn, string year)
        {
            var firstOutput = fundingOutputs.FirstOrDefault();

            if (firstOutput != null)
            {
                firstOutput.Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(o => o.Learners).ToList();

                return firstOutput;
            }

            return new ALBGlobal
            {
                UKPRN = ukprn
            };
        }
    }
}
