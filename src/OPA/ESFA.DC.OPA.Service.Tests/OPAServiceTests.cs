//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using ESFA.DC.OPA.Model;
//using ESFA.DC.OPA.Model.Interface;
//using ESFA.DC.OPA.Service.Builders;
//using ESFA.DC.OPA.Service.Interface;
//using ESFA.DC.OPA.Service.Interface.Rulebase;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace ESFA.DC.OPA.Service.Tests
//{
//    public class OPAServiceTests
//    {
//        [Fact]
//        public void OPAService_InitiateAndCheckEntityName()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var result = MockOPAService(TestDataEntity());

//            // ASSERT
//            result.EntityName.Should().BeEquivalentTo("Global");
//        }

//        [Fact]
//        public void OPAService_Global()
//        {
//            // ARRANGE
//            // Use Test Helpers

//            // ACT
//            var result = MockOPAService(TestDataEntity());

//            // ASSERT
//            result.EntityName.Should().Be("global");
//            result.Attributes.Count.Should().Be(16);
//            result.IsGlobal.Should().BeTrue();
//            result.Children.Single().EntityName.Should().Be("Learner");
//            result.Children.Count.Should().Be(1);
//            result.Children.Select(a => a.Attributes).Should().HaveCount(1);
//            result.Attributes["UKPRN"].Value.Should().Be(12345678);
//            result.Children.First().Attributes["LearnRefNumber"].Value.Should().Be("Learner1");
//        }

//        private IDataEntity TestDataEntity()
//        {
//            IDataEntity globalEntity = new DataEntity("Global")
//            {
//                Attributes = new Dictionary<string, IAttributeData>()
//                {
//                    { "UKPRN", new AttributeData(12345678) }
//                }
//            };

//            globalEntity.AddChild(
//                new DataEntity("Learner")
//                {
//                    Attributes = new Dictionary<string, IAttributeData>()
//                    {
//                        { "LearnRefNumber", new AttributeData("Learner1") }
//                    }
//                });

//            return globalEntity;
//        }

//        private IRulebaseStreamProvider RulebaseProviderMock()
//        {
//            var rulebaseZipPath =
//                Assembly.GetCallingAssembly().GetManifestResourceNames()
//                .Where(n => n.Contains("Rulebase"))
//                .Select(r => r).SingleOrDefault();

//            var ruleBaseProviderMock = new Mock<IRulebaseStreamProvider>();

//            ruleBaseProviderMock.Setup(m => m.GetStream()).Returns(Assembly.GetExecutingAssembly().GetManifestResourceStream(rulebaseZipPath));

//            return ruleBaseProviderMock.Object;
//        }

//        private IOPAService MockTestObject()
//        {
//            return new OPAService(new SessionBuilder(), new OPADataEntityBuilder(new DateTime(2017, 8, 1)), RulebaseProviderMock());
//        }

//        private IDataEntity MockOPAService(IDataEntity dataEntity)
//        {
//            var serviceMock = new Mock<IOPAService>();

//            serviceMock.Setup(sm => sm.ExecuteSession(dataEntity)).Returns(TestDataEntity);
//            var mockData = MockTestObject();

//            return mockData.ExecuteSession(dataEntity);
//        }
//    }
//}
