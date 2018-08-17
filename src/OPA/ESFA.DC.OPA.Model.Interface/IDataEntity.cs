using System.Collections.Generic;

namespace ESFA.DC.OPA.Model.Interface
{
    public interface IDataEntity
    {
        string EntityName { get; }

        IDictionary<string, IAttributeData> Attributes { get; }

        IList<IDataEntity> Children { get; }

        IDataEntity Parent { get; }

        bool IsGlobal { get; }

        void AddChild(IDataEntity childDataEntity);

        void AddChildren(IEnumerable<IDataEntity> childDataEntities);
    }
}
