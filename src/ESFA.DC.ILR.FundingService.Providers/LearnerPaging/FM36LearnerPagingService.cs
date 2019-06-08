using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class FM36LearnerPagingService : AbstractLearnerPagingService, ILearnerPagingService<FM36LearnerDto>
    {
        public IEnumerable<IEnumerable<FM36LearnerDto>> ProvideDtos(int fundModelFilter, IMessage message)
        {
            List<IEnumerable<FM36LearnerDto>> dtos = new List<IEnumerable<FM36LearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page)));

            return dtos;
        }

        private IEnumerable<FM36LearnerDto> BuildDtos(IEnumerable<ILearner> learners)
        {
            return learners.Select(l => new FM36LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                PostcodePrior = l.PostcodePrior,
                PMUKPRN = l.PMUKPRNNullable,
                PrevUKPRN = l.PrevUKPRNNullable,
                ULN = l.ULN,
                LearnerEmploymentStatuses = (List<MessageLearnerLearnerEmploymentStatus>)l.LearnerEmploymentStatuses,
                LearningDeliveries = (List<MessageLearnerLearningDelivery>)l.LearningDeliveries
            });
        }
    }
}
