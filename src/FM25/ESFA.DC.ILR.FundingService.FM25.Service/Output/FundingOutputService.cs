using ESFA.DC.ILR.FundingService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.OPA.Service.Abstract;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Output
{
    public class FundingOutputService : AbstractFundingOutputService, IOutputService<IEnumerable<Global>>
    {
        public IEnumerable<Global> ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            return dataEntities.Select(MapGlobal);
        }

        public Global MapGlobal(IDataEntity dataEntity)
        {
            return new Global()
            {
                LARSVersion = GetAttributeValue(dataEntity, OutputAttributeNames.LARSVersion),
                OrgVersion = GetAttributeValue(dataEntity, OutputAttributeNames.OrgVersion),
                PostcodeDisadvantageVersion = GetAttributeValue(dataEntity, OutputAttributeNames.PostcodeDisadvantageVersion),
                RulebaseVersion = GetAttributeValue(dataEntity, OutputAttributeNames.RulebaseVersion),
                UKPRN = int.Parse(GetAttributeValue(dataEntity, OutputAttributeNames.UKPRN)),
                Learners = dataEntity
                    .Children?
                    .Where(c => c.EntityName == OutputAttributeNames.Learner)
                    .Select(MapLearner)
                    .ToList()
            };
        }

        public Learner MapLearner(IDataEntity dataEntity)
        {
            return new Learner()
            {
                AcadMonthPayment = GetIntAttributeValue(dataEntity, OutputAttributeNames.AcadMonthPayment),
                AcadProg = GetBoolAttributeValue(dataEntity, OutputAttributeNames.AcadProg),
                ActualDaysILCurrYear = GetIntAttributeValue(dataEntity, OutputAttributeNames.ActualDaysILCurrYear),

                //TODO : Finish
            };
        }
    }
}
