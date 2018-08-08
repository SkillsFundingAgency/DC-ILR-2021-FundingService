using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Output
{
    public class FM25FundingOutputCondenserService : IFundingOutputCondenserService<Global>
    {
        public Global Condense(IEnumerable<Global> fundingOutputs)
        {
            var firstOutput = fundingOutputs.FirstOrDefault();

            if (firstOutput != null)
            {
                firstOutput.Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(o => o.Learners).ToList();

                return firstOutput;
            }

            return new Global();
        }
    }
}
