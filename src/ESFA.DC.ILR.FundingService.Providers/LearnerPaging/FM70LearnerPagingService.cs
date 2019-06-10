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
            var learnerDPOutcomeDictionary = BuildLearnerDPOutcomeDictionary(learnerDestinationAndProgressions);
            var ldFams = BuildLearningDeliveryFAMDictionary(learners);

            var learnerDto = learners.Select(l => new FM70LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                LearnerEmploymentStatuses = l.LearnerEmploymentStatuses.Select(les => new LearnerEmploymentStatus
                {
                    DateEmpStatApp = les.DateEmpStatApp,
                    EmpId = les.EmpIdNullable,
                    EmpStat = les.EmpStat
                }).ToList(),
                DPOutcomes = learnerDPOutcomeDictionary[l.LearnRefNumber].ToList() ?? null,
                LearningDeliveries = l.LearningDeliveries.Select(ld => new LearningDelivery
                {
                    AchDate = ld.AchDateNullable,
                    AddHours = ld.AddHoursNullable,
                    AimSeqNumber = ld.AimSeqNumber,
                    CompStatus = ld.CompStatus,
                    ConRefNumber = ld.ConRefNumber,
                    DelLocPostCode = ld.DelLocPostCode,
                    FundModel = ld.FundModel,
                    LearnAimRef = ld.LearnAimRef,
                    LearnActEndDate = ld.LearnActEndDateNullable,
                    LearnPlanEndDate = ld.LearnPlanEndDate,
                    LearnStartDate = ld.LearnStartDate,
                    OrigLearnStartDate = ld.OrigLearnStartDateNullable,
                    OtherFundAdj = ld.OtherFundAdjNullable,
                    Outcome = ld.OutcomeNullable,
                    PriorLearnFundAdj = ld.PriorLearnFundAdjNullable
                }).ToList()
            });

            foreach (var learner in learnerDto)
            {
                foreach (var learningDelivery in learner.LearningDeliveries)
                {
                    ldFams.TryGetValue(learner.LearnRefNumber, out var delivery);

                    delivery.TryGetValue(learningDelivery.AimSeqNumber, out var learningDeliveryFams);

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
