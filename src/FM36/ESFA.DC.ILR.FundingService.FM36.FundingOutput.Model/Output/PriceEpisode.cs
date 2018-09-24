using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class PriceEpisode
    {
        public string PriceEpisodeIdentifier { get; set; }

        public PriceEpisodeValues PriceEpisodeValues { get; set; }

        public List<PriceEpisodePeriodisedValues> PriceEpisodePeriodisedValues { get; set; }
    }
}
