using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Stateless.Models
{
    public class ALBActorModel
    {
        public int JobId { get; set; }
        public int Ukprn { get; set; }
        public byte[] AlbValidLearners { get; set; }
        public byte[] ReferenceDataCache { get; set; }
    }
}
