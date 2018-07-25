using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface ILARSDataRetrievalService
    {
        IEnumerable<string> UniqueLearnAimRefs(IMessage message);

        IDictionary<string, IEnumerable<LARSFunding>> LARSFundingsForLearnAimRefs(IEnumerable<string> learnAimRefs);

        IDictionary<string, LARSLearningDelivery> LARSLearningDeliveriesForLearnAimRefs(IEnumerable<string> learnAimRefs);
    }
}
