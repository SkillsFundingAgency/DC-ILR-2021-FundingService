﻿using System.Collections.Generic;
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
            var learnerDPOutcomes = BuildLearnerDPOutcomeDictionary(learnerDestinationAndProgressions);
            var ldFams = BuildLearningDeliveryFAMDictionary(learners);

            return learners.Select(l => new FM70LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                LearnerEmploymentStatuses = l.LearnerEmploymentStatuses?.Select(les => new LearnerEmploymentStatus
                {
                    DateEmpStatApp = les.DateEmpStatApp,
                    EmpId = les.EmpIdNullable,
                    EmpStat = les.EmpStat
                }).ToList(),
                DPOutcomes = learnerDPOutcomes.TryGetValue(l.LearnRefNumber, out var index) ? learnerDPOutcomes[l.LearnRefNumber] : null,
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
                    PriorLearnFundAdj = ld.PriorLearnFundAdjNullable,
                    LrnDelFAM_RES = ldFams[l.LearnRefNumber][ld.AimSeqNumber].RES,
                    LrnDelFAM_LDM1 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM1,
                    LrnDelFAM_LDM2 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM2,
                    LrnDelFAM_LDM3 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM3,
                    LrnDelFAM_LDM4 = ldFams[l.LearnRefNumber][ld.AimSeqNumber].LDM4,
                }).ToList()
            });
        }
    }
}