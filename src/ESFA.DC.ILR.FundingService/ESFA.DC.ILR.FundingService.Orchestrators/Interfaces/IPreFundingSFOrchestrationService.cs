using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IPreFundingSFOrchestrationService
    {
        void Execute(IJobContextMessage jobContextMessage);
    }
}
