using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class AbstractLearnerPagingService
    {
        private const int PageSize = 500;

        public IEnumerable<IEnumerable<MessageLearner>> BuildPages(int fundModelFilter, IEnumerable<ILearner> learners)
        {
            var pagedLearners = learners.Where(l => l.LearningDeliveries.Any(ld => fundModelFilter == ld.FundModel)).ToList().Cast<MessageLearner>();

            return SplitList(pagedLearners, PageSize);
        }

        protected IEnumerable<MessageLearnerDestinationandProgressionDPOutcome> BuildDPOutcomes(string learnRefNumber, IEnumerable<ILearnerDestinationAndProgression> learnerDestinationAndProgressions)
        {
            return
                learnerDestinationAndProgressions
                .Where(l => l.LearnRefNumber.Equals(learnRefNumber, StringComparison.OrdinalIgnoreCase))
                .SelectMany(dp => dp.DPOutcomes).ToList() as IEnumerable<MessageLearnerDestinationandProgressionDPOutcome>;
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
