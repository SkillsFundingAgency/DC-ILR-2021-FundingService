using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface ILearnerPerActorService<T, out U>
       where T : class
    {
        IEnumerable<U> Process();
    }
}
