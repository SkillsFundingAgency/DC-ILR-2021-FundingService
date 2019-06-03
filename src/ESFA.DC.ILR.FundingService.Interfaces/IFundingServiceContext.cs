using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IFundingServiceContext
    {
        long JobId { get; }

        string FileReference { get; set; }

        string Container { get; }

        string IlrReferenceDataKey { get; }

        string[] TaskKeys { get; }

        string FundingALBOutputKey { get; }

        string FundingFm25OutputKey { get; }

        string FundingFm35OutputKey { get; }

        string FundingFm36OutputKey { get; }

        string FundingFm70OutputKey { get; }

        string FundingFm81OutputKey { get; }


    }
}
