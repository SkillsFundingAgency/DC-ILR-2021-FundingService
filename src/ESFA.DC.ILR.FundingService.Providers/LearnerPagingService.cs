﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers
{
    public class LearnerPagingService : ILearnerPagingService
    {
        private const int PageSize = 500;

        public IEnumerable<IEnumerable<MessageLearner>> BuildPages(IEnumerable<int> fundModelFilter, IEnumerable<ILearner> learners)
        {
            var pagedLearners = learners.Where(l => l.LearningDeliveries.Any(ld => fundModelFilter.Contains(ld.FundModel))).ToList().Cast<MessageLearner>();

            return SplitList(pagedLearners, PageSize);
        }

        private IEnumerable<IEnumerable<MessageLearner>> SplitList(IEnumerable<MessageLearner> learners, int pageSize)
        {
            var learnerList = learners.ToList();

            for (var i = 0; i < learnerList.Count; i += pageSize)
            {
                yield return learnerList.Skip(i).Take(pageSize);
            }
        }
    }
}
