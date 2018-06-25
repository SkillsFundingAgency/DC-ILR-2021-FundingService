using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stubs
{
    public class LearnerPerActorServiceStub<T, U> : ILearnerPerActorService<T, List<T>>
        where T : class
    {
        private readonly IFundingContext _fundingContext;

        public LearnerPerActorServiceStub(IFundingContext fundingContext)
        {
            _fundingContext = fundingContext;
        }

        public IEnumerable<List<T>> Process()
        {
            var validLearners = _fundingContext.ValidLearners.Cast<T>().ToList();

            var learnersPerActors = CalculateLearnersPerActor(validLearners.Count);

            return SplitList(validLearners, learnersPerActors);
        }

        private int CalculateLearnersPerActor(int totalMessagesCount)
        {
            if (totalMessagesCount <= 500)
            {
                return 100;
            }

            if (totalMessagesCount <= 1700)
            {
                return 500;
            }

            if (totalMessagesCount <= 10000)
            {
                return 1000;
            }

            if (totalMessagesCount <= 30000)
            {
                return 5000;
            }

            return 10000;
        }

        private IEnumerable<List<T>> SplitList(List<T> learners, int nSize = 30)
        {
            for (int i = 0; i < learners.Count; i += nSize)
            {
                yield return learners.GetRange(i, Math.Min(nSize, learners.Count - i));
            }
        }
    }
}
