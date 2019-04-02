using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class PostcodesMapperService : IPostcodesMapperService
    {
        public PostcodesMapperService()
        {
        }

        public IDictionary<string, PostcodeRoot> MapPostcodes(IReadOnlyCollection<Postcode> postcodes)
        {
            return postcodes
                .ToDictionary(
                p => p.PostCode,
                p => new PostcodeRoot
                {
                    Postcode = p.PostCode,
                    CareerLearningPilots = p.CareerLearningPilots?.Select(clp => MapCareerLearningPilot(clp, p.PostCode)).ToList(),
                    DasDisadvantages = p.DasDisadvantages?.Select(dd => MapDasDisadvantage(dd, p.PostCode)).ToList(),
                    EfaDisadvantages = p.EfaDisadvantages?.Select(ed => MapEfaDisadvantage(ed, p.PostCode)).ToList(),
                    SfaAreaCosts = p.SfaAreaCosts?.Select(sa => MapSfaAreaCost(sa, p.PostCode)).ToList(),
                    SfaDisadvantages = p.SfaDisadvantages?.Select(sd => MapSfaDisadvantage(sd, p.PostCode)).ToList(),
                }, StringComparer.OrdinalIgnoreCase);
        }

        private Data.External.Postcodes.Model.CareerLearningPilot MapCareerLearningPilot(ReferenceDataService.Model.Postcodes.CareerLearningPilot careerLearningPilot, string postcode)
        {
            return new Data.External.Postcodes.Model.CareerLearningPilot
            {
                Postcode = postcode,
                AreaCode = careerLearningPilot.AreaCode,
                EffectiveFrom = careerLearningPilot.EffectiveFrom,
                EffectiveTo = careerLearningPilot.EffectiveTo
            };
        }

        private Data.External.Postcodes.Model.DasDisadvantage MapDasDisadvantage(ReferenceDataService.Model.Postcodes.DasDisadvantage dasDisadvantage, string postcode)
        {
            return new Data.External.Postcodes.Model.DasDisadvantage
            {
                Postcode = postcode,
                Uplift = dasDisadvantage.Uplift,
                EffectiveFrom = dasDisadvantage.EffectiveFrom,
                EffectiveTo = dasDisadvantage.EffectiveTo
            };
        }

        private Data.External.Postcodes.Model.EfaDisadvantage MapEfaDisadvantage(ReferenceDataService.Model.Postcodes.EfaDisadvantage efaDisadvantage, string postcode)
        {
            return new Data.External.Postcodes.Model.EfaDisadvantage
            {
                Postcode = postcode,
                Uplift = efaDisadvantage.Uplift,
                EffectiveFrom = efaDisadvantage.EffectiveFrom,
                EffectiveTo = efaDisadvantage.EffectiveTo
            };
        }

        private Data.External.Postcodes.Model.SfaAreaCost MapSfaAreaCost(ReferenceDataService.Model.Postcodes.SfaAreaCost sfaAreaCost, string postcode)
        {
            return new Data.External.Postcodes.Model.SfaAreaCost
            {
                Postcode = postcode,
                AreaCostFactor = sfaAreaCost.AreaCostFactor,
                EffectiveFrom = sfaAreaCost.EffectiveFrom,
                EffectiveTo = sfaAreaCost.EffectiveTo
            };
        }

        private Data.External.Postcodes.Model.SfaDisadvantage MapSfaDisadvantage(ReferenceDataService.Model.Postcodes.SfaDisadvantage sfaDisadvantage, string postcode)
        {
            return new Data.External.Postcodes.Model.SfaDisadvantage
            {
                Postcode = postcode,
                Uplift = sfaDisadvantage.Uplift,
                EffectiveFrom = sfaDisadvantage.EffectiveFrom,
                EffectiveTo = sfaDisadvantage.EffectiveTo
            };
        }
    }
}
