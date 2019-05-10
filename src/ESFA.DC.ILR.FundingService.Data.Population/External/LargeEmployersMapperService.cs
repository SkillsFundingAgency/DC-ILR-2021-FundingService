using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class LargeEmployersMapperService : ILargeEmployersMapperService
    {
        public LargeEmployersMapperService()
        {
        }

        public IDictionary<int, IReadOnlyCollection<LargeEmployers>> MapLargeEmployers(IReadOnlyCollection<Employer> employers)
        {
            return employers?
                .Where(o => o.LargeEmployerEffectiveDates.Any())
                    .ToDictionary(
                    e => e.ERN,
                    l => l.LargeEmployerEffectiveDates?.Select(le => LargeEmployersFromEntity(le, l.ERN)).ToList() as IReadOnlyCollection<LargeEmployers>);
        }

        private LargeEmployers LargeEmployersFromEntity(LargeEmployerEffectiveDates entity, int employerId)
        {
            return new LargeEmployers()
            {
                ERN = employerId,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}
