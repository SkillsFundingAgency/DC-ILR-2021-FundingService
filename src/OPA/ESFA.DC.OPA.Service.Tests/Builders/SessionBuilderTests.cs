using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface.Builders;
using FluentAssertions;
using Oracle.Determinations.Engine;
using Oracle.Determinations.Masquerade.IO;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.OPA.Service.Tests.Builders
{
    public class SessionBuilderTests
    {
        [Fact]
        public void CreateOPASession_SessionExists()
        {
            // ARRANGE
            ISessionBuilder sessionBuilder = new SessionBuilder();

            // ACT
            var session = CreateSessionTestSession(sessionBuilder);

            // ASSERT
            session.Should().NotBeNull();
        }

        [Fact]
        public void SessionBuilder_MapToOPA_InstancesExists()
        {
            // ARRANGE
            ISessionBuilder createSession1 = new SessionBuilder();
            ISessionBuilder createSession2 = new SessionBuilder();

            // ACT
            var session1 = CreateSessionTestSession(createSession1);
            var session2 = CreateSessionTestSession(createSession2);

            // ASSERT
            session1.Should().NotBeNull();
            session2.Should().NotBeNull();

            session2.Should().NotBeSameAs(session1);
        }

        [Fact]
        public void SessionBuilder_CreateSession_rulebaseNotInitialised()
        {
            // ARRANGE
            SessionBuilder sessionRBNotInitialised = new SessionBuilder();

            // ACT
            var rbInitPre = sessionRBNotInitialised.RulebaseInitialised;
            CreateSessionTestSession(sessionRBNotInitialised);
            var rbInitPost = sessionRBNotInitialised.RulebaseInitialised;

            // ASSERT
            rbInitPre.Should().BeFalse();
            rbInitPost.Should().BeTrue();
        }

        [Fact]
        public void SessionBuilder_CreateSession_rulebaseInitialised()
        {
            // ARRANGE
            SessionBuilder sessionRBInitialised = new SessionBuilder();

            // ACT
            var rbInitPreFirst = sessionRBInitialised.RulebaseInitialised;
            CreateSessionTestSession(sessionRBInitialised);
            var rbInitPostFirst = sessionRBInitialised.RulebaseInitialised;

            var rbInitPreSecond = sessionRBInitialised.RulebaseInitialised;
            CreateSessionTestSession(sessionRBInitialised);
            var rbInitPostSecond = sessionRBInitialised.RulebaseInitialised;

            // ASSERT
            rbInitPreFirst.Should().BeFalse();
            rbInitPostFirst.Should().BeTrue();
            rbInitPreSecond.Should().BeTrue();
            rbInitPostSecond.Should().BeTrue();
        }

        [Fact]
        public void SessionBuilder_CreateSession_RulebaseExists()
        {
            // ARRANGE
            SessionBuilder sessionRBExists = new SessionBuilder();

            // ACT
            CreateSessionTestSession(sessionRBExists);

            // ASSERT
            sessionRBExists.Rulebase.Should().NotBeNull();
        }

        [Fact]
        public void SessionBuilder_CreateSession_RulebaseCorrect()
        {
            // ARRANGE
            SessionBuilder sessionRBCorrect = new SessionBuilder();

            // ACT
            CreateSessionTestSession(sessionRBCorrect);
            var rulebaseName = sessionRBCorrect.Rulebase.GetBaseFileName();

            // ASSERT
            rulebaseName.Should().BeEquivalentTo("Loans Bursary 17_18");
        }

        [Fact]
        public void SessionBuilder_CreateSession_EngineCorrect()
        {
            // ARRANGE
            SessionBuilder sessionEngineCorrect = new SessionBuilder();

            // ACT
            CreateSessionTestSession(sessionEngineCorrect);

            Engine.INSTANCE.GetVersion().Should().BeEquivalentTo("10.4.4.21");
        }

        [Fact]
        public void SessionBuilder_MapGlobalDataEntityToOpa_InstanceExists()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var session = MapToOPATestSession();

            // ACT
            sessionBuilder.MapGlobalDataEntityToOpa(TestGlobalEntity(), session, session.GetGlobalEntityInstance());

            // ASSERT
            session.Should().NotBeNull();
        }

        [Fact]
        public void SessionBuilder_MapGlobalDataEntityToOpa_GlobalEntityUpdated()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var sessionPre = MapToOPATestSession();
            var sessionPost = MapToOPATestSession();

            // ACT
            sessionBuilder.MapGlobalDataEntityToOpa(TestGlobalEntity(), sessionPost, sessionPost.GetGlobalEntityInstance());
            var ukprnPre = AttributeValue(sessionPre, "UKPRN");
            var ukprnPost = AttributeValue(sessionPost, "UKPRN");

            // ASSERT
            sessionPost.Should().NotBe(sessionPre);
            ukprnPre.Should().BeNull();
            ukprnPost.Should().Be(12345678);
        }

        [Fact]
        public void SessionBuilder_MapGlobalDataEntityToOpa_GlobalEntityChildrenUpdated()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var sessionPre = MapToOPATestSession();
            var sessionPost = MapToOPATestSession();

            // ACT
            sessionBuilder.MapGlobalDataEntityToOpa(TestGlobalEntity(), sessionPost, sessionPost.GetGlobalEntityInstance());
            var learnerPre = EntityList(sessionPre);
            var learnerPost = EntityList(sessionPost);

            var learnerPrelist = EntityInstanceList(sessionPre, learnerPre);
            var learnerPostlist = EntityInstanceList(sessionPost, learnerPost);

            // ASSERT
            sessionPost.Should().NotBe(sessionPre);
            learnerPrelist.Should().BeNullOrEmpty();
            learnerPostlist.Count.Should().Be(1);
        }

        [Fact]
        public void SessionBuilder_MapDataEntityToOpa_EntityIsGlobal()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var session = MapToOPATestSession();

            // ACT
            var instance = sessionBuilder.MapDataEntityToOpa(new DataEntity("global"), session, session.GetGlobalEntityInstance());

            // ASSERT
            instance.Should().NotBeNull();
            instance.GetEntity().IsGlobal().Should().BeTrue();
        }

        [Fact]
        public void SessionBuilder_MapDataEntityToOpa_EntityIsNotGlobal()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var session = MapToOPATestSession();

            // ACT
            var instance = sessionBuilder.MapDataEntityToOpa(new DataEntity("Learner"), session, session.GetGlobalEntityInstance());

            // ASSERT
            instance.Should().NotBeNull();
            instance.GetEntity().IsGlobal().Should().BeFalse();
        }

        [Fact]
        public void SessionBuilder_MapDataEntityToOpa_ChildEntityUpdated()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var sessionPre = MapToOPATestSession();
            var sessionPost = MapToOPATestSession();

            // ACT
            sessionBuilder.MapDataEntityToOpa(TestGlobalEntity(), sessionPost, sessionPost.GetGlobalEntityInstance());
            var learnerPre = EntityList(sessionPre);
            var learnerPost = EntityList(sessionPost);

            var learnerPrelist = EntityInstanceList(sessionPre, learnerPre);
            var learnerPostlist = EntityInstanceList(sessionPost, learnerPost);

            // ASSERT
            sessionPost.Should().NotBe(sessionPre);
            learnerPrelist.Should().BeNullOrEmpty();
            learnerPostlist.Count.Should().Be(1);
        }

        [Fact]
        public void SessionBuilder_SetAttribute_AttributeCorrect()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var session = MapToOPATestSession();
            var instance = session.GetGlobalEntityInstance();
            var entity = instance.GetEntity();
            var attributeData = new AttributeData(12345678);

            // ACT
            sessionBuilder.SetAttribute(entity, instance, "UKPRN", attributeData);

            // ASSERT
            var ukprn = AttributeValue(session, "UKPRN");

            ukprn.Should().BeEquivalentTo(12345678);
        }

        [Fact]
        public void SessionBuilder_SetAttribute_AttributeAndChangePointValuesNull()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var session = MapToOPATestSession();
            var instance = session.GetGlobalEntityInstance();
            var entity = instance.GetEntity();
            var attributeData = new AttributeData(null);

            // ACT
            sessionBuilder.SetAttribute(entity, instance, "UKPRN", attributeData);

            // ASSERT
            var ukprn = AttributeValue(session, "UKPRN");

            ukprn.Should().BeNull();
        }

        [Fact]
        public void SessionBuilder_SetAttribute_ChangePointValuesExist()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();
            var session = MapToOPATestSession();
            var instance = session.GetGlobalEntityInstance();
            var entity = instance.GetEntity();

            var attributeData = new AttributeData(12345678);
            attributeData.AddChangepoints(TestChangePoints());

            // ACT
            sessionBuilder.SetAttribute(entity, instance, "UKPRN", attributeData);

            // ASSERT
            var ukprnChangePoint = entity.GetAttribute("UKPRN").GetValue(instance);

            ukprnChangePoint.Should().NotBeNull();
            ukprnChangePoint.ToString().Should().BeEquivalentTo("{unknown, 100.0 from 2017-08-01, 100.0 from 2017-09-01}");
        }

        [Fact]
        public void SessionBuilder_MapTemporal_Exists()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();

            var attributeData = new AttributeData(12345678);
            attributeData.AddChangepoints(TestChangePoints());

            // ACT
            var temporal = sessionBuilder.MapTemporalValue(attributeData.Changepoints);

            // ASSERT
            temporal.Should().NotBeNull();
        }

        [Fact]
        public void SessionBuilder_MapTemporal_Count()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();

            var attributeData = new AttributeData(12345678);
            attributeData.AddChangepoints(TestChangePoints());

            // ACT
            var temporal = sessionBuilder.MapTemporalValue(attributeData.Changepoints);

            // ASSERT
            temporal.Count.Should().Be(2);
        }

        [Fact]
        public void SessionBuilder_MapTemporal_Correct()
        {
            // ARRANGE
            var sessionBuilder = new SessionBuilder();

            var attributeData = new AttributeData(12345678);
            attributeData.AddChangepoints(TestChangePoints());

            // ACT
            var temporal = sessionBuilder.MapTemporalValue(attributeData.Changepoints);

            // ASSERT
            temporal.Should().NotBeNull();
            temporal[0].ToString().Should().BeEquivalentTo("100.0@2017-08-01");
            temporal[1].ToString().Should().BeEquivalentTo("100.0@2017-09-01");
        }

        private IDataEntity TestGlobalEntity()
        {
            IDataEntity globalEntity = new DataEntity("global")
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { "UKPRN", new AttributeData(12345678) }
                }
            };

            globalEntity.AddChild(
                new DataEntity("Learner")
                {
                    Attributes = new Dictionary<string, IAttributeData>()
                    {
                        { "LearnRefNumber", new AttributeData("Learner1") }
                    }
                });

            return globalEntity;
        }

        private IEnumerable<ITemporalValueItem> TestChangePoints() =>
            new List<ITemporalValueItem>()
            {
                new TemporalValueItem(DateTime.Parse("2017-08-01"), 100, "currency"),
                new TemporalValueItem(DateTime.Parse("2017-09-01"), 100, "currency")
            };

        private Stream Stream() => Assembly.GetExecutingAssembly().GetManifestResourceStream(@"ESFA.DC.OPA.Service.Tests.Rulebase.Loans Bursary 17_18.zip");

        private Session CreateSessionTestSession(ISessionBuilder sessionBuilder)
        {
            using (var stream = Stream())
            {
                return sessionBuilder.CreateOPASession(stream, TestGlobalEntity());
            }
        }

        private Session MapToOPATestSession()
        {
            using (var stream = Stream())
            {
                Oracle.Determinations.Engine.Rulebase testRulebase = Engine.INSTANCE.GetRulebase(new InputStreamAdapter(stream));
                return Engine.INSTANCE.CreateSession(testRulebase);
            }
        }

        private object AttributeValue(Session session, string atttributeName)
        {
            var obj = session.GetGlobalEntityInstance().GetEntity()
                .GetAttribute(atttributeName).GetValue(session.GetGlobalEntityInstance());

            return obj;
        }

        private List EntityList(Session session)
        {
            var entities = session.GetGlobalEntityInstance().GetEntity().GetChildEntities();

            return entities;
        }

        private List<EntityInstance> EntityInstanceList(Session session, List entityList)
        {
            List<EntityInstance> entityInstanceList = new List<EntityInstance>();

            foreach (Entity childEntity in entityList)
            {
                var instance = session.GetGlobalEntityInstance().GetChildren(childEntity);
                foreach (EntityInstance entityInstance in instance)
                {
                    entityInstanceList.Add(entityInstance);
                }
            }

            return entityInstanceList;
        }
    }
}
