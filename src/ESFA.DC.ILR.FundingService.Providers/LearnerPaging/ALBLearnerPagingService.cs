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
            var ukprn = message.LearningProviderEntity.UKPRN;

            List<IEnumerable<ALBLearnerDto>> dtos = new List<IEnumerable<ALBLearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page, ukprn)));

            return dtos;
        }

        private IEnumerable<ALBLearnerDto> BuildDtos(IEnumerable<ILearner> learners, int ukprn)
        {
            return learners.Select(l => new ALBLearnerDto
            {
                UKPRN = ukprn,
                LearnRefNumber = l.LearnRefNumber,
                LearningDeliveries = (List<MessageLearnerLearningDelivery>)l.LearningDeliveries
            });
        }
    }
}
