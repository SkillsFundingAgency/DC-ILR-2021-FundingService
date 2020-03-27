using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class FM25FundingOutputCondenserService : IFM25FundingOutputCondenserService<FM25Global, PeriodisationGlobal>
    {
        public FM25Global Condense(IEnumerable<FM25Global> fundingOutputs, int ukprn, string year)
        {
            var firstOutput = fundingOutputs.FirstOrDefault();

            if (firstOutput != null)
            {
                firstOutput.Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(o => o.Learners).ToList();

                return firstOutput;
            }

            return new FM25Global
            {
                UKPRN = ukprn,
            };
        }

        public FM25Global CondensePeriodisationResults(IEnumerable<FM25Global> globals, IEnumerable<PeriodisationGlobal> periodisationGlobals)
        {
            var first = globals.FirstOrDefault();

            var emptyLearnerPeriodsList = new List<LearnerPeriod>();
            var emptyLearnerPeriodisedValuesList = new List<LearnerPeriodisedValues>();

            if (first != null)
            {
                var learners = globals.SelectMany(g => g.Learners).ToList();
                var learnerPeriodsDictionary = periodisationGlobals.SelectMany(pg => pg.LearnerPeriods).GroupBy(lp => lp.LearnRefNumber).ToDictionary(lp => lp.Key, lp => lp.ToList());
                var learnerPeriodisedValuesDictionary = periodisationGlobals.SelectMany(pg => pg.LearnerPeriodisedValues).GroupBy(lp => lp.LearnRefNumber).ToDictionary(lp => lp.Key, lp => lp.ToList());

                foreach (var learner in learners)
                {
                    learnerPeriodsDictionary.TryGetValue(learner.LearnRefNumber, out var matchingLearnerPeriods);
                    learnerPeriodisedValuesDictionary.TryGetValue(learner.LearnRefNumber, out var matchinglearnerPeriodisedValues);

                    learner.LearnerPeriods = matchingLearnerPeriods ?? emptyLearnerPeriodsList;
                    learner.LearnerPeriodisedValues = matchinglearnerPeriodisedValues ?? emptyLearnerPeriodisedValuesList;
                }

                first.Learners = learners;

                return first;
            }

            return new FM25Global();
        }
    }
}
