using System;

namespace ESFA.DC.ILR.FundingService.Data.External.Organisation.Model
{
    public class CampusIdentifierSpecResource
    {
        public string CampusIdentifier { get; set; }

        public string SpecialistResources { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
