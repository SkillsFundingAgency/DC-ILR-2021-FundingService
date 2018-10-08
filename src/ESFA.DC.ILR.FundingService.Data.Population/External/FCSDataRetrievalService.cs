using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class FCSDataRetrievalService : IFCSDataRetrievalService
    {
        private readonly IFcsContext _fcs;

        public FCSDataRetrievalService()
        {
        }

        public FCSDataRetrievalService(IFcsContext fcs)
        {
            _fcs = fcs;
        }

        public virtual IQueryable<Contractor> Contractors => _fcs.Contractors;

        public virtual IQueryable<EsfEligibilityRule> EsfEligibilityRules => _fcs.EsfEligibilityRules;

        public IEnumerable<string> UniqueConRefNumbers(IMessage message)
        {
            return message.Learners
                .Where(l => l.LearningDeliveries != null)
                .SelectMany(l => l.LearningDeliveries)
                .Select(ld => ld.ConRefNumber)
                .Distinct();
        }

        public IDictionary<string, IEnumerable<FCSContractAllocation>> FCSContractAllocationsForUKPRN(int ukprn, IEnumerable<string> conRefNumbers)
        {
            return Contractors
               .Where(c => c.Ukprn == ukprn)
               .SelectMany(c => c.Contracts
               .SelectMany(ca => ca.ContractAllocations.Where(cr => conRefNumbers.Contains(cr.ContractAllocationNumber))
               .Select(FCSContractAllocationFromEntity)))
               .GroupBy(e => e.ContractAllocationNumber)
               .ToDictionary(a => a.Key, a => a as IEnumerable<FCSContractAllocation>);
        }

        public FCSContractAllocation FCSContractAllocationFromEntity(ContractAllocation entity)
        {
            return new FCSContractAllocation
            {
                ContractAllocationNumber = entity.ContractAllocationNumber,
                TenderSpecReference = entity.TenderSpecReference,
                LotReference = entity.LotReference,
                CalcMethod = EsfEligibilityRules?
                       .Where(e => e.TenderSpecReference == entity.TenderSpecReference && e.LotReference == entity.LotReference)
                       .Select(cm => cm.CalcMethod).SingleOrDefault()
            };
        }
    }
}
