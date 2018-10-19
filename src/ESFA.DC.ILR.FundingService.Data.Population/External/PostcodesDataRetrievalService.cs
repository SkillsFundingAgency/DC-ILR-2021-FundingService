using System;
using System.Collections.Generic;
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

        public virtual IQueryable<DAS_PostcodeDisadvantage> DasPostcodeDisadvantages => _postcodes.DAS_PostcodeDisadvantage;

        public virtual IQueryable<EFA_PostcodeDisadvantage> EfaPostcodeDisadvantages => _postcodes.EFA_PostcodeDisadvantage;

        public virtual IQueryable<CareerLearningPilot_Postcode> CareerLearningPilot_Postcodes => _postcodes.CareerLearningPilot_Postcode;

        public IEnumerable<string> UniquePostcodes(IMessage message)
        {
            return
                message.Learners.Select(l => l.Postcode)
                .Union(message.Learners.Select(l => l.PostcodePrior))
                .Union(message.Learners.Where(l => l.LearningDeliveries != null).SelectMany(l => l.LearningDeliveries).Select(ld => ld.DelLocPostCode))
                .Distinct()
                .ToList();
        }

        public string CurrentVersion()
        {
            return VersionInfos.OrderByDescending(v => v.VersionNumber).Select(v => v.VersionNumber).FirstOrDefault();
        }

        public IDictionary<string, IEnumerable<SfaAreaCost>> SfaAreaCostsForPostcodes(IEnumerable<string> postcodes)
        {
            IDictionary<string, IEnumerable<SfaAreaCost>> result = new Dictionary<string, IEnumerable<SfaAreaCost>>();

            var postcodeShards = SplitList(postcodes, 5000);
            foreach (var shard in postcodeShards)
            {
                var data = SfaPostcodeAreaCosts
                    .Where(p => postcodes.Contains(p.Postcode))
                    .GroupBy(a => a.Postcode)
                    .ToDictionary(a => a.Key, a => a.Select(SfaAreaCostFromEntity).ToList() as IEnumerable<SfaAreaCost>);
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
            IDictionary<string, IEnumerable<DasDisadvantage>> result = new Dictionary<string, IEnumerable<DasDisadvantage>>();

            var postcodeShards = SplitList(postcodes, 5000);
            foreach (var shard in postcodeShards)
            {
                var data = DasPostcodeDisadvantages
                .Where(p => shard.Contains(p.Postcode))
                .GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(DasDisadvantageFromEntity).ToList() as IEnumerable<DasDisadvantage>);
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
            IDictionary<string, IEnumerable<SfaDisadvantage>> result = new Dictionary<string, IEnumerable<SfaDisadvantage>>();

            var postcodeShards = SplitList(postcodes, 5000);
            foreach (var shard in postcodeShards)
            {
                var data = SfaPostcodeDisadvantages
                .Where(p => postcodes.Contains(p.Postcode))
                .GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(SfaDisadvantageFromEntity).ToList() as IEnumerable<SfaDisadvantage>);
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
            IDictionary<string, IEnumerable<EfaDisadvantage>> result = new Dictionary<string, IEnumerable<EfaDisadvantage>>();

            var postcodeShards = SplitList(postcodes, 5000);
            foreach (var shard in postcodeShards)
            {
                var data = EfaPostcodeDisadvantages
                .Where(p => postcodes.Contains(p.Postcode))
                .GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(EfaDisadvantageFromEntity).ToList() as IEnumerable<EfaDisadvantage>);
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
            IDictionary<string, IEnumerable<CareerLearningPilot>> result = new Dictionary<string, IEnumerable<CareerLearningPilot>>();

            var postcodeShards = SplitList(postcodes, 5000);
            foreach (var shard in postcodeShards)
            {
                var data = CareerLearningPilot_Postcodes
                .Where(p => postcodes.Contains(p.Postcode))
                .GroupBy(a => a.Postcode)
                .ToDictionary(a => a.Key, a => a.Select(CareerLearningPilotFromEntity).ToList() as IEnumerable<CareerLearningPilot>);
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

        private IEnumerable<IEnumerable<string>> SplitList(IEnumerable<string> postcodes, int nSize = 30)
        {
            var l = postcodes.ToList();

            for (var i = 0; i < l.Count; i += nSize)
            {
                yield return l.GetRange(i, Math.Min(nSize, l.Count - i));
            }
        }
    }
}
