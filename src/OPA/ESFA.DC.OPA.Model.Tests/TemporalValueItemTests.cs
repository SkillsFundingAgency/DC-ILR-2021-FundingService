using System;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.OPA.Model.Tests
{
    public class TemporalValueItemTests
    {
        [Fact]
        public void Constructor()
        {
            var changePoint = new DateTime(2017, 1, 1);
            var value = 1;
            var type = "Type";

            var temporalValueItem = new TemporalValueItem(changePoint, value, type);

            temporalValueItem.ChangePoint.Should().Be(changePoint);
            temporalValueItem.Value.Should().Be(value);
            temporalValueItem.Type.Should().Be(type);
        }
    }
}
