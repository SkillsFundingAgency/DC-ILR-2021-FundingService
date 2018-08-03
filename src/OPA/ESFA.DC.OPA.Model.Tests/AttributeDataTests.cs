using System;
using System.Collections.Generic;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.OPA.Model.Tests
{
    public class AttributeDataTests
    {
        [Fact]
        public void Constructor()
        {
            var name = "Name";
            var value = "Value";

            var attributeDataResult = new AttributeData(name, value);

            attributeDataResult.Value.Should().Be(value);
        }

        [Fact]
        public void AddChangePoint()
        {
            var attributeData = new AttributeData(null, null);
            var temporalValueItem = new TemporalValueItem(new DateTime(2017, 1, 1), null, null);

            attributeData.AddChangepoint(temporalValueItem);

            attributeData.Changepoints.Should().HaveCount(1);
            attributeData.Changepoints.Should().Contain(temporalValueItem);
        }

        [Fact]
        public void AddChangePoint_Multiple()
        {
            var attributeData = new AttributeData(null, null);
            var temporalValueItemOne = new TemporalValueItem(new DateTime(2017, 1, 1), null, null);
            var temporalValueItemTwo = new TemporalValueItem(new DateTime(2017, 1, 1), null, null);

            attributeData.AddChangepoint(temporalValueItemOne);
            attributeData.AddChangepoint(temporalValueItemTwo);

            attributeData.Changepoints.Should().HaveCount(2);
            attributeData.Changepoints.Should().Contain(temporalValueItemOne);
            attributeData.Changepoints.Should().Contain(temporalValueItemTwo);
        }

        [Fact]
        public void ChangePoints_Default()
        {
            new AttributeData(null, null).Changepoints.Should().BeEmpty();
        }

        [Fact]
        public void AddChangePoints()
        {
            var temporalValueItemOne = new TemporalValueItem(new DateTime(2017, 1, 1), null, null);
            var temporalValueItemTwo = new TemporalValueItem(new DateTime(2017, 1, 1), null, null);
            var temporalValueItemThree = new TemporalValueItem(new DateTime(2017, 1, 1), null, null);

            var changePointCountValues = new List<TemporalValueItem>
            {
                temporalValueItemOne,
                temporalValueItemTwo,
                temporalValueItemThree,
            };

            var attributeData = new AttributeData(null, null);

            attributeData.AddChangepoints(changePointCountValues);

            attributeData.Changepoints.Should().HaveCount(3);
            attributeData.Changepoints.Should().Contain(new[] { temporalValueItemOne, temporalValueItemTwo, temporalValueItemThree });
        }

        [Fact]
        public void IsTemporal_True_NullValueOneCP()
        {
            IAttributeData attributeDataIsTemporalTrue = new AttributeData(null, null);

            attributeDataIsTemporalTrue.AddChangepoint(new TemporalValueItem(new DateTime(2017, 1, 1), null, null));

            attributeDataIsTemporalTrue.IsTemporal.Should().BeTrue();
        }

        [Fact]
        public void IsTemporal_False_NullValueZeroCP()
        {
            IAttributeData attributeData = new AttributeData(null, null);

            attributeData.IsTemporal.Should().BeFalse();
        }

        [Fact]
        public void IsTemporal_False_ValueZeroCP()
        {
            var attributeData = new AttributeData(null, "Not Null");

            attributeData.IsTemporal.Should().BeFalse();
        }

        [Fact]
        public void IsTemporal_False_ValueAndCP()
        {
            IAttributeData attributeData = new AttributeData(null, "Not Null");

            attributeData.AddChangepoint(new TemporalValueItem(new DateTime(2017, 1, 1), null, null));

            attributeData.IsTemporal.Should().BeFalse();
        }
    }
}
