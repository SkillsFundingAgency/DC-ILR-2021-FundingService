using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes
{
    public class PostcodesReferenceDataService : IPostcodesReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public PostcodesReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public string PostcodesCurrentVersion()
        {
            return _referenceDataCache.PostcodeCurrentVersion;
        }

        public IEnumerable<SfaAreaCost> SFAAreaCostsForPostcode(string postcode)
        {
            _referenceDataCache.SfaAreaCost.TryGetValue(postcode, out IEnumerable<SfaAreaCost> sfaAreaCost);

            return sfaAreaCost;
        }

        public IEnumerable<SfaDisadvantage> SFADisadvantagesForPostcode(string postcode)
        {
            _referenceDataCache.SfaDisadvantage.TryGetValue(postcode, out IEnumerable<SfaDisadvantage> sfaDisadvantage);

            return sfaDisadvantage;
        }
    }
}
