﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FundingActor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.FundingActor.Providers
{
    public class FM70ActorDtoProvider : IActorDtoProvider
    {
        private readonly int fundModelFilter = 70;

        private readonly ILearnerPagingService<FM70LearnerDto> _learnerPagingService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public FM70ActorDtoProvider(ILearnerPagingService<FM70LearnerDto> learnerPagingService, IJsonSerializationService jsonSerializationService)
        {
            _learnerPagingService = learnerPagingService;
            _jsonSerializationService = jsonSerializationService;
        }

        public List<FundingDto> Provide(IFundingServiceContext fundingServiceContext, IMessage message, string externalDataCache, CancellationToken cancellationToken)
        {
            return _learnerPagingService
                .ProvideDtos(fundModelFilter, message)
                .Select(p =>
                    new FundingDto
                    {
                        JobId = fundingServiceContext.JobId,
                        Container = fundingServiceContext.Container,
                        OutputKey = fundingServiceContext.FundingFm70OutputKey,
                        UKPRN = message.LearningProviderEntity.UKPRN,
                        ExternalDataCache = externalDataCache,
                        ValidLearners = _jsonSerializationService.Serialize(p)
                    }).ToList();
        }
    }
}