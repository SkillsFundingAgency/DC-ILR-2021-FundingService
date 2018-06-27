using System.Collections.Generic;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Service.Interface
{
    public interface IDataEntityMapper<T, U>
    {
        IEnumerable<IDataEntity> MapTo(IEnumerable<T> inputModels);

        U MapFrom(IEnumerable<IDataEntity> inputModels);
    }
}
