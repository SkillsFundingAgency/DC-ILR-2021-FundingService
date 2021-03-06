﻿using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface ILearnerPagingService<T>
    {
        IEnumerable<IEnumerable<T>> ProvideDtos(IEnumerable<int> fundModelFilter, IMessage message);
    }
}
