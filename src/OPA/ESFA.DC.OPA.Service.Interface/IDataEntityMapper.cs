﻿using System.Collections.Generic;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Service.Interface
{
    public interface IDataEntityMapper<T>
    {
        IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<T> inputModels);
    }
}
