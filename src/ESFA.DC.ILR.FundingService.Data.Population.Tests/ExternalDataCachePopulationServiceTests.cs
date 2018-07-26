using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;

namespace ESFA.DC.ILR.FundingService.ALB.ExternalData.Tests
{
    public class ExternalDataCachePopulationServiceTests
    {
        private ExternalDataCachePopulationService NewService(
            IExternalDataCache externalDataCache = null,
            IPostcodesDataRetrievalService postcodesDataRetrievalService = null,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService = null,
            ILARSDataRetrievalService larsDataRetrievalService = null,
            IOrganisationDataRetrievalService organisationDataRetrievalService = null,
            ILARS lars = null,
            IFundingServiceDto fundingServiceDto = null)
        {
            return new ExternalDataCachePopulationService(externalDataCache, postcodesDataRetrievalService, largeEmployersDataRetrievalService, larsDataRetrievalService, organisationDataRetrievalService, lars, fundingServiceDto);
        }
    }
}
