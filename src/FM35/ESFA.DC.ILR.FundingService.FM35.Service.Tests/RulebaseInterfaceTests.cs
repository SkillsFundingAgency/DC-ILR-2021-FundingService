using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM35.Service.Constants;
using ESFA.DC.ILR.FundingService.FM35.Service.Input;
using ESFA.DC.ILR.FundingService.FM35.Service.Models;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity;
using ESFA.DC.OPA.XSRC.Model.XSRCEntity;
using ESFA.DC.OPA.XSRC.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests
{
    public class RulebaseInterfaceTests
    {
        public const string AcademicYear = "1819";
        public const string RulebaseName = "FM35 Funding Calc 18_19";
        public const string RulebaseFolder = "Rulebase";
        public const string RulebaseMasterFolder = "RulebaseMasterFiles";
        public const string XsrcName = "Inputs";

        private IList<string> xsrcEntityList = new List<string>();
        private IList<string> dataEntityEntityList = new List<string>();
        private IList<string> xsrcAttributeList = new List<string>();
        private IList<string> dataEntityAttributeList = new List<string>();
        private IDictionary<string, List<string>> xsrcEntityDictionary = new Dictionary<string, List<string>>();
        private IDictionary<string, List<string>> dataEntityDictionary = new Dictionary<string, List<string>>();

        [Fact]
        public void RulebaseVersion_AcademicYear()
        {
            var rulebaseAcademicYear = GetRulebaseVersion(RulebaseFolder).Substring(0, 4);

            rulebaseAcademicYear.Should().NotBeNull();
            rulebaseAcademicYear.Should().Be(AcademicYear);
        }

        [Fact]
        public void RulebaseVersion_AcademicYear_Match()
        {
            var rulebaseAcademicYear = GetRulebaseVersion(RulebaseFolder).Substring(0, 4);
            var masterRulebaseAcademicYear = GetRulebaseVersion(RulebaseMasterFolder).Substring(0, 4);

            masterRulebaseAcademicYear.Should().Be(rulebaseAcademicYear);
        }

        [Fact]
        public void RulebaseVersion_InterfaceVersion()
        {
            var rulebaseInterfaceVersion = GetRulebaseVersion(RulebaseFolder).Substring(5, 2);

            rulebaseInterfaceVersion.Should().NotBeNull();
        }

        [Fact]
        public void RulebaseVersion_InterfaceVersion_Match()
        {
            var rulebaseInterfaceVersion = GetRulebaseVersion(RulebaseFolder).Substring(5, 2);
            var masterRulebaseInterfaceVersion = GetRulebaseVersion(RulebaseMasterFolder).Substring(5, 2);

            masterRulebaseInterfaceVersion.Should().Be(rulebaseInterfaceVersion);
        }

        [Fact]
        public void XSRC_File_Valid()
        {
            GetXSRC(RulebaseFolder).OuterXml.ToString().Should().NotBeNull();
        }

        [Fact]
        public void XSRC_File_FilesMatch()
        {
            var xsrcFile = GetXSRC(RulebaseFolder).OuterXml.ToString();
            var masterXsrcFile = GetXSRC(RulebaseMasterFolder).OuterXml.ToString();

            masterXsrcFile.Should().BeEquivalentTo(xsrcFile);
        }

        [Fact]
        public void XSRC_File_CheckEntitiesExpected()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;

            ExpectedEntities().Should().BeEquivalentTo(GetEntityList(xsrcEntities));
        }

        [Fact]
        public void XSRC_File_CheckEntitiesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var masterXsrcEntities = GetXSRCEntity(RulebaseMasterFolder).GlobalEntity;

            GetEntityList(masterXsrcEntities).Should().BeEquivalentTo(GetEntityList(xsrcEntities));
        }

        [Fact]
        public void XSRC_File_CheckAttributesExpected()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;

            ExpectedAttributes().Should().BeEquivalentTo(GetAttributeList(xsrcEntities));
        }

        [Fact]
        public void XSRC_File_CheckAttributesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var masterXsrcEntities = GetXSRCEntity(RulebaseMasterFolder).GlobalEntity;

            GetAttributeList(xsrcEntities).Should().BeEquivalentTo(GetAttributeList(masterXsrcEntities));
        }

        [Fact]
        public void XSRC_vs_DataEntityMapper_EntitiesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var dateEntityMapperEntities = GetDataEntityMapperEntity();

            GetEntityList(dateEntityMapperEntities).Should().BeEquivalentTo(GetEntityList(xsrcEntities));
        }

        [Fact]
        public void XSRC_vs_DataEntityMapper_AttributesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var dateEntityMapperEntities = GetDataEntityMapperEntity();

            GetAttributeList(dateEntityMapperEntities).Should().BeEquivalentTo(GetAttributeList(xsrcEntities));
        }

        [Fact]
        public void XSRC_vs_DataEntityMapper_EntityStructureMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var dateEntityMapperEntities = GetDataEntityMapperEntity();

            var xsrcEntity = ConvertToDataEntity(xsrcEntities);

            // Get Dictionaries to compare
            var xsrcEntityAttributeDictionaries = GetXsrcEntityAttributeDictionaries(xsrcEntity);
            var dataEntityMapperAttributeDictionaries = GetDataEntityAttributeDictionaries(dateEntityMapperEntities);

            dataEntityMapperAttributeDictionaries.Should().BeEquivalentTo(xsrcEntityAttributeDictionaries);
        }

        public IList<string> ExpectedEntities()
        {
            return new List<string>
            {
                Attributes.EntityGlobal,
                Attributes.EntityOrgFunding,
                Attributes.EntityLearner,
                Attributes.EntityLearnerEmploymentStatus,
                Attributes.EntityLargeEmployerReferenceData,
                Attributes.EntitySFA_PostcodeDisadvantage,
                Attributes.EntityLearningDelivery,
                Attributes.EntityLearningDeliveryFAM,
                Attributes.EntityLearningDeliverySFA_PostcodeAreaCost,
                Attributes.EntityLearningDeliveryLARS_AnnualValue,
                Attributes.EntityLearningDeliveryLARS_Category,
                Attributes.EntityLearningDeliveryLARS_Funding,
            };
        }

        public IList<string> ExpectedAttributes()
        {
            return new List<string>
            {
                Attributes.LARSVersion,
                Attributes.OrgVersion,
                Attributes.PostcodeDisadvantageVersion,
                Attributes.UKPRN,
                Attributes.LearnRefNumber,
                Attributes.DateOfBirth,
                Attributes.OrgFundEffectiveFrom,
                Attributes.OrgFundEffectiveTo,
                Attributes.OrgFundFactor,
                Attributes.OrgFundFactType,
                Attributes.OrgFundFactValue,
                Attributes.AchDate,
                Attributes.AddHours,
                Attributes.AimSeqNumber,
                Attributes.AimType,
                Attributes.CompStatus,
                Attributes.EmpOutcome,
                Attributes.EnglandFEHEStatus,
                Attributes.EnglPrscID,
                Attributes.FworkCode,
                Attributes.FrameworkCommonComponent,
                Attributes.FrameworkComponentType,
                Attributes.LearnActEndDate,
                Attributes.LearnPlanEndDate,
                Attributes.LearnStartDate,
                Attributes.LrnDelFAM_EEF,
                Attributes.LrnDelFAM_LDM1,
                Attributes.LrnDelFAM_LDM2,
                Attributes.LrnDelFAM_LDM3,
                Attributes.LrnDelFAM_LDM4,
                Attributes.LrnDelFAM_FFI,
                Attributes.LrnDelFAM_RES,
                Attributes.OrigLearnStartDate,
                Attributes.OtherFundAdj,
                Attributes.Outcome,
                Attributes.PriorLearnFundAdj,
                Attributes.ProgType,
                Attributes.PwayCode,
                Attributes.LearnDelFAMCode,
                Attributes.LearnDelFAMDateTo,
                Attributes.LearnDelFAMDateFrom,
                Attributes.LearnDelFAMType,
                Attributes.DateEmpStatApp,
                Attributes.EmpId,
                Attributes.DisUplift,
                Attributes.DisUpEffectiveFrom,
                Attributes.DisUpEffectiveTo,
                Attributes.LearnDelAnnValBasicSkillsTypeCode,
                Attributes.LearnDelAnnValDateFrom,
                Attributes.LearnDelAnnValDateTo,
                Attributes.LargeEmpEffectiveFrom,
                Attributes.LargeEmpEffectiveTo,
                Attributes.LARSFundCategory,
                Attributes.LARSFundEffectiveFrom,
                Attributes.LARSFundEffectiveTo,
                Attributes.LARSFundWeightedRate,
                Attributes.LARSFundUnweightedRate,
                Attributes.LARSFundWeightingFactor,
                Attributes.LearnDelCatRef,
                Attributes.LearnDelCatDateFrom,
                Attributes.LearnDelCatDateTo,
                Attributes.AreaCosFactor,
                Attributes.AreaCosEffectiveFrom,
                Attributes.AreaCosEffectiveTo,
            };
        }

        public IList<string> GetEntityList(IXsrcEntity entity)
        {
            xsrcEntityList.Add(entity.PublicName);

            foreach (var child in entity.Children)
            {
                GetEntityList(child);
            }

            return xsrcEntityList;
        }

        public IList<string> GetEntityList(IDataEntity entity)
        {
            dataEntityEntityList.Add(entity.EntityName);

            foreach (var child in entity.Children)
            {
                GetEntityList(child);
            }

            return dataEntityEntityList;
        }

        public IList<string> GetAttributeList(IXsrcEntity entity)
        {
            foreach (var attribute in entity.Attributes)
            {
                xsrcAttributeList.Add(attribute.PublicName);
            }

            foreach (var child in entity.Children)
            {
                GetAttributeList(child);
            }

            return xsrcAttributeList;
        }

        public IList<string> GetAttributeList(IDataEntity entity)
        {
            foreach (var attribute in entity.Attributes)
            {
                dataEntityAttributeList.Add(attribute.Key);
            }

            foreach (var child in entity.Children)
            {
                GetAttributeList(child);
            }

            return dataEntityAttributeList;
        }

        public IDataEntity ConvertToDataEntity(IXsrcEntity xsrcEntity)
        {
            IDataEntity dataEntity =
                new DataEntity(xsrcEntity.PublicName)
                {
                    Attributes = xsrcEntity
                    .Attributes
                    .GroupBy(a => a.PublicName)
                    .ToDictionary(a => a.Key, b => new AttributeData(null) as IAttributeData),
                    Children = GetChidren(xsrcEntity.Children)
                };

            return dataEntity;
        }

        public IList<IDataEntity> GetChidren(IEnumerable<IXsrcEntity> xsrcEntities)
        {
            IList<IDataEntity> dataEntities = new List<IDataEntity>();

            foreach (var entity in xsrcEntities)
            {
                dataEntities.Add(new DataEntity(entity.PublicName)
                {
                    Attributes = entity
                    .Attributes
                    .GroupBy(a => a.PublicName)
                    .ToDictionary(a => a.Key, b => new AttributeData(null) as IAttributeData),
                    Children = GetChidren(entity.Children)
                });
            }

            return dataEntities;
        }

        public IDictionary<string, List<string>> GetXsrcEntityAttributeDictionaries(IDataEntity dataEntity)
        {
            xsrcEntityDictionary.Add(dataEntity.EntityName, dataEntity.Attributes.Keys.ToList());

            foreach (var child in dataEntity.Children)
            {
                GetXsrcEntityAttributeDictionaries(child);
            }

            return xsrcEntityDictionary;
        }

        public IDictionary<string, List<string>> GetDataEntityAttributeDictionaries(IDataEntity dataEntity)
        {
            dataEntityDictionary.Add(dataEntity.EntityName, dataEntity.Attributes.Keys.ToList());

            foreach (var child in dataEntity.Children)
            {
                GetDataEntityAttributeDictionaries(child);
            }

            return dataEntityDictionary;
        }

        public XsrcGlobal GetXSRCEntity(string folderName)
        {
            return new XsrcEntityBuilder(folderName + "\\XSRC\\Inputs.xsrc").BuildXsrc();
        }

        public IDataEntity GetDataEntityMapperEntity()
        {
            var largeEmployersRefererenceDataServiceMock = new Mock<ILargeEmployersReferenceDataService>();
            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var organisationRefererenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            var learner = new FM35LearnerDto
            {
                LearnRefNumber = "Learner1",
                PostcodePrior = "Postcode",
                LearnerEmploymentStatuses = new List<LearnerEmploymentStatus>
                {
                    new LearnerEmploymentStatus
                    {
                        EmpId = 10,
                        AgreeId = "1",
                        DateEmpStatApp = new DateTime(2018, 8, 1),
                        EmpStat = 2,
                    },
                },
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        LearnAimRef = "1",
                        AimSeqNumber = 2,
                        AimType = 3,
                        CompStatus = 4,
                        PwayCode = 5,
                        ProgType = 6,
                        FworkCode = 7,
                        FundModel = 35,
                        StdCode = 8,
                        LearnStartDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2019, 8, 1),
                        DelLocPostCode = "Postcode",
                        LearningDeliveryFAMs = new List<LearningDeliveryFAM>
                        {
                            new LearningDeliveryFAM()
                        },
                    },
                },
            };

            var frameworks = new List<LARSFramework>
            {
                new LARSFramework
                {
                    EffectiveFromNullable = new DateTime(2018, 1, 1),
                    EffectiveTo = new DateTime(2019, 1, 1),
                    FworkCode = 7,
                    ProgType = 6,
                    PwayCode = 5,
                    LARSFrameworkAim = new LARSFrameworkAim
                    {
                        EffectiveFrom = new DateTime(2018, 1, 1),
                        EffectiveTo = new DateTime(2019, 1, 1),
                        FrameworkComponentType = 1,
                    },
                    LARSFrameworkApprenticeshipFundings = new List<LARSFrameworkApprenticeshipFunding>
                    {
                        new LARSFrameworkApprenticeshipFunding()
                    },
                    LARSFrameworkCommonComponents = new List<LARSFrameworkCommonComponent>
                    {
                        new LARSFrameworkCommonComponent()
                    }
                },
                new LARSFramework
                {
                    EffectiveFromNullable = new DateTime(2018, 1, 1),
                    EffectiveTo = new DateTime(2019, 1, 1),
                    FworkCode = 9,
                    ProgType = 9,
                    PwayCode = 5,
                    LARSFrameworkAim = new LARSFrameworkAim
                    {
                        EffectiveFrom = new DateTime(2018, 1, 1),
                        EffectiveTo = new DateTime(2019, 1, 1),
                        FrameworkComponentType = 1,
                    }
                }
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                LARSAnnualValues = new List<LARSAnnualValue>
                {
                    new LARSAnnualValue()
                },
                LARSLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>
                {
                    new LARSLearningDeliveryCategory()
                },
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding()
                },
                LARSFrameworks = frameworks
            };

            var learningDelivery = learner.LearningDeliveries.First();

            largeEmployersRefererenceDataServiceMock.Setup(l => l.LargeEmployersforEmpID(It.IsAny<int>())).Returns(new List<LargeEmployers> { new LargeEmployers() });
            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            organisationRefererenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(It.IsAny<int>())).Returns(new List<OrgFunding> { new OrgFunding { OrgFundFactType = Attributes.OrgFundFactorTypeAdultSkills } });
            postcodesReferenceDataServiceMock.Setup(p => p.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(new List<SfaAreaCost> { new SfaAreaCost() });
            postcodesReferenceDataServiceMock.Setup(p => p.SFADisadvantagesForPostcode(learner.PostcodePrior)).Returns(new List<SfaDisadvantage> { new SfaDisadvantage() });

            return new DataEntityMapper(
                largeEmployersRefererenceDataServiceMock.Object,
                larsReferenceDataServiceMock.Object,
                organisationRefererenceDataServiceMock.Object,
                postcodesReferenceDataServiceMock.Object).BuildGlobalDataEntity(learner, new Global());
        }

        public string GetRulebaseVersion(string folderName)
        {
            var zipStream = new FileStream(folderName + "//" + RulebaseName + ".zip", FileMode.Open);
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

        public XmlDocument GetXmlDocumentFromZip(FileStream stream)
        {
            ZipArchive zip = new ZipArchive(stream);

            var file = zip.Entries.First(f => f.Name == RulebaseName + ".xml").Open();

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            return doc;
        }

        public XmlDocument GetXSRC(string folderName)
        {
            var stream = new FileStream(folderName + "//XSRC//" + XsrcName + ".xsrc", FileMode.Open);
            var doc = new XmlDocument();

            doc.Load(stream);
            stream.Close();

            return doc;
        }
    }
}