using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Providers.Constants;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class AbstractLearnerPagingService
    {
        private const int PageSize = 500;

        public IEnumerable<IEnumerable<MessageLearner>> BuildPages(IEnumerable<int> fundModelFilter, IEnumerable<ILearner> learners)
        {
            var pagedLearners = learners?.Where(l => l.LearningDeliveries.Any(ld => fundModelFilter.Contains(ld.FundModel))).ToList().Cast<MessageLearner>() ?? Enumerable.Empty<MessageLearner>();

            return SplitList(pagedLearners, PageSize);
        }

        protected IDictionary<string, List<DPOutcome>> BuildLearnerDPOutcomeDictionary(IEnumerable<ILearnerDestinationAndProgression> learnerDestinationAndProgressions)
        {
            return
                learnerDestinationAndProgressions?
                .ToDictionary(
                    l => l.LearnRefNumber,
                    dp => dp.DPOutcomes?.Select(dpo => new DPOutcome
                    {
                        OutCode = dpo.OutCode,
                        OutCollDate = dpo.OutCollDate,
                        OutType = dpo.OutType,
                        OutStartDate = dpo.OutStartDate,
                        OutEndDate = dpo.OutEndDateNullable
                    }).ToList(), StringComparer.OrdinalIgnoreCase) ?? new Dictionary<string, List<DPOutcome>>();
        }

        protected IDictionary<string, LearnerFAMDenormalized> BuildLearnerFAMDictionary(IEnumerable<ILearner> learners)
        {
            var learnerFamsDictionary = new Dictionary<string, LearnerFAMDenormalized>();

            foreach (var learner in learners)
            {
                var fams = BuildLearnerFAMDenormalized(learner.LearnerFAMs);

                learnerFamsDictionary.Add(learner.LearnRefNumber, fams);
            }

            return learnerFamsDictionary;
        }

        private LearnerFAMDenormalized BuildLearnerFAMDenormalized(IEnumerable<ILearnerFAM> learnerFams)
        {
            var learnerFam = new LearnerFAMDenormalized();

            if (learnerFams != null)
            {
                learnerFams = learnerFams.ToList();

                var edfArray = learnerFams.Where(f => f.LearnFAMType.CaseInsensitiveEquals(LearnerPagingConstants.LearnerFAMTypeEDF)).Select(f => (int?)f.LearnFAMCode).ToArray();

                Array.Resize(ref edfArray, 2);

                learnerFam.ECF = learnerFams.Where(f => f.LearnFAMType.CaseInsensitiveEquals(LearnerPagingConstants.LearnerFAMTypeECF)).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.EDF1 = edfArray[0];
                learnerFam.EDF2 = edfArray[1];
                learnerFam.EHC = learnerFams.Where(f => f.LearnFAMType.CaseInsensitiveEquals(LearnerPagingConstants.LearnerFAMTypeEHC)).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.HNS = learnerFams.Where(f => f.LearnFAMType.CaseInsensitiveEquals(LearnerPagingConstants.LearnerFAMTypeHNS)).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.MCF = learnerFams.Where(f => f.LearnFAMType.CaseInsensitiveEquals(LearnerPagingConstants.LearnerFAMTypeMCF)).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
            }

            return learnerFam;
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
