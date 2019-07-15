using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class FM25PeriodisationFundingService : IFundingService<FM25Global, IEnumerable<PeriodisationGlobal>>
    {
        public IEnumerable<PeriodisationGlobal> ProcessFunding(int ukprn, IEnumerable<FM25Global> inputList, CancellationToken cancellationToken)
        {
            var result = new List<PeriodisationGlobal>();
            var periodisationService = new PeriodisationDateService();
            foreach (var global in inputList)
            {
                foreach (var learner in global.Learners)
                {
                    var periods = periodisationService.GetPeriodisedValues(learner);

                    result.Add(new PeriodisationGlobal()
                    {
                        UKPRN = ukprn,
                        LearnerPeriodisedValues = new List<LearnerPeriodisedValues>()
                        {
                            new LearnerPeriodisedValues()
                            {
                                LearnRefNumber = learner.LearnRefNumber,
                                AttributeName = periods.AttributeName,
                                Period1 = periods.Period1,
                                Period2 = periods.Period2,
                                Period3 = periods.Period3,
                                Period4 = periods.Period4,
                                Period5 = periods.Period5,
                                Period6 = periods.Period6,
                                Period7 = periods.Period7,
                                Period8 = periods.Period8,
                                Period9 = periods.Period9,
                                Period10 = periods.Period10,
                                Period11 = periods.Period11,
                                Period12 = periods.Period12
                            }
                        }
                    });
                }
            }

            return result;
        }
    }
}
