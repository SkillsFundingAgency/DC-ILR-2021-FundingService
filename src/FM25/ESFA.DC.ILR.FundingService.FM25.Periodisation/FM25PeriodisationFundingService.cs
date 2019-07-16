using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Periodisation.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace ESFA.DC.ILR.FundingService.FM25.Periodisation
{
    public class FM25PeriodisationFundingService : IFundingService<FM25Global, IEnumerable<PeriodisationGlobal>>
    {
        private IPeriodisationService _periodisationService;
        private FM25PeriodisationFundingService(IPeriodisationService periodisationService)
        {
            _periodisationService = periodisationService;
        }
        public IEnumerable<PeriodisationGlobal> ProcessFunding(int ukprn, IEnumerable<FM25Global> inputList, CancellationToken cancellationToken)
        {
            var result = new List<PeriodisationGlobal>();
            foreach (var global in inputList)
            {
                foreach (var learner in global.Learners)
                {
                    var periods = _periodisationService.GetPeriodisedValues(learner);

                    result.Add(new PeriodisationGlobal()
                    {
                        UKPRN = ukprn,
                        LearnerPeriodisedValues = new List<LearnerPeriodisedValues>()
                        {
                            new LearnerPeriodisedValues()
                            {
                                LearnRefNumber = learner.LearnRefNumber,
                                Period1 = periods[0],
                                Period2 = periods[1],
                                Period3 = periods[2],
                                Period4 = periods[3],
                                Period5 = periods[4],
                                Period6 = periods[5],
                                Period7 = periods[6],
                                Period8 = periods[7],
                                Period9 = periods[8],
                                Period10 = periods[9],
                                Period11 = periods[10],
                                Period12 = periods[11]
                            }
                        }
                    });
                }
            }

            return result;
        }
    }
}
