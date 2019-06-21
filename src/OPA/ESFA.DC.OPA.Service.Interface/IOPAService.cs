using System;
using System.IO;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Service.Interface
{
    public interface IOPAService<T>
    {
        IDataEntity ExecuteSession(IDataEntity globalEntity);
    }
}
