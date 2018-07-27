using System.Collections.Generic;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;

namespace ESFA.DC.OPA.XSRC.Model.XSRC
{
    public partial class Root : IRoot
    {
        public RootEntity[] Entities => entitiesField;

        public string Rules => rulesField;

        public RootInteractiveitems InteractiveItems => interactiveitemsField;

        public RootRelationship[] Relationship => relationshipsField;

        public string RuleFolders => rulefoldersField;

        public sbyte SchemaVersion => schemaversionField;

        public bool SchemaVersionSpecified => schemaversionFieldSpecified;

        public sbyte? SchemaVersionNullable => schemaversionFieldSpecified ? (sbyte?)schemaversionField : null;

        public string ProductVersion => productversionField;

        public IEnumerable<IRootEntity> RootEntities => Entities;

        public IRootInteractiveitems RootInteractiveItems => InteractiveItems;

        public IEnumerable<IRootRelationship> RootRelationship => Relationship;
    }
}