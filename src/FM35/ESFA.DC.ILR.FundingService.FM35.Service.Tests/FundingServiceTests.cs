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
using ESFA.DC.ILR.FundingService.FM35.Contexts;
using ESFA.DC.ILR.FundingService.FM35.Contexts.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Organisation.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Service.Interface;
using ESFA.DC.ILR.FundingService.FM35.InternalData;
using ESFA.DC.ILR.FundingService.FM35.InternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService;
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
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests
{
    public class FundingServiceTests
    {
        /// <summary>
        /// Return FundingOutputs from the Funding Service
        /// </summary>
        [Fact(DisplayName = "ProcessFunding - FundingOutput Exists"), Trait("Funding Service", "Unit")]
        public void ProcessFunding_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var fundingOutput = RunFundingService();

            // ASSERT
            fundingOutput.Should().NotBeNull();
        }

        private static readonly ISessionBuilder _sessionBuilder = new SessionBuilder();
        private static readonly IOPADataEntityBuilder _dataEntityBuilder = new OPADataEntityBuilder(new System.DateTime(2018, 8, 1));

        private readonly IOPAService opaService =
            new OPAService(_sessionBuilder, _dataEntityBuilder, MockRulebaseProviderFactory());

        private static IRulebaseProvider RulebaseProviderMock()
        {
            return new RulebaseProvider(@"ESFA.DC.ILR.FundingService.FM35.Service.Rulebase.FM35 Funding Calc 18_19.zip");
        }

        private static IRulebaseProviderFactory MockRulebaseProviderFactory()
        {
            var mock = new Mock<IRulebaseProviderFactory>();

            mock.Setup(m => m.Build()).Returns(RulebaseProviderMock());

            return mock.Object;
        }

        private IFM35FundingOutputs RunFundingService()
        {
            IMessage message = ILRFile(@"Files\ILR-10006341-1819-20180607-102124-03.xml");
            IFundingContext fundingContext = SetupFundingContext(message);
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(referenceDataCache);
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(referenceDataCache);
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(referenceDataCache);
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(referenceDataCache);

            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            IReferenceDataCachePopulationService referenceDataCachePopulationService = new ReferenceDataCachePopulationService(referenceDataCache, LARSMock().Object, PostcodesMock().Object, OrganisationMock().Object, LargeEmployersMock().Object);
            IDataEntityBuilder dataEntityBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);
            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
            IFundingOutputService fundingOutputService = new FundingOutputService(dateTimeProvider);
            IInternalDataCache validALBLearnersCache = new InternalDataCache();

            var fundingService = new FundingService(dataEntityBuilder, opaService, fundingOutputService);

            var preFundingOrchestrationService = new PreFundingFM35PopulationService(referenceDataCachePopulationService, fundingContext, validALBLearnersCache);
            preFundingOrchestrationService.PopulateData();

            return fundingService.ProcessFunding(fundingContext.UKPRN, validALBLearnersCache.ValidLearners);
        }

        private IFundingContext SetupFundingContext(IMessage message)
        {
            IKeyValuePersistenceService keyValuePersistenceService = BuildKeyValueDictionary(message);
            IXmlSerializationService serializationService = new XmlSerializationService();
            IFundingContextManager fundingContextManager = new FundingContextManager(JobContextMessage, keyValuePersistenceService, serializationService);

            return new FundingContext(fundingContextManager);
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

        private static IJobContextMessage JobContextMessage => new JobContextMessage
        {
            JobId = 1,
            SubmissionDateTimeUtc = new System.DateTime(2018, 08, 01).ToUniversalTime(),
            Topics = Topics,
            TopicPointer = 1,
            KeyValuePairs = KeyValuePairsDictionary,
        };

        private static IReadOnlyList<ITopicItem> Topics => new List<TopicItem>();

        private static IDictionary<JobContextMessageKey, object> KeyValuePairsDictionary => new Dictionary<JobContextMessageKey, object>()
            {
                { JobContextMessageKey.Filename, "FileName" },
                { JobContextMessageKey.UkPrn, 10006341 },
                { JobContextMessageKey.ValidLearnRefNumbers, "ValidLearnRefNumbers" },
            };

        private static DictionaryKeyValuePersistenceService BuildKeyValueDictionary(IMessage message)
        {
            var messageNew = (Message)message;

            var learners = messageNew.Learner.ToList();

            var list = new DictionaryKeyValuePersistenceService();
            var serializer = new XmlSerializationService();

            list.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();

            return list;
        }

        private static readonly Mock<ILARS> larsContextMock = new Mock<ILARS>();
        private static readonly Mock<IPostcodes> postcodesContextMock = new Mock<IPostcodes>();
        private static readonly Mock<IOrganisations> organisationContextMock = new Mock<IOrganisations>();
        private static readonly Mock<ILargeEmployer> largeEmployersContextMock = new Mock<ILargeEmployer>();

        private Mock<ILARS> LARSMock()
        {
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
            var orgVersionMock = MockOrgVersionArray().AsMockDbSet();
            var orgFundingMock = MockOrgFundingArray().AsMockDbSet();

            organisationContextMock.Setup(x => x.Org_Version).Returns(orgVersionMock);
            organisationContextMock.Setup(x => x.Org_Funding).Returns(orgFundingMock);

            return organisationContextMock;
        }

        private Mock<ILargeEmployer> LargeEmployersMock()
        {
            var largeEmployerMock = MockLargeEmployerArray().AsMockDbSet();

            largeEmployersContextMock.Setup(x => x.LEMP_Employers).Returns(largeEmployerMock);

            return largeEmployersContextMock;
        }

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
                ActivationDate = System.DateTime.Parse("2017-07-01"),
                ExpiryDate = null,
                Description = "Fifth Version of LARS",
                Comment = null,
                Created_On = System.DateTime.Parse("2017-07-01"),
                Created_By = "System",
                Modified_On = System.DateTime.Parse("2018-07-01"),
                Modified_By = "System"
            };

        private static LARS_LearningDelivery[] MockLARSLearningDeliveryArray()
        {
            return new LARS_LearningDelivery[]
            {
                LarsLearningDeliveryTestValue1,
                LarsLearningDeliveryTestValue2,
                LarsLearningDeliveryTestValue3,
            };
        }

        private static readonly LARS_LearningDelivery LarsLearningDeliveryTestValue1 =
            new LARS_LearningDelivery()
            {
                LearnAimRef = "60133533",
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

        private static readonly LARS_LearningDelivery LarsLearningDeliveryTestValue2 =
            new LARS_LearningDelivery()
            {
                LearnAimRef = "50104767",
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

        private static readonly LARS_LearningDelivery LarsLearningDeliveryTestValue3 =
        new LARS_LearningDelivery()
        {
            LearnAimRef = "60126334",
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

        private static LARS_Funding[] MockLARSFundingArray()
        {
            return new LARS_Funding[]
            {
                LarsFundingTestValue1,
                LarsFundingTestValue2,
                LarsFundingTestValue3,
            };
        }

        private static readonly LARS_Funding LarsFundingTestValue1 =
            new LARS_Funding()
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

        private static readonly LARS_Funding LarsFundingTestValue2 =
            new LARS_Funding()
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

        private static readonly LARS_Funding LarsFundingTestValue3 =
            new LARS_Funding()
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

        private static LARS_LearningDeliveryCategory[] MockLARSCategoryArray()
        {
            return new LARS_LearningDeliveryCategory[]
            {
                LarsCategoryTestValue,
            };
        }

        private static readonly LARS_LearningDeliveryCategory LarsCategoryTestValue =
            new LARS_LearningDeliveryCategory()
            {
                LearnAimRef = "60133533",
                CategoryRef = 1,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };

        private static LARS_AnnualValue[] MockLARSAnnualValueArray()
        {
            return new LARS_AnnualValue[]
            {
                LarsAnnualValueTestValue,
            };
        }

        private static readonly LARS_AnnualValue LarsAnnualValueTestValue =
            new LARS_AnnualValue()
            {
                LearnAimRef = "60133533",
                BasicSkillsType = 5,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };

        private static LARS_FrameworkAims[] MockLARSFrameworkAimsArray()
        {
            return new LARS_FrameworkAims[]
            {
                LarsFrameworkAimsTestValue1,
                LarsFrameworkAimsTestValue2,
                LarsFrameworkAimsTestValue3,
            };
        }

        private static readonly LARS_FrameworkAims LarsFrameworkAimsTestValue1 =
            new LARS_FrameworkAims()
            {
                LearnAimRef = "60133533",
                FworkCode = 420,
                ProgType = 2,
                PwayCode = 1,
                FrameworkComponentType = 1,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };

        private static readonly LARS_FrameworkAims LarsFrameworkAimsTestValue2 =
           new LARS_FrameworkAims()
           {
               LearnAimRef = "50104767",
               FworkCode = 420,
               ProgType = 2,
               PwayCode = 1,
               FrameworkComponentType = 1,
               EffectiveFrom = System.DateTime.Parse("2000-01-01"),
               EffectiveTo = null,
           };

        private static readonly LARS_FrameworkAims LarsFrameworkAimsTestValue3 =
          new LARS_FrameworkAims()
          {
              LearnAimRef = "60126334",
              FworkCode = 420,
              ProgType = 2,
              PwayCode = 1,
              FrameworkComponentType = 1,
              EffectiveFrom = System.DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
          };

        private static Data.Postcodes.Model.VersionInfo[] MockPostcodesVersionArray()
        {
            return new Data.Postcodes.Model.VersionInfo[]
            {
                PostcodesVersionTestValue,
            };
        }

        private static readonly Data.Postcodes.Model.VersionInfo PostcodesVersionTestValue =
            new Data.Postcodes.Model.VersionInfo
            {
                VersionNumber = "Version_002",
                DataSource = "Source",
                Comments = "Comments",
                ModifiedAt = System.DateTime.Parse("2018-01-01"),
                ModifiedBy = "System"
            };

        private static SFA_PostcodeAreaCost[] MockSFAAreaCostArray()
        {
            return new SFA_PostcodeAreaCost[]
            {
                SFAAreaCostTestValue1,
                SFAAreaCostTestValue2,
                SFAAreaCostTestValue3,
            };
        }

        private static readonly SFA_PostcodeAreaCost SFAAreaCostTestValue1 =
          new SFA_PostcodeAreaCost()
          {
              MasterPostcode = new MasterPostcode { Postcode = "ZZ99 9ZZ" },
              Postcode = "ZZ99 9ZZ",
              AreaCostFactor = 1.2m,
              EffectiveFrom = System.DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
          };

          private static readonly SFA_PostcodeAreaCost SFAAreaCostTestValue2 =
          new SFA_PostcodeAreaCost()
          {
              MasterPostcode = new MasterPostcode { Postcode = "AL1 1AA" },
              Postcode = "AL1 1AA",
              AreaCostFactor = 1.2m,
              EffectiveFrom = System.DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
          };

        private static readonly SFA_PostcodeAreaCost SFAAreaCostTestValue3 =
         new SFA_PostcodeAreaCost()
         {
             MasterPostcode = new MasterPostcode { Postcode = "B10 0BL" },
             Postcode = "B10 0BL",
             AreaCostFactor = 1.2m,
             EffectiveFrom = System.DateTime.Parse("2000-01-01"),
             EffectiveTo = null,
         };

        private static SFA_PostcodeDisadvantage[] MockSFADisadvantageArray()
        {
            return new SFA_PostcodeDisadvantage[]
            {
                SFADisadvantageValue1,
                SFADisadvantageValue2,
            };
        }

        private static readonly SFA_PostcodeDisadvantage SFADisadvantageValue1 =
          new SFA_PostcodeDisadvantage()
          {
              MasterPostcode = new MasterPostcode { Postcode = "ZZ99 9ZZ" },
              Postcode = "ZZ99 9ZZ",
              Uplift = 1.2m,
              EffectiveFrom = System.DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
          };

        private static readonly SFA_PostcodeDisadvantage SFADisadvantageValue2 =
          new SFA_PostcodeDisadvantage()
          {
              MasterPostcode = new MasterPostcode { Postcode = "B10 0BL" },
              Postcode = "B10 0BL",
              Uplift = 1.2m,
              EffectiveFrom = System.DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
          };

        private static Org_Version[] MockOrgVersionArray()
        {
            return new Org_Version[]
            {
                OrgVersionTestValue,
            };
        }

        private static readonly Org_Version OrgVersionTestValue =
            new Org_Version()
            {
                MainDataSchemaName = "Version_003"
            };

        private static Org_Funding[] MockOrgFundingArray()
        {
            return new Org_Funding[]
            {
                OrgFundingTestValue,
            };
        }

        private static readonly Org_Funding OrgFundingTestValue =
           new Org_Funding
           {
               UKPRN = 10006341,
               FundingFactor = "Factor",
               FundingFactorType = "Adult Skills",
               FundingFactorValue = "1,54",
               EffectiveFrom = new System.DateTime(2018, 08, 01),
               EffectiveTo = new System.DateTime(2019, 07, 31),
           };

        private static LEMP_Employers[] MockLargeEmployerArray()
        {
            return new LEMP_Employers[]
            {
                LargeEmployerTestValue,
            };
        }

        private static readonly LEMP_Employers LargeEmployerTestValue =
           new LEMP_Employers
           {
               ERN = 154549452,
               EffectiveFrom = new System.DateTime(2018, 08, 01),
               EffectiveTo = new System.DateTime(2019, 07, 31),
           };
    }
}
