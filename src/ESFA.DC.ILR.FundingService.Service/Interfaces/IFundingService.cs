﻿using System.Collections.Generic;
using System.Threading;

namespace ESFA.DC.ILR.FundingService.Service.Interfaces
{
    public interface IFundingService<in TIn, out TOut>
    {
        TOut ProcessFunding(int ukprn, IEnumerable<TIn> inputList, CancellationToken cancellationToken);
    }
}
