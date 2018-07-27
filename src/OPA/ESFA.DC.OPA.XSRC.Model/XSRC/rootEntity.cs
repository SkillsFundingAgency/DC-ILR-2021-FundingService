using System.Collections.Generic;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;

namespace ESFA.DC.OPA.XSRC.Model.XSRC
{
    public partial class RootEntity : IRootEntity
    {
        public RootEntityAttribute[] Attribute => attributeField;

        public string @Ref => refField;

        public string Id => idField;

        public string Name => nameField;

        public string ContinmentRelationshipId => containmentrelationshipidField;

        public string ContainmentParentId => containmentparentidField;

        public string PublicId => publicidField;

        public IEnumerable<IRootEntityAttribute> EntityAttributes => Attribute;
    }
}
