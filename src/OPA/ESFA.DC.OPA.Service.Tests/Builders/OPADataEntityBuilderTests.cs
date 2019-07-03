﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using ESFA.DC.OPA.Model;
//using ESFA.DC.OPA.Model.Interface;
//using ESFA.DC.OPA.Service.Builders;
//using ESFA.DC.OPA.Service.Interface.Builders;
//using FluentAssertions;
//using Oracle.Determinations.Engine;
//using Xunit;

//namespace ESFA.DC.OPA.Service.Tests.Builders
//{
//    public class OPADataEntityBuilderTests
//    {
//        [Fact]
//        public void DataEntityBuilder_CreateDataEntity_EntitiesCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = GetOutputEntity();

//            // ASSERT
//            outputEntity.EntityName.Should().Be("global");
//            outputEntity.Children.Select(e => e.EntityName).FirstOrDefault().Should().Be("Learner");
//        }

//        [Fact]
//        public void DataEntityBuilder_CreateDataEntity_EntitiesCountCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = GetOutputEntity();

//            // ASSERT
//            outputEntity.IsGlobal.Should().BeTrue();
//            outputEntity.Children.Select(e => e.EntityName).Count().Should().Be(1);
//        }

//        [Fact]
//        public void DataEntityBuilder_CreateDataEntity_AttributesCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = GetOutputEntity();
//            var attributes = outputEntity.Attributes;

//            // ASSERT
//            attributes["UKPRN"].Value.Should().Be(12345678);
//            attributes["LARSVersion"].Value.Should().Be("Version_005");
//            outputEntity.Children.First().Attributes["LearnRefNumber"].Value.Should().Be("TestLearner");
//        }

//        [Fact]
//        public void DataEntityBuilder_CreateDataEntity_AttributesCountCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = GetOutputEntity();

//            // ASSERT
//            outputEntity.Attributes.Count.Should().Be(16);
//            outputEntity.Children.Select(a => a.Attributes).Count().Should().Be(1);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaToEntity_GlobalExists()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = SetupMapToOpDataEntity();

//            // ASSERT
//            outputEntity.EntityName.Should().NotBeNull();
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaToEntity_GlobalNoParent()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = SetupMapToOpDataEntity();

//            // ASSERT
//            outputEntity.Parent.Should().BeNull();
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaToEntity_GlobalCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = SetupMapToOpDataEntity();

//            // ASSERT
//            outputEntity.EntityName.Should().Be("global");
//            outputEntity.Attributes.Count().Should().Be(16);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaToEntity_ChildExists()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = SetupMapToOpDataEntity();

//            // ASSERT
//            outputEntity.Children.Select(c => c.EntityName).Should().NotBeNull();
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaToEntity_ChildsParentExists()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = SetupMapToOpDataEntity();

//            // ASSERT
//            outputEntity.Children.Select(c => c.Parent).Should().NotBeNull();
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaToEntity_ChildsParentCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = SetupMapToOpDataEntity();

//            // ASSERT
//            outputEntity.Children.Select(c => c.Parent.EntityName).Should().BeEquivalentTo("global");
//            outputEntity.Children.Select(c => c.Parent.Attributes.Count).Should().BeEquivalentTo(16);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaToEntity_ChildCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var outputEntity = SetupMapToOpDataEntity();

//            // ASSERT
//            outputEntity.Children.Select(e => e.EntityName).Should().BeEquivalentTo("Learner");
//            outputEntity.Children.Select(a => a.Attributes).Count().Should().Be(1);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapAttributes_AttributesExist()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapAttributes();

//            // ASSERT
//            dataEntity.Attributes.Should().NotBeNull();
//        }

//        [Fact]
//        public void DataEntityBuilder_MapAttributes_AttributesCorrectCount()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapAttributes();

//            // ASSERT
//            dataEntity.Attributes.Count.Should().Be(16);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapAttributes_AttributesCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapAttributes();

//            // ASSERT
//            dataEntity.Attributes["UKPRN"].Value.Should().Be(12345678);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapOpaAttributesToDataEntity_Attributes()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var attributeList = SetupMapOpaAttribute();

//            // ASSERT
//            attributeList.Count.Should().Be(16);
//            attributeList["UKPRN"].Value.Should().Be(12345678);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapEntities_GlobalCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapEntities();

//            // ASSERT
//            dataEntity.EntityName.Should().Be("global");
//        }

//        [Fact]
//        public void DataEntityBuilder_MapEntities_GlobalChildrenExists()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapEntities();

//            // ASSERT
//            dataEntity.Children.Should().NotBeNull();
//        }

//        [Fact]
//        public void DataEntityBuilder_MapEntities_GlobalChildrenCount()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapEntities();

//            // ASSERT
//            dataEntity.Children.Count.Should().Be(1);
//        }

//        [Fact]
//        public void DataEntityBuilder_MapEntities_GlobalChildrenCorrect()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapEntities();

//            // ASSERT
//            dataEntity.Children.Select(e => e.EntityName).Should().BeEquivalentTo("Learner");
//        }

//        [Fact]
//        public void DataEntityBuilder_MapEntities_GlobalChildrenCorrectParent()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var dataEntity = SetupMapEntities();

//            // ASSERT
//            dataEntity.Children.Select(p => p.Parent.EntityName).Should().BeEquivalentTo("global");
//        }

//        private IDataEntity TestGlobalEntity()
//        {
//            IDataEntity globalEntity = new DataEntity("global")
//            {
//                Attributes = new Dictionary<string, IAttributeData>()
//                {
//                    { "UKPRN", new AttributeData(12345678) },
//                    { "LARSVersion", new AttributeData("Version_005") }
//                }
//            };

//            globalEntity.AddChild(
//                new DataEntity("Learner")
//                {
//                    Attributes = new Dictionary<string, IAttributeData>()
//                    {
//                        { "LearnRefNumber", new AttributeData("TestLearner") }
//                    }
//                });

//            return globalEntity;
//        }

//        private EntityInstance TestEntityInstance()
//        {
//            ISessionBuilder sessionBuilder = new SessionBuilder();
//            Session session;
//            var assembly = Assembly.GetCallingAssembly();
//            var rulebaseLocation = assembly.GetName().Name + @".Rulebase.Loans Bursary 17_18.zip";

//            using (Stream stream = assembly.GetManifestResourceStream(rulebaseLocation))
//            {
//                session = sessionBuilder.CreateOPASession(stream, TestGlobalEntity());
//            }

//            session.Think();

//            return session.GetGlobalEntityInstance();
//        }

//        private IDataEntity GetOutputEntity()
//        {
//            IOPADataEntityBuilder createDataEntity = new OPADataEntityBuilder(new DateTime(2017, 8, 1));
//            EntityInstance entityInstance = TestEntityInstance();

//            return createDataEntity.CreateOPADataEntity(entityInstance, null);
//        }

//        private IDataEntity SetupMapToOpDataEntity()
//        {
//            var mapToDataEntity = new OPADataEntityBuilder(new DateTime(2017, 8, 1));
//            EntityInstance entityInstance = TestEntityInstance();

//            return mapToDataEntity.MapOpaToEntity(entityInstance, null);
//        }

//        private IDictionary<string, IAttributeData> SetupMapOpaAttribute()
//        {
//            var builder = new OPADataEntityBuilder(new DateTime(2017, 8, 1));
//            var instance = TestEntityInstance();
//            IDictionary<string, IAttributeData> attributeDictionary = new Dictionary<string, IAttributeData>();
//            var rbAttributes = instance.GetEntity().GetAttributes();

//            foreach (RBAttr attribute in rbAttributes)
//            {
//                var attData = builder.MapOpaAttributeToDataEntity(instance, attribute);
//                attributeDictionary.Add(attribute.GetName(), attData);
//            }

//            return attributeDictionary;
//        }

//        private IDataEntity SetupMapAttributes()
//        {
//            var mapAttributes = new OPADataEntityBuilder(new DateTime(2017, 8, 1));
//            EntityInstance entityInstance = TestEntityInstance();
//            IDataEntity dataEntity = new DataEntity(entityInstance.GetEntity().GetName());

//            mapAttributes.MapAttributes(entityInstance, dataEntity);

//            return dataEntity;
//        }

//        private IDataEntity SetupMapEntities()
//        {
//            var mapEntities = new OPADataEntityBuilder(new DateTime(2017, 8, 1));
//            var instance = TestEntityInstance();
//            var childEntities = instance.GetEntity().GetChildEntities();
//            var dataEntity = new DataEntity(instance.GetEntity().GetName());

//            mapEntities.MapEntities(instance, childEntities, dataEntity);

//            return dataEntity;
//        }

//        private int DecimalStrToInt(string value)
//        {
//            var valueInt = value.Substring(0, value.IndexOf('.', 0));
//            return int.Parse(valueInt);
//        }
//    }
//}