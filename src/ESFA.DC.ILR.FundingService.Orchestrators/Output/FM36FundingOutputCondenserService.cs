using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class FM36FundingOutputCondenserService : IFundingOutputCondenserService<FM36Global>
    {
        public FM36Global Condense(IEnumerable<FM36Global> fundingOutputs, int ukprn, string year)
        {
            var firstOutput = fundingOutputs.FirstOrDefault();

            if (firstOutput != null)
            {
                firstOutput.Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(o => o.Learners).ToList();

                return firstOutput;
            }

            return new FM36Global
            {
                UKPRN = ukprn,
                Year = year,
            };
        }
    }
}
