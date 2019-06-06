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
            var ukprn = message.LearningProviderEntity.UKPRN;
            var learnerDestinationAndProgressions = message.LearnerDestinationAndProgressions;

            List<IEnumerable<FM25LearnerDto>> dtos = new List<IEnumerable<FM25LearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page, ukprn, learnerDestinationAndProgressions)));

            return dtos;
        }

        private IEnumerable<FM25LearnerDto> BuildDtos(IEnumerable<ILearner> learners, int ukprn, IEnumerable<ILearnerDestinationAndProgression> learnerDestinationAndProgressions)
        {
            return learners.Select(l => new FM25LearnerDto
            {
                UKPRN = ukprn,
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
                LearningDeliveries = (List<MessageLearnerLearningDelivery>)l.LearningDeliveries
            });
        }
    }
}
