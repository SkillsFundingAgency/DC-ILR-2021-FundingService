using System.Linq;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.JobContext.Interface;
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

        public string FileReference
        {
            get => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString();
            set => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename] = value;
        }

        public string Container => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Container].ToString();

        public string IlrReferenceDataKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.IlrReferenceData].ToString();

        public string[] TaskKeys => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks).ToArray();

        public string FundingALBOutputKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput].ToString();

        public string FundingFm25OutputKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm25Output].ToString();

        public string FundingFm35OutputKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm35Output].ToString();

        public string FundingFm36OutputKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm36Output].ToString();

        public string FundingFm70OutputKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm70Output].ToString();

        public string FundingFm81OutputKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm81Output].ToString();
    }
}
