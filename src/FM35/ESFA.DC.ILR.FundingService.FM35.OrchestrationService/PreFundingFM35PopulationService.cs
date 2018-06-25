using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.OrchestrationService
{
    public class PreFundingFM35PopulationService : IPopulationService
    {
        private readonly IReferenceDataCachePopulationService _referenceDataCachePopulationService;
        private readonly IFileDataCachePopulationService _fileDataCachePopulationService;
        private readonly IFundingContext _fundingContext;

        public PreFundingFM35PopulationService(IReferenceDataCachePopulationService referenceDataCachePopulationService, IFundingContext fundingContext, IFileDataCachePopulationService fileDataCachePopulationService)
        {
            _referenceDataCachePopulationService = referenceDataCachePopulationService;
            _fileDataCachePopulationService = fileDataCachePopulationService;
            _fundingContext = fundingContext;
        }

        public void Populate()
        {
            var learners = _fundingContext.ValidLearners;
            IList<ILearner> learnerList = new List<ILearner>();
            HashSet<string> postcodesTempList = new HashSet<string>();
            HashSet<string> learnAimRefsList = new HashSet<string>();
            HashSet<long> orgUKPRNList = new HashSet<long>();
            HashSet<int?> lEmpIdTempList = new HashSet<int?>();
            bool added = false;

            // orgUKPRNList.Add(_fundingContext.UKPRN);

            foreach (var learner in learners)
            {
                foreach (var learningDelivery in learner.LearningDeliveries.Where(ld => ld.FundModel == 35).ToList())
                {
                    if (!added)
                    {
                        learnerList.Add(learner);
                        added = true;
                    }

                    if (added)
                    {
                        postcodesTempList.Add(learningDelivery.DelLocPostCode);
                        learnAimRefsList.Add(learningDelivery.LearnAimRef);
                    }

                    foreach (var empStatus in learner.LearnerEmploymentStatuses)
                    {
                        lEmpIdTempList.Add(empStatus.EmpIdNullable);
                    }

                    postcodesTempList.Add(learner.PostcodePrior);
                }

                added = false;
            }

            var empIdList = lEmpIdTempList.Where(x => x != null).Select(v => (int)v.Value).Distinct().ToList();
            var postcodesList = postcodesTempList.Select(p => p).Distinct().ToList();

            _referenceDataCachePopulationService.Populate(learnAimRefsList.ToList(), postcodesList, orgUKPRNList.ToList(), empIdList);

            _fileDataCachePopulationService.Populate();
        }
    }
}
