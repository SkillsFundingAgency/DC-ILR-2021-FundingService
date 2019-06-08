using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class FM81LearnerPagingService : AbstractLearnerPagingService, ILearnerPagingService<FM81LearnerDto>
    {
        public IEnumerable<IEnumerable<FM81LearnerDto>> ProvideDtos(int fundModelFilter, IMessage message)
        {
            List<IEnumerable<FM81LearnerDto>> dtos = new List<IEnumerable<FM81LearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page)));

            return dtos;
        }

        private IEnumerable<FM81LearnerDto> BuildDtos(IEnumerable<ILearner> learners)
        {
            return learners.Select(l => new FM81LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                LearnerEmploymentStatuses = (List<MessageLearnerLearnerEmploymentStatus>)l.LearnerEmploymentStatuses,
                LearningDeliveries = (List<MessageLearnerLearningDelivery>)l.LearningDeliveries
            });
        }
    }
}
