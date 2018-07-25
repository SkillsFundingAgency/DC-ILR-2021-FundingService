using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.Tests.Common;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.ALB.ExternalData.Tests
{
    public class ReferenceDataCachePopulationServiceTests
    {
        /// <summary>
        /// Return LARS Version Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS Version Data - Value Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSVersionData_ValueCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            output.LARSCurrentVersion.Should().Be("Version_005");
        }

        /// <summary>
        /// Return LARS LearningDelivery Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS LearningDelivery Data - Count Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSLearningDelivery_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            output.LARSLearningDelivery.Count.Should().Be(2);
        }

        /// <summary>
        /// Return LARS LearningDelivery Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS LearningDelivery Data - Keys Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSLearningDelivery_KeysCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            output.LARSLearningDelivery.Where(k => k.Key == "123456").Select(o => o.Key).Should().BeEquivalentTo("123456");
            output.LARSLearningDelivery.Where(k => k.Key == "7890").Select(o => o.Key).Should().BeEquivalentTo("7890");
            output.LARSLearningDelivery.Where(k => k.Key == "999").Select(o => o.Key).Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return LARS LearningDelivery Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS LearningDelivery Data - Keys Not Found"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSLARSLearningDelivery_KeysNotFound()
        {
            // ARRANGE
            var learnAimRef = new List<string>
            {
                "99999"
            };

            // ACT
            var output = MockDBOutput(learnAimRef);

            // ASSERT
            output.LARSLearningDelivery.Where(k => k.Key == "123456").Select(o => o.Key).Should().BeNullOrEmpty();
            output.LARSLearningDelivery.Where(k => k.Key == "99999").Select(o => o.Key).Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return LARS LearningDelivery Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS LearningDelivery Data - Data Count Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSLearningDelivery_DataCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            output.LARSLearningDelivery.Where(k => k.Key == "123456").Select(o => o.Value).Count().Should().Be(1);
            output.LARSLearningDelivery.Where(k => k.Key == "7890").Select(o => o.Value).Count().Should().Be(1);
        }

        /// <summary>
        /// Return LARS LearningDelivery Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS LearningDelivery Data - Data Values Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSLearningDelivery_DataValuesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            var expectedOutput1 = new LARSLearningDelivery
            {
                LearnAimRef = "123456",
                LearnAimRefType = "0030",
                NotionalNVQLevelv2 = "2",
                RegulatedCreditValue = 180
            };

            var expectedOutput2 = new LARSLearningDelivery
            {
                LearnAimRef = "7890",
                LearnAimRefType = "0032",
                NotionalNVQLevelv2 = "4",
                RegulatedCreditValue = 100
            };

            var output1 = output.LARSLearningDelivery.Where(k => k.Key == "123456").Select(o => o.Value);
            var output2 = output.LARSLearningDelivery.Where(k => k.Key == "7890").Select(o => o.Value);

            output1.FirstOrDefault().Should().BeEquivalentTo(expectedOutput1);
            output2.FirstOrDefault().Should().BeEquivalentTo(expectedOutput2);
        }

        /// <summary>
        /// Return LARS Funding Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS Funding Data - Count Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSFundingData_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            output.LARSFunding.Count.Should().Be(2);
        }

        /// <summary>
        /// Return LARS Funding Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS Funding Data - Keys Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSFundingData_KeysCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            output.LARSFunding.Where(k => k.Key == "123456").Select(o => o.Key).Should().BeEquivalentTo("123456");
            output.LARSFunding.Where(k => k.Key == "7890").Select(o => o.Key).Should().BeEquivalentTo("7890");
            output.LARSFunding.Where(k => k.Key == "999").Select(o => o.Key).Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return LARS Funding Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS Funding Data - Keys Not Found"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSFundingData_KeysNotFound()
        {
            // ARRANGE
            var learnAimRef = new List<string>
            {
                "99999"
            };

            // ACT
            var output = MockDBOutput(learnAimRef);

            // ASSERT
            output.LARSFunding.Where(k => k.Key == "123456").Select(o => o.Key).Should().BeNullOrEmpty();
            output.LARSFunding.Where(k => k.Key == "99999").Select(o => o.Key).Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return LARS Funding Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS Funding Data - Data Count Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSFundingData_DataCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            output.LARSFunding.Where(k => k.Key == "123456").Select(o => o.Value).Count().Should().Be(1);
            output.LARSFunding.Where(k => k.Key == "123456").SelectMany(o => o.Value).Count().Should().Be(2);
            output.LARSFunding.Where(k => k.Key == "7890").Select(o => o.Value).Count().Should().Be(1);
            output.LARSFunding.Where(k => k.Key == "7890").SelectMany(o => o.Value).Count().Should().Be(1);
        }

        /// <summary>
        /// Return LARS Funding Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS Funding Data - Data Values Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSFundingData_DataValuesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList);

            // ASSERT
            var expectedOutput1 = new LARSFunding
            {
                LearnAimRef = "123456",
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                FundingCategory = "Matrix",
                WeightingFactor = "W-Factor",
                RateWeighted = 1.5m
            };

            var expectedOutput2 = new LARSFunding
            {
                LearnAimRef = "123456",
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                FundingCategory = "ADULT_ILR",
                WeightingFactor = "W-Factor",
                RateWeighted = 1.5m
            };

            var expectedOutput3 = new LARSFunding
            {
                LearnAimRef = "7890",
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                FundingCategory = "Matrix",
                WeightingFactor = "W-Factor",
                RateWeighted = 2.5m
            };

            var output1 = output.LARSFunding.Where(k => k.Key == "123456").SelectMany(o => o.Value);
            var output3 = output.LARSFunding.Where(k => k.Key == "7890").SelectMany(o => o.Value);

            output1.FirstOrDefault().Should().BeEquivalentTo(expectedOutput1);
            output1.Skip(1).Single().Should().BeEquivalentTo(expectedOutput2);
            output3.FirstOrDefault().Should().BeEquivalentTo(expectedOutput3);
        }


        #region Test Helpers

        private IExternalDataCache MockDBOutput(IEnumerable<string> learnAimRefs = null,  IEnumerable<long> orgUkprns = null, IEnumerable<int> lEmpIDs = null)
        {
            var larsMock = new Mock<ILARS>();
            var organisationsMock = new Mock<IOrganisations>();

            learnAimRefs = learnAimRefs ?? new List<string>();
            orgUkprns = orgUkprns ?? new List<long>();
            lEmpIDs = lEmpIDs ?? new List<int>();
            
            IExternalDataCache referenceDataCache = new ExternalDataCache();

            larsMock.Setup(x => x.LARS_Version).Returns(MockLARSVersionArray().AsMockDbSet());
            larsMock.Setup(x => x.LARS_LearningDelivery).Returns(MockLARSLearningDeliveryArray().AsMockDbSet());
            larsMock.Setup(x => x.LARS_Funding).Returns(MockLARSFundingArray().AsMockDbSet());
            larsMock.Setup(x => x.LARS_AnnualValue).Returns(new List<LARS_AnnualValue>().AsMockDbSet);
            larsMock.Setup(x => x.LARS_LearningDeliveryCategory).Returns(new List<LARS_LearningDeliveryCategory>().AsMockDbSet);
            larsMock.Setup(x => x.LARS_FrameworkAims).Returns(new List<LARS_FrameworkAims>().AsMockDbSet());
            
            organisationsMock.Setup(x => x.Org_Version).Returns(new List<Org_Version>() { new Org_Version() { MainDataSchemaName = "test" }}.AsMockDbSet());
            organisationsMock.Setup(x => x.Org_Funding).Returns(new List<Org_Funding>().AsMockDbSet());


            var fundingServiceDtoMock = new Mock<IFundingServiceDto>();

            fundingServiceDtoMock
                .SetupGet(m => m.Message)
                .Returns(
                    new TestMessage()
                    {
                        LearningProviderEntity = new TestLearningProvider()
                        {
                            UKPRN = 1
                        },
                        Learners = new List<TestLearner>()
                    });

            var postcodesDataRetrievalServiceMock = new Mock<IPostcodesDataRetrievalService>();
            var largeEmployersDataRetrievalServiceMock = new Mock<ILargeEmployersDataRetrievalService>();

            var service = NewService(referenceDataCache, postcodesDataRetrievalServiceMock.Object, largeEmployersDataRetrievalServiceMock.Object, larsMock.Object, organisationsMock.Object, fundingServiceDtoMock.Object);
            service.Populate();

            return referenceDataCache;
        }

        #region Test Data

        private static readonly IList<string> LearnAimRefList = new List<string>
        {
            "123456", "7890"
        };

        private static LARS_Version[] MockLARSVersionArray()
        {
            return new LARS_Version[]
            {
                LarsVersionTestValue,
            };
        }

        private static readonly LARS_Version LarsVersionTestValue =
            new LARS_Version()
            {
                MajorNumber = 5,
                MinorNumber = 0,
                MaintenanceNumber = 0,
                MainDataSchemaName = "Version_005",
                RefDataSchemaName = "REF_Version_005",
                ActivationDate = DateTime.Parse("2017-07-01"),
                ExpiryDate = null,
                Description = "Fifth Version of LARS",
                Comment = null,
                Created_On = DateTime.Parse("2017-07-01"),
                Created_By = "System",
                Modified_On = DateTime.Parse("2018-07-01"),
                Modified_By = "System"
            };

        private static LARS_LearningDelivery[] MockLARSLearningDeliveryArray()
        {
            return new LARS_LearningDelivery[]
            {
                LarsLearningDeliveryTestValue1,
                LarsLearningDeliveryTestValue2
            };
        }

        private static readonly LARS_LearningDelivery LarsLearningDeliveryTestValue1 =
            new LARS_LearningDelivery()
            {
                LearnAimRef = "123456",
                LearnAimRefTitle = "Test Learning Aim Title 123456",
                LearnAimRefType = "0030",
                NotionalNVQLevel = "2",
                NotionalNVQLevelv2 = "2",
                CertificationEndDate = DateTime.Parse("2018-01-01"),
                OperationalStartDate = DateTime.Parse("2018-01-01"),
                OperationalEndDate = DateTime.Parse("2018-01-01"),
                RegulatedCreditValue = 180,
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };

        private static readonly LARS_LearningDelivery LarsLearningDeliveryTestValue2 =
           new LARS_LearningDelivery()
           {
               LearnAimRef = "7890",
               LearnAimRefTitle = "Test Learning Aim Title 7890",
               LearnAimRefType = "0032",
               NotionalNVQLevel = "4",
               NotionalNVQLevelv2 = "4",
               CertificationEndDate = DateTime.Parse("2018-01-01"),
               OperationalStartDate = DateTime.Parse("2018-01-01"),
               OperationalEndDate = DateTime.Parse("2018-01-01"),
               RegulatedCreditValue = 100,
               EffectiveFrom = DateTime.Parse("2000-01-01"),
               EffectiveTo = null,
               Created_On = DateTime.Parse("2017-01-01"),
               Created_By = "TestUser",
               Modified_On = DateTime.Parse("2018-01-01"),
               Modified_By = "TestUser"
           };

        private static LARS_Funding[] MockLARSFundingArray()
        {
            return new LARS_Funding[]
            {
                LarsFundingTestValue1,
                LarsFundingTestValue2,
                LarsFundingTestValue3
            };
        }

        private static readonly LARS_Funding LarsFundingTestValue1 =
            new LARS_Funding()
            {
                LearnAimRef = "123456",
                FundingCategory = "Matrix",
                RateWeighted = 1.5m,
                RateUnWeighted = null,
                WeightingFactor = "W-Factor",
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };

        private static readonly LARS_Funding LarsFundingTestValue2 =
          new LARS_Funding()
          {
              LearnAimRef = "7890",
              FundingCategory = "Matrix",
              RateWeighted = 2.5m,
              RateUnWeighted = null,
              WeightingFactor = "W-Factor",
              EffectiveFrom = DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
              Created_On = DateTime.Parse("2017-01-01"),
              Created_By = "TestUser",
              Modified_On = DateTime.Parse("2018-01-01"),
              Modified_By = "TestUser"
          };

        private static readonly LARS_Funding LarsFundingTestValue3 =
           new LARS_Funding()
           {
               LearnAimRef = "123456",
               FundingCategory = "ADULT_ILR",
               RateWeighted = 1.5m,
               RateUnWeighted = null,
               WeightingFactor = "W-Factor",
               EffectiveFrom = DateTime.Parse("2000-01-01"),
               EffectiveTo = null,
               Created_On = DateTime.Parse("2017-01-01"),
               Created_By = "TestUser",
               Modified_On = DateTime.Parse("2018-01-01"),
               Modified_By = "TestUser"
           };

        private static readonly IList<string> PostcodesList = new List<string>
        {
            "CV1 2WT", "CV1 2TT"
        };
        #endregion

        #endregion

        private ExternalDataCachePopulationService NewService(
            IExternalDataCache referenceDataCache = null,
            IPostcodesDataRetrievalService postcodesDataRetrievalService = null,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService = null,
            ILARS lars = null,
            IOrganisations organisations = null,
            IFundingServiceDto fundingServiceDto = null)
        {
            return new ExternalDataCachePopulationService(referenceDataCache, postcodesDataRetrievalService, largeEmployersDataRetrievalService, lars, organisations, fundingServiceDto);
        }
    }
}
