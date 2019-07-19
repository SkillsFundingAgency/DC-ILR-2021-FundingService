using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class FM35FundingOutputCondenserService : IFundingOutputCondenserService<FM35Global>
    {
        public FM35Global Condense(IEnumerable<FM35Global> fundingOutputs, int ukprn, string year)
        {
            var firstOutput = fundingOutputs.FirstOrDefault();

            if (firstOutput != null)
            {
                firstOutput.Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(o => o.Learners).ToList();

                return firstOutput;
            }

            return new FM35Global
            {
                UKPRN = ukprn,
                CurFundYr = year,
            };
        }
    }
}
