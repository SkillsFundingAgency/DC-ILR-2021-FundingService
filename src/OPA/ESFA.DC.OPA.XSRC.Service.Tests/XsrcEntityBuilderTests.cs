using System.Linq;
using ESFA.DC.OPA.XSRC.Service.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.OPA.XSRC.Service.Tests
{
    public class XsrcEntityBuilderTests
    {
        #region Serializer Tests

        /// <summary>
        /// Return Xsrc Input Model from XSRC file
        /// </summary>
        [Fact(DisplayName = "XSRC Serializer - Model Exists"), Trait("XSRC Model Entity", "Unit")]
        public void Serializer_Exists()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var xsrcInput = builder.Deserialize();

            // ASSERT
            xsrcInput.Should().NotBeNull();
        }

        /// <summary>
        /// Return Xsrc Input Model from XSRC file
        /// </summary>
        [Fact(DisplayName = "XSRC Serializer - Model Correct"), Trait("XSRC Model Entity", "Unit")]
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

        #endregion

        #region XSRC Entity Builder Tests

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - BuildXsrc Exists"), Trait("XSRC Model Entity", "Unit")]
        public void XSRCEntityBuilder_BuildXsrc_Exists()
        {
            // ARRANGE
            IXsrcEntityBuilder builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var global = builder.BuildXsrc();

            // ASSERT
            global.Should().NotBeNull();
        }

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - BuildXsrc Correct"), Trait("XSRC Model Entity", "Unit")]
        public void XSRCEntityBuilder_BuildXsrc_Correct()
        {
            // ARRANGE
            IXsrcEntityBuilder builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var global = builder.BuildXsrc();

            // ASSERT
            global.GlobalEntity.PublicName.Should().BeEquivalentTo("global");
            global.GlobalEntity.Children.Count().Should().Be(1);
        }

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Global Exists"), Trait("XSRC Model Entity", "Unit")]
        public void XSRCEntityBuilder_Global_Exists()
        {
            // ARRANGE
            var builder = new XsrcEntityBuilder(@"Rulebase\Inputs.xsrc");

            // ACT
            var model = builder.Deserialize();
            var global = builder.GlobalEntity(model);

            // ASSERT
            global.Should().NotBeNull();
        }

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Global Correct"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Child Exists"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Child Correct"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Global Attributes Exist"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Global Attributes Correct"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Global Attributes Count Correct"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Child Attributes Exist"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Child Attributes Correct"), Trait("XSRC Model Entity", "Unit")]
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

        /// <summary>
        /// Return Xsrc Entity Model from XSRC Input
        /// </summary>
        [Fact(DisplayName = "XSRC EntityBuilder - Child Attributes Count Correct"), Trait("XSRC Model Entity", "Unit")]
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

        #endregion

        #region Test Helpers

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

        #endregion
    }
}
