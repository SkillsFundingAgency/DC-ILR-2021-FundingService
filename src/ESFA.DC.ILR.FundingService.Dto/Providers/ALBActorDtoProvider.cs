using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Dto.Providers
{
    public class ALBActorDtoProvider : IActorDtoProvider
    {
        private readonly int fundModelFilter = 99;

        private readonly ILearnerPagingService<ALBLearnerDto> _learnerPagingService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public ALBActorDtoProvider(ILearnerPagingService<ALBLearnerDto> learnerPagingService, IJsonSerializationService jsonSerializationService)
        {
            _learnerPagingService = learnerPagingService;
            _jsonSerializationService = jsonSerializationService;
        }

        public List<FundingActorDto> Provide(IFundingServiceContext fundingServiceContext, IMessage message, string externalDataCache, CancellationToken cancellationToken)
        {
            return _learnerPagingService
                .ProvideDtos(fundModelFilter, message)
                .Select(p =>
                    new FundingActorDto
                    {
                        JobId = fundingServiceContext.JobId,
                        Container = fundingServiceContext.Container,
                        OutputKey = fundingServiceContext.FundingALBOutputKey,
                        ExternalDataCache = externalDataCache,
                        ValidLearners = _jsonSerializationService.Serialize(p)
                    }).ToList();
        }
    }
}
