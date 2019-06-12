using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FundingActor.Interfaces
{
    public interface IActorDtoProvider
    {
        List<FundingDto> Provide(IFundingServiceContext fundingServiceContext, IMessage message, string externalDataCache, CancellationToken cancellationToken);
    }
}
