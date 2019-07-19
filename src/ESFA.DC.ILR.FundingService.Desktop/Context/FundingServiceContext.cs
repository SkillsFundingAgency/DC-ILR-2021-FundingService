using System.Collections.Generic;
using System.Linq;
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

        public string[] TaskKeys => GetFundingTasks();

        public string FundingALBOutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingAlbOutput].ToString();

        public string FundingFm25OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm25Output].ToString();

        public string FundingFm35OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm35Output].ToString();

        public string FundingFm36OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm36Output].ToString();

        public string FundingFm70OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm70Output].ToString();

        public string FundingFm81OutputKey => _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm81Output].ToString();

        public IReadOnlyDictionary<string, string> FundingOutputKeys => new Dictionary<string, string>
        {
           { ILRContextKeys.FundingAlbOutput, _desktopContext.KeyValuePairs[ILRContextKeys.FundingAlbOutput].ToString() },
           //{ ILRContextKeys.FundingFm25Output, _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm25Output].ToString() },
           { ILRContextKeys.FundingFm35Output, _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm35Output].ToString() },
           { ILRContextKeys.FundingFm36Output, _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm36Output].ToString() },
           //{ ILRContextKeys.FundingFm70Output, _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm70Output].ToString() },
           //{ ILRContextKeys.FundingFm81Output, _desktopContext.KeyValuePairs[ILRContextKeys.FundingFm81Output].ToString() },
        };

        public long JobId => 0;

        public int Ukprn => int.Parse(_desktopContext.KeyValuePairs[ILRContextKeys.Ukprn].ToString());

        public string Year => _desktopContext.KeyValuePairs[ILRContextKeys.CollectionYear].ToString();

        private string[] GetFundingTasks()
        {
            var fundingTasks = new List<string>
            {
                _desktopContext.KeyValuePairs.TryGetValue(ILRContextKeys.FundingTaskALB, out var albOutputKey) ? albOutputKey.ToString() : null,
                _desktopContext.KeyValuePairs.TryGetValue(ILRContextKeys.FundingTaskFM25, out var fm25OutputKey) ? fm25OutputKey.ToString() : null,
                _desktopContext.KeyValuePairs.TryGetValue(ILRContextKeys.FundingTaskFM35, out var fm35OutputKey) ? fm35OutputKey.ToString() : null,
                _desktopContext.KeyValuePairs.TryGetValue(ILRContextKeys.FundingTaskFM36, out var fm36OutputKey) ? fm36OutputKey.ToString() : null,
                _desktopContext.KeyValuePairs.TryGetValue(ILRContextKeys.FundingTaskFM70, out var fm70OutputKey) ? fm70OutputKey.ToString() : null,
                _desktopContext.KeyValuePairs.TryGetValue(ILRContextKeys.FundingTaskFM81, out var fm81OutputKey) ? fm81OutputKey.ToString() : null,
            };

            return fundingTasks.Where(f => f != null).ToArray();
        }
    }
}
