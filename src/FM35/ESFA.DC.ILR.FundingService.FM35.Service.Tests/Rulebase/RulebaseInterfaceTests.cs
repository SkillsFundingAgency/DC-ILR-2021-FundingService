using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using ESFA.DC.Data.LargeEmployer.Model;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
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
using ESFA.DC.ILR.FundingService.FM35.Service.Builders;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface.Builders;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity;
using ESFA.DC.OPA.XSRC.Model.XSRC;
using ESFA.DC.OPA.XSRC.Model.XSRCEntity;
using ESFA.DC.OPA.XSRC.Service;
using ESFA.DC.OPA.XSRC.Service.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;
using ESFA.DC.TestHelpers.Mock.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests.Rulebase
{
    public class RulebaseInterfaceTests
    {
        private const string academicYear = "1819";
        private const string rulebaseName = "FM35 Funding Calc 18_19";
        private const string rulebaseFolder = "Rulebase";
        private const string rulebaseMasterFolder = "RulebaseMasterFiles";
        private const string xsrcName = "FM35Inputs";

        /// <summary>
        /// Return Rulebase and check version numbers
        /// </summary>
        [Fact(DisplayName = "RulebaseVersion - AcademicYear Exists"), Trait("Rulebase Interface", "Unit")]
        public void RulebaseVersion_AcademicYear_Exists()
        {
            // ARRANGE
            var rulebaseVersion = GetVersion(rulebaseFolder);

            // ACT
            var year = rulebaseVersion.Substring(0, 4);

            // ASSERT
            year.Should().NotBeNull();
        }

        /// <summary>
        /// Return Rulebase and check version numbers
        /// </summary>
        [Fact(DisplayName = "RulebaseVersion - AcademicYear Correct"), Trait("Rulebase Interface", "Unit")]
        public void RulebaseVersion_AcademicYear_Correct()
        {
            // ARRANGE
            var rulebaseVersion = GetVersion(rulebaseFolder);

            // ACT
            var year = rulebaseVersion.Substring(0, 4);

            // ASSERT
            year.Should().Be(academicYear);
        }

        /// <summary>
        /// Return Rulebase and check version numbers
        /// </summary>
        [Fact(DisplayName = "RulebaseVersion - AcademicYear Matches Previous"), Trait("Rulebase Interface", "Unit")]
        public void RulebaseVersion_AcademicYear_Match()
        {
            // ARRANGE
            var rulebaseVersion = GetVersion(rulebaseFolder);
            var masterRulebaseVersion = GetVersion(rulebaseMasterFolder);

            // ACT
            var acaedmicYear = rulebaseVersion.Substring(0, 4);
            var masterAcaedmicYear = rulebaseVersion.Substring(0, 4);

            // ASSERT
            masterAcaedmicYear.Should().Be(acaedmicYear);
        }

        /// <summary>
        /// Return Rulebase and check version numbers
        /// </summary>
        [Fact(DisplayName = "RulebaseVersion - MajorVersion Exists"), Trait("Rulebase Interface", "Unit")]
        public void RulebaseVersion_InterfaceVersion_Exists()
        {
            // ARRANGE
            var rulebaseVersion = GetVersion(rulebaseFolder);

            // ACT
            var interfaceVersion = rulebaseVersion.Substring(5, 2);

            // ASSERT
            interfaceVersion.Should().NotBeNull();
        }

        /// <summary>
        /// Return Rulebase and check version numbers
        /// </summary>
        [Fact(DisplayName = "RulebaseVersion - MajorVersion Matches Previous"), Trait("Rulebase Interface", "Unit")]
        public void RulebaseVersion_InterfaceVersion_Match()
        {
            // ARRANGE
            var rulebaseVersion = GetVersion(rulebaseFolder);
            var masterRulebaseVersion = GetVersion(rulebaseMasterFolder);

            // ACT
            var interfaceVersion = rulebaseVersion.Substring(5, 2);
            var masterInterfaceVersion = rulebaseVersion.Substring(5, 2);

            // ASSERT
            masterInterfaceVersion.Should().Be(interfaceVersion);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - XML File Not Empty"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_File_NotNull()
        {
            // ARRANGE
            var xsrcFile = GetXSRC(rulebaseFolder);

            // ACT
            var xml = xsrcFile.OuterXml.ToString();

            // ASSERT
            xml.Should().NotBeNull();
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - XML Files Match"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_File_FilesMatch()
        {
            // ARRANGE
            var xsrcFile = GetXSRC(rulebaseFolder);
            var masterXsrcFile = GetXSRC(rulebaseMasterFolder);

            // ACT
            var xml = xsrcFile.OuterXml.ToString();
            var masterXml = masterXsrcFile.OuterXml.ToString();

            // ASSERT
            masterXml.Should().BeEquivalentTo(xml);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - XML File Check Entities Expected"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_File_CheckEntitiesExpected()
        {
            // ARRANGE
            var xsrcFile = GetXSRC(rulebaseFolder);

            // ACT
            var entities = GetXRSCEntities(xsrcFile);

            // ASSERT
            expectedEntities.Should().BeEquivalentTo(entities);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - XML File Check Entities Match"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_File_CheckEntitiesMatch()
        {
            // ARRANGE
            var xsrcFile = GetXSRC(rulebaseFolder);
            var masterXsrcFile = GetXSRC(rulebaseMasterFolder);

            // ACT
            var entities = GetXRSCEntities(xsrcFile);
            var masterEntities = GetXRSCEntities(masterXsrcFile);

            // ASSERT
            masterEntities.Should().BeEquivalentTo(entities);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - XML File Check Attributes Expected"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_File_CheckAttributesExpected()
        {
            // ARRANGE
            var xsrcFile = GetXSRC(rulebaseFolder);

            // ACT
            var attributes = GetXRSCAttributes(xsrcFile).OrderBy(attribute => attribute);

            // ASSERT
            expectedAttributes.OrderBy(attribute => attribute).Should().BeEquivalentTo(attributes);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - XML File Check Attributes Match"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_File_CheckAttributesMatch()
        {
            // ARRANGE
            var xsrcFile = GetXSRC(rulebaseFolder);
            var masterXsrcFile = GetXSRC(rulebaseMasterFolder);

            // ACT
            var attributes = GetXRSCAttributes(xsrcFile);
            var masterattributes = GetXRSCAttributes(masterXsrcFile);

            // ASSERT
            masterattributes.Should().BeEquivalentTo(attributes);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - Deserialize file - Object Exists"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_Object_NotNull()
        {
            // ARRANGE
            var xsrc = DeserializedXSRC(rulebaseFolder);

            // ACT

            // ASSERT
            xsrc.Should().NotBeNull();
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRC - Deserialize file - Object Matches"), Trait("Rulebase Interface", "Unit")]
        public void XSRC_Object_Matches()
        {
            // ARRANGE
            var xsrc = DeserializedXSRC(rulebaseFolder);
            var masterXSRC = DeserializedXSRC(rulebaseMasterFolder);

            // ACT

            // ASSERT
            masterXSRC.Should().BeEquivalentTo(xsrc);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - Object Exists"), Trait("Rulebase Interface", "Unit")]
        public void XSRCEntity_Object_NotNull()
        {
            // ARRANGE

            // ACT
            var entity = XSRCBuilder(rulebaseFolder);

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - Object Matches"), Trait("Rulebase Interface", "Unit")]
        public void XSRCEntity_Object_Matches()
        {
            // ARRANGE

            // ACT
            var entity = XSRCBuilder(rulebaseFolder);
            var masterEntity = XSRCBuilder(rulebaseMasterFolder);

            // ASSERT
            masterEntity.Should().BeEquivalentTo(entity);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XSRC Entities Matches Linq Query"), Trait("Rulebase Interface", "Unit")]
        public void XSRCEntity_Entties_MatchesLinq()
        {
            // ARRANGE
            var entityXSRC = XSRCBuilder(rulebaseFolder);
            var entityLinq = LinqEntityBuilder();

            // ACT
            var xsrcList = GetXSRCEntityList(entityXSRC.GlobalEntity);
            var linqList = GetLinqEntityList(entityLinq.First());

            // ASSERT
            var xsrc = xsrcList.Select(v => v).Distinct();
            var linq = linqList.Select(v => v).Distinct();

            xsrc.Should().BeEquivalentTo(linq);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - Base XSRC Entities Matches Linq Query"), Trait("Rulebase Interface", "Unit")]
        public void XSRCEntity_BaseEntties_MatchesLinq()
        {
            // ARRANGE
            var entityXSRC = XSRCBuilder(rulebaseMasterFolder);
            var entityLinq = LinqEntityBuilder();

            // ACT
            var xsrcList = GetXSRCEntityList(entityXSRC.GlobalEntity);
            var linqList = GetLinqEntityList(entityLinq.First());

            // ASSERT
            var xsrc = xsrcList.Select(v => v).Distinct();
            var linq = linqList.Select(v => v).Distinct();

            xsrc.Should().BeEquivalentTo(linq);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XSRC Attributes Matches Linq Query"), Trait("Rulebase Interface", "Unit")]
        public void XSRCEntity_Attributes_MatchesLinq()
        {
            // ARRANGE
            var entityXSRC = XSRCBuilder(rulebaseFolder);
            var entityLinq = LinqEntityBuilder();

            // ACT
            var xsrcList = GetXSRCAttributeList(entityXSRC.GlobalEntity);
            var linqList = GetLinqAttributeList(entityLinq.First());

            // ASSERT
            var xsrc = xsrcList.Select(v => v).Distinct();
            var linq = linqList.Select(v => v).Distinct();

            xsrc.Should().BeEquivalentTo(linq);
        }

        /// <summary>
        /// Return Rulebase XSRC and check files are as expected.
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - Base XSRC Attributes Matches Linq Query"), Trait("Rulebase Interface", "Unit")]
        public void XSRCEntity_BaseAttributes_MatchesLinq()
        {
            // ARRANGE
            var entityXSRC = XSRCBuilder(rulebaseMasterFolder);
            var entityLinq = LinqEntityBuilder();

            // ACT
            var xsrcList = GetXSRCAttributeList(entityXSRC.GlobalEntity);
            var linqList = GetLinqAttributeList(entityLinq.First());

            // ASSERT
            var xsrc = xsrcList.Select(v => v).Distinct();
            var linq = linqList.Select(v => v).Distinct();

            xsrc.Should().BeEquivalentTo(linq);
        }

        #region Test Helpers

        private IList<string> expectedEntities = new List<string>
        {
            "global",
            "learner",
            "learningdelivery",
            "learningdeliveryfam",
            "learneremploymentstatus",
            "learningdeliverylarscategory",
            "learningdeliveryannualvalue",
            "learneremploymentstatuslargeemployerreferencedata",
            "learningdeliverylarsfunding",
            "organisationalfunding",
            "learnerpostcodedisadvantageupliftreferencedata",
            "learningdeliverypostcodeareacostreferencedata",
        };

        private IList<string> expectedAttributes = new List<string>
        {
            // Global
            "LARSVersion",
            "OrgVersion",
            "UKPRN",
            "PostcodeDisadvantageVersion",

            // OrgFunding
            "OrgFundEffectiveTo",
            "OrgFundEffectiveFrom",
            "OrgFundFactor",
            "OrgFundFactValue",
            "OrgFundFactType",

            // Learner
            "LearnRefNumber",
            "DateOfBirth",

            // Learner Employment Status
            "EmpId",
            "DateEmpStatApp",

            // Large Employer
            "LargeEmpEffectiveFrom",
            "LargeEmpEffectiveTo",

            // SFA Postcode Disadvantage
            "DisUplift",
            "DisUpEffectiveFrom",
            "DisUpEffectiveTo",

            // LearningDelivery
            "AchDate",
            "AddHours",
            "AimSeqNumber",
            "AimType",
            "CompStatus",
            "EmpOutcome",
            "EnglandFEHEStatus",
            "EnglPrscID",
            "FworkCode",
            "FrameworkCommonComponent",
            "FrameworkComponentType",
            "LearnActEndDate",
            "LearnPlanEndDate",
            "LearnStartDate",
            "LrnDelFAM_EEF",
            "LrnDelFAM_LDM1",
            "LrnDelFAM_LDM2",
            "LrnDelFAM_LDM3",
            "LrnDelFAM_LDM4",
            "LrnDelFAM_FFI",
            "LrnDelFAM_RES",
            "OrigLearnStartDate",
            "OtherFundAdj",
            "Outcome",
            "PriorLearnFundAdj",
            "ProgType",
            "PwayCode",

            // LearningDeliveryFAM
            "LearnDelFAMCode",
            "LearnDelFAMDateFrom",
            "LearnDelFAMDateTo",
            "LearnDelFAMType",

            // LearningDeliveryLARSCategory
            "LearnDelCatRef",
            "LearnDelCatDateFrom",
            "LearnDelCatDateTo",

            // LearningDeliveryLARSAnnualValue
            "LearnDelAnnValBasicSkillsTypeCode",
            "LearnDelAnnValDateFrom",
            "LearnDelAnnValDateTo",

            // LearningDeliveryLARSFunding
            "LARSFundCategory",
            "LARSFundEffectiveFrom",
            "LARSFundEffectiveTo",
            "LARSFundUnweightedRate",
            "LARSFundWeightedRate",
            "LARSFundWeightingFactor",

            // SFAPostcodeAreaCost
            "AreaCosEffectiveFrom",
            "AreaCosEffectiveTo",
            "AreaCosFactor",
        };

        private IList<string> linqEntityList = new List<string>();

        private IList<string> GetLinqEntityList(IDataEntity entity)
        {
            linqEntityList.Add(entity.EntityName);

            foreach (var child in entity.Children)
            {
                GetLinqEntityList(child);
            }

            return linqEntityList;
        }

        private IList<string> xsrcEntityList = new List<string>();

        private IList<string> GetXSRCEntityList(IXsrcEntity entity)
        {
            xsrcEntityList.Add(entity.PublicName);

            foreach (var child in entity.Children)
            {
                GetXSRCEntityList(child);
            }

            return xsrcEntityList;
        }

        private IList<string> linqAttributeList = new List<string>();

        private IList<string> GetLinqAttributeList(IDataEntity entity)
        {
            foreach (var attribute in entity.Attributes)
            {
                linqAttributeList.Add(attribute.Key);
            }

            foreach (var child in entity.Children)
            {
                GetLinqAttributeList(child);
            }

            return linqAttributeList;
        }

        private IList<string> xsrcAttributeList = new List<string>();

        private IList<string> GetXSRCAttributeList(IXsrcEntity entity)
        {
            foreach (var attribute in entity.Attributes)
            {
                xsrcAttributeList.Add(attribute.PublicName);
            }

            foreach (var child in entity.Children)
            {
                GetXSRCAttributeList(child);
            }

            return xsrcAttributeList;
        }

        private XsrcGlobal XSRCBuilder(string folderName)
        {
            IXsrcEntityBuilder builder = new XsrcEntityBuilder(string.Empty + folderName + "//" + xsrcName + ".xsrc");

            return builder.BuildXsrc();
        }

        private IRoot DeserializedXSRC(string folderName)
        {
            var stream = new FileStream(string.Empty + folderName + "//" + xsrcName + ".xsrc", FileMode.Open);

            ISerializationService serializationService = new XmlSerializationService();

            var root = serializationService.Deserialize<Root>(stream);

            stream.Close();

            return root;
        }

        private IList<string> GetXRSCEntities(XmlDocument xml)
        {
            IList<string> entities = new List<string>();
            var elemList = xml.GetElementsByTagName("entity");
            for (int i = 0; i < elemList.Count; i++)
            {
                var e = elemList[i];

                entities.Add(e.Attributes[0].Value);
            }

            return entities;
        }

        private IList<string> GetXRSCAttributes(XmlDocument xml)
        {
            IList<string> attributes = new List<string>();
            var elemList = xml.GetElementsByTagName("attribute");
            for (int i = 0; i < elemList.Count; i++)
            {
                var e = elemList[i];

                attributes.Add(e.Attributes["public-name"].Value);
            }

            return attributes;
        }

        private string GetVersion(string folderName)
        {
            var zipStream = new FileStream(string.Empty + folderName + "//" + rulebaseName + ".zip", FileMode.Open);
            var doc = GetXmlDocumentFromZip(zipStream);
            zipStream.Close();

            XmlElement root = doc.DocumentElement;
            var nodes = root.LastChild;
            XmlNodeList elemList = doc.GetElementsByTagName("conclude");

            IDictionary<string, string> attributes = new Dictionary<string, string>();

            for (int i = 0; i < elemList.Count; i++)
            {
                var e = elemList[i];

                attributes.Add(e.Attributes["attr-id"].Value, e.InnerText);
            }

            return attributes.Where(k => k.Key == "RulebaseVersion").Select(v => v.Value).SingleOrDefault();
        }

        private XmlDocument GetXmlDocumentFromZip(FileStream stream)
        {
            ZipArchive zip = new ZipArchive(stream);

            var file = zip.Entries.First(f => f.Name == rulebaseName + ".xml").Open();

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            return doc;
        }

        private XmlDocument GetXSRC(string folderName)
        {
            var stream = new FileStream(string.Empty + folderName + "//" + xsrcName + ".xsrc", FileMode.Open);
            var doc = GetXmlDocument(stream);
            stream.Close();

            return doc;
        }

        private XmlDocument GetXmlDocument(FileStream stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            return doc;
        }

        private IEnumerable<IDataEntity> LinqEntityBuilder()
        {
            IReferenceDataCache referenceDataCache = new ReferenceDataCache();
            IReferenceDataCachePopulationService referenceDataCachePopulationService = new ReferenceDataCachePopulationService(referenceDataCache, LARSMock().Object, PostcodesMock().Object, OrganisationMock().Object, LargeEmployersMock().Object);
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(referenceDataCache);
            ILARSReferenceDataService larsReferenceDataService = new LARSReferenceDataService(referenceDataCache);
            IOrganisationReferenceDataService organisationReferenceDataService = new OrganisationReferenceDataService(referenceDataCache);
            IPostcodesReferenceDataService postcodesReferenceDataService = new PostcodesReferenceDataService(referenceDataCache);

            referenceDataCachePopulationService.Populate(new List<string> { "123456" }, new List<string> { "CV1 2WT" }, new List<long> { 12345678 }, new List<int> { 99999 });
            IAttributeBuilder<IAttributeData> attributeBuilder = new AttributeBuilder();
            var dataEntityBuilder = new DataEntityBuilder(largeEmployersReferenceDataService, larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService, attributeBuilder);

            return dataEntityBuilder.EntityBuilder(12345678, testMessage.Learners);
        }

        private static readonly Mock<ILARS> larsContextMock = new Mock<ILARS>();
        private static readonly Mock<IPostcodes> postcodesContextMock = new Mock<IPostcodes>();
        private static readonly Mock<IOrganisations> organisationContextMock = new Mock<IOrganisations>();
        private static readonly Mock<ILargeEmployer> largeEmployersContextMock = new Mock<ILargeEmployer>();

        private Mock<ILARS> LARSMock()
        {
            var larsVersionMock = MockDBSetHelper.GetQueryableMockDbSet(MockLARSVersionArray());
            var larsLearningDeliveryMock = MockDBSetHelper.GetQueryableMockDbSet(MockLARSLearningDeliveryArray());
            var larsFundingMock = MockDBSetHelper.GetQueryableMockDbSet(MockLARSFundingArray());
            var larsAnnualValueMock = MockDBSetHelper.GetQueryableMockDbSet(MockLARSAnnualValueArray());
            var larsCategoryMock = MockDBSetHelper.GetQueryableMockDbSet(MockLARSCategoryArray());
            var larsFrameworkAimsMock = MockDBSetHelper.GetQueryableMockDbSet(MockLARSFrameworkAimsArray());

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
            var postcodesVersionMock = MockDBSetHelper.GetQueryableMockDbSet(MockPostcodesVersionArray());
            var sfaAreaCostMock = MockDBSetHelper.GetQueryableMockDbSet(MockSFAAreaCostArray());
            var sfaDisadvantageMock = MockDBSetHelper.GetQueryableMockDbSet(MockSFADisadvantageArray());

            postcodesContextMock.Setup(x => x.SFA_PostcodeAreaCost).Returns(sfaAreaCostMock);
            postcodesContextMock.Setup(x => x.VersionInfos).Returns(postcodesVersionMock);
            postcodesContextMock.Setup(x => x.SFA_PostcodeDisadvantage).Returns(sfaDisadvantageMock);

            return postcodesContextMock;
        }

        private Mock<IOrganisations> OrganisationMock()
        {
            var orgVersionMock = MockDBSetHelper.GetQueryableMockDbSet(MockOrgVersionArray());
            var orgFundingMock = MockDBSetHelper.GetQueryableMockDbSet(MockOrgFundingArray());

            organisationContextMock.Setup(x => x.Org_Version).Returns(orgVersionMock);
            organisationContextMock.Setup(x => x.Org_Funding).Returns(orgFundingMock);

            return organisationContextMock;
        }

        private Mock<ILargeEmployer> LargeEmployersMock()
        {
            var largeEmployerMock = MockDBSetHelper.GetQueryableMockDbSet(MockLargeEmployerArray());

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
                LarsLearningDeliveryTestValue,
            };
        }

        private static readonly LARS_LearningDelivery LarsLearningDeliveryTestValue =
            new LARS_LearningDelivery()
            {
                LearnAimRef = "123456",
                LearnAimRefTitle = "Test Learning Aim Title 50094488",
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
                LarsFundingTestValue,
            };
        }

        private static readonly LARS_Funding LarsFundingTestValue =
            new LARS_Funding()
            {
                LearnAimRef = "123456",
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
                LearnAimRef = "123456",
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
                LearnAimRef = "123456",
                BasicSkillsType = 5,
                EffectiveFrom = System.DateTime.Parse("2000-01-01"),
                EffectiveTo = null,
            };

        private static LARS_FrameworkAims[] MockLARSFrameworkAimsArray()
        {
            return new LARS_FrameworkAims[]
            {
                LarsFrameworkAimsTestValue,
            };
        }

        private static readonly LARS_FrameworkAims LarsFrameworkAimsTestValue =
            new LARS_FrameworkAims()
            {
                LearnAimRef = "123456",
                FworkCode = 20,
                ProgType = 1,
                PwayCode = 2,
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
            };
        }

        private static readonly SFA_PostcodeAreaCost SFAAreaCostTestValue1 =
          new SFA_PostcodeAreaCost()
          {
              MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
              Postcode = "CV1 2WT",
              AreaCostFactor = 1.2m,
              EffectiveFrom = System.DateTime.Parse("2000-01-01"),
              EffectiveTo = null,
          };

        private static SFA_PostcodeDisadvantage[] MockSFADisadvantageArray()
        {
            return new SFA_PostcodeDisadvantage[]
            {
                SFADisadvantageValue,
            };
        }

        private static readonly SFA_PostcodeDisadvantage SFADisadvantageValue =
          new SFA_PostcodeDisadvantage()
          {
              MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
              Postcode = "CV1 2WT",
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
               UKPRN = 12345678,
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
               ERN = 99999,
               EffectiveFrom = new System.DateTime(2018, 08, 01),
               EffectiveTo = new System.DateTime(2019, 07, 31),
           };

        private readonly IMessage testMessage = new Message
        {
            LearningProvider = new MessageLearningProvider
            {
                UKPRN = 12345678,
            },
            Learner = new MessageLearner[]
          {
                new MessageLearner
                {
                    LearnRefNumber = "Learner1",
                    DateOfBirth = new System.DateTime(2000, 01, 01),
                    PostcodePrior = "CV1 2WT",
                    LearnerEmploymentStatus = new MessageLearnerLearnerEmploymentStatus[]
                    {
                        new MessageLearnerLearnerEmploymentStatus
                        {
                            EmpIdSpecified = true,
                            EmpId = 99999,
                            AgreeId = "AgreeID",
                            EmpStat = 1,
                            DateEmpStatApp = new System.DateTime(2018, 08, 01),
                        }
                    },
                    LearningDelivery = new[]
                    {
                        new MessageLearnerLearningDelivery
                        {
                            LearnAimRef = "123456",
                            AimSeqNumber = 1,
                            CompStatus = 1,
                            DelLocPostCode = "CV1 2WT",
                            FworkCodeSpecified = true,
                            FworkCode = 20,
                            LearnActEndDateSpecified = true,
                            LearnActEndDate = System.DateTime.Parse("2018-06-30"),
                            LearnStartDate = System.DateTime.Parse("2017-08-30"),
                            LearnPlanEndDate = System.DateTime.Parse("2018-07-30"),
                            OrigLearnStartDateSpecified = true,
                            OrigLearnStartDate = System.DateTime.Parse("2017-08-30"),
                            OtherFundAdjSpecified = false,
                            OutcomeSpecified = false,
                            PriorLearnFundAdjSpecified = false,
                            ProgTypeSpecified = true,
                            ProgType = 1,
                            PwayCodeSpecified = true,
                            PwayCode = 2,
                            LearningDeliveryFAM = new[]
                            {
                                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                {
                                    LearnDelFAMCode = "1",
                                    LearnDelFAMType = "ADL",
                                    LearnDelFAMDateFromSpecified = true,
                                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-08-30"),
                                    LearnDelFAMDateToSpecified = true,
                                    LearnDelFAMDateTo = System.DateTime.Parse("2017-10-31"),
                                },
                                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                {
                                    LearnDelFAMCode = "100",
                                    LearnDelFAMType = "SOF",
                                    LearnDelFAMDateFromSpecified = true,
                                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-10-31"),
                                    LearnDelFAMDateToSpecified = true,
                                    LearnDelFAMDateTo = System.DateTime.Parse("2017-11-30"),
                                },
                                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                {
                                    LearnDelFAMCode = "1",
                                    LearnDelFAMType = "RES",
                                    LearnDelFAMDateFromSpecified = true,
                                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-12-01"),
                                    LearnDelFAMDateToSpecified = false,
                                }
                            }
                        }
                    }
                },
                new MessageLearner
                {
                    LearnRefNumber = "Learner2",
                    DateOfBirth = new System.DateTime(2000, 01, 01),
                    PostcodePrior = "CV1 2WT",
                    LearnerEmploymentStatus = new MessageLearnerLearnerEmploymentStatus[]
                    {
                        new MessageLearnerLearnerEmploymentStatus
                        {
                            EmpIdSpecified = true,
                            EmpId = 99999,
                            AgreeId = "AgreeID",
                            EmpStat = 1,
                            DateEmpStatApp = new System.DateTime(2018, 08, 01),
                        }
                    },
                    LearningDelivery = new[]
                    {
                        new MessageLearnerLearningDelivery
                        {
                            LearnAimRef = "123456",
                            AimSeqNumber = 1,
                            CompStatus = 1,
                            DelLocPostCode = "CV1 2WT",
                            FworkCodeSpecified = true,
                            FworkCode = 20,
                            LearnActEndDateSpecified = true,
                            LearnActEndDate = System.DateTime.Parse("2018-06-30"),
                            LearnStartDate = System.DateTime.Parse("2017-08-30"),
                            LearnPlanEndDate = System.DateTime.Parse("2018-07-30"),
                            OrigLearnStartDateSpecified = true,
                            OrigLearnStartDate = System.DateTime.Parse("2017-08-30"),
                            OtherFundAdjSpecified = false,
                            OutcomeSpecified = false,
                            PriorLearnFundAdjSpecified = false,
                            ProgTypeSpecified = true,
                            ProgType = 1,
                            PwayCodeSpecified = true,
                            PwayCode = 2,
                            LearningDeliveryFAM = new[]
                            {
                                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                {
                                    LearnDelFAMCode = "100",
                                    LearnDelFAMType = "LDM",
                                    LearnDelFAMDateFromSpecified = true,
                                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-08-30"),
                                    LearnDelFAMDateToSpecified = true,
                                    LearnDelFAMDateTo = System.DateTime.Parse("2017-10-31"),
                                },
                                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                {
                                    LearnDelFAMCode = "200",
                                    LearnDelFAMType = "LDM",
                                    LearnDelFAMDateFromSpecified = true,
                                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-08-30"),
                                    LearnDelFAMDateToSpecified = true,
                                    LearnDelFAMDateTo = System.DateTime.Parse("2017-10-31"),
                                },
                                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                {
                                    LearnDelFAMCode = "100",
                                    LearnDelFAMType = "SOF",
                                    LearnDelFAMDateFromSpecified = true,
                                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-10-31"),
                                    LearnDelFAMDateToSpecified = true,
                                    LearnDelFAMDateTo = System.DateTime.Parse("2017-11-30"),
                                },
                                new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                {
                                    LearnDelFAMCode = "1",
                                    LearnDelFAMType = "RES",
                                    LearnDelFAMDateFromSpecified = true,
                                    LearnDelFAMDateFrom = System.DateTime.Parse("2017-12-01"),
                                    LearnDelFAMDateToSpecified = false,
                                }
                            }
                        }
                    }
                }
            }
        };

        #endregion
    }
}
