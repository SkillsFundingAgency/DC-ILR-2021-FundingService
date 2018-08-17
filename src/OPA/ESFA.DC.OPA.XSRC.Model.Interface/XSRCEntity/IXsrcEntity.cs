using System.Collections.Generic;

namespace ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity
{
    public interface IXsrcEntity
    {
        string Name { get; }

        string PublicName { get; }

        string Parent { get; }

        IEnumerable<IXsrcAttribute> Attributes { get; }

        IEnumerable<IXsrcEntity> Children { get; }
    }
}
