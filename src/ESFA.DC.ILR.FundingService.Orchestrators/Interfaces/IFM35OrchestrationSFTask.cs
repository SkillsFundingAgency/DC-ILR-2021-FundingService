using System.Threading.Tasks;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IFM35OrchestrationSFTask
    {
        Task Execute(IJobContextMessage jobContextMessage);
    }
}