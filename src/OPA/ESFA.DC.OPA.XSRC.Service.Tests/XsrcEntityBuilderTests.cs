using System.Linq;
using ESFA.DC.OPA.XSRC.Service.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.OPA.XSRC.Service.Tests
{
    public class XsrcEntityBuilderTests
    {
        [Fact]
        public void Serializer_Exists()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var xsrcInput = builder.Deserialize();

            // ASSERT
            xsrcInput.Should().NotBeNull();
        }

        [Fact]
        public void Serializer_Correct()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var xsrcInput = builder.Deserialize();

            // ASSERT
            xsrcInput.RootEntities.Where(g => g.@Ref == "global").Select(n => n.@Ref).Should().BeEquivalentTo("global");
            xsrcInput.RootEntities.Select(n => n.Id).Should().BeEquivalentTo(EntityIDs());
        }

        [Fact]
        public void XSRCEntityBuilder_Global_Correct()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var global = builder.GlobalEntity(model);

            // ASSERT
            global.GlobalEntity.PublicName.Should().BeEquivalentTo("global");
            global.GlobalEntity.Children.Count().Should().Be(1);
        }

        [Fact]
        public void XSRCEntityBuilder_Child_Exists()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var child = builder.GetChildren("global", model);

            // ASSERT
            child.Should().NotBeNull();
        }

        [Fact]
        public void XSRCEntityBuilder_Child_Correct()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var child = builder.GetChildren("global", model);

            // ASSERT
            child.Select(p => p.Name).Should().BeEquivalentTo("customer");
            child.Select(c => c.Children.Count()).Should().BeEquivalentTo(1);
        }

        [Fact]
        public void XSRCEntityBuilder_GlobalAttributes_Exist()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var global = builder.GlobalEntity(model);

            // ASSERT
            global.GlobalEntity.Attributes.Select(a => a.PublicName).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void XSRCEntityBuilder_GlobalAttributes_Correct()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var global = builder.GlobalEntity(model);

            // ASSERT
            var attrobutes = global.GlobalEntity.Attributes.Select(a => a.PublicName).ToList();

            attrobutes.Select(a => a).Should().BeEquivalentTo("Version");
        }

        [Fact]
        public void XSRCEntityBuilder_GlobalAttributes_CountCorrect()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var global = builder.GlobalEntity(model);

            // ASSERT
            var attrobutes = global.GlobalEntity.Attributes.Select(a => a.PublicName).ToList();

            attrobutes.Count.Should().Be(1);
        }

        [Fact]
        public void XSRCEntityBuilder_ChildAttributes_Exist()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var child = builder.GetChildren("global", model);

            // ASSERT
            child.Select(p => p.Attributes.Select(a => a.PublicName)).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void XSRCEntityBuilder_ChildAttributes_Correct()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var child = builder.GetChildren("global", model);

            // ASSERT
            var attributes = child.SelectMany(p => p.Attributes.Select(a => a.PublicName)).ToList();

            attributes.Should().BeEquivalentTo(CustomerAttributes());
        }

        [Fact]
        public void XSRCEntityBuilder_ChildAttributes_CountCorrect()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var child = builder.GetChildren("global", model);

            // ASSERT
            var attributes = child.SelectMany(p => p.Attributes.Select(a => a.PublicName)).ToList();

            attributes.Count.Should().Be(5);
        }

        private string[] EntityIDs()
        {
            return new string[]
            {
                null,
                "customer",
                "employer",
            };
        }

        private string[] CustomerAttributes()
        {
            return new string[]
            {
                "CustomerAlive",
                "CustomerDOB",
                "CustomerFunds",
                "CustomerID",
                "CustomerName",
            };
        }
    }
}
