using System.Collections.Generic;

namespace ESFA.DC.OPA.XSRC.Model.Interface.XSRC
{
    public interface IRootEntity
    {
        IEnumerable<IRootEntityAttribute> EntityAttributes { get; }

        string @Ref { get; }

        string Id { get; }

        string Name { get; }

        string ContinmentRelationshipId { get; }

        string ContainmentParentId { get; }

        string PublicId { get; }
    }
}
