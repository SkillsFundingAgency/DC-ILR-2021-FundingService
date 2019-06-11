using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Constants;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class FM35LearnerPagingService : AbstractLearnerPagingService, ILearnerPagingService<FM35LearnerDto>
    {
        public IEnumerable<IEnumerable<FM35LearnerDto>> ProvideDtos(int fundModelFilter, IMessage message)
        {
            List<IEnumerable<FM35LearnerDto>> dtos = new List<IEnumerable<FM35LearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page)));

            return dtos;
        }

        private IEnumerable<FM35LearnerDto> BuildDtos(IEnumerable<ILearner> learners)
        {
            var ldFams = BuildLearningDeliveryFAMDictionary(learners);

            return learners.Select(l => new FM35LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                PostcodePrior = l.PostcodePrior,
                LearnerEmploymentStatuses = l.LearnerEmploymentStatuses?.Select(les => new LearnerEmploymentStatus
                {
                    DateEmpStatApp = les.DateEmpStatApp,
                    EmpId = les.EmpIdNullable,
                    EmpStat = les.EmpStat
                }).ToList(),
                LearningDeliveries = l.LearningDeliveries?.Select(ld => new LearningDelivery
                {
                    AchDate = ld.AchDateNullable,
                    AddHours = ld.AddHoursNullable,
                    AimSeqNumber = ld.AimSeqNumber,
                    AimType = ld.AimType,
                    CompStatus = ld.CompStatus,
                    DelLocPostCode = ld.DelLocPostCode,
                    EmpOutcome = ld.EmpOutcomeNullable,
                    FworkCode = ld.FworkCodeNullable,
                    FundModel = ld.FundModel,
                    LearnAimRef = ld.LearnAimRef,
                    LearnActEndDate = ld.LearnActEndDateNullable,
                    LearnPlanEndDate = ld.LearnPlanEndDate,
                    LearnStartDate = ld.LearnStartDate,
                    OrigLearnStartDate = ld.OrigLearnStartDateNullable,
                    OtherFundAdj = ld.OtherFundAdjNullable,
                    Outcome = ld.OutcomeNullable,
                    PriorLearnFundAdj = ld.PriorLearnFundAdjNullable,
                    ProgType = ld.ProgTypeNullable,
                    PwayCode = ld.PwayCodeNullable,
                    LrnDelFAM_EEF = ldFams[l.LearnRefNumber][ld.AimSeqNumber].EEF,
                    LrnDelFAM_FFI = ldFams[l.LearnRefNumber][ld.AimSeqNumber].FFI,
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
