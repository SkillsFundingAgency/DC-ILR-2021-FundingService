using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFM25FundingOutputCondenserService<TOut, TPeriodsationOut>
    {
        TOut Condense(IEnumerable<TOut> fundingOutputs, int ukprn, string year);

        TOut CondensePeriodisationResults(IEnumerable<TOut> fm25Results, IEnumerable<TPeriodsationOut> fm25PeriodisationResults);
    }
}
