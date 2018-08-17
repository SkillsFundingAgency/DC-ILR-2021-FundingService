using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;

namespace ESFA.DC.OPA.XSRC.Model.XSRC
{
    public partial class RootEntityAttributeProp : IRootEntityAttributeProp
    {
        public string Name => nameField;

        public string Values => valueField;
    }
}
