using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class PostcodesDataRetrievalService : IPostcodesDataRetrievalService
    {
        private const int ShardSize = 1000;
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

        public virtual IQueryable<DAS_PostcodeDisadvantage> DasPostcodeDisadvantages => _postcodes.DAS_PostcodeDisadvantage;

        public virtual IQueryable<EFA_PostcodeDisadvantage> EfaPostcodeDisadvantages => _postcodes.EFA_PostcodeDisadvantage;

        public virtual IQueryable<CareerLearningPilot_Postcode> CareerLearningPilot_Postcodes => _postcodes.CareerLearningPilot_Postcode;

        public IEnumerable<string> UniquePostcodes(IMessage message)
        {
            return
                message.Learners.Select(l => l.Postcode)
                .Union(message.Learners.Select(l => l.PostcodePrior))
                .Union(message.Learners.Where(l => l.LearningDeliveries != null).SelectMany(l => l.LearningDeliveries).Select(ld => ld.DelLocPostCode))
                .Distinct();
        }

        public string CurrentVersion()
        {
            return VersionInfos.OrderByDescending(v => v.VersionNumber).Select(v => v.VersionNumber).FirstOrDefault();
        }

        public IDictionary<string, IEnumerable<SfaAreaCost>> SfaAreaCostsForPostcodes(IEnumerable<string> postcodes)
        {
            IDictionary<string, IEnumerable<SfaAreaCost>> result = new Dictionary<string, IEnumerable<SfaAreaCost>>().ToCaseInsensitiveDictionary();

            var postcodeShards = postcodes.SplitList(ShardSize);
            foreach (var shard in postcodeShards)
            {
                var data = SfaPostcodeAreaCosts
                    .Join(
                    shard,
                    p => p.Postcode,
                    s => s,
                    (p, s) => new { shard = s, postcode = p })
                    .Select(pc => new SfaAreaCost
                    {
                        Postcode = pc.postcode.Postcode,
                        AreaCostFactor = pc.postcode.AreaCostFactor,
                        EffectiveFrom = pc.postcode.EffectiveFrom,
                        EffectiveTo = pc.postcode.EffectiveTo,
                    })
                    .GroupBy(p => p.Postcode)
                    .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<SfaAreaCost>);

                foreach (var kvp in data)
                {
                    result.Add(kvp);
                }
            }

            return result;
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

        public IDictionary<string, IEnumerable<DasDisadvantage>> DasDisadvantagesForPostcodes(IEnumerable<string> postcodes)
        {
            IDictionary<string, IEnumerable<DasDisadvantage>> result = new Dictionary<string, IEnumerable<DasDisadvantage>>().ToCaseInsensitiveDictionary();

            var postcodeShards = postcodes.SplitList(ShardSize);
            foreach (var shard in postcodeShards)
            {
                 var data = DasPostcodeDisadvantages
                   .Join(
                   shard,
                   p => p.Postcode,
                   s => s,
                   (p, s) => new { shard = s, postcode = p })
                   .Select(pc => new DasDisadvantage
                   {
                       Postcode = pc.postcode.Postcode,
                       Uplift = pc.postcode.Uplift,
                       EffectiveFrom = pc.postcode.EffectiveFrom,
                       EffectiveTo = pc.postcode.EffectiveTo,
                   })
                   .GroupBy(p => p.Postcode)
                   .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<DasDisadvantage>);

                foreach (var kvp in data)
                {
                    result.Add(kvp);
                }
            }

            return result;
        }

        public DasDisadvantage DasDisadvantageFromEntity(DAS_PostcodeDisadvantage entity)
        {
            return new DasDisadvantage
            {
                Postcode = entity.Postcode,
                Uplift = entity.Uplift,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<string, IEnumerable<SfaDisadvantage>> SfaDisadvantagesForPostcodes(IEnumerable<string> postcodes)
        {
            IDictionary<string, IEnumerable<SfaDisadvantage>> result = new Dictionary<string, IEnumerable<SfaDisadvantage>>().ToCaseInsensitiveDictionary();

            var postcodeShards = postcodes.SplitList(ShardSize);
            foreach (var shard in postcodeShards)
            {
                var data = SfaPostcodeDisadvantages
                  .Join(
                  shard,
                  p => p.Postcode,
                  s => s,
                  (p, s) => new { shard = s, postcode = p })
                  .Select(pc => new SfaDisadvantage
                  {
                      Postcode = pc.postcode.Postcode,
                      Uplift = pc.postcode.Uplift,
                      EffectiveFrom = pc.postcode.EffectiveFrom,
                      EffectiveTo = pc.postcode.EffectiveTo,
                  })
                  .GroupBy(p => p.Postcode)
                  .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<SfaDisadvantage>);

                foreach (var kvp in data)
                {
                    result.Add(kvp);
                }
            }

            return result;
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
            IDictionary<string, IEnumerable<EfaDisadvantage>> result = new Dictionary<string, IEnumerable<EfaDisadvantage>>().ToCaseInsensitiveDictionary();

            var postcodeShards = postcodes.SplitList(ShardSize);
            foreach (var shard in postcodeShards)
            {
                var data = EfaPostcodeDisadvantages
                 .Join(
                 shard,
                 p => p.Postcode,
                 s => s,
                 (p, s) => new { shard = s, postcode = p })
                 .Select(pc => new EfaDisadvantage
                 {
                     Postcode = pc.postcode.Postcode,
                     Uplift = pc.postcode.Uplift,
                     EffectiveFrom = pc.postcode.EffectiveFrom,
                     EffectiveTo = pc.postcode.EffectiveTo,
                 })
                 .GroupBy(p => p.Postcode)
                 .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<EfaDisadvantage>);

                foreach (var kvp in data)
                {
                    result.Add(kvp);
                }
            }

            return result;
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

        public IDictionary<string, IEnumerable<CareerLearningPilot>> CareerLearningPilotsForPostcodes(IEnumerable<string> postcodes)
        {
            IDictionary<string, IEnumerable<CareerLearningPilot>> result = new Dictionary<string, IEnumerable<CareerLearningPilot>>().ToCaseInsensitiveDictionary();

            var postcodeShards = postcodes.SplitList(ShardSize);
            foreach (var shard in postcodeShards)
            {
               var data = CareerLearningPilot_Postcodes
                 .Join(
                 shard,
                 p => p.Postcode,
                 s => s,
                 (p, s) => new { shard = s, postcode = p })
                 .Select(pc => new CareerLearningPilot
                 {
                     Postcode = pc.postcode.Postcode,
                     AreaCode = pc.postcode.AreaCode,
                     EffectiveFrom = pc.postcode.EffectiveFrom,
                     EffectiveTo = pc.postcode.EffectiveTo,
                 })
                 .GroupBy(p => p.Postcode)
                 .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<CareerLearningPilot>);

                foreach (var kvp in data)
                {
                    result.Add(kvp);
                }
            }

            return result;
        }

        public CareerLearningPilot CareerLearningPilotFromEntity(CareerLearningPilot_Postcode entity)
        {
            return new CareerLearningPilot()
            {
                Postcode = entity.Postcode,
                AreaCode = entity.AreaCode,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}
