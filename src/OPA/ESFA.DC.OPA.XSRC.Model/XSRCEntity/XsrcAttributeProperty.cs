using ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity;

namespace ESFA.DC.OPA.XSRC.Model.XSRCEntity
{
    public class XsrcAttributeProperty : IXsrcAttributeProperty
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}
