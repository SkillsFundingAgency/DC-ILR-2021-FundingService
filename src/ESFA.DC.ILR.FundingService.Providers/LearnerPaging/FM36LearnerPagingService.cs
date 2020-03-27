using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Constants;
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
                LearnerEmploymentStatuses = l.LearnerEmploymentStatuses?.Select(les => new LearnerEmploymentStatus
                {
                    AgreeId = les.AgreeId,
                    DateEmpStatApp = les.DateEmpStatApp,
                    EmpId = les.EmpIdNullable,
                    EmpStat = les.EmpStat,
                    SEM = les.EmploymentStatusMonitorings?.Where(e => e.ESMType == LearnerPagingConstants.LearnerEmploymentStatusSEM).Select(e => (int?)e.ESMCode).FirstOrDefault()
                }).ToList(),
                LearningDeliveries = l.LearningDeliveries?.Select(ld => new LearningDelivery
                {
                    AimSeqNumber = ld.AimSeqNumber,
                    AchDate = ld.AchDateNullable,
                    AimType = ld.AimType,
                    CompStatus = ld.CompStatus,
                    FundModel = ld.FundModel,
                    FworkCode = ld.FworkCodeNullable,
                    LearnAimRef = ld.LearnAimRef,
                    LearnActEndDate = ld.LearnActEndDateNullable,
                    LearnPlanEndDate = ld.LearnPlanEndDate,
                    LearnStartDate = ld.LearnStartDate,
                    OrigLearnStartDate = ld.OrigLearnStartDateNullable,
                    OtherFundAdj = ld.OtherFundAdjNullable,
                    PriorLearnFundAdj = ld.PriorLearnFundAdjNullable,
                    ProgType = ld.ProgTypeNullable,
                    PwayCode = ld.PwayCodeNullable,
                    StdCode = ld.StdCodeNullable,
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
