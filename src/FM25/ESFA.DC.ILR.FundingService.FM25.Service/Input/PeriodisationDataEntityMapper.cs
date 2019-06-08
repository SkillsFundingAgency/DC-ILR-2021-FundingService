using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Input
{
    public class PeriodisationDataEntityMapper : IDataEntityMapper<FM25Global>
    {
        public IEnumerable<IDataEntity> MapTo(int ukprn, IEnumerable<FM25Global> inputModels)
        {
            return inputModels.Select(BuildGlobalDataEntity);
        }

        public IDataEntity BuildGlobalDataEntity(FM25Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children = global.Learners?.Select(BuildLearnerDataEntity).ToList() ?? new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(FM25Learner learner)
        {
            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AcadMonthPayment, new AttributeData(learner.AcadMonthPayment) },
                    { Attributes.FundLine, new AttributeData(learner.FundLine) },
                    { Attributes.LearnerActEndDate, new AttributeData(learner.LearnerActEndDate) },
                    { Attributes.LearnerPlanEndDate, new AttributeData(learner.LearnerPlanEndDate) },
                    { Attributes.LearnerStartDate, new AttributeData(learner.LearnerStartDate) },
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.OnProgPayment, new AttributeData(learner.OnProgPayment) },
                }
            };
        }
    }
}
