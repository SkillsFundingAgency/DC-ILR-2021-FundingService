using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Providers.Constants;
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

        protected IDictionary<string, IEnumerable<DPOutcome>> BuildLearnerDPOutcomeDictionary(IEnumerable<ILearnerDestinationAndProgression> learnerDestinationAndProgressions)
        {
            return
                learnerDestinationAndProgressions
                .ToDictionary(
                    l => l.LearnRefNumber,
                    dp => dp.DPOutcomes.Select(dpo => new DPOutcome
                    {
                        OutCode = dpo.OutCode,
                        OutCollDate = dpo.OutCollDate,
                        OutType = dpo.OutType,
                        OutStartDate = dpo.OutStartDate,
                        OutEndDate = dpo.OutEndDateNullable
                    }).ToList() as IEnumerable<DPOutcome>, StringComparer.OrdinalIgnoreCase);
        }

        protected IDictionary<string, IDictionary<int, LearningDeliveryFAMDenormalized>> BuildLearningDeliveryFAMDictionary(IEnumerable<ILearner> learners)
        {
            var learningDeliveryFamsDictionary = new Dictionary<string, IDictionary<int, LearningDeliveryFAMDenormalized>>();

            foreach (var learner in learners)
            {
                var learningDeliveryDictionary = new Dictionary<int, LearningDeliveryFAMDenormalized>();

                foreach (var ld in learner.LearningDeliveries)
                {
                    var fams = BuildLearningDeliveryFAMDenormalized(ld.LearningDeliveryFAMs);

                    learningDeliveryDictionary.Add(ld.AimSeqNumber, fams);
                }

                learningDeliveryFamsDictionary.Add(learner.LearnRefNumber, learningDeliveryDictionary);
            }

            return learningDeliveryFamsDictionary;
        }

        private LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == LearnerPagingConstants.LearningDeliveryFAMTypeLDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.ADL = learningDeliveryFams.Where(f => f.LearnDelFAMType == LearnerPagingConstants.LearningDeliveryFAMTypeADL).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.EEF = learningDeliveryFams.Where(f => f.LearnDelFAMType == LearnerPagingConstants.LearningDeliveryFAMTypeEEF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.FFI = learningDeliveryFams.Where(f => f.LearnDelFAMType == LearnerPagingConstants.LearningDeliveryFAMTypeFFI).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.RES = learningDeliveryFams.Where(f => f.LearnDelFAMType == LearnerPagingConstants.LearningDeliveryFAMTypeRES).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.SOF = learningDeliveryFams.Where(f => f.LearnDelFAMType == LearnerPagingConstants.LearningDeliveryFAMTypeSOF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.SPP = learningDeliveryFams.Where(f => f.LearnDelFAMType == LearnerPagingConstants.LearningDeliveryFAMTypeSPP).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
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
