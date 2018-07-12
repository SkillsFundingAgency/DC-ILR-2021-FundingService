﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stubs
{
    public class LearnerPagingService<T> : IPagingService<T>
        where T : class
    {
        private const int PageSize = 100;

        private readonly IFundingContext _fundingContext;

        public LearnerPagingService(IFundingContext fundingContext)
        {
            _fundingContext = fundingContext;
        }

        public IEnumerable<IEnumerable<T>> BuildPages()
        {
            var validLearners = _fundingContext.ValidLearners.Cast<T>();

            return SplitList(validLearners, PageSize);
        }

        private IEnumerable<IEnumerable<T>> SplitList(IEnumerable<T> learners, int pageSize)
        {
            var learnerList = learners.ToList();

            for (var i = 0; i < learnerList.Count; i += pageSize)
            {
                yield return learnerList.Skip(i).Take(pageSize);
            }
        }
    }
}
