using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Context;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Logging.Interfaces;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.Stateless.Handlers
{
    public class MessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly StatelessServiceContext _context;
        private readonly ILogger _logger;

        public MessageHandler(ILifetimeScope lifeTimeScope, StatelessServiceContext context, ILogger logger)
        {
            _lifetimeScope = lifeTimeScope;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(JobContextMessage jobContextMessage, CancellationToken cancellationToken)
         {
            var fundingServiceContext = new FundingServiceContext(jobContextMessage);

            try
            {
                using (var childLifeTimeScope = _lifetimeScope.BeginLifetimeScope())
                {
                    // Get logger
                    ExecutionContext executionContext = (ExecutionContext)childLifeTimeScope.Resolve<IExecutionContext>();
                    executionContext.JobId = jobContextMessage.JobId.ToString();

                    _logger.LogDebug("Started Funding Calc Service");
                    var preFundingSfOrchestrationService = childLifeTimeScope.Resolve<IFundingOrchestrationService>();

                    // Call logic asynchronously, and return on initial context (no .ConfigureAwait(false))
                    await preFundingSfOrchestrationService.ExecuteAsync(fundingServiceContext, cancellationToken);

                    _logger.LogDebug("Completed Funding Calc Service");
                }

                ServiceEventSource.Current.ServiceMessage(_context, "Completed Funding Calc Service");
                return true;
            }
            catch (OutOfMemoryException oom)
            {
                Environment.FailFast("Funding Service Out Of Memory", oom);
                throw;
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(_context, "Exception-{0}", ex.ToString());
                throw;
            }
        }
    }
}
