using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;
using ESFA.DC.OPA.XSRC.Model.XSRC;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.OPA.XSRC.Model.Tests
{
    public class XsrcModelTests
    {
        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root Exists"), Trait("XSRC Model", "Unit")]
        public void XSRC_Root_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - Entities"), Trait("XSRC Model", "Unit")]
        public void XSRC_Root_Entities()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.RootEntities.Should().BeEquivalentTo(TestRootEntities());
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - Rules"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_Rules()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.Rules.Should().Be("Rules");
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - InteractiveItems"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_InteractiveItems()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.RootInteractiveItems.Should().BeEquivalentTo(TestRootInteractiveitems());
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - Relationship"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_Relationship()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.RootRelationship.Should().BeEquivalentTo(TestRootRelationships());
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - RuleFolders"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_RuleFolders()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.RuleFolders.Should().Be("Folders");
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - SchemaVersion"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_SchemaVersion()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            Root entity = TestRoot();

            // ASSERT
            entity.SchemaVersion.Should().Be(1);
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - SchemaVersionSpecified True"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_SchemaVersionSpecified_True()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            Root entity = TestRoot();

            // ASSERT
            entity.SchemaVersionSpecified.Should().Be(true);
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - SchemaVersionSpecified False"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_SchemaVersionSpecified_False()
        {
            // ARRANGE
            Root root = new Root()
            {
                entities = TestRootEntities(),
                rules = "Rules",
                interactiveitems = TestRootInteractiveitems(),
                relationships = TestRootRelationships(),
                rulefolders = "Folders",
                productversion = "Version_1"
            };

            // ACT
            Root entity = root;

            // ASSERT
            entity.SchemaVersionSpecified.Should().Be(false);
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - SchemaVersionNullable"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_SchemaVersionNullable()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.SchemaVersionNullable.Should().Be(1);
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - SchemaVersionNullable Null"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_SchemaVersionNullable_Null()
        {
            // ARRANGE
            Root root = new Root()
            {
                entities = TestRootEntities(),
                rules = "Rules",
                interactiveitems = TestRootInteractiveitems(),
                relationships = TestRootRelationships(),
                rulefolders = "Folders",
                productversion = "Version_1"
            };

            // ACT
            IRoot entity = root;

            // ASSERT
            entity.SchemaVersionNullable.Should().BeNull();
        }

        /// <summary>
        /// Return Root
        /// </summary>
        [Fact(DisplayName = "XSRC - Root - ProductVersion"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_ProductVersion()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRoot entity = TestRoot();

            // ASSERT
            entity.ProductVersion.Should().Be("Version_1");
        }

        /// <summary>
        /// Return RootEntity
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntity Exists"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntity entity = TestRootEntity();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return RootEntity
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntity - Attribute"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_Attribute()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntity entity = TestRootEntity();

            // ASSERT
            entity.EntityAttributes.Should().BeEquivalentTo(TestRootEntityAttributes());
        }

        /// <summary>
        /// Return RootEntity
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntity - Ref"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_Ref()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntity entity = TestRootEntity();

            // ASSERT
            entity.Ref.Should().Be("Ref");
        }

        /// <summary>
        /// Return RootEntity
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntity - ID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_ID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntity entity = TestRootEntity();

            // ASSERT
            entity.Id.Should().Be("ID");
        }

        /// <summary>
        /// Return RootEntity
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntity - ContainmentRelationshipID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_ContainmentRelationshipID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntity entity = TestRootEntity();

            // ASSERT
            entity.ContinmentRelationshipId.Should().Be("ContainmentRelationshipID");
        }

        /// <summary>
        /// Return RootEntity
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntity - ContainmentParentID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_ContainmentParentID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntity entity = TestRootEntity();

            // ASSERT
            entity.ContainmentParentId.Should().Be("ContainmentParentID");
        }

        /// <summary>
        /// Return RootEntity
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntity - PublicID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntity_PublicID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntity entity = TestRootEntity();

            // ASSERT
            entity.PublicId.Should().Be("PublicID");
        }

        /// <summary>
        /// Return RootEntityAttribute
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttribute Exists"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttribute_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttribute entity = TestRootEntityAttribute();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return RootEntityAttribute
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttribute - Text"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttribute_Text()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttribute entity = TestRootEntityAttribute();

            // ASSERT
            entity.AttributeText.Should().BeEquivalentTo(TestRootEntityAttributeText());
        }

        /// <summary>
        /// Return RootEntityAttribute
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttribute - Props"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttribute_Props()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttribute entity = TestRootEntityAttribute();

            // ASSERT
            entity.Properties.Should().BeEquivalentTo(TestRootEntityAttributeProps());
        }

        /// <summary>
        /// Return RootEntityAttribute
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttribute - Name"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttribute_Name()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttribute entity = TestRootEntityAttribute();

            // ASSERT
            entity.Name.Should().Be("Name");
        }

        /// <summary>
        /// Return RootEntityAttribute
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttribute - Type"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttribute_Type()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttribute entity = TestRootEntityAttribute();

            // ASSERT
            entity.Type.Should().Be("Type");
        }

        /// <summary>
        /// Return RootEntityAttribute
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttribute - PublicName"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttribute_PublicName()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttribute entity = TestRootEntityAttribute();

            // ASSERT
            entity.PublicName.Should().Be("PublicName");
        }

        /// <summary>
        /// Return RootEntityAttributeProp
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttributeProp Exists"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttributeProp_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttributeProp entity = TestRootEntityAttributeProp();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return RootEntityAttributeProp
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttributeProp - Name"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttributeProp_Name()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttributeProp entity = TestRootEntityAttributeProp();

            // ASSERT
            entity.Name.Should().Be("Name");
        }

        /// <summary>
        /// Return RootEntityAttributeProp
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttributeProp - Values"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttributeProp_Values()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttributeProp entity = TestRootEntityAttributeProp();

            // ASSERT
            entity.Values.Should().Be("Value");
        }

        /// <summary>
        /// Return RootEntityAttributeText
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttributeText Exists"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttributeText_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttributeText entity = TestRootEntityAttributeText();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return RootEntityAttributeText
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttributeText - Base"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttributeText_Base()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttributeText entity = TestRootEntityAttributeText();

            // ASSERT
            entity.Base.Should().Be("Base");
        }

        /// <summary>
        /// Return RootEntityAttributeText
        /// </summary>
        [Fact(DisplayName = "XSRC - RootEntityAttributeText - Parse"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootEntityAttributeText_Parse()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootEntityAttributeText entity = TestRootEntityAttributeText();

            // ASSERT
            entity.Parse.Should().Be("Parse");
        }

        /// <summary>
        /// Return RootInteractiveItems
        /// </summary>
        [Fact(DisplayName = "XSRC - RootInteractiveItems Exists"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootInteractiveItems_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootInteractiveitems entity = TestRootInteractiveitems();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootInteractiveItems - Folders"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootInteractiveItems_Folders()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootInteractiveitems entity = TestRootInteractiveitems();

            // ASSERT
            entity.Folders.Should().Be("Folders");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootInteractiveItems - Screens"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootInteractiveItems_Screens()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootInteractiveitems entity = TestRootInteractiveitems();

            // ASSERT
            entity.Screens.Should().Be("Screens");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootInteractiveItems - Documents"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootInteractiveItems_Documents()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootInteractiveitems entity = TestRootInteractiveitems();

            // ASSERT
            entity.Documents.Should().Be("Documents");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship Exists"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.Should().NotBeNull();
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - Source"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_Source()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.Source.Should().Be("Source");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - Target"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_Target()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.Target.Should().Be("Target");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - Text"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_Text()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.Text.Should().Be("Text");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - RelationshipID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_RelationshipID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.RelationshipId.Should().Be("RelationshipID");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - ReverseText"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_ReverseText()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.ReverseText.Should().Be("ReverseText");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - ReverseRelationshipID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_ReverseRelationshipID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.ReverseRelationshipId.Should().Be("ReverseRelationshipID");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - Type"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_Type()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.Type.Should().Be("Type");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - IsComputed"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_IsComputed()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.IsComputed.Should().Be("IsComputed");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - IsContainment"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_IsContainment()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.IsContainment.Should().Be("IsContainment");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - PublicID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_PublicID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.PublicId.Should().Be("PublicID");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - ReversePublicID"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_ReversePublicID()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.ReversePublicId.Should().Be("ReversePublicID");
        }

        /// <summary>
        /// Return RootRelationship
        /// </summary>
        [Fact(DisplayName = "XSRC - RootRelationship - Values"), Trait("XSRC Model", "Unit")]
        public void XSRC_RootRelationship_Values()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            IRootRelationship entity = TestRootRelationship();

            // ASSERT
            entity.Values.Should().Be("Value");
        }

        private RootEntity TestRootEntity()
        {
            return new RootEntity
            {
                attribute = TestRootEntityAttributes(),
                @ref = "Ref",
                id = "ID",
                name = "Name",
                containmentrelationshipid = "ContainmentRelationshipID",
                containmentparentid = "ContainmentParentID",
                publicid = "PublicID"
            };
        }

        private RootEntity[] TestRootEntities()
        {
            return new RootEntity[]
            {
                TestRootEntity(),
                TestRootEntity()
            };
        }

        private RootRelationship TestRootRelationship()
        {
            return new RootRelationship
            {
                iscomputed = "IsComputed",
                iscontainment = "IsContainment",
                publicid = "PublicID",
                relationshipid = "RelationshipID",
                reversepublicid = "ReversePublicID",
                reverserelationshipid = "ReverseRelationshipID",
                reversetext = "ReverseText",
                source = "Source",
                target = "Target",
                text = "Text",
                type = "Type",
                Value = "Value"
            };
        }

        private RootRelationship[] TestRootRelationships()
        {
            return new RootRelationship[]
            {
                TestRootRelationship(),
                TestRootRelationship()
            };
        }

        private RootEntityAttribute[] TestRootEntityAttributes()
        {
            return new RootEntityAttribute[]
            {
                TestRootEntityAttribute(),
                TestRootEntityAttribute()
            };
        }

        private RootEntityAttribute TestRootEntityAttribute()
        {
            return new RootEntityAttribute
            {
                text = TestRootEntityAttributeText(),
                props = TestRootEntityAttributeProps(),
                name = "Name",
                type = "Type",
                publicname = "PublicName"
            };
        }

        private RootEntityAttributeProp[] TestRootEntityAttributeProps()
        {
            return new RootEntityAttributeProp[]
            {
                TestRootEntityAttributeProp(),
                TestRootEntityAttributeProp()
            };
        }

        private RootEntityAttributeProp TestRootEntityAttributeProp()
        {
            return new RootEntityAttributeProp
            {
                name = "Name",
                Value = "Value"
            };
        }

        private RootEntityAttributeText TestRootEntityAttributeText()
        {
            return new RootEntityAttributeText
            {
                @base = "Base",
                parse = "Parse"
            };
        }

        private RootInteractiveitems TestRootInteractiveitems()
        {
            return new RootInteractiveitems
            {
                folders = "Folders",
                screens = "Screens",
                documents = "Documents"
            };
        }

        private Root TestRoot()
        {
            return new Root()
            {
                entities = TestRootEntities(),
                rules = "Rules",
                interactiveitems = TestRootInteractiveitems(),
                relationships = TestRootRelationships(),
                rulefolders = "Folders",
                schemaversion = 1,
                schemaversionSpecified = true,
                productversion = "Version_1"
            };
        }
    }
}
