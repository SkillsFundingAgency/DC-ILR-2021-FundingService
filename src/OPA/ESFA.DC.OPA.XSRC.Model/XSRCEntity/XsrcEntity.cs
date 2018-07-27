using System.Collections.Generic;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity;

namespace ESFA.DC.OPA.XSRC.Model.XSRCEntity
{
    public class XsrcEntity : IXsrcEntity
    {
        public string Name { get; set; }

        public string PublicName { get; set; }

        public string Parent { get; set; }

        public IEnumerable<IXsrcAttribute> Attributes { get; set; }

        public IEnumerable<IXsrcEntity> Children { get; set; }
    }
}
