using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Constants;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Providers.LearnerPaging
{
    public class FM25LearnerPagingService : AbstractLearnerPagingService, ILearnerPagingService<FM25LearnerDto>
    {
        public IEnumerable<IEnumerable<FM25LearnerDto>> ProvideDtos(int fundModelFilter, IMessage message)
        {
            var learnerDestinationAndProgressions = message.LearnerDestinationAndProgressions;

            List<IEnumerable<FM25LearnerDto>> dtos = new List<IEnumerable<FM25LearnerDto>>();

            var pagedLearners = BuildPages(fundModelFilter, message.Learners).ToList();

            pagedLearners.ForEach(page => dtos.Add(BuildDtos(page, learnerDestinationAndProgressions)));

            return dtos;
        }

        private IEnumerable<FM25LearnerDto> BuildDtos(IEnumerable<ILearner> learners, IEnumerable<ILearnerDestinationAndProgression> learnerDestinationAndProgressions)
        {
            var learnerDPOutcomeDictionary = BuildLearnerDPOutcomeDictionary(learnerDestinationAndProgressions);
            var learnerFamsDictionary = BuildLearnerFAMDictionary(learners);
            var ldFamDictionarys = BuildLearningDeliveryFAMDictionary(learners);

            var learnerDto = learners.Select(l => new FM25LearnerDto
            {
                LearnRefNumber = l.LearnRefNumber,
                DateOfBirth = l.DateOfBirthNullable,
                EngGrade = l.EngGrade,
                MathGrade = l.MathGrade,
                PlanEEPHours = l.PlanEEPHoursNullable,
                PlanLearnHours = l.PlanLearnHoursNullable,
                Postcode = l.Postcode,
                ULN = l.ULN,
                DPOutcomes = learnerDPOutcomeDictionary[l.LearnRefNumber].ToList() ?? null,
                LearningDeliveries = l.LearningDeliveries.Select(ld => new LearningDelivery
                {
                    AimSeqNumber = ld.AimSeqNumber,
                    AimType = ld.AimType,
                    CompStatus = ld.CompStatus,
                    FundModel = ld.FundModel,
                    LearnAimRef = ld.LearnAimRef,
                    LearnActEndDate = ld.LearnActEndDateNullable,
                    LearnPlanEndDate = ld.LearnPlanEndDate,
                    LearnStartDate = ld.LearnStartDate,
                    ProgType = ld.ProgTypeNullable,
                    WithdrawReason = ld.WithdrawReasonNullable,
                    LearningDeliveryFAMs = ld.LearningDeliveryFAMs.Select(ldf => new LearningDeliveryFAM
                    {
                        LearnDelFAMCode = ldf.LearnDelFAMCode,
                        LearnDelFAMType = ldf.LearnDelFAMType,
                        LearnDelFAMDateFrom = ldf.LearnDelFAMDateFromNullable,
                        LearnDelFAMDateTo = ldf.LearnDelFAMDateToNullable
                    }).ToList()
                }).ToList()
            });

            foreach (var learner in learnerDto)
            {
                learnerFamsDictionary.TryGetValue(learner.LearnRefNumber, out var learnerFams);

                learner.LrnFAM_ECF = learnerFams.ECF;
                learner.LrnFAM_EDF1 = learnerFams.EDF1;
                learner.LrnFAM_EDF2 = learnerFams.EDF2;
                learner.LrnFAM_EHC = learnerFams.EHC;
                learner.LrnFAM_HNS = learnerFams.HNS;
                learner.LrnFAM_MCF = learnerFams.MCF;

                foreach (var learningDelivery in learner.LearningDeliveries)
                {
                    ldFamDictionarys.TryGetValue(learner.LearnRefNumber, out var delivery);

                    delivery.TryGetValue(learningDelivery.AimSeqNumber, out var learningDeliveryFams);

                    learningDelivery.LrnDelFAM_SOF = learningDeliveryFams.SOF;
                    learningDelivery.LrnDelFAM_LDM1 = learningDeliveryFams.LDM1;
                    learningDelivery.LrnDelFAM_LDM2 = learningDeliveryFams.LDM2;
                    learningDelivery.LrnDelFAM_LDM3 = learningDeliveryFams.LDM3;
                    learningDelivery.LrnDelFAM_LDM4 = learningDeliveryFams.LDM4;
                }
            }

            return learnerDto;
        }

        private IDictionary<string, LearnerFAMDenormalized> BuildLearnerFAMDictionary(IEnumerable<ILearner> learners)
        {
            var learnerFamsDictionary = new Dictionary<string, LearnerFAMDenormalized>();

            foreach (var learner in learners)
            {
                var fams = BuildLearnerFAMDenormalized(learner.LearnerFAMs);

                learnerFamsDictionary.Add(learner.LearnRefNumber, fams);
            }

            return learnerFamsDictionary;
        }

        private LearnerFAMDenormalized BuildLearnerFAMDenormalized(IEnumerable<ILearnerFAM> learnerFams)
        {
            var learnerFam = new LearnerFAMDenormalized();

            if (learnerFams != null)
            {
                learnerFams = learnerFams.ToList();

                var edfArray = learnerFams.Where(f => f.LearnFAMType == LearnerPagingConstants.LearnerFAMTypeEDF).Select(f => (int?)f.LearnFAMCode).ToArray();

                Array.Resize(ref edfArray, 2);

                learnerFam.ECF = learnerFams.Where(f => f.LearnFAMType == LearnerPagingConstants.LearnerFAMTypeECF).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.EDF1 = edfArray[0];
                learnerFam.EDF2 = edfArray[1];
                learnerFam.EHC = learnerFams.Where(f => f.LearnFAMType == LearnerPagingConstants.LearnerFAMTypeEHC).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.HNS = learnerFams.Where(f => f.LearnFAMType == LearnerPagingConstants.LearnerFAMTypeHNS).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.MCF = learnerFams.Where(f => f.LearnFAMType == LearnerPagingConstants.LearnerFAMTypeMCF).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
            }

            return learnerFam;
        }
    }
}
