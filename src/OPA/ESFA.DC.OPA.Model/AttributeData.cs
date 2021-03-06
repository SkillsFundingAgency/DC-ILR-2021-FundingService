﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Model
{
    public class AttributeData : IAttributeData
    {
        private readonly List<ITemporalValueItem> _changePoints;

        public AttributeData(object value)
        {
            Value = value;
            _changePoints = new List<ITemporalValueItem>();
        }

        public IEnumerable<ITemporalValueItem> Changepoints => _changePoints;

        public object Value { get; set; }

        public bool IsTemporal => Value == null && _changePoints.Any();

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
