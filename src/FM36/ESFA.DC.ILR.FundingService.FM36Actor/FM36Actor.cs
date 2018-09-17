using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
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

namespace ESFA.DC.ILR.FundingService.FM36Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM36)]
    public class FM36Actor : AbstractFundingActor, IFM36Actor
    {
        public FM36Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope)
            : base(actorService, actorId, lifetimeScope)
        {
        }

        public Task<string> Process(FundingActorDto fm36ActorModel)
        {
            var jsonSerializationService = LifetimeScope.Resolve<ISerializationService>();

            var referenceDataCache = jsonSerializationService.Deserialize<ExternalDataCache>(fm36ActorModel.ExternalDataCache);

            var fileDataCache = jsonSerializationService.Deserialize<FileDataCache>(fm36ActorModel.FileDataCache);

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(referenceDataCache).As<IExternalDataCache>();
                c.RegisterInstance(fileDataCache).As<IFileDataCache>();
            }))
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = fm36ActorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();
                var logger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    logger.LogDebug("FM36 Actor started processing");
                    var fundingService = childLifetimeScope.Resolve<IFundingService<ILearner, FM36FundingOutputs>>();

                    var learners = jsonSerializationService.Deserialize<List<MessageLearner>>(fm36ActorModel.ValidLearners);

                    fm36ActorModel = null;

                    var results = fundingService.ProcessFunding(learners);

                    logger.LogDebug("FM36 Actor completed processing");
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
