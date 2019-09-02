using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class LARSMapperServiceTests
    {
        [Fact]
        public void MapLARSLearningDeliveries()
        {
            var expectedLarsLearningDeliveries = new Dictionary<string, Data.External.LARS.Model.LARSLearningDelivery>
            {
                {
                    "LearnAimRef1",
                    new Data.External.LARS.Model.LARSLearningDelivery
                    {
                        LearnAimRef = "LearnAimRef1",
                        LARSAnnualValues = new List<Data.External.LARS.Model.LARSAnnualValue>
                        {
                            new Data.External.LARS.Model.LARSAnnualValue
                            {
                                BasicSkillsType = 1,
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
                        },
                        LARSFundings = new List<Data.External.LARS.Model.LARSFunding>
                        {
                            new Data.External.LARS.Model.LARSFunding
                            {
                                FundingCategory = "FundingCategory",
                                EffectiveFrom = new DateTime(2018, 8, 1),
                                EffectiveTo = new DateTime(2018, 8, 2),
                                RateWeighted = 1.0m,
                                WeightingFactor = "Factor",
                                RateUnWeighted = 1.0m
                            }
                        },
                    }
                },
                {
                    "LearnAimRef2",
                    new Data.External.LARS.Model.LARSLearningDelivery
                    {
                        LearnAimRef = "LearnAimRef2",
                        LARSLearningDeliveryCategories = new List<Data.External.LARS.Model.LARSLearningDeliveryCategory>
                        {
                            new Data.External.LARS.Model.LARSLearningDeliveryCategory
                            {
                                CategoryRef = 1,
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
                        },
                        LARSFrameworks = new List<Data.External.LARS.Model.LARSFramework>
                        {
                            new Data.External.LARS.Model.LARSFramework
                            {
                                FworkCode = 1,
                                ProgType = 1,
                                PwayCode = 1,
                                LARSFrameworkAim = new Data.External.LARS.Model.LARSFrameworkAim
                                {
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                    FrameworkComponentType = 2
                                },
                                LARSFrameworkApprenticeshipFundings = new List<Data.External.LARS.Model.LARSFrameworkApprenticeshipFunding>
                                {
                                    new Data.External.LARS.Model.LARSFrameworkApprenticeshipFunding
                                    {
                                        BandNumber = 1,
                                        CareLeaverAdditionalPayment = 1.0m,
                                        FundingCategory = "FundingCategory",
                                        EffectiveFrom = new DateTime(2018, 8, 1),
                                        CoreGovContributionCap = 1.0m,
                                        SixteenToEighteenIncentive = 1.0m,
                                        SixteenToEighteenProviderAdditionalPayment = 1.0m,
                                        SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                                        SixteenToEighteenFrameworkUplift = 1.0m,
                                        Duration = 1.0m,
                                        ReservedValue2 = 1.0m,
                                        ReservedValue3 = 1.0m,
                                        MaxEmployerLevyCap = 1.0m,
                                        FundableWithoutEmployer = "Fundable"
                                    }
                                },
                                LARSFrameworkCommonComponents = new List<Data.External.LARS.Model.LARSFrameworkCommonComponent>
                                {
                                    new Data.External.LARS.Model.LARSFrameworkCommonComponent
                                    {
                                        CommonComponent = 1
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var larsLearningDeliveries = new List<ReferenceDataService.Model.LARS.LARSLearningDelivery>
            {
                new ReferenceDataService.Model.LARS.LARSLearningDelivery
                {
                    LearnAimRef = "LearnAimRef1",
                    LARSAnnualValues = new List<ReferenceDataService.Model.LARS.LARSAnnualValue>
                    {
                        new ReferenceDataService.Model.LARS.LARSAnnualValue
                            {
                                BasicSkillsType = 1,
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
                    },
                    LARSFundings = new List<ReferenceDataService.Model.LARS.LARSFunding>
                    {
                        new ReferenceDataService.Model.LARS.LARSFunding
                        {
                            FundingCategory = "FundingCategory",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 8, 2),
                            RateWeighted = 1.0m,
                            WeightingFactor = "Factor",
                            RateUnWeighted = 1.0m
                        }
                    },
                },
                new ReferenceDataService.Model.LARS.LARSLearningDelivery
                {
                    LearnAimRef = "LearnAimRef2",
                    LARSLearningDeliveryCategories = new List<ReferenceDataService.Model.LARS.LARSLearningDeliveryCategory>
                    {
                        new ReferenceDataService.Model.LARS.LARSLearningDeliveryCategory
                        {
                            CategoryRef = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LARSFrameworks = new List<ReferenceDataService.Model.LARS.LARSFramework>
                    {
                        new ReferenceDataService.Model.LARS.LARSFramework
                        {
                            FworkCode = 1,
                            ProgType = 1,
                            PwayCode = 1,
                            LARSFrameworkAim = new ReferenceDataService.Model.LARS.LARSFrameworkAim
                            {
                                EffectiveFrom = new DateTime(2018, 8, 1),
                                FrameworkComponentType = 2
                            },
                            LARSFrameworkApprenticeshipFundings = new List<ReferenceDataService.Model.LARS.LARSFrameworkApprenticeshipFunding>
                            {
                                new ReferenceDataService.Model.LARS.LARSFrameworkApprenticeshipFunding
                                {
                                    BandNumber = 1,
                                    CareLeaverAdditionalPayment = 1.0m,
                                    FundingCategory = "FundingCategory",
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                    CoreGovContributionCap = 1.0m,
                                    SixteenToEighteenIncentive = 1.0m,
                                    SixteenToEighteenProviderAdditionalPayment = 1.0m,
                                    SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                                    SixteenToEighteenFrameworkUplift = 1.0m,
                                    Duration = 1.0m,
                                    ReservedValue2 = 1.0m,
                                    ReservedValue3 = 1.0m,
                                    MaxEmployerLevyCap = 1.0m,
                                    FundableWithoutEmployer = "Fundable"
                                }
                            },
                            LARSFrameworkCommonComponents = new List<ReferenceDataService.Model.LARS.LARSFrameworkCommonComponent>
                            {
                                new ReferenceDataService.Model.LARS.LARSFrameworkCommonComponent
                                {
                                    CommonComponent = 1
                                }
                            }
                        }
                    }
                }
            };

            expectedLarsLearningDeliveries.Should().BeEquivalentTo(NewService().MapLARSLearningDeliveries(larsLearningDeliveries));
        }

        [Fact]
        public void MapLARSLearningDeliveries_Null()
        {
            NewService().MapLARSLearningDeliveries(null).Should().BeNull();
        }

        [Fact]
        public void MapLARSStandards()
        {
            var expectedlarsStandards = new Dictionary<int, Data.External.LARS.Model.LARSStandard>
            {
                {
                    1,
                    new Data.External.LARS.Model.LARSStandard
                    {
                        StandardCode = 1,
                        StandardSectorCode = "1",
                        LARSStandardApprenticeshipFundings = new List<Data.External.LARS.Model.LARSStandardApprenticeshipFunding>
                        {
                            new Data.External.LARS.Model.LARSStandardApprenticeshipFunding
                            {
                                BandNumber = 1,
                                CareLeaverAdditionalPayment = 1.0m,
                                FundingCategory = "FundingCategory",
                                EffectiveFrom = new DateTime(2018, 8, 1),
                                CoreGovContributionCap = 1.0m,
                                SixteenToEighteenIncentive = 1.0m,
                                SixteenToEighteenProviderAdditionalPayment = 1.0m,
                                SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                                SixteenToEighteenFrameworkUplift = 1.0m,
                                Duration = 1.0m,
                                ReservedValue2 = 1.0m,
                                ReservedValue3 = 1.0m,
                                MaxEmployerLevyCap = 1.0m,
                                FundableWithoutEmployer = "Fundable"
                            }
                        },
                        LARSStandardCommonComponents = new List<Data.External.LARS.Model.LARSStandardCommonComponent>
                        {
                            new Data.External.LARS.Model.LARSStandardCommonComponent
                            {
                                CommonComponent = 1,
                                EffectiveFrom = new DateTime(2018, 8, 1),
                            }
                        },
                    }
                },
                {
                    2,
                    new Data.External.LARS.Model.LARSStandard
                    {
                        StandardCode = 2,
                        StandardSectorCode = "2",
                        LARSStandardApprenticeshipFundings = new List<Data.External.LARS.Model.LARSStandardApprenticeshipFunding>
                        {
                            new Data.External.LARS.Model.LARSStandardApprenticeshipFunding
                            {
                                BandNumber = 1,
                                CareLeaverAdditionalPayment = 1.0m,
                                FundingCategory = "FundingCategory",
                                EffectiveFrom = new DateTime(2018, 8, 1),
                                CoreGovContributionCap = 1.0m,
                                SixteenToEighteenIncentive = 1.0m,
                                SixteenToEighteenProviderAdditionalPayment = 1.0m,
                                SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                                SixteenToEighteenFrameworkUplift = 1.0m,
                                Duration = 1.0m,
                                ReservedValue2 = 1.0m,
                                ReservedValue3 = 1.0m,
                                MaxEmployerLevyCap = 1.0m,
                                FundableWithoutEmployer = "Fundable"
                            }
                        },
                        LARSStandardFundings = new List<Data.External.LARS.Model.LARSStandardFunding>
                        {
                            new Data.External.LARS.Model.LARSStandardFunding
                            {
                                FundingCategory = "FundingCategory",
                                EffectiveFrom = new DateTime(2018, 8, 1),
                                AchievementIncentive = 1.0m,
                                BandNumber = 1,
                                CoreGovContributionCap = 1.0m,
                                FundableWithoutEmployer = "Funadable",
                                SixteenToEighteenIncentive = 1.0m,
                                SmallBusinessIncentive = 1.0m
                            }
                        }
                    }
                }
            };

            var larsStandards = new List<ReferenceDataService.Model.LARS.LARSStandard>
            {
                new ReferenceDataService.Model.LARS.LARSStandard
                {
                    StandardCode = 1,
                    StandardSectorCode = "1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = new DateTime(2018, 9, 1),
                    LARSStandardApprenticeshipFundings = new List<ReferenceDataService.Model.LARS.LARSStandardApprenticeshipFunding>
                    {
                        new ReferenceDataService.Model.LARS.LARSStandardApprenticeshipFunding
                        {
                            BandNumber = 1,
                            CareLeaverAdditionalPayment = 1.0m,
                            FundingCategory = "FundingCategory",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            CoreGovContributionCap = 1.0m,
                            SixteenToEighteenIncentive = 1.0m,
                            SixteenToEighteenProviderAdditionalPayment = 1.0m,
                            SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                            SixteenToEighteenFrameworkUplift = 1.0m,
                            Duration = 1.0m,
                            ReservedValue2 = 1.0m,
                            ReservedValue3 = 1.0m,
                            MaxEmployerLevyCap = 1.0m,
                            FundableWithoutEmployer = "Fundable"
                        }
                    },
                    LARSStandardCommonComponents = new List<ReferenceDataService.Model.LARS.LARSStandardCommonComponent>
                    {
                        new ReferenceDataService.Model.LARS.LARSStandardCommonComponent
                        {
                            CommonComponent = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        }
                    },
                },
                new ReferenceDataService.Model.LARS.LARSStandard
                {
                    StandardCode = 2,
                    StandardSectorCode = "2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LARSStandardApprenticeshipFundings = new List<ReferenceDataService.Model.LARS.LARSStandardApprenticeshipFunding>
                    {
                        new ReferenceDataService.Model.LARS.LARSStandardApprenticeshipFunding
                        {
                            BandNumber = 1,
                            CareLeaverAdditionalPayment = 1.0m,
                            FundingCategory = "FundingCategory",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            CoreGovContributionCap = 1.0m,
                            SixteenToEighteenIncentive = 1.0m,
                            SixteenToEighteenProviderAdditionalPayment = 1.0m,
                            SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                            SixteenToEighteenFrameworkUplift = 1.0m,
                            Duration = 1.0m,
                            ReservedValue2 = 1.0m,
                            ReservedValue3 = 1.0m,
                            MaxEmployerLevyCap = 1.0m,
                            FundableWithoutEmployer = "Fundable"
                        }
                    },
                    LARSStandardFundings = new List<ReferenceDataService.Model.LARS.LARSStandardFunding>
                    {
                        new ReferenceDataService.Model.LARS.LARSStandardFunding
                        {
                            FundingCategory = "FundingCategory",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            AchievementIncentive = 1.0m,
                            BandNumber = 1,
                            CoreGovContributionCap = 1.0m,
                            FundableWithoutEmployer = "Funadable",
                            SixteenToEighteenIncentive = 1.0m,
                            SmallBusinessIncentive = 1.0m
                        }
                    }
                }
            };

            expectedlarsStandards.Should().BeEquivalentTo(NewService().MapLARSStandards(larsStandards));
        }

        [Fact]
        public void MapLARSStandards_Null()
        {
            NewService().MapLARSStandards(null).Should().BeNull();
        }

        private LARSMapperService NewService()
        {
            return new LARSMapperService();
        }
    }
}
