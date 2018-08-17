using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRC;
using ESFA.DC.OPA.XSRC.Model.XSRC;
using ESFA.DC.OPA.XSRC.Model.XSRCEntity;
using ESFA.DC.OPA.XSRC.Service.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;

// Setting internals visiible for unit test purposes
[assembly: InternalsVisibleTo("ESFA.DC.OPA.XSRC.Service.Tests")]

namespace ESFA.DC.OPA.XSRC.Service
{
    public class XsrcEntityBuilder : IXsrcEntityBuilder
    {
        private readonly string _xsrcInput;

        public XsrcEntityBuilder(string xsrcInput)
        {
            _xsrcInput = xsrcInput;
        }

        public XsrcGlobal BuildXsrc()
        {
            var rootEntities = Deserialize();

            return GlobalEntity(rootEntities);
        }

        protected internal IRoot Deserialize()
        {
            Stream stream = new FileStream(_xsrcInput, FileMode.Open);

            ISerializationService serializationService = new XmlSerializationService();

            IRoot rootEntities = serializationService.Deserialize<Root>(stream);

            stream.Close();

            return rootEntities;
        }

        protected internal XsrcGlobal GlobalEntity(IRoot rootEntities)
        {
            return new XsrcGlobal
            {
                GlobalEntity =
                rootEntities.RootEntities.Where(r => r.@Ref == "global")
                .Select(g => new XsrcEntity
                {
                    PublicName = g.@Ref,
                    Name = g.@Ref,
                    Attributes = g.EntityAttributes.Select(ga =>
                    new XsrcAttribute
                    {
                        PublicName = ga.PublicName,
                        Type = ga.Type,
                    }),
                    Children = GetChildren(g.@Ref, rootEntities)
                }).Single()
            };
        }

        protected internal IEnumerable<XsrcEntity> GetChildren(string parentName, IRoot rootEntities)
        {
            return
               rootEntities.RootEntities.Where(r => r.ContainmentParentId == parentName)
               .Select(c => new XsrcEntity
               {
                   PublicName = c.PublicId,
                   Name = c.Id,
                   Parent = parentName,
                   Attributes = c.EntityAttributes.Select(ca =>
                   new XsrcAttribute
                   {
                       PublicName = ca.PublicName,
                       Type = ca.Type,
                   }),
                   Children = GetChildren(c.Id, rootEntities)
               });
        }
    }
}
