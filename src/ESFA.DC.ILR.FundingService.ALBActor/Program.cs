using System;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.ServiceFabric.Helpers;

namespace ESFA.DC.ILR.FundingService.ALBActor
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

                // Register the Autofac magic for Service Fabric support.
                builder.RegisterServiceFabricSupport();

                // Register the actor service.
                builder.RegisterActor<ALBActor>();

                using (var container = builder.Build())
                {
                    var ss = container.Resolve<IALBOrchestrationService>();
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
            containerBuilder.RegisterModule<ActorFundingALBModule>();

            var loggerOptions =
                configHelper.GetSectionValues<LoggerOptions>("LoggerSection");
            containerBuilder.RegisterInstance(loggerOptions).As<LoggerOptions>().SingleInstance();
            containerBuilder.RegisterModule<LoggerModule>();

            // register serialization
            containerBuilder.RegisterType<JsonSerializationService>()
                .As<ISerializationService>();

            return containerBuilder;
        }
    }
}
