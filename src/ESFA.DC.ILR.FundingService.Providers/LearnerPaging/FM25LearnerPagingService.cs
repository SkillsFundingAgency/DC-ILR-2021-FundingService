using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class FM25LearnerPagingService : AbstractLearnerPagingService, ILearnerPagingService<FM25LearnerDto>
    {
        public IEnumerable<IEnumerable<FM25LearnerDto>> ProvideDtos(int fundModelFilter, IMessage message)
        {
            var learnerDestinationAndProgressions = message.LearnerDestinationAndProgressions;

            List<IEnumerable<FM25LearnerDto>> dtos = new List<IEnumerable<FM25LearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page, learnerDestinationAndProgressions)));

            return dtos;
        }

        private IEnumerable<FM25LearnerDto> BuildDtos(IEnumerable<ILearner> learners, IEnumerable<ILearnerDestinationAndProgression> learnerDestinationAndProgressions)
        {
            var ldFams = BuildLearningDeliveryFAMDictionary(learners);

            var learnerDto = learners.Select(l => new FM25LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                EngGrade = l.EngGrade,
                MathGrade = l.MathGrade,
                PlanEEPHours = l.PlanEEPHoursNullable,
                PlanLearnHours = l.PlanLearnHoursNullable,
                Postcode = l.Postcode,
                ULN = l.ULN,
                LearnerFAMs = (List<MessageLearnerLearnerFAM>)l.LearnerFAMs,
                DPOutcomes = BuildDPOutcomes(l.LearnRefNumber, learnerDestinationAndProgressions).ToList(),
                LearningDeliveries = l.LearningDeliveries.Select(ld => new LearningDelivery
                {
                    AimSeqNumber = ld.AimSeqNumber,
                    AimType = ld.AimType,
                    CompStatus = ld.CompStatus,
                    FundModel = ld.FundModel,
                    LearnAimRef = ld.LearnAimRef,
                    LearnActEndDate = ld.LearnActEndDateNullable,
                    LearnPlanEndDate = ld.LearnPlanEndDate,
                    LearnStartDate = ld.LearnStartDate,
                    ProgType = ld.ProgTypeNullable,
                    WithdrawReason = ld.WithdrawReasonNullable,
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

                    learningDelivery.LrnDelFAM_SOF = learningDeliveryFams.SOF;
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
