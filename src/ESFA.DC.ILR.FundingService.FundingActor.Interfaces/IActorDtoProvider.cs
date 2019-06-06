using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FundingActor.Interfaces
{
    public interface IActorDtoProvider
    {
        List<FundingActorDto> Provide(IFundingServiceContext fundingServiceContext, IMessage message, string externalDataCache, CancellationToken cancellationToken);
    }
}
