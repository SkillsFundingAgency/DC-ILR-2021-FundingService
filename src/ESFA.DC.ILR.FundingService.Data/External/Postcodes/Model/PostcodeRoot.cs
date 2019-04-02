using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model
{
    public class PostcodeRoot
    {
        public string Postcode { get; set; }

        public IReadOnlyCollection<CareerLearningPilot> CareerLearningPilots { get; set; }

        public IReadOnlyCollection<DasDisadvantage> DasDisadvantages { get; set; }

        public IReadOnlyCollection<EfaDisadvantage> EfaDisadvantages { get; set; }

        public IReadOnlyCollection<SfaAreaCost> SfaAreaCosts { get; set; }

        public IReadOnlyCollection<SfaDisadvantage> SfaDisadvantages { get; set; }
    }
}
