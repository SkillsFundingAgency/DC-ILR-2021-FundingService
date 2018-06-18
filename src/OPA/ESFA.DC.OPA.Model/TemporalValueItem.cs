using System;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Model
{
    public class TemporalValueItem : ITemporalValueItem
    {
        public TemporalValueItem(DateTime changePoint, object value, string type)
        {
            ChangePoint = changePoint;
            Value = value;
            Type = type;
        }

        public DateTime ChangePoint { get; }

        public object Value { get; }

        public string Type { get; }
    }
}
