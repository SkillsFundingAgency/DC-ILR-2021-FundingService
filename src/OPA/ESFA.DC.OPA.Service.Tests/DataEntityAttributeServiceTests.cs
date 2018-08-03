using System;
using System.Collections.Generic;
using ESFA.DC.OPA.Model;
using FluentAssertions;
using Oracle.Determinations.Engine;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.OPA.Service.Tests
{
    public class DataEntityAttributeServiceTests
    {
        [Fact]
        public void GetAttributeValue_Missing()
        {
            var dataEntity = new DataEntity(string.Empty);

            Action action = () => NewService().GetAttributeValue(dataEntity, "missing");

            action.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetAttributeValue()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = new object();

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetAttributeValue(dataEntity, attributeName).Should().BeSameAs(value);
        }

        [Fact]
        public void GetAttributeValue_Uncertain()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = default(Uncertain);

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetAttributeValue(dataEntity, attributeName).Should().BeNull();
        }

        [Fact]
        public void GetStringAttributeValue_Missing()
        {
            var dataEntity = new DataEntity(string.Empty);

            Action action = () => NewService().GetStringAttributeValue(dataEntity, "missing");

            action.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetStringAttributeValue()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = 1;

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetStringAttributeValue(dataEntity, attributeName).Should().Be("1");
        }

        [Fact]
        public void GetStringAttributeValue_Uncertain()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = default(Uncertain);

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetStringAttributeValue(dataEntity, attributeName).Should().BeNull();
        }

        [Fact]
        public void GetIntAttributeValue_Missing()
        {
            var dataEntity = new DataEntity(string.Empty);

            Action action = () => NewService().GetIntAttributeValue(dataEntity, "missing");

            action.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetIntAttributeValue_NotInt()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = "Not An Integer";

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            Action action = () => NewService().GetIntAttributeValue(dataEntity, attributeName);

            action.Should().Throw<FormatException>();
        }

        [Fact]
        public void GetIntAttributeValue_Decimal()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = 1.0m;

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetIntAttributeValue(dataEntity, attributeName).Should().Be(1);
        }

        [Fact]
        public void GetIntAttributeValue()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = 1;

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetIntAttributeValue(dataEntity, attributeName).Should().Be(1);
        }

        [Fact]
        public void GetIntAttributeValue_Large()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            long value = 154549452;

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetIntAttributeValue(dataEntity, attributeName).Should().Be(154549452);
        }

        [Fact]
        public void GetIntAttributeValue_Uncertain()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = default(Uncertain);

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetIntAttributeValue(dataEntity, attributeName).Should().BeNull();
        }

        [Fact]
        public void GetDecimalAttributeValue_Missing()
        {
            var dataEntity = new DataEntity(string.Empty);

            Action action = () => NewService().GetDecimalAttributeValue(dataEntity, "missing");

            action.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetDecimalAttributeValue_NotDecimal()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = "Not A Decimal";

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            Action action = () => NewService().GetIntAttributeValue(dataEntity, attributeName);

            action.Should().Throw<FormatException>();
        }

        [Fact]
        public void GetDecimalAttributeValue()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = 1.2m;

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetDecimalAttributeValue(dataEntity, attributeName).Should().Be(1.2m);
        }

        [Fact]
        public void GetDecimalAttributeValue_Uncertain()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = default(Uncertain);

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetDecimalAttributeValue(dataEntity, attributeName).Should().BeNull();
        }

        [Fact]
        public void GetBoolAttributeValue_Missing()
        {
            var dataEntity = new DataEntity(string.Empty);

            Action action = () => NewService().GetBoolAttributeValue(dataEntity, "missing");

            action.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetBoolAttributeValue_True()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = "true";

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetBoolAttributeValue(dataEntity, attributeName).Should().BeTrue();
        }

        [Fact]
        public void GetBoolAttributeValue_NotTrue()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = "Not true";

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetBoolAttributeValue(dataEntity, attributeName).Should().BeFalse();
        }

        [Fact]
        public void GetBoolAttributeValue_Uncertain()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = default(Uncertain);

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetBoolAttributeValue(dataEntity, attributeName).Should().BeNull();
        }

        [Fact]
        public void GetDateTimeAttributeValue_Missing()
        {
            var dataEntity = new DataEntity(string.Empty);

            Action action = () => NewService().GetDateTimeAttributeValue(dataEntity, "missing");

            action.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetDateTimeAttributeValue_NotDate()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = "Not a Date";

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            Action action = () => NewService().GetDateTimeAttributeValue(dataEntity, attributeName);

            action.Should().Throw<InvalidCastException>();
        }

        [Fact]
        public void GetDateTimeAttributeValue()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = new Date(new DateTime(2017, 1, 1));

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetDateTimeAttributeValue(dataEntity, attributeName).Should().Be(new DateTime(2017, 1, 1));
        }

        [Fact]
        public void GetDateTimeAttributeValue_Uncertain()
        {
            var attributeName = "attribute";
            var dataEntity = new DataEntity(string.Empty);

            var value = default(Uncertain);

            dataEntity.Attributes.Add(attributeName, new AttributeData(value));

            NewService().GetDateTimeAttributeValue(dataEntity, attributeName).Should().BeNull();
        }

        private DataEntityAttributeService NewService()
        {
            return new DataEntityAttributeService();
        }
    }
}
