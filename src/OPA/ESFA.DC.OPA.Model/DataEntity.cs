using System.Collections.Generic;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Model
{
    public class DataEntity : IDataEntity
    {
        private const string attributeLearnRefNumber = "LearnRefNumber";
        private const string entityNameGlobal = "global";

        private readonly List<IDataEntity> _children;

        public DataEntity(string entityName)
        {
            EntityName = entityName;
            Attributes = new Dictionary<string, IAttributeData>();
            _children = new List<IDataEntity>();
        }

        public string EntityName { get; set; }

        public IDictionary<string, IAttributeData> Attributes { get; set; }

        public IList<IDataEntity> Children => _children;

        public IDataEntity Parent { get; set; }

        public string LearnRefNumber
        {
            get
            {
                Attributes.TryGetValue(attributeLearnRefNumber, out IAttributeData attribute);

                return attribute?.Value.ToString();
            }
        }

        public bool IsGlobal => EntityName != null && EntityName.Equals(entityNameGlobal);

        public void AddChild(IDataEntity childDataEntity)
        {
            _children.Add(childDataEntity);
        }

        public void AddChildren(IEnumerable<IDataEntity> childDataEntities)
        {
            _children.AddRange(childDataEntities);
        }
    }
}
