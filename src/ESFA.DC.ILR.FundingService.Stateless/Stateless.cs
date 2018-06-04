using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DC.JobContextManager.Interface;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ESFA.DC.ILR.FundingService.Stateless
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public class Stateless : StatelessService
    {
        private readonly ILifetimeScope _parentLifeTimeScope;

        public Stateless(
             StatelessServiceContext context,
             ILifetimeScope parentLifeTimeScope)
            : base(context)
        {
            _parentLifeTimeScope = parentLifeTimeScope;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(
              context => _parentLifeTimeScope.Resolve<IJobContextManager>(),
              "FundingService-SBTopicListener");
        }
    }
}
