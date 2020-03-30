﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM70.Service.Constants;
using ESFA.DC.ILR.FundingService.FM70.Service.Input;
using ESFA.DC.ILR.FundingService.FM70.Service.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity;
using ESFA.DC.OPA.XSRC.Model.XSRCEntity;
using ESFA.DC.OPA.XSRC.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Tests
{
    public class RulebaseInterfaceTests
    {
        public const string AcademicYear = "1920";
        public const string RulebaseName = "ESF 1920 Funding Calc";
        public const string RulebaseFolder = "Rulebase";
        public const string RulebaseMasterFolder = "RulebaseMasterFiles";
        public const string XsrcName = "Input";

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

            GetAttributeList(dateEntityMapperEntities).OrderBy(a => a)
                .Should().BeEquivalentTo(GetAttributeList(xsrcEntities).OrderBy(a => a));
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
                Attributes.EntityLearner,
                Attributes.EntityDPOutcome,
                Attributes.EntityLearnerEmploymentStatus,
                Attributes.EntityLearningDelivery,
                Attributes.EntityLearningDeliveryFAM,
                Attributes.EntityESFData,
                Attributes.EntityLearningDeliverySFA_PostcodeAreaCost,
                Attributes.EntityLearningDeliveryLARSFunding,
                Attributes.EntityLearningDeliveryLARS_AnnualValue,
            };
        }

        public IList<string> ExpectedAttributes()
        {
            return new List<string>
            {
                Attributes.UKPRN,
                Attributes.LearnRefNumber,
                Attributes.DateOfBirth,
                Attributes.AchDate,
                Attributes.AddHours,
                Attributes.AimSeqNumber,
                Attributes.CalcMethod,
                Attributes.CompStatus,
                Attributes.ConRefNumber,
                Attributes.GenreCode,
                Attributes.LearnActEndDate,
                Attributes.LearnAimRef,
                Attributes.LearnPlanEndDate,
                Attributes.LearnStartDate,
                Attributes.OrigLearnStartDate,
                Attributes.OtherFundAdj,
                Attributes.Outcome,
                Attributes.PriorLearnFundAdj,
                Attributes.DateEmpStatApp,
                Attributes.EmpStat,
                Attributes.OutCode,
                Attributes.OutType,
                Attributes.OutCollDate,
                Attributes.OutStartDate,
                Attributes.OutEndDate,
                Attributes.LearnDelAnnValBasicSkillsTypeCode,
                Attributes.LearnDelAnnValDateFrom,
                Attributes.LearnDelAnnValDateTo,
                Attributes.LARSFundingCategory,
                Attributes.LARSFundingEffectiveFrom,
                Attributes.LARSFundingEffectiveTo,
                Attributes.LARSFundingWeightedRate,
                Attributes.AreaCosFactor,
                Attributes.AreaCosEffectiveFrom,
                Attributes.AreaCosEffectiveTo,
                Attributes.UnitCost,
                Attributes.ESFDeliverableCode,
                Attributes.ESFDataPremiumFactor,
                Attributes.EffectiveContractStartDate,
                Attributes.EffectiveContractEndDate,
                Attributes.LearnDelFAMType,
                Attributes.LearnDelFAMCode,
                Attributes.LearnDelFAMDateFrom,
                Attributes.LearnDelFAMDateTo
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
            return new XsrcEntityBuilder(folderName + "\\XSRC\\" + XsrcName + ".xsrc").BuildXsrc();
        }

        public IDataEntity GetDataEntityMapperEntity()
        {
            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var fcsReferenceDataServiceMock = new Mock<IFCSReferenceDataService>();

            var learner = new FM70LearnerDto
            {
                LearnRefNumber = "Learner1",
                LearnerEmploymentStatuses = new List<LearnerEmploymentStatus>
                {
                    new LearnerEmploymentStatus
                    {
                        EmpId = 10,
                        DateEmpStatApp = new DateTime(2019, 8, 1),
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
                        ConRefNumber = "Conref",
                        PwayCode = 5,
                        ProgType = 6,
                        FworkCode = 7,
                        FundModel = 70,
                        StdCode = 8,
                        LearnStartDate = new DateTime(2019, 8, 1),
                        LearnPlanEndDate = new DateTime(2020, 8, 1),
                        DelLocPostCode = "Postcode",
                        LearningDeliveryFAMs = new List<LearningDeliveryFAM>
                        {
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "RES",
                                LearnDelFAMDateFrom = new DateTime(2019, 8, 1)
                            }
                        },
                    },
                },
                DPOutcomes = new List<DPOutcome>
                {
                    new DPOutcome
                    {
                        OutCode = 1,
                        OutType = "2",
                    }
                }
            };

            var fcsContract = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable()
                    }
                }
            };

            var sfaAreaCosts = new List<SfaAreaCost>
            {
                new SfaAreaCost
                {
                    Postcode = "Postcode",
                    AreaCostFactor = 1.2m,
                    EffectiveFrom = new DateTime(2000, 8, 1)
                }
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                LARSAnnualValues = new List<LARSAnnualValue>
                {
                    new LARSAnnualValue()
                },
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding()
                }
            };

            var larsFunding = new List<LARSFunding>
            {
                new LARSFunding()
            };

            var learningDelivery = learner.LearningDeliveries.First();

            larsRefererenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            postcodesReferenceDataServiceMock.Setup(p => p.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(sfaAreaCosts);
            fcsReferenceDataServiceMock.Setup(p => p.FcsContractsForConRef(learningDelivery.ConRefNumber)).Returns(fcsContract);

            return new DataEntityMapper(
                fcsReferenceDataServiceMock.Object,
                larsRefererenceDataServiceMock.Object,
                postcodesReferenceDataServiceMock.Object)
                .BuildGlobalDataEntity(learner, new Global());
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