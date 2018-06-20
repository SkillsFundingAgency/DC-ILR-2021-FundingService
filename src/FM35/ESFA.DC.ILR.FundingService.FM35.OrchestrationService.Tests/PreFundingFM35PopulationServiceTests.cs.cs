using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ESFA.DC.Data.LargeEmployer.Model;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.DateTime.Provider;
using ESFA.DC.DateTime.Provider.Interface;
using ESFA.DC.ILR.FundingService.ALB.ExternalData.LARS;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.FM35.Contexts;
using ESFA.DC.ILR.FundingService.FM35.Contexts.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service.Interface;
using ESFA.DC.ILR.FundingService.FM35.Service.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface.Builders;
using ESFA.DC.ILR.FundingService.Tests.Common;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Dictionary;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.Serialization.Xml;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Tests
{
    public class PreFundingFM35PopulationServiceTests
    {
        /// <summary>
        /// Return PreFundingFM35PopulationService
        /// </summary>
        [Fact(DisplayName = "PreFundingOrchestration - Instance Exists"), Trait("PreFundingOrchestration", "Unit")]
        public void PreFundingALBPopulationService_Exists()
        {
            // ARRANGE
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180118-023456-02.xml");
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();

            // ACT
            var preFundingALBPopulationService = SetupPreFundingFM35PopulationService(message, referenceDataCache);

            // ASSERT
            preFundingALBPopulationService.Should().NotBeNull();
        }

        /// <summary>
        /// Populate reference data cache and check values
        /// </summary>
        [Fact(DisplayName = "PopulateReferenceData - LARS Version Correct"), Trait("PreFundingOrchestration", "Unit")]
        public void PopulateReferenceData_LARSVersion_Correct()
        {
            // ARRANGE
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180118-023456-02.xml");
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            var preFundingALBPopulationService = SetupPreFundingFM35PopulationService(message, referenceDataCache);

            // ACT
            preFundingALBPopulationService.PopulateData();

            // ASSERT
            referenceDataCache.LARSCurrentVersion.Should().Be("Version_005");
        }

        /// <summary>
        /// Populate reference data cache and check values
        /// </summary>
        [Fact(DisplayName = "PopulateReferenceData - LARS LearningDelivery Correct"), Trait("PreFundingOrchestration", "Unit")]
        public void PopulateReferenceData_LARSVLearningDelivery_Correct()
        {
            // ARRANGE
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180118-023456-02.xml");
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            var preFundingALBPopulationService = SetupPreFundingFM35PopulationService(message, referenceDataCache);

            // ACT
            preFundingALBPopulationService.PopulateData();

            // ASSERT
            var expectedOutput1 = new LARSLearningDelivery
            {
                LearnAimRef = "50094488",
                EnglandFEHEStatus = "England",
                EnglPrscID = 100,
                FrameworkCommonComponent = 1,
            };

            var expectedOutput2 = new LARSLearningDelivery
            {
                LearnAimRef = "60005415",
                EnglandFEHEStatus = "England",
                EnglPrscID = 100,
                FrameworkCommonComponent = 1,
            };

            var output1 = referenceDataCache.LARSLearningDelivery.Where(k => k.Key == "50094488").Select(o => o.Value);
            var output2 = referenceDataCache.LARSLearningDelivery.Where(k => k.Key == "60005415").Select(o => o.Value);

            output1.FirstOrDefault().Should().BeEquivalentTo(expectedOutput1);
            output2.FirstOrDefault().Should().BeEquivalentTo(expectedOutput2);
        }

        /// <summary>
        /// Populate reference data cache and check values
        /// </summary>
        [Fact(DisplayName = "PopulateReferenceData - LARS Funding Correct"), Trait("PreFundingOrchestration", "Unit")]
        public void PopulateReferenceData_LARSFunding_Correct()
        {
            // ARRANGE
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180118-023456-02.xml");
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            var preFundingALBPopulationService = SetupPreFundingFM35PopulationService(message, referenceDataCache);

            // ACT
            preFundingALBPopulationService.PopulateData();

            // ASSERT
            var expectedOutput1 = new LARSFunding
            {
                LearnAimRef = "50094488",
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                FundingCategory = "Matrix",
                WeightingFactor = "G",
                RateWeighted = 11356m
            };

            var expectedOutput2 = new LARSFunding
            {
                LearnAimRef = "60005415",
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                FundingCategory = "Matrix",
                WeightingFactor = "C",
                RateWeighted = 2583m
            };

            var output1 = referenceDataCache.LARSFunding.Where(k => k.Key == "50094488").Select(o => o.Value).SingleOrDefault();
            var output2 = referenceDataCache.LARSFunding.Where(k => k.Key == "60005415").SelectMany(o => o.Value).SingleOrDefault();

            output1.Should().BeEquivalentTo(expectedOutput1);
            output2.Should().BeEquivalentTo(expectedOutput2);
        }

        /// <summary>
        /// Populate reference data cache and check values
        /// </summary>
        [Fact(DisplayName = "PopulateReferenceData - Postcodes Version Correct"), Trait("Funding Service", "Unit")]
        public void PopulateReferenceData_Postcodes_Correct()
        {
            // ARRANGE
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180118-023456-02.xml");
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            var preFundingALBPopulationService = SetupPreFundingFM35PopulationService(message, referenceDataCache);

            // ACT
            preFundingALBPopulationService.PopulateData();

            // ASSERT
            referenceDataCache.PostcodeCurrentVersion.Should().Be("Version_002");
        }

        /// <summary>
        /// Populate reference data cache and check values
        /// </summary>
        [Fact(DisplayName = "PopulateReferenceData - Postcodes SFA AreaCost Correct"), Trait("PreFundingOrchestration", "Unit")]
        public void PopulateReferenceData_PostcodesSFAAreaCost_Correct()
        {
            // ARRANGE
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180118-023456-02.xml");
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            var preFundingALBPopulationService = SetupPreFundingFM35PopulationService(message, referenceDataCache);

            // ACT
            preFundingALBPopulationService.PopulateData();

            // ASSERT
            var expectedOutput1 = new SfaAreaCost
            {
                Postcode = "CV1 2WT",
                AreaCostFactor = 1.2m,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };

            var output = referenceDataCache.SfaAreaCost.Where(k => k.Key == "CV1 2WT").SelectMany(o => o.Value).FirstOrDefault();

            output.Should().BeEquivalentTo(expectedOutput1);
        }

        /// <summary>
        /// Populate reference data cache and check values
        /// </summary>
        [Fact(DisplayName = "PopulateReferenceData - Org Version Correct"), Trait("PreFundingOrchestration", "Unit")]
        public void PopulateReferenceData_OrgVersion_Correct()
        {
            // ARRANGE
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180118-023456-02.xml");
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            var preFundingALBPopulationService = SetupPreFundingFM35PopulationService(message, referenceDataCache);

            // ACT
            preFundingALBPopulationService.PopulateData();

            // ASSERT
            referenceDataCache.OrgVersion.Should().Be("Version_005");
        }

        private IOPAService OpaService()
        {
            return
             new OPAService(new SessionBuilder(), new OPADataEntityBuilder(new System.DateTime(2017, 8, 1)), MockRulebaseProviderFactory());
        }

        private PreFundingFM35PopulationService SetupPreFundingFM35PopulationService(IMessage message, IReferenceDataCache referenceDataCache)
        {
            IFundingContext fundingContext = SetupFundingContext(message);
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(referenceDataCache);
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(referenceDataCache);
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(referenceDataCache);
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(referenceDataCache);

            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();

            var dataEntityBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            var referenceDataCachePopulationService = new ReferenceDataCachePopulationService(referenceDataCache, LARSMock().Object, PostcodesMock().Object, OrganisationMock().Object, LargeEmployersMock().Object);
            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
            IFundingOutputService fundingOutputService = new FundingOutputService(dateTimeProvider);
            IInternalDataCache validALBLearnersCache = new InternalDataCache();

            var fundingService = new Service.FundingService(dataEntityBuilder, OpaService(), fundingOutputService);

            return new PreFundingFM35PopulationService(referenceDataCachePopulationService, fundingContext, validALBLearnersCache);
        }

        private Mock<ILARS> LARSMock()
        {
            Mock<ILARS> larsContextMock = new Mock<ILARS>();

            var larsVersionMock = MockLARSVersionArray().AsMockDbSet();
            var larsLearningDeliveryMock = MockLARSLearningDeliveryArray().AsMockDbSet();
            var larsFundingMock = MockLARSFundingArray().AsMockDbSet();
            var larsAnnualValueMock = MockLARSAnnualValueArray().AsMockDbSet();
            var larsCategoryMock = MockLARSCategoryArray().AsMockDbSet();
            var larsFrameworkAimsMock = MockLARSFrameworkAimsArray().AsMockDbSet();

            larsContextMock.Setup(x => x.LARS_Version).Returns(larsVersionMock);
            larsContextMock.Setup(x => x.LARS_LearningDelivery).Returns(larsLearningDeliveryMock);
            larsContextMock.Setup(x => x.LARS_Funding).Returns(larsFundingMock);
            larsContextMock.Setup(x => x.LARS_AnnualValue).Returns(larsAnnualValueMock);
            larsContextMock.Setup(x => x.LARS_LearningDeliveryCategory).Returns(larsCategoryMock);
            larsContextMock.Setup(x => x.LARS_FrameworkAims).Returns(larsFrameworkAimsMock);

            return larsContextMock;
        }

        private Mock<IPostcodes> PostcodesMock()
        {
            Mock<IPostcodes> postcodesContextMock = new Mock<IPostcodes>();

            var postcodesVersionMock = MockPostcodesVersionArray().AsMockDbSet();
            var sfaAreaCostMock = MockSFAAreaCostArray().AsMockDbSet();
            var sfaDisadvantageMock = MockSFADisadvantageArray().AsMockDbSet();

            postcodesContextMock.Setup(x => x.SFA_PostcodeAreaCost).Returns(sfaAreaCostMock);
            postcodesContextMock.Setup(x => x.VersionInfos).Returns(postcodesVersionMock);
            postcodesContextMock.Setup(x => x.SFA_PostcodeDisadvantage).Returns(sfaDisadvantageMock);

            return postcodesContextMock;
        }

        private Mock<IOrganisations> OrganisationMock()
        {
            Mock<IOrganisations> organisationContextMock = new Mock<IOrganisations>();

            var orgVersionMock = MockOrgVersionArray().AsMockDbSet();
            var orgFundingMock = MockOrgFundingArray().AsMockDbSet();

            organisationContextMock.Setup(x => x.Org_Version).Returns(orgVersionMock);
            organisationContextMock.Setup(x => x.Org_Funding).Returns(orgFundingMock);

            return organisationContextMock;
        }

        private Mock<ILargeEmployer> LargeEmployersMock()
        {
            Mock<ILargeEmployer> largeEmployersContextMock = new Mock<ILargeEmployer>();
            var largeEmployerMock = MockLargeEmployerArray().AsMockDbSet();

            largeEmployersContextMock.Setup(x => x.LEMP_Employers).Returns(largeEmployerMock);

            return largeEmployersContextMock;
        }

        private static LARS_Version[] MockLARSVersionArray()
        {
            return new LARS_Version[]
            {
                LarsVersionTestValue(),
            };
        }

        private static LARS_Version LarsVersionTestValue()
        {
            return new LARS_Version()
            {
                MajorNumber = 5,
                MinorNumber = 0,
                MaintenanceNumber = 0,
                MainDataSchemaName = "Version_005",
                RefDataSchemaName = "REF_Version_005",
                ActivationDate = System.DateTime.Parse("2017-07-01"),
                ExpiryDate = null,
                Description = "Fifth Version of LARS",
                Comment = null,
                Created_On = System.DateTime.Parse("2017-07-01"),
                Created_By = "System",
                Modified_On = System.DateTime.Parse("2018-07-01"),
                Modified_By = "System"
            };
        }

        private static LARS_LearningDelivery[] MockLARSLearningDeliveryArray()
        {
            return new LARS_LearningDelivery[]
            {
                LarsLearningDeliveryTestValue1(),
                LarsLearningDeliveryTestValue2(),
                LarsLearningDeliveryTestValue3(),
            };
        }

        private static LARS_LearningDelivery LarsLearningDeliveryTestValue1()
        {
            return new LARS_LearningDelivery()
            {
                LearnAimRef = "60133533",
                EnglandFEHEStatus = "England",
                EnglPrscID = 100,
                FrameworkCommonComponent = 1,
                LearnAimRefTitle = "Test Learning Aim Title 60133533",
                LearnAimRefType = "0006",
                NotionalNVQLevel = "2",
                NotionalNVQLevelv2 = "2",
                CertificationEndDate = System.DateTime.Parse("2018-01-01"),
                OperationalStartDate = System.DateTime.Parse("2018-01-01"),
                OperationalEndDate = System.DateTime.Parse("2018-01-01"),
                RegulatedCreditValue = 180,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = System.DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = System.DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };
        }

        private static LARS_LearningDelivery LarsLearningDeliveryTestValue2()
        {
            return new LARS_LearningDelivery()
            {
                LearnAimRef = "50104767",
                EnglandFEHEStatus = "England",
                EnglPrscID = 100,
                FrameworkCommonComponent = 1,
                LearnAimRefTitle = "Test Learning Aim Title 50104767",
                LearnAimRefType = "0006",
                NotionalNVQLevel = "2",
                NotionalNVQLevelv2 = "2",
                CertificationEndDate = System.DateTime.Parse("2018-01-01"),
                OperationalStartDate = System.DateTime.Parse("2018-01-01"),
                OperationalEndDate = System.DateTime.Parse("2018-01-01"),
                RegulatedCreditValue = 180,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = System.DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = System.DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };
        }

        private static LARS_LearningDelivery LarsLearningDeliveryTestValue3()
        {
            return new LARS_LearningDelivery()
            {
                LearnAimRef = "60126334",
                EnglandFEHEStatus = "England",
                EnglPrscID = 100,
                FrameworkCommonComponent = 1,
                LearnAimRefTitle = "Test Learning Aim Title 50104767",
                LearnAimRefType = "0006",
                NotionalNVQLevel = "2",
                NotionalNVQLevelv2 = "2",
                CertificationEndDate = System.DateTime.Parse("2018-01-01"),
                OperationalStartDate = System.DateTime.Parse("2018-01-01"),
                OperationalEndDate = System.DateTime.Parse("2018-01-01"),
                RegulatedCreditValue = 180,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = System.DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = System.DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };
        }

        private static LARS_Funding[] MockLARSFundingArray()
        {
            return new LARS_Funding[]
            {
                LarsFundingTestValue1(),
                LarsFundingTestValue2(),
                LarsFundingTestValue3(),
            };
        }

        private static LARS_Funding LarsFundingTestValue1()
        {
            return new LARS_Funding()
            {
                LearnAimRef = "60133533",
                FundingCategory = "Matrix",
                RateWeighted = 11356m,
                RateUnWeighted = null,
                WeightingFactor = "G",
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = System.DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = System.DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };
        }

        private static LARS_Funding LarsFundingTestValue2()
        {
            return new LARS_Funding()
            {
                LearnAimRef = "50104767",
                FundingCategory = "Matrix",
                RateWeighted = 11356m,
                RateUnWeighted = null,
                WeightingFactor = "G",
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = System.DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = System.DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };
        }

        private static LARS_Funding LarsFundingTestValue3()
        {
            return new LARS_Funding()
            {
                LearnAimRef = "60126334",
                FundingCategory = "Matrix",
                RateWeighted = 11356m,
                RateUnWeighted = null,
                WeightingFactor = "G",
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
                Created_On = System.DateTime.Parse("2017-01-01"),
                Created_By = "TestUser",
                Modified_On = System.DateTime.Parse("2018-01-01"),
                Modified_By = "TestUser"
            };
        }

        private static LARS_LearningDeliveryCategory[] MockLARSCategoryArray()
        {
            return new LARS_LearningDeliveryCategory[]
            {
                LarsCategoryTestValue(),
            };
        }

        private static LARS_LearningDeliveryCategory LarsCategoryTestValue()
        {
            return new LARS_LearningDeliveryCategory()
            {
                LearnAimRef = "60133533",
                CategoryRef = 1,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static LARS_AnnualValue[] MockLARSAnnualValueArray()
        {
            return new LARS_AnnualValue[]
            {
                LarsAnnualValueTestValue(),
            };
        }

        private static LARS_AnnualValue LarsAnnualValueTestValue()
        {
            return new LARS_AnnualValue()
            {
                LearnAimRef = "60133533",
                BasicSkillsType = 5,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static LARS_FrameworkAims[] MockLARSFrameworkAimsArray()
        {
            return new LARS_FrameworkAims[]
            {
                LarsFrameworkAimsTestValue1(),
                LarsFrameworkAimsTestValue2(),
                LarsFrameworkAimsTestValue3(),
            };
        }

        private static LARS_FrameworkAims LarsFrameworkAimsTestValue1()
        {
            return new LARS_FrameworkAims()
            {
                LearnAimRef = "60133533",
                FworkCode = 420,
                ProgType = 2,
                PwayCode = 1,
                FrameworkComponentType = 1,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static LARS_FrameworkAims LarsFrameworkAimsTestValue2()
        {
            return new LARS_FrameworkAims()
            {
                LearnAimRef = "50104767",
                FworkCode = 420,
                ProgType = 2,
                PwayCode = 1,
                FrameworkComponentType = 1,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static LARS_FrameworkAims LarsFrameworkAimsTestValue3()
        {
            return new LARS_FrameworkAims()
            {
                LearnAimRef = "60126334",
                FworkCode = 420,
                ProgType = 2,
                PwayCode = 1,
                FrameworkComponentType = 1,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static VersionInfo[] MockPostcodesVersionArray()
        {
            return new VersionInfo[]
            {
                PostcodesVersionTestValue(),
            };
        }

        private static VersionInfo PostcodesVersionTestValue()
        {
            return new VersionInfo
            {
                VersionNumber = "Version_002",
                DataSource = "Source",
                Comments = "Comments",
                ModifiedAt = System.DateTime.Parse("2018-01-01"),
                ModifiedBy = "System"
            };
        }

        private static SFA_PostcodeAreaCost[] MockSFAAreaCostArray()
        {
            return new SFA_PostcodeAreaCost[]
            {
                SFAAreaCostTestValue1(),
                SFAAreaCostTestValue2(),
                SFAAreaCostTestValue3(),
            };
        }

        private static SFA_PostcodeAreaCost SFAAreaCostTestValue1()
        {
            return new SFA_PostcodeAreaCost()
            {
                MasterPostcode = new MasterPostcode { Postcode = "ZZ99 9ZZ" },
                Postcode = "ZZ99 9ZZ",
                AreaCostFactor = 1.2m,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static SFA_PostcodeAreaCost SFAAreaCostTestValue2()
        {
            return new SFA_PostcodeAreaCost()
            {
                MasterPostcode = new MasterPostcode { Postcode = "AL1 1AA" },
                Postcode = "AL1 1AA",
                AreaCostFactor = 1.2m,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static SFA_PostcodeAreaCost SFAAreaCostTestValue3()
        {
            return new SFA_PostcodeAreaCost()
            {
                MasterPostcode = new MasterPostcode { Postcode = "B10 0BL" },
                Postcode = "B10 0BL",
                AreaCostFactor = 1.2m,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static SFA_PostcodeDisadvantage[] MockSFADisadvantageArray()
        {
            return new SFA_PostcodeDisadvantage[]
            {
                SFADisadvantageValue1(),
                SFADisadvantageValue2(),
            };
        }

        private static SFA_PostcodeDisadvantage SFADisadvantageValue1()
        {
            return new SFA_PostcodeDisadvantage()
            {
                MasterPostcode = new MasterPostcode { Postcode = "ZZ99 9ZZ" },
                Postcode = "ZZ99 9ZZ",
                Uplift = 1.2m,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static SFA_PostcodeDisadvantage SFADisadvantageValue2()
        {
            return new SFA_PostcodeDisadvantage()
            {
                MasterPostcode = new MasterPostcode { Postcode = "B10 0BL" },
                Postcode = "B10 0BL",
                Uplift = 1.2m,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };
        }

        private static Org_Version[] MockOrgVersionArray()
        {
            return new Org_Version[]
            {
                OrgVersionTestValue(),
            };
        }

        private static Org_Version OrgVersionTestValue()
        {
            return new Org_Version()
            {
                MainDataSchemaName = "Version_003",
            };
        }

        private static Org_Funding[] MockOrgFundingArray()
        {
            return new Org_Funding[]
            {
                OrgFundingTestValue(),
            };
        }

        private static Org_Funding OrgFundingTestValue()
        {
            return new Org_Funding
            {
                UKPRN = 10006341,
                FundingFactor = "Factor",
                FundingFactorType = "Adult Skills",
                FundingFactorValue = "1,54",
                EffectiveFrom = new System.DateTime(2018, 08, 01),
                EffectiveTo = new System.DateTime(2019, 07, 31),
            };
        }

        private static LEMP_Employers[] MockLargeEmployerArray()
        {
            return new LEMP_Employers[]
            {
                LargeEmployerTestValue(),
            };
        }

        private static LEMP_Employers LargeEmployerTestValue()
        {
            return new LEMP_Employers
            {
                ERN = 154549452,
                EffectiveFrom = new System.DateTime(2018, 08, 01),
                EffectiveTo = new System.DateTime(2019, 07, 31),
            };
        }

        private IFundingContext SetupFundingContext(IMessage message)
        {
            IKeyValuePersistenceService keyValuePersistenceService = BuildKeyValueDictionary(message);
            IXmlSerializationService serializationService = new XmlSerializationService();
            IFundingContextManager fundingContextManager = new FundingContextManager(JobContextMessage(), keyValuePersistenceService, serializationService);

            return new FundingContext(fundingContextManager);
        }

        private IFundingContext SetupFundingContext(IList<string> validLearners)
        {
            IKeyValuePersistenceService keyValuePersistenceService = new DictionaryKeyValuePersistenceService();
            IXmlSerializationService serializationService = new XmlSerializationService();
            IFundingContextManager fundingContextManager = new FundingContextManager(JobContextMessage(), keyValuePersistenceService, serializationService);

            return new FundingContext(fundingContextManager);
        }

        private static IRulebaseProvider RulebaseProviderMock()
        {
            return new RulebaseProvider(@"ESFA.DC.ILR.FundingService.ALB.Service.Rulebase.Loans Bursary 17_18.zip");
        }

        private static IRulebaseProviderFactory MockRulebaseProviderFactory()
        {
            var mock = new Mock<IRulebaseProviderFactory>();

            mock.Setup(m => m.Build()).Returns(RulebaseProviderMock());

            return mock.Object;
        }

        private static DictionaryKeyValuePersistenceService BuildKeyValueDictionary(IMessage message)
        {
            var learners = message.Learners.ToList();

            var list = new DictionaryKeyValuePersistenceService();
            var serializer = new JsonSerializationService();

            list.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();

            return list;
        }

        private static IReadOnlyList<ITopicItem> Topics()
        {
            return new List<TopicItem>();
        }

        private static IDictionary<JobContextMessageKey, object> KeyValuePairsDictionary()
        {
            return new Dictionary<JobContextMessageKey, object>()
            {
                { JobContextMessageKey.Filename, "FileName" },
                { JobContextMessageKey.UkPrn, 10006341 },
                { JobContextMessageKey.ValidLearnRefNumbers, "ValidLearnRefNumbers" },
            };
        }

        private static IJobContextMessage JobContextMessage()
        {
            return new JobContextMessage
            {
                JobId = 1,
                SubmissionDateTimeUtc = new System.DateTime(2018, 08, 01).ToUniversalTime(),
                Topics = Topics(),
                TopicPointer = 1,
                KeyValuePairs = KeyValuePairsDictionary(),
            };
        }

        private Message ILRFile(string filePath)
        {
            Message message;
            Stream stream = new FileStream(filePath, FileMode.Open);

            using (var reader = XmlReader.Create(stream))
            {
                var serializer = new XmlSerializer(typeof(Message));
                message = serializer.Deserialize(reader) as Message;
            }

            stream.Close();

            return message;
        }
    }
}
