using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Context
{
    public class FundingContext : IFundingContext
    {
        public virtual int UKPRN { get; set; }

        public virtual IEnumerable<ILearner> ValidLearners { get; set; }
    }
}
