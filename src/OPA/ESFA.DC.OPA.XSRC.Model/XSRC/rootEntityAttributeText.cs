using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;

namespace ESFA.DC.OPA.XSRC.Model.XSRC
{
    public partial class RootEntityAttributeText : IRootEntityAttributeText
    {
        public string @Base => baseField;

        public string Parse => parseField;
    }
}
