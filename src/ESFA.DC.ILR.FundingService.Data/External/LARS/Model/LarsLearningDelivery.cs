using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSLearningDelivery
    {
        public string LearnAimRef { get; set; }

        public string LearnAimRefTitle { get; set; }

        public string LearnAimRefType { get; set; }

        public string LearningDeliveryGenre { get; set; }

        public string NotionalNVQLevelv2 { get; set; }

        public int? RegulatedCreditValue { get; set; }

        public string EnglandFEHEStatus { get; set; }

        public int? EnglPrscID { get; set; }

        public int? FrameworkCommonComponent { get; set; }

        public string AwardOrgCode { get; set; }

        public int? EFACOFType { get; set; }

        public decimal? SectorSubjectAreaTier2 { get; set; }

        public IReadOnlyCollection<LARSAnnualValue> LARSAnnualValues { get; set; }

        public IReadOnlyCollection<LARSFramework> LARSFrameworks { get; set; }

        public IReadOnlyCollection<LARSFunding> LARSFundings { get; set; }

        public IReadOnlyCollection<LARSLearningDeliveryCategory> LARSLearningDeliveryCategories { get; set; }
       
        public IReadOnlyCollection<LARSValidity> LARSValidities { get; set; }
    }
}
