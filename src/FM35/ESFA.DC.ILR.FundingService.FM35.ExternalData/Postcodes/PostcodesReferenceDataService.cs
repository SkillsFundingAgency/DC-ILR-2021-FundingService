using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes.Model;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Postcodes
{
    public class PostcodesReferenceDataService : IPostcodesReferenceDataService
    {
        private readonly IReferenceDataCache _referenceDataCache;

        public PostcodesReferenceDataService(IReferenceDataCache referenceDataCache)
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