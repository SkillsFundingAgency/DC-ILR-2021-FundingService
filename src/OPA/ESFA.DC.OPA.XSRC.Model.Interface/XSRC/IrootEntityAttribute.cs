using System.Collections.Generic;

namespace ESFA.DC.OPA.XSRC.Model.Interface.XSRC
{
    public interface IRootEntityAttribute
    {
        IRootEntityAttributeText AttributeText { get; }

        IEnumerable<IRootEntityAttributeProp> Properties { get; }

        string Name { get; }

        string Type { get; }

        string PublicName { get; }
    }
}
