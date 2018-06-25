using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LargeEmployer.Model;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population;
using ESFA.DC.ILR.FundingService.Tests.Common;
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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(learnAimRef, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(learnAimRef, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

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

        /// <summary>
        /// Return Postcodes Version Data from Postcodes database
        /// </summary>
        [Fact(DisplayName = "MockDB - Postcodes Version Data - Value Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_PostcodesVersionData_ValueCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

            // ASSERT
            output.PostcodeCurrentVersion.Should().Be("Version_002");
        }

        /// <summary>
        /// Return Postcodes SFAAreaCost Data from Postcodes database
        /// </summary>
        [Fact(DisplayName = "MockDB - Postcodes SFAAreaCost Data - Count Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_SFAAreaCostData_CountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

            // ASSERT
            output.SfaAreaCost.Count.Should().Be(2);
        }

        /// <summary>
        /// Return Postcodes SFAAreaCost Data from Postcodes database
        /// </summary>
        [Fact(DisplayName = "MockDB - Postcodes SFAAreaCost Data Keys Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_SFAAreaCostData_KeysCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

            // ASSERT
            output.SfaAreaCost.Where(k => k.Key == "CV1 2WT").Select(o => o.Key).Should().BeEquivalentTo("CV1 2WT");
            output.SfaAreaCost.Where(k => k.Key == "CV1 2TT").Select(o => o.Key).Should().BeEquivalentTo("CV1 2TT");
            output.SfaAreaCost.Where(k => k.Key == "CV1 2AA").Select(o => o.Key).Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Postcodes SFAAreaCost Data from Postcodes database
        /// </summary>
        [Fact(DisplayName = "MockDB - Postcodes SFAAreaCost Data Keys Not Found"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_SFAAreaCostData_KeysNotFound()
        {
            // ARRANGE
            var postcode = new List<string>
            {
                "CV1 1AA"
            };

            // ACT
            var output = MockDBOutput(LearnAimRefList, postcode);

            // ASSERT
            output.LARSFunding.Where(k => k.Key == "CV1 2WT").Select(o => o.Key).Should().BeNullOrEmpty();
            output.LARSFunding.Where(k => k.Key == "CV1 2TT").Select(o => o.Key).Should().BeNullOrEmpty();
        }

        /// <summary>
        /// Return Postcodes SFAAreaCost Data from Postcodes database
        /// </summary>
        [Fact(DisplayName = "MockDB - Postcodes SFAAreaCost Data - Data Count Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_SFAAreaCostData_DataCountCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

            // ASSERT
            output.SfaAreaCost.Where(k => k.Key == "CV1 2WT").Select(o => o.Value).Count().Should().Be(1);
            output.SfaAreaCost.Where(k => k.Key == "CV1 2WT").SelectMany(o => o.Value).Count().Should().Be(1);
            output.SfaAreaCost.Where(k => k.Key == "CV1 2TT").Select(o => o.Value).Count().Should().Be(1);
            output.SfaAreaCost.Where(k => k.Key == "CV1 2TT").SelectMany(o => o.Value).Count().Should().Be(2);
        }

        /// <summary>
        /// Return Postcodes SFAAreaCost Data from Postcodes database
        /// </summary>
        [Fact(DisplayName = "MockDB - Postcodes SFAAreaCost Data - Data Values Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_SFAAreaCostData_DataValuesCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput(LearnAimRefList, PostcodesList);

            // ASSERT
            var expectedOutput1 = new SfaAreaCost
            {
                Postcode = "CV1 2WT",
                AreaCostFactor = 1.2m,
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };

            var expectedOutput2 = new SfaAreaCost
            {
                Postcode = "CV1 2TT",
                AreaCostFactor = 1.5m,
                EffectiveFrom = DateTime.Parse("2000-01-01"),
                EffectiveTo = DateTime.Parse("2015-12-31")
            };

            var expectedOutput3 = new SfaAreaCost
            {
                Postcode = "CV1 2TT",
                AreaCostFactor = 2.1m,
                EffectiveFrom = DateTime.Parse("2016-01-01"),
                EffectiveTo = null,
            };

            var output1 = output.SfaAreaCost.Where(k => k.Key == "CV1 2WT").SelectMany(o => o.Value);
            var output3 = output.SfaAreaCost.Where(k => k.Key == "CV1 2TT").SelectMany(o => o.Value);

            output1.FirstOrDefault().Should().BeEquivalentTo(expectedOutput1);
            output3.FirstOrDefault().Should().BeEquivalentTo(expectedOutput2);
            output3.Skip(1).Single().Should().BeEquivalentTo(expectedOutput3);
        }
        
        #region Test Helpers

        private IReferenceDataCache MockDBOutput(IEnumerable<string> learnAimRefs = null, IEnumerable<string> postcodes = null, IEnumerable<long> orgUkprns = null, IEnumerable<int> lEmpIDs = null)
        {
            var larsMock = new Mock<ILARS>();
            var postcodesMock = new Mock<IPostcodes>();
            var organisationsMock = new Mock<IOrganisations>();
            var largeEmployersMock = new Mock<ILargeEmployer>();

            learnAimRefs = learnAimRefs ?? new List<string>();
            postcodes = postcodes ?? new List<string>();
            orgUkprns = orgUkprns ?? new List<long>();
            lEmpIDs = lEmpIDs ?? new List<int>();
            
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();

            larsMock.Setup(x => x.LARS_Version).Returns(MockLARSVersionArray().AsMockDbSet());
            larsMock.Setup(x => x.LARS_LearningDelivery).Returns(MockLARSLearningDeliveryArray().AsMockDbSet());
            larsMock.Setup(x => x.LARS_Funding).Returns(MockLARSFundingArray().AsMockDbSet());
            larsMock.Setup(x => x.LARS_AnnualValue).Returns(new List<LARS_AnnualValue>().AsMockDbSet);
            larsMock.Setup(x => x.LARS_LearningDeliveryCategory).Returns(new List<LARS_LearningDeliveryCategory>().AsMockDbSet);
            larsMock.Setup(x => x.LARS_FrameworkAims).Returns(new List<LARS_FrameworkAims>().AsMockDbSet());

            postcodesMock.Setup(x => x.SFA_PostcodeAreaCost).Returns(MockSFAAreaCostArray().AsMockDbSet());
            postcodesMock.Setup(x => x.VersionInfos).Returns(MockPostcodesVersionArray().AsMockDbSet());
            postcodesMock.Setup(x => x.SFA_PostcodeDisadvantage).Returns(new List<SFA_PostcodeDisadvantage>().AsMockDbSet());

            organisationsMock.Setup(x => x.Org_Version).Returns(new List<Org_Version>() { new Org_Version() { MainDataSchemaName = "test" }}.AsMockDbSet());
            organisationsMock.Setup(x => x.Org_Funding).Returns(new List<Org_Funding>().AsMockDbSet());

            largeEmployersMock.Setup(x => x.LEMP_Employers).Returns(new List<LEMP_Employers>().AsMockDbSet());

            var service = NewReferenceDataCachePopulationService(referenceDataCache, larsMock.Object, postcodesMock.Object, organisationsMock.Object, largeEmployersMock.Object);
            service.Populate(learnAimRefs, postcodes, orgUkprns, lEmpIDs);

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

        private static VersionInfo[] MockPostcodesVersionArray()
        {
            return new VersionInfo[]
            {
                PostcodesVersionTestValue,
            };
        }

        private static readonly VersionInfo PostcodesVersionTestValue =
            new VersionInfo
            {
                VersionNumber = "Version_002",
                DataSource = "Source",
                Comments = "Comments",
                ModifiedAt = DateTime.Parse("2018-01-01"),
                ModifiedBy = "System"
            };

        private static SFA_PostcodeAreaCost[] MockSFAAreaCostArray()
        {
            return new SFA_PostcodeAreaCost[]
            {
                SFAAreaCostTestValue1,
                SFAAreaCostTestValue2,
                SFAAreaCostTestValue3
            };
        }

        private static readonly SFA_PostcodeAreaCost SFAAreaCostTestValue1 =
          new SFA_PostcodeAreaCost()
          {
              MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
              Postcode = "CV1 2WT",
              AreaCostFactor = 1.2m,
              EffectiveFrom = DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
          };

        private static readonly SFA_PostcodeAreaCost SFAAreaCostTestValue2 =
         new SFA_PostcodeAreaCost()
         {
             MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
             Postcode = "CV1 2TT",
             AreaCostFactor = 1.5m,
             EffectiveFrom = DateTime.Parse("2000-01-01"),
             EffectiveTo = DateTime.Parse("2015-12-31")
         };

        private static readonly SFA_PostcodeAreaCost SFAAreaCostTestValue3 =
        new SFA_PostcodeAreaCost()
        {
            MasterPostcode = new MasterPostcode { Postcode = "CV1 2TT" },
            Postcode = "CV1 2TT",
            AreaCostFactor = 2.1m,
            EffectiveFrom = DateTime.Parse("2016-01-01"),
            EffectiveTo = null,
        };

        #endregion

        #endregion

        private ReferenceDataCachePopulationService NewReferenceDataCachePopulationService(
            IReferenceDataCache referenceDataCache = null,
            ILARS lars = null,
            IPostcodes postcodes = null,
            IOrganisations organisations = null,
            ILargeEmployer largeEmployers = null)
        {
            return new ReferenceDataCachePopulationService(referenceDataCache, lars, postcodes, organisations, largeEmployers);
        }
    }
}
