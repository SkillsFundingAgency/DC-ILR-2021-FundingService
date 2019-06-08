using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
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
            return learners.Select(l => new ALBLearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                LearningDeliveries = (List<MessageLearnerLearningDelivery>)l.LearningDeliveries
            });
        }
    }
}
