using System.Collections.Generic;
using System.Linq;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.OPA.Model.Tests
{
    public class DataEntityTests
    {
        [Fact]
        public void Constructor()
        {
            var entityName = "Entity Name";
            var dataEntity = new DataEntity(entityName);

            dataEntity.EntityName.Should().Be(entityName);
            dataEntity.Attributes.Should().BeEmpty();
            dataEntity.Parent.Should().BeNull();
            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void Attributes()
        {
            DataEntity attributesExistNotEmpty = new DataEntity(null);

            attributesExistNotEmpty.Attributes.Add("name", new AttributeData("value"));

            attributesExistNotEmpty.Attributes.Should().HaveCount(1);
        }

        [Fact]
        public void Parent()
        {
            var parent = new DataEntity("Parent");
            var child = new DataEntity("Child");

            parent.AddChild(child);

            child.Parent = parent;

            parent.Children.First().Parent.Should().Be(parent);
        }

        [Fact]
        public void IsGlobal_True()
        {
            new DataEntity("global").IsGlobal.Should().BeTrue();
        }

        [Fact]
        public void IsGlobal_False()
        {
            new DataEntity("NotGlobal").IsGlobal.Should().BeFalse();
        }

        [Fact]
        public void AddChild()
        {
            var dataEntity = new DataEntity(null);
            var child = new DataEntity(null);

            dataEntity.AddChild(child);

            dataEntity.Children.Should().HaveCount(1);
            dataEntity.Children.Should().Contain(child);
        }

        [Fact]
        public void AddChildren()
        {
            var dataEntity = new DataEntity(null);
            var childOne = new DataEntity(null);
            var childTwo = new DataEntity(null);

            var children = new List<IDataEntity>()
            {
                childOne,
                childTwo
            };

            dataEntity.AddChildren(children);

            dataEntity.Children.Should().HaveCount(2);
            dataEntity.Children.Should().Contain(children);
        }
    }
}
