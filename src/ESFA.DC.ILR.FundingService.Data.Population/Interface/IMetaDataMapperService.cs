using ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IMetaDataMapperService
    {
        ReferenceDataVersion GetReferenceDataVersions(MetaData metaData);

        Periods BuildPeriods(MetaData metaDatas);
    }
}
