using System;

namespace ESFA.DC.OPA.Model.Interface
{
    public interface ITemporalValueItem
    {
        DateTime ChangePoint { get; }

        object Value { get; }

        string Type { get; }
    }
}
