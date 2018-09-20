using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.FM35Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM35)]
    public class FM35Actor : AbstractFundingActor, IFM35Actor
    {
        public FM35Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId, lifetimeScope, executionContext, jsonSerializationService)
        {
        }

        public async Task<string> Process(FundingActorDto actorModel, CancellationToken cancellationToken)
        {
            if (ExecutionContext is ExecutionContext executionContextObj)
            {
                executionContextObj.JobId = "-1";
                executionContextObj.TaskKey = ActorId.ToString();
            }

            ILogger logger = LifetimeScope.Resolve<ILogger>();

            IExternalDataCache externalDataCache;
            IInternalDataCache internalDataCache;
            IFileDataCache fileDataCache;

            try
            {
                logger.LogDebug($"{nameof(FM35Actor)} {ActorId} starting");

                externalDataCache = JsonSerializationService.Deserialize<ExternalDataCache>(actorModel.ExternalDataCache);
                internalDataCache = JsonSerializationService.Deserialize<InternalDataCache>(actorModel.InternalDataCache);
                fileDataCache = JsonSerializationService.Deserialize<FileDataCache>(actorModel.FileDataCache);

                logger.LogDebug($"{nameof(FM35Actor)} {ActorId} finished getting input data");

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                logger.LogError($"Error while processing {nameof(FM35Actor)} job", ex);
                throw;
            }

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(externalDataCache).As<IExternalDataCache>();
                c.RegisterInstance(internalDataCache).As<IInternalDataCache>();
                c.RegisterInstance(fileDataCache).As<IFileDataCache>();
            }))
            {
                ExecutionContext executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = actorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();
                ILogger jobLogger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    jobLogger.LogDebug($"{nameof(FM35Actor)} {ActorId} started processing");
                    IFundingService<ILearner, FM35FundingOutputs> fundingService = childLifetimeScope.Resolve<IFundingService<ILearner, FM35FundingOutputs>>();

                    List<MessageLearner> learners = JsonSerializationService.Deserialize<List<MessageLearner>>(actorModel.ValidLearners);

                    actorModel = null;

                    FM35FundingOutputs results = fundingService.ProcessFunding(learners, cancellationToken);

                    jobLogger.LogDebug($"{nameof(FM35Actor)} {ActorId} completed processing");
                    return JsonSerializationService.Serialize(results);
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    jobLogger.LogError($"Error while processing {nameof(FM35Actor)} job", ex);
                    throw;
                }
            }
        }
    }
}
