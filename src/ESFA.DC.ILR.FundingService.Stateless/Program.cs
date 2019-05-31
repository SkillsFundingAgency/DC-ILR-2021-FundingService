using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using StatelessModule = ESFA.DC.ILR.FundingService.Stateless.Modules.StatelessModule;

namespace ESFA.DC.ILR.FundingService.Stateless
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
                var builder = BuildContainerBuilder();

                builder.RegisterServiceFabricSupport();

                builder.RegisterStatelessService<ServiceFabric.Common.Stateless>("ESFA.DC.ILR1920.FundingService.StatelessType");

                using (var container = builder.Build())
                {
                    var ss = container.Resolve<IPreFundingSFOrchestrationService>();
                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ServiceFabric.Common.Stateless).Name);

                    // Prevents this host process from terminating so services keep running.
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ContainerBuilder BuildContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<StatelessModule>();

            return builder;
        }
    }
}
