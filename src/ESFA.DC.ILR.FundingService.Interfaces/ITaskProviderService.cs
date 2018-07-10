using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface ITaskProviderService
    {
        void ExecuteTasks(IMessage message);
    }
}
