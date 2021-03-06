﻿using System;
using System.Collections.Generic;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Config.Interfaces;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Modules;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ServiceFabric.Common.Modules;
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.FM25Actor
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                var builder = BuildContainer();

                builder.RegisterServiceFabricSupport();
                builder.RegisterActor<FM25Actor>(settings: new ActorServiceSettings
                {
                    ActorGarbageCollectionSettings = new ActorGarbageCollectionSettings(30, 30)
                });

                using (var container = builder.Build())
                {
                    // Not sure why this is being resolved here, to review
                    //var fm25 = container.Resolve<IFundingService<FM25LearnerDto, IEnumerable<FM25Global>>>();
                    //var actor = container.Resolve<IFundingService<FM25Global, IEnumerable<PeriodisationGlobal>>>();
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ContainerBuilder BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            var configHelper = new ConfigurationHelper();

            // register actor ALB funding module
            containerBuilder.RegisterModule<ActorFundingFM25Module>();

            var loggerConfig = configHelper.GetSectionValues<LoggerConfig>("LoggerSection");

            containerBuilder.RegisterInstance(loggerConfig).As<ILoggerConfig>().SingleInstance();

            // register serialization
            return containerBuilder;
        }
    }
}
