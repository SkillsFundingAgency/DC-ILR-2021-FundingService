﻿namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class FundingActorDto
    {
        public int JobId { get; set; }

        public int Ukprn { get; set; }

        public string ValidLearners { get; set; }

        public string ExternalDataCache { get; set; }

        public string FileDataCache { get; set; }
    }
}