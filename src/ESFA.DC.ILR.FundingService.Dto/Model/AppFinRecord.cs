using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class AppFinRecord
    {
        public int AFinAmount { get; set; }

        public int AFinCode { get; set; }

        public DateTime AFinDate { get; set; }

        public string AFinType { get; set; }
    }
}