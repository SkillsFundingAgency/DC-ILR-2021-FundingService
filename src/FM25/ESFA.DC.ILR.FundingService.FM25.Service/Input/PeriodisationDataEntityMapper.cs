using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Input
{
    public class PeriodisationDataEntityMapper : IDataEntityMapper<FM25Global>
    {
        private const string EntityGlobal = "global";
        private const string EntityLearner = "Learner";

        private const string GlobalUKPRN = "UKPRN";

        private const string LearnerAcadMonthPayment = "AcadMonthPayment";
        private const string LearnerFundLine = "FundLine";
        private const string LearnerLearnerActEndDate = "LearnerActEndDate";
        private const string LearnerLearnerPlanEndDate = "LearnerPlanEndDate";
        private const string LearnerLearnerStartDate = "LearnerStartDate";
        private const string LearnerLearnRefNumber = "LearnRefNumber";
        private const string LearnerOnProgPayment = "OnProgPayment";

        public IEnumerable<IDataEntity> MapTo(IEnumerable<FM25Global> inputModels)
        {
            return inputModels.Select(BuildGlobalDataEntity);
        }

        public IDataEntity BuildGlobalDataEntity(FM25Global global)
        {
            return new DataEntity(EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { GlobalUKPRN, new AttributeData(global.UKPRN) }
                },
                Children = global.Learners?.Select(BuildLearnerDataEntity).ToList() ?? new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM25Learner learner)
        {
            return new DataEntity(EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { LearnerAcadMonthPayment, new AttributeData(learner.AcadMonthPayment) },
                    { LearnerFundLine, new AttributeData(learner.FundLine) },
                    { LearnerLearnerActEndDate, new AttributeData(learner.LearnerActEndDate) },
                    { LearnerLearnerPlanEndDate, new AttributeData(learner.LearnerPlanEndDate) },
                    { LearnerLearnerStartDate, new AttributeData(learner.LearnerStartDate) },
                    { LearnerLearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { LearnerOnProgPayment, new AttributeData(learner.OnProgPayment) },
                }
            };
        }
    }
}
