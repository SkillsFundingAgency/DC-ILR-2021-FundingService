﻿using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IFCSDataRetrievalService
    {
        IEnumerable<string> UniqueConRefNumbers(IMessage message);

        IDictionary<string, IEnumerable<FCSContractAllocation>> FCSContractAllocationsForUKPRN(int ukprn, IEnumerable<string> conRefNumbers);
    }
}
