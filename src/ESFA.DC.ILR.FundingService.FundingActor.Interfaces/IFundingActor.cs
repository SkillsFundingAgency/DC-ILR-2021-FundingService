﻿using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Dto;
using Microsoft.ServiceFabric.Actors;

namespace ESFA.DC.ILR.FundingService.FundingActor.Interfaces
{
    public interface IFundingActor : IActor
    {
        Task<string> Process(FundingDto actorModel, CancellationToken cancellationToken);
    }
}
