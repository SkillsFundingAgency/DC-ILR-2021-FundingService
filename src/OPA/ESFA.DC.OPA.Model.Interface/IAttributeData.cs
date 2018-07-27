using System.Collections.Generic;

namespace ESFA.DC.OPA.Model.Interface
{
    public interface IAttributeData
    {
        string Name { get; }

        IList<ITemporalValueItem> Changepoints { get; }

        object Value { get; }

        bool IsTemporal { get; }

        void AddChangepoint(ITemporalValueItem temporalValue);

        void AddChangepoints(IEnumerable<ITemporalValueItem> temporalValues);
    }
}
