namespace ESFA.DC.ILR.FundingService.Data.External.FCS.Model
{
    public class FCSContractAllocation
    {
        public string ContractAllocationNumber { get; set; }

        public string TenderSpecReference { get; set; }

        public string LotReference { get; set; }

        public int? CalcMethod { get; set; }
    }
}
