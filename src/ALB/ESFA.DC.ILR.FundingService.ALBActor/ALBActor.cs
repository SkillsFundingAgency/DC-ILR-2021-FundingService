﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.ALBActor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.ALB)]
    public class ALBActor : AbstractFundingActor, IALBActor
    {
        public ALBActor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope)
            : base(actorService, actorId, lifetimeScope)
        {
        }

        public Task<string> Process(FundingActorDto albActorModel)
        {
            var jsonSerializationService = LifetimeScope.Resolve<ISerializationService>();

            var referenceDataCache = jsonSerializationService.Deserialize<ExternalDataCache>(albActorModel.ExternalDataCache);

            var fileDataCache = jsonSerializationService.Deserialize<FileDataCache>(albActorModel.FileDataCache);

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(referenceDataCache).As<IExternalDataCache>();
                c.RegisterInstance(fileDataCache).As<IFileDataCache>();
            }))
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = albActorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();
                var logger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    logger.LogDebug("ALB Actor started processing");
                    var fundingService = childLifetimeScope.Resolve<IFundingService<ILearner, FundingOutputs>>();

                    var learners = jsonSerializationService.Deserialize<List<MessageLearner>>(albActorModel.ValidLearners);

                    albActorModel = null;

                    var results = fundingService.ProcessFunding(learners);

                    logger.LogDebug("ALB Actor completed processing");
                    return Task.FromResult(jsonSerializationService.Serialize(results));
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    logger.LogError("Error while processing Actor job", ex);
                    throw;
                }
            }
        }
    }
}