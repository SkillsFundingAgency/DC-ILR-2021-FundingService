using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;

namespace ESFA.DC.OPA.XSRC.Model.XSRC
{
    public partial class RootRelationship : IRootRelationship
    {
        public string Source => sourceField;

        public string Target => targetField;

        public string Text => textField;

        public string RelationshipId => relationshipidField;

        public string ReverseText => reversetextField;

        public string ReverseRelationshipId => reverserelationshipidField;

        public string Type => typeField;

        public string IsComputed => iscomputedField;

        public string IsContainment => iscontainmentField;

        public string PublicId => publicidField;

        public string ReversePublicId => reversepublicidField;

        public string Values => valueField;
    }
}
