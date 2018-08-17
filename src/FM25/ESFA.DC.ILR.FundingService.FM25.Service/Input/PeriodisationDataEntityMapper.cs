using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Input
{
    public class PeriodisationDataEntityMapper : IDataEntityMapper<Global>
    {
        private const string EntityGlobal = "global";

        private const string GlobalUKPRN = "UKPRN";

        public IEnumerable<IDataEntity> MapTo(IEnumerable<Global> inputModels)
        {
            return inputModels.Select(BuildGlobalDataEntity);
        }

        public IDataEntity BuildGlobalDataEntity(Global global)
        {
            return new DataEntity(EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { GlobalUKPRN, new AttributeData(global.UKPRN) }
                }
            };
        }
    }
}
