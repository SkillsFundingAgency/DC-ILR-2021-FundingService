﻿using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes
{
    public class PostcodesReferenceDataService : IPostcodesReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;
        private readonly IEnumerable<SfaAreaCost> _emptySfaAreaCost = new List<SfaAreaCost>();
        private readonly IEnumerable<DasDisadvantage> _emptyDasDisadvantage = new List<DasDisadvantage>();
        private readonly IEnumerable<SfaDisadvantage> _emptySfaDisadvantage = new List<SfaDisadvantage>();
        private readonly IEnumerable<EfaDisadvantage> _emptyEfaDisadvantage = new List<EfaDisadvantage>();
        private readonly IEnumerable<CareerLearningPilot> _emptyCareerLearningPilot = new List<CareerLearningPilot>();

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

            return sfaAreaCost ?? _emptySfaAreaCost;
        }

        public IEnumerable<DasDisadvantage> DASDisadvantagesForPostcode(string postcode)
        {
            _referenceDataCache.DasDisadvantage.TryGetValue(postcode, out IEnumerable<DasDisadvantage> dasDisadvantage);

            return dasDisadvantage ?? _emptyDasDisadvantage;
        }


        public IEnumerable<SfaDisadvantage> SFADisadvantagesForPostcode(string postcode)
        {
            _referenceDataCache.SfaDisadvantage.TryGetValue(postcode, out IEnumerable<SfaDisadvantage> sfaDisadvantage);

            return sfaDisadvantage ?? _emptySfaDisadvantage;
        }

        public IEnumerable<EfaDisadvantage> EFADisadvantagesForPostcode(string postcode)
        {
            _referenceDataCache.EfaDisadvantage.TryGetValue(postcode, out IEnumerable<EfaDisadvantage> efaDisadvantages);

            return efaDisadvantages ?? _emptyEfaDisadvantage;
        }

        public IEnumerable<CareerLearningPilot> CareerLearningPilotsForPostcode(string postcode)
        {
            _referenceDataCache.CareerLearningPilot.TryGetValue(postcode, out IEnumerable<CareerLearningPilot> careerLearningPilots);

            return careerLearningPilots ?? _emptyCareerLearningPilot;
        }
    }
}
