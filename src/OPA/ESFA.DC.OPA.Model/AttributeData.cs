using System.Collections.Generic;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Model
{
    public class AttributeData : IAttributeData
    {
        private readonly List<ITemporalValueItem> _changePoints;

        public AttributeData(string name, object value)
        {
            Name = name;
            Value = value;
            _changePoints = new List<ITemporalValueItem>();
        }

        public string Name { get; set; }

        public IList<ITemporalValueItem> Changepoints => _changePoints;

        public object Value { get; set; }

        public bool IsTemporal => (Value == null) && (Changepoints.Count > 0);

        public void AddChangepoint(ITemporalValueItem temporalValue)
        {
            _changePoints.Add(temporalValue);
        }

        public void AddChangepoints(IEnumerable<ITemporalValueItem> temporalValues)
        {
            _changePoints.AddRange(temporalValues);
        }
    }
}
