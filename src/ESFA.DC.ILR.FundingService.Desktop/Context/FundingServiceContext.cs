using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop.Context
{
    public class FundingServiceContext : IFundingServiceContext
    {
        private readonly IDesktopContext _desktopContext;

        public FundingServiceContext(IDesktopContext desktopContext)
        {
            _desktopContext = desktopContext;
        }

        public string FileReference => _desktopContext.KeyValuePairs[ILRContextKeys.Filename].ToString();

        public string Container => _desktopContext.KeyValuePairs[ILRContextKeys.Container].ToString();

        public string IlrReferenceDataKey => _desktopContext.KeyValuePairs[ILRContextKeys.IlrReferenceData].ToString();

        public string[] TaskKeys => new string[]
        {
            ILRContextKeys.FundingTaskALB,
            ILRContextKeys.FundingTaskFM25,
            ILRContextKeys.FundingTaskFM35,
            ILRContextKeys.FundingTaskFM36,
            ILRContextKeys.FundingTaskFM70,
            ILRContextKeys.FundingTaskFM81,
        };

        public string FundingALBOutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingAlbOutput].ToString();

        public string FundingFm25OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm25Output].ToString();

        public string FundingFm35OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm35Output].ToString();

        public string FundingFm36OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm36Output].ToString();

        public string FundingFm70OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm70Output].ToString();

        public string FundingFm81OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm81Output].ToString();

        public long JobId => throw new System.NotImplementedException();
    }
}
