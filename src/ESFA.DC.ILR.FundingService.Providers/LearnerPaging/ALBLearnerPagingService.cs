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

            return learners.Select(l => new ALBLearnerDto
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
                    LrnDelFAM_ADL = ldFams[l.LearnRefNumber][ld.AimSeqNumber].ADL,
                    LrnDelFAM_RES = ldFams[l.LearnRefNumber][ld.AimSeqNumber].RES,
                    LrnDelFAM_LDM1 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM1,
                    LrnDelFAM_LDM2 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM2,
                    LrnDelFAM_LDM3 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM3,
                    LrnDelFAM_LDM4 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM4,
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
