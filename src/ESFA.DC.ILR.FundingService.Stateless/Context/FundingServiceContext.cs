using System.Linq;
using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.JobContextManager.Model;

namespace ESFA.DC.ILR.FundingService.Stateless.Context
{
    public class FundingServiceContext : IFundingServiceContext
    {
        private readonly JobContextMessage _jobContextMessage;

        public FundingServiceContext(JobContextMessage jobContextMessage)
        {
            _jobContextMessage = jobContextMessage;
        }

        public long JobId => _jobContextMessage.JobId;

        public string FileReference => _jobContextMessage.KeyValuePairs[ILRContextKeys.Filename].ToString();

        public string Container => _jobContextMessage.KeyValuePairs[ILRContextKeys.Container].ToString();

        public string IlrReferenceDataKey => _jobContextMessage.KeyValuePairs[ILRContextKeys.IlrReferenceData].ToString();

        public string[] TaskKeys => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks).ToArray();

        public string FundingALBOutputKey => _jobContextMessage.KeyValuePairs[ILRContextKeys.FundingAlbOutput].ToString();

        public string FundingFm25OutputKey => _jobContextMessage.KeyValuePairs[ILRContextKeys.FundingFm25Output].ToString();

        public string FundingFm35OutputKey => _jobContextMessage.KeyValuePairs[ILRContextKeys.FundingFm35Output].ToString();

        public string FundingFm36OutputKey => _jobContextMessage.KeyValuePairs[ILRContextKeys.FundingFm36Output].ToString();

        public string FundingFm70OutputKey => _jobContextMessage.KeyValuePairs[ILRContextKeys.FundingFm70Output].ToString();

        public string FundingFm81OutputKey => _jobContextMessage.KeyValuePairs[ILRContextKeys.FundingFm81Output].ToString();
    }
}
