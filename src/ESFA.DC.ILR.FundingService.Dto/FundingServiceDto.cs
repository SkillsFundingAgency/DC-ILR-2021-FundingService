using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Dto
{
    public class FundingServiceDto : IFundingServiceDto
    {
        public virtual IMessage Message { get; set; }

        public virtual string[] ValidLearners { get; set; }
    }
}
