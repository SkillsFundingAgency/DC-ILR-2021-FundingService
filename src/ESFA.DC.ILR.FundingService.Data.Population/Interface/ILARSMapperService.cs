using System.Collections.Generic;
using LARSLearningDelivery = ESFA.DC.ILR.FundingService.Data.External.LARS.Model.LARSLearningDelivery;
using LARSLearningDeliveryInput = ESFA.DC.ILR.ReferenceDataService.Model.LARS.LARSLearningDelivery;
using LARSStandard = ESFA.DC.ILR.FundingService.Data.External.LARS.Model.LARSStandard;
using LARSStandardInput = ESFA.DC.ILR.ReferenceDataService.Model.LARS.LARSStandard;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface ILARSMapperService
    {
        IDictionary<string, LARSLearningDelivery> MapLARSLearningDeliveries(IReadOnlyCollection<LARSLearningDeliveryInput> larsLearningDeliveries);

        IDictionary<int, LARSStandard> MapLARSStandards(IReadOnlyCollection<LARSStandardInput> larsStandards);
    }
}
