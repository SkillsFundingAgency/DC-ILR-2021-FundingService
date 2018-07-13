using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.KeyGenerator.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.FundingService.Stateless.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ILifetimeScope _parentLifeTimeScope;
        private readonly StatelessServiceContext _context;

        public MessageHandler(ILifetimeScope parentLifeTimeScope, StatelessServiceContext context)
        {
            _parentLifeTimeScope = parentLifeTimeScope;
            _context = context;
        }

        public async Task<bool> Handle(JobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            try
            {
                var keyGenerator = _parentLifeTimeScope.Resolve<IKeyGenerator>();
                var ukprn = Convert.ToInt64(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn]);

                jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput] =
                    keyGenerator.GenerateKey(ukprn, jobContextMessage.JobId, TaskKeys.FundingAlbOutput);

                using (var childLifeTimeScope = _parentLifeTimeScope.BeginLifetimeScope(c =>
                    c.RegisterInstance(jobContextMessage).As<IJobContextMessage>()))
                {
                    // get logger
                    var executionContext = (Logging.ExecutionContext)childLifeTimeScope.Resolve<IExecutionContext>();
                    executionContext.JobId = jobContextMessage.JobId.ToString();
                    var logger = childLifeTimeScope.Resolve<ILogger>();

                    logger.LogDebug("started funding calc");
                    var preFundingSfOrchestrationService =
                        childLifeTimeScope.Resolve<IPreFundingSFOrchestrationService>();

                    await preFundingSfOrchestrationService.Execute(jobContextMessage);
                }

                ServiceEventSource.Current.ServiceMessage(_context, "Job complete");
                return true;
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(_context, "Exception-{0}", ex.ToString());
                throw;
            }
        }
    }
}
