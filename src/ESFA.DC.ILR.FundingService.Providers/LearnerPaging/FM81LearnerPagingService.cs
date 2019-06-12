﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Constants;
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
            var ldFams = BuildLearningDeliveryFAMDictionary(learners);

            return learners.Select(l => new FM81LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                LearnerEmploymentStatuses = l.LearnerEmploymentStatuses?.Select(les => new LearnerEmploymentStatus
                {
                    DateEmpStatApp = les.DateEmpStatApp,
                    EmpId = les.EmpIdNullable,
                    EmpStat = les.EmpStat,
                    SEM = les.EmploymentStatusMonitorings?.Where(e => e.ESMType == LearnerPagingConstants.LearnerEmploymentStatusSEM).Select(e => (int?)e.ESMCode).FirstOrDefault()
                }).ToList(),
                LearningDeliveries = l.LearningDeliveries?.Select(ld => new LearningDelivery
                {
                    AchDate = ld.AchDateNullable,
                    AimSeqNumber = ld.AimSeqNumber,
                    AimType = ld.AimType,
                    CompStatus = ld.CompStatus,
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
                    StdCode = ld.StdCodeNullable,
                    WithdrawReason = ld.WithdrawReasonNullable,
                    LrnDelFAM_EEF = ldFams[l.LearnRefNumber][ld.AimSeqNumber].EEF,
                    LrnDelFAM_FFI = ldFams[l.LearnRefNumber][ld.AimSeqNumber].FFI,
                    LrnDelFAM_RES = ldFams[l.LearnRefNumber][ld.AimSeqNumber].RES,
                    LrnDelFAM_SOF = ldFams[l.LearnRefNumber][ld.AimSeqNumber].SOF,
                    LrnDelFAM_SPP = ldFams[l.LearnRefNumber][ld.AimSeqNumber].SPP,
                    LrnDelFAM_LDM1 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM1,
                    LrnDelFAM_LDM2 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM2,
                    LrnDelFAM_LDM3 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM3,
                    LrnDelFAM_LDM4 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM4,
                    AppFinRecords = ld.AppFinRecords?.Select(af => new AppFinRecord
                    {
                        AFinAmount = af.AFinAmount,
                        AFinCode = af.AFinCode,
                        AFinDate = af.AFinDate,
                        AFinType = af.AFinType
                    }).ToList(),
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
