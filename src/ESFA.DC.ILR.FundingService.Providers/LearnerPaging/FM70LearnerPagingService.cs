using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class FM70LearnerPagingService : AbstractLearnerPagingService, ILearnerPagingService<FM70LearnerDto>
    {
        public IEnumerable<IEnumerable<FM70LearnerDto>> ProvideDtos(int fundModelFilter, IMessage message)
        {
            var learnerDestinationAndProgressions = message.LearnerDestinationAndProgressions;

            List<IEnumerable<FM70LearnerDto>> dtos = new List<IEnumerable<FM70LearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page, learnerDestinationAndProgressions)));

            return dtos;
        }

        private IEnumerable<FM70LearnerDto> BuildDtos(IEnumerable<ILearner> learners, IEnumerable<ILearnerDestinationAndProgression> learnerDestinationAndProgressions)
        {
            return learners.Select(l => new FM70LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                DPOutcomes = BuildDPOutcomes(l.LearnRefNumber, learnerDestinationAndProgressions).ToList(),
                LearnerEmploymentStatuses = (List<MessageLearnerLearnerEmploymentStatus>)l.LearnerEmploymentStatuses,
                LearningDeliveries = (List<MessageLearnerLearningDelivery>)l.LearningDeliveries
            });
        }
    }
}
