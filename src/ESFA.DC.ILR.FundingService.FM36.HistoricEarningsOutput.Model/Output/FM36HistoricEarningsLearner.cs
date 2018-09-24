using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM36.HistoricEarningsOutput.Model.Output
{
    public class FM36HistoricEarningsLearner
    {
        public string LearnRefNumber { get; set; }

        public List<FM36HistoricEarningOutputValues> HistoricEarningOutputValues { get; set; }
    }
}
