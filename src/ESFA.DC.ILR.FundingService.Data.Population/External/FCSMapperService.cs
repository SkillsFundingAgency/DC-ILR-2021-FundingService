using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class FCSMapperService : IFCSMapperService
    {
        public FCSMapperService()
        {
        }

        public IReadOnlyCollection<FCSContractAllocation> MapFCSContractAllocations(IReadOnlyCollection<FcsContractAllocation> fcsContractAllocations)
        {
            return fcsContractAllocations.Select(MapFCSContractAllocation).ToList();
        }

        private FCSContractAllocation MapFCSContractAllocation(FcsContractAllocation fcsContractAllocation)
        {
            return new FCSContractAllocation
            {
                ContractAllocationNumber = fcsContractAllocation.ContractAllocationNumber,
                ContractStartDate = fcsContractAllocation.StartDate,
                ContractEndDate = fcsContractAllocation.EndDate,
                FundingStreamPeriodCode = fcsContractAllocation.FundingStreamPeriodCode,
                LearningRatePremiumFactor = fcsContractAllocation.LearningRatePremiumFactor,
                TenderSpecReference = fcsContractAllocation.TenderSpecReference,
                LotReference = fcsContractAllocation.LotReference,
                CalcMethod = fcsContractAllocation.EsfEligibilityRule.CalcMethod,
                FCSContractDeliverables = fcsContractAllocation.FCSContractDeliverables?
                .Select(cd =>
                new FCSContractDeliverable
                {
                    DeliverableCode = cd.DeliverableCode,
                    DeliverableDescription = cd.DeliverableDescription,
                    PlannedValue = cd.PlannedValue,
                    PlannedVolume = cd.PlannedVolume,
                    UnitCost = cd.UnitCost,
                    ExternalDeliverableCode = cd.ExternalDeliverableCode
                }).ToList()
            };
        }
    }
}
