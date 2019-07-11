using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class FM25PeriodisationFundingService : IFundingService<FM25Global, PeriodisationGlobal>
    {
        public PeriodisationGlobal ProcessFunding(int ukprn, IEnumerable<FM25Global> inputList, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
