﻿using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM81.Service.Constants;
using ESFA.DC.ILR.FundingService.FM81.Service.Model;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM81.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<FM81LearnerDto>
    {
        private readonly int _fundModel = Attributes.FundModel_81;
        private readonly int? _progType = Attributes.ProgType_25;

        private readonly ILARSReferenceDataService _larsReferenceDataService;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
        }

        public IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<FM81LearnerDto> inputModels)
        {
            var global = BuildGlobal(ukprn);

            var entities = inputModels?
                .Where(l => l.LearningDeliveries
                .Any(ld => _fundModel == ld.FundModel && _progType == ld.ProgType))
                .Select(l => BuildGlobalDataEntity(l, global)) ?? Enumerable.Empty<IDataEntity>();

            return entities.Any() ? entities : new List<IDataEntity> { BuildDefaultGlobalDataEntity(global) };
        }

        public IDataEntity BuildGlobalDataEntity(FM81LearnerDto learner, Global global)
        {
            var entity = new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = BuildGlobalAttributes(global)
            };

            if (learner != null)
            {
                entity.AddChild(BuildLearnerDataEntity(learner));
            }

            return entity;
        }

        public IDataEntity BuildDefaultGlobalDataEntity(Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = BuildGlobalAttributes(global)
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM81LearnerDto learner)
        {
            var learnerEmploymentStatusEntities = learner.LearnerEmploymentStatuses?.Select(BuildLearnerEmploymentStatus) ?? Enumerable.Empty<IDataEntity>();
            var learningDeliveryEntities = learner.LearningDeliveries?.Where(ld => ld.FundModel == _fundModel && ld.ProgType == _progType)
                .Select(BuildLearningDeliveryDataEntity) ?? Enumerable.Empty<IDataEntity>();

            var entity = new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirth) },
                }
            };

            entity.AddChildren(learningDeliveryEntities);
            entity.AddChildren(learnerEmploymentStatusEntities);

            return entity;
        }

        public IDataEntity BuildLearningDeliveryDataEntity(LearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var larsStandard = _larsReferenceDataService.LARSStandardForStandardCode(learningDelivery.StdCode);

            var learningDeliveryFamsEntities = learningDelivery?.LearningDeliveryFAMs?.Select(BuildLearningDeliveryFAM) ?? Enumerable.Empty<IDataEntity>();
            var appFinRecordEntities = learningDelivery?.AppFinRecords?.Select(BuildApprenticeshipFinancialRecord) ?? Enumerable.Empty<IDataEntity>();
            var larsStandardCommonComponentEntities = larsStandard?.LARSStandardCommonComponents?
                .Select(ls => BuildLARSStandardCommonComponent(ls, larsStandard.StandardCode)) ?? Enumerable.Empty<IDataEntity>();
            var larsStandardFundingEntities = larsStandard?.LARSStandardFundings?.Select(BuildLARSStandardFunding) ?? Enumerable.Empty<IDataEntity>();

            var entity = new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AchDate, new AttributeData(learningDelivery.AchDate) },
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.FrameworkCommonComponent, new AttributeData(larsLearningDelivery.FrameworkCommonComponent) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDate) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDate) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdj) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.Outcome) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdj) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgType) },
                    { Attributes.STDCode, new AttributeData(learningDelivery.StdCode) },
                    { Attributes.WithdrawReason, new AttributeData(learningDelivery.WithdrawReason) },
                }
            };

            entity.AddChildren(learningDeliveryFamsEntities);
            entity.AddChildren(appFinRecordEntities);
            entity.AddChildren(larsStandardCommonComponentEntities);
            entity.AddChildren(larsStandardFundingEntities);

            return entity;
        }

        public IDataEntity BuildLearningDeliveryFAM(LearningDeliveryFAM learningDeliveryFAM)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryFAM)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelFAMCode, new AttributeData(learningDeliveryFAM.LearnDelFAMCode) },
                    { Attributes.LearnDelFAMDateTo, new AttributeData(learningDeliveryFAM.LearnDelFAMDateTo) },
                    { Attributes.LearnDelFAMDateFrom, new AttributeData(learningDeliveryFAM.LearnDelFAMDateFrom) },
                    { Attributes.LearnDelFAMType, new AttributeData(learningDeliveryFAM.LearnDelFAMType) },
                }
            };
        }

        public IDataEntity BuildLearnerEmploymentStatus(LearnerEmploymentStatus learnerEmploymentStatus)
        {
            return new DataEntity(Attributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpId, new AttributeData(learnerEmploymentStatus.EmpId) },
                    { Attributes.EMPStat, new AttributeData(learnerEmploymentStatus.EmpStat) },
                    { Attributes.EmpStatMon_SEM, new AttributeData(learnerEmploymentStatus.SEM) }
                }
            };
        }

        public IDataEntity BuildApprenticeshipFinancialRecord(AppFinRecord appFinRecord)
        {
            return new DataEntity(Attributes.EntityApprenticeshipFinancialRecord)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AFinAmount, new AttributeData(appFinRecord.AFinAmount) },
                    { Attributes.AFinCode, new AttributeData(appFinRecord.AFinCode) },
                    { Attributes.AFinDate, new AttributeData(appFinRecord.AFinDate) },
                    { Attributes.AFinType, new AttributeData(appFinRecord.AFinType) }
                }
            };
        }

        public IDataEntity BuildLARSStandardCommonComponent(LARSStandardCommonComponent larsStandardCommonComponent, int stdCode)
        {
            return new DataEntity(Attributes.EntityStandardCommonComponent)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSStandardCommonComponentCode, new AttributeData(larsStandardCommonComponent.CommonComponent) },
                    { Attributes.LARSStandardCommonComponentStandardCode, new AttributeData(stdCode) },
                    { Attributes.LARSStandardCommonComponentEffectiveFrom, new AttributeData(larsStandardCommonComponent.EffectiveFrom) },
                    { Attributes.LARSStandardCommonComponentEffectiveTo, new AttributeData(larsStandardCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSStandardFunding(LARSStandardFunding larsStdFunding)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_StandardFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.SFFundableWithoutEmployer, new AttributeData(larsStdFunding.FundableWithoutEmployer) },
                    { Attributes.SF1618Incentive, new AttributeData(larsStdFunding.SixteenToEighteenIncentive) },
                    { Attributes.SFAchIncentive, new AttributeData(larsStdFunding.AchievementIncentive) },
                    { Attributes.SFCoreGovContCap, new AttributeData(larsStdFunding.CoreGovContributionCap) },
                    { Attributes.SFEffectiveFromDate, new AttributeData(larsStdFunding.EffectiveFrom) },
                    { Attributes.SFEffectiveToDate, new AttributeData(larsStdFunding.EffectiveTo) },
                    { Attributes.SFSmallBusIncentive, new AttributeData(larsStdFunding.SmallBusinessIncentive) },
                }
            };
        }

        public Global BuildGlobal(int ukprn)
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                UKPRN = ukprn
            };
        }

        private IDictionary<string, IAttributeData> BuildGlobalAttributes(Global global)
        {
            return new Dictionary<string, IAttributeData>
            {
                { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                { Attributes.UKPRN, new AttributeData(global.UKPRN) }
            };
        }
    }
}