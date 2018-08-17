using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;

namespace ESFA.DC.OPA.XSRC.Model.XSRC
{
    public partial class RootInteractiveitems : IRootInteractiveitems
    {
        public string Folders => foldersField;

        public string Screens => screensField;

        public string Documents => documentsField;
   }
}
