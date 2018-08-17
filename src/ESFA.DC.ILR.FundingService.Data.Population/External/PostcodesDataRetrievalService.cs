﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class PostcodesDataRetrievalService : IPostcodesDataRetrievalService
    {
        private readonly IPostcodes _postcodes;

        public PostcodesDataRetrievalService()
        {
        }

        public PostcodesDataRetrievalService(IPostcodes postcodes)
        {
            _postcodes = postcodes;
        }

        public virtual IQueryable<VersionInfo> VersionInfos => _postcodes.VersionInfos;

        public virtual IQueryable<SFA_PostcodeAreaCost> SfaPostcodeAreaCosts => _postcodes.SFA_PostcodeAreaCost;

        public virtual IQueryable<SFA_PostcodeDisadvantage> SfaPostcodeDisadvantages => _postcodes.SFA_PostcodeDisadvantage;

        public virtual IQueryable<EFA_PostcodeDisadvantage> EfaPostcodeDisadvantages => _postcodes.EFA_PostcodeDisadvantage;

        public IEnumerable<string> UniquePostcodes(IMessage message)
        {
            return message.Learners.Where(l => l.LearningDeliveries != null).SelectMany(l => l.LearningDeliveries).Select(ld => ld.DelLocPostCode).Distinct();
        }

        public string CurrentVersion()
        {
            return VersionInfos.OrderByDescending(v => v.VersionNumber).Select(v => v.VersionNumber).FirstOrDefault();
        }

        public IDictionary<string, IEnumerable<SfaAreaCost>> SfaAreaCostsForPostcodes(IEnumerable<string> postcodes)
        {
            return SfaPostcodeAreaCosts
                    .Where(p => postcodes.Contains(p.Postcode))
                    .GroupBy(a => a.Postcode)
                    .ToDictionary(a => a.Key, a => a.Select(SfaAreaCostFromEntity).ToList() as IEnumerable<SfaAreaCost>);
        }

        public SfaAreaCost SfaAreaCostFromEntity(SFA_PostcodeAreaCost entity)
        {
            return new SfaAreaCost()
            {
                Postcode = entity.Postcode,
                AreaCostFactor = entity.AreaCostFactor,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo
            };
        }

        public IDictionary<string, IEnumerable<SfaDisadvantage>> SfaDisadvantagesForPostcodes(IEnumerable<string> postcodes)
        {
            return SfaPostcodeDisadvantages
                .Where(p => postcodes.Contains(p.Postcode))
                .GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(SfaDisadvantageFromEntity).ToList() as IEnumerable<SfaDisadvantage>);
        }

        public SfaDisadvantage SfaDisadvantageFromEntity(SFA_PostcodeDisadvantage entity)
        {
            return new SfaDisadvantage
            {
                Postcode = entity.Postcode,
                Uplift = entity.Uplift,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<string, IEnumerable<EfaDisadvantage>> EfaDisadvantagesForPostcodes(IEnumerable<string> postcodes)
        {
            return EfaPostcodeDisadvantages
                .Where(p => postcodes.Contains(p.Postcode))
                .GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(EfaDisadvantageFromEntity).ToList() as IEnumerable<EfaDisadvantage>);
        }

        public EfaDisadvantage EfaDisadvantageFromEntity(EFA_PostcodeDisadvantage entity)
        {
            return new EfaDisadvantage()
            {
                Postcode = entity.Postcode,
                Uplift = entity.Uplift,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}