using ESFA.DC.ILR.Model;

namespace ESFA.DC.ILR.FundingService.FM35.TaskProvider.Interface
{
    public interface ITaskProviderService
    {
        void ExecuteTasks(Message message);
    }
}
