using System;

namespace ESFA.DC.ILR.FundingService.Data.External.Organisation.Model
{
    public class PostcodeSpecialistResource
    {
        public string Postcode { get; set; }

        public string SpecialistResources { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
