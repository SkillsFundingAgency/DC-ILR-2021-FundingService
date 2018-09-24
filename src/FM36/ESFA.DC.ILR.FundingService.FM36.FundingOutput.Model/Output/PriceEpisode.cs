using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class PriceEpisode
    {
        public string PriceEpisodeIdentifier { get; set; }

        public List<PriceEpisodeValues> PriceEpisodeDatas { get; set; }

        public List<PriceEpisodePeriodisedValues> PriceEpisodePeriodisedValues { get; set; }
    }
}
