using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Constants;
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
            var learnerDPOutcomes = BuildLearnerDPOutcomeDictionary(learnerDestinationAndProgressions);
            var learnerFams = BuildLearnerFAMDictionary(learners);

            return learners.Select(l => new FM25LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                CampId = l.CampId,
                EngGrade = l.EngGrade,
                MathGrade = l.MathGrade,
                PlanEEPHours = l.PlanEEPHoursNullable,
                PlanLearnHours = l.PlanLearnHoursNullable,
                Postcode = l.Postcode,
                ULN = l.ULN,
                LrnFAM_ECF = learnerFams[l.LearnRefNumber].ECF,
                LrnFAM_EDF1 = learnerFams[l.LearnRefNumber].EDF1,
                LrnFAM_EDF2 = learnerFams[l.LearnRefNumber].EDF2,
                LrnFAM_EHC = learnerFams[l.LearnRefNumber].EHC,
                LrnFAM_HNS = learnerFams[l.LearnRefNumber].HNS,
                LrnFAM_MCF = learnerFams[l.LearnRefNumber].MCF,
                DPOutcomes = learnerDPOutcomes.TryGetValue(l.LearnRefNumber, out var index) ? learnerDPOutcomes[l.LearnRefNumber] : null,
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
                    PHours = ld.PHoursNullable,
                    ProgType = ld.ProgTypeNullable,
                    WithdrawReason = ld.WithdrawReasonNullable,
                    LearningDeliveryFAMs = ld.LearningDeliveryFAMs?.Select(ldf => new LearningDeliveryFAM
                    {
                        LearnDelFAMCode = ldf.LearnDelFAMCode,
                        LearnDelFAMType = ldf.LearnDelFAMType,
                        LearnDelFAMDateFrom = ldf.LearnDelFAMDateFromNullable,
                        LearnDelFAMDateTo = ldf.LearnDelFAMDateToNullable
                    }).ToList()
                }).ToList()
            });
        }
    }
}
