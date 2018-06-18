using ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity;
using ESFA.DC.OPA.XSRC.Model.XSRCEntity;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.OPA.XSRC.Model.Tests
{
    public class XsrcEntityModelTests
    {
        #region XsrcGlobal

        /// <summary>
        /// Return XsrcGlobal
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcGlobal Exists"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcGlobal_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcGlobal entity = TestXsrcGlobal();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return XsrcGlobalGlobal
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcGlobal GlobalEntity"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcGlobal_Name()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcGlobal entity = TestXsrcGlobal();

            // ASSERT
            entity.GlobalEntity.Should().BeEquivalentTo(TestXsrcEntity());
        }

        #endregion

        #region XsrcEntity

        /// <summary>
        /// Return XsrcEntity
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcEntity Exists"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XSRCEntity_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcEntity entity = TestXsrcEntity();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return XsrcEntity
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcEntity Name"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcEntity_Name()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcEntity entity = TestXsrcEntity();

            // ASSERT
            entity.Name.Should().Be("Name");
        }

        /// <summary>
        /// Return XsrcEntity
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcEntity PublicName"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcEntity_PublicName()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcEntity entity = TestXsrcEntity();

            // ASSERT
            entity.PublicName.Should().Be("PublicName");
        }

        /// <summary>
        /// Return XsrcEntity
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcEntity Parent"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcEntity_Parent()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcEntity entity = TestXsrcEntity();

            // ASSERT
            entity.Parent.Should().Be("Parent");
        }

        /// <summary>
        /// Return XsrcEntity
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcEntity Attributes"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcEntity_Attributes()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcEntity entity = TestXsrcEntity();

            // ASSERT
            entity.Attributes.Should().BeEquivalentTo(TestXsrcAttributes());
        }

        /// <summary>
        /// Return XsrcEntity
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcEntity Children"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcEntity_Children()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcEntity entity = TestXsrcEntity();

            // ASSERT
            entity.Children.Should().BeEquivalentTo(TestChildXsrcEntity());
        }

        #endregion

        #region XsrcAttribute

        /// <summary>
        /// Return XsrcAttribute
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcAttribute Exists"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcAttribute_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcAttribute entity = TestXsrcAttribute();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return XsrcAttribute
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcAttribute PublicName"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcAttribute_PublicName()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcAttribute entity = TestXsrcAttribute();

            // ASSERT
            entity.PublicName.Should().Be("PublicName");
        }

        /// <summary>
        /// Return XsrcAttribute
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcAttribute Value"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcAttribute_Type()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcAttribute entity = TestXsrcAttribute();

            // ASSERT
            entity.Type.Should().Be("Type");
        }

        #endregion

        #region XsrcAttributeProperty

        /// <summary>
        /// Return XsrcAttributeProperty
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcAttributeProperty Exists"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcAttributeProperty_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcAttributeProperty entity = TestXsrcAttributeProperty();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return XsrcAttributeProperty
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcAttributeProperty Name"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcAttributeProperty_Name()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcAttributeProperty entity = TestXsrcAttributeProperty();

            // ASSERT
            entity.Name.Should().Be("Name");
        }

        /// <summary>
        /// Return XsrcAttributeProperty
        /// </summary>
        [Fact(DisplayName = "XSRCEntity - XsrcAttributeProperty Value"), Trait("XSRCEntity Model", "Unit")]
        public void XSRC_XsrcAttributeProperty_Value()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IXsrcAttributeProperty entity = TestXsrcAttributeProperty();

            // ASSERT
            entity.Value.Should().Be("Value");
        }

        #endregion

        #region Test Helpers

        private XsrcGlobal TestXsrcGlobal()
        {
            return new XsrcGlobal
            {
                GlobalEntity = TestXsrcEntity()
            };
        }

        private XsrcEntity TestXsrcEntity()
        {
            return new XsrcEntity
            {
                Name = "Name",
                PublicName = "PublicName",
                Parent = "Parent",
                Attributes = TestXsrcAttributes(),
                Children = TestChildXsrcEntity()
            };
        }

        private XsrcEntity[] TestChildXsrcEntity()
        {
            return new XsrcEntity[]
            {
                new XsrcEntity
                {
                    Name = "Name",
                    PublicName = "PublicName",
                    Parent = "Parent",
                    Attributes = TestXsrcAttributes()
                }
            };
        }

        private XsrcAttribute[] TestXsrcAttributes()
        {
            return new XsrcAttribute[]
            {
                TestXsrcAttribute(),
                TestXsrcAttribute()
            };
        }

        private XsrcAttribute TestXsrcAttribute()
        {
            return new XsrcAttribute
            {
                PublicName = "PublicName",
                Type = "Type"
            };
        }

        private XsrcAttributeProperty TestXsrcAttributeProperty()
        {
            return new XsrcAttributeProperty
            {
                Name = "Name",
                Value = "Value"
            };
        }

        #endregion
    }
}
