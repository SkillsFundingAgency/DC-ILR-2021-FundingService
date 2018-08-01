﻿using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSLearningDelivery
    {
        public string LearnAimRef { get; set; }

        public string LearnAimRefType { get; set; }

        public string NotionalNVQLevelv2 { get; set; }

        public int? RegulatedCreditValue { get; set; }

        public string EnglandFEHEStatus { get; set; }

        public int? EnglPrscID { get; set; }

        public int? FrameworkCommonComponent { get; set; }

        public IEnumerable<LARSValidity> LARSValidities { get; set; }
    }
}
