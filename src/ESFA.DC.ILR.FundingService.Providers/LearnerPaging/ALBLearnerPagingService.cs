using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class ALBLearnerPagingService : AbstractLearnerPagingService, ILearnerPagingService<ALBLearnerDto>
    {
        public IEnumerable<IEnumerable<ALBLearnerDto>> ProvideDtos(int fundModelFilter, IMessage message)
        {
            List<IEnumerable<ALBLearnerDto>> dtos = new List<IEnumerable<ALBLearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page)));

            return dtos;
        }

        private IEnumerable<ALBLearnerDto> BuildDtos(IEnumerable<ILearner> learners)
        {
            var ldFams = BuildLearningDeliveryFAMDictionary(learners);

            var learnerDto = learners.Select(l => new ALBLearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                LearningDeliveries = l.LearningDeliveries.Select(ld => new LearningDelivery
                {
                    AimSeqNumber = ld.AimSeqNumber,
                    CompStatus = ld.CompStatus,
                    DelLocPostCode = ld.DelLocPostCode,
                    FundModel = ld.FundModel,
                    LearnAimRef = ld.LearnAimRef,
                    LearnActEndDate = ld.LearnActEndDateNullable,
                    LearnPlanEndDate = ld.LearnPlanEndDate,
                    LearnStartDate = ld.LearnStartDate,
                    OrigLearnStartDate = ld.OrigLearnStartDateNullable,
                    OtherFundAdj = ld.OtherFundAdjNullable,
                    Outcome = ld.OutcomeNullable,
                    PriorLearnFundAdj = ld.PriorLearnFundAdjNullable,
                    LearningDeliveryFAMs = ld.LearningDeliveryFAMs.Select(ldf => new LearningDeliveryFAM
                    {
                        LearnDelFAMCode = ldf.LearnDelFAMCode,
                        LearnDelFAMType = ldf.LearnDelFAMType,
                        LearnDelFAMDateFrom = ldf.LearnDelFAMDateFromNullable,
                        LearnDelFAMDateTo = ldf.LearnDelFAMDateToNullable
                    }).ToList()
                }).ToList()
            });

            foreach (var learner in learnerDto)
            {
                foreach (var learningDelivery in learner.LearningDeliveries)
                {
                    ldFams.TryGetValue(learner.LearnRefNumber, out var delivery);

                    delivery.TryGetValue(learningDelivery.AimSeqNumber, out var learningDeliveryFams);

                    learningDelivery.LrnDelFAM_ADL = learningDeliveryFams.ADL;
                    learningDelivery.LrnDelFAM_RES = learningDeliveryFams.RES;
                    learningDelivery.LrnDelFAM_LDM1 = learningDeliveryFams.LDM1;
                    learningDelivery.LrnDelFAM_LDM2 = learningDeliveryFams.LDM2;
                    learningDelivery.LrnDelFAM_LDM3 = learningDeliveryFams.LDM3;
                    learningDelivery.LrnDelFAM_LDM4 = learningDeliveryFams.LDM4;
                }
            }

            return learnerDto;
        }
    }
}
