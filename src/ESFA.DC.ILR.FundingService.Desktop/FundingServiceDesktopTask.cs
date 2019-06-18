using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Desktop.Context;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.FundingService.Desktop
{
    public class FundingServiceDesktopTask : IDesktopTask
    {
        private ILifetimeScope _lifeTimeScope;

        public FundingServiceDesktopTask(ILifetimeScope lifeTimeScope)
        {
            _lifeTimeScope = lifeTimeScope;
        }

        public async Task<IDesktopContext> ExecuteAsync(IDesktopContext desktopContext, CancellationToken cancellationToken)
        {
            // Create context
            var fundingServiceContext = new FundingServiceContext(desktopContext);

            using (var cacheLifetimeScope = _lifeTimeScope.BeginLifetimeScope())
            {
                var refereceData = await cacheLifetimeScope.Resolve<IFileProviderService<ReferenceDataRoot>>().ProvideAsync(fundingServiceContext, cancellationToken);
                var externalDataCache = cacheLifetimeScope.Resolve<IExternalDataCachePopulationService>().PopulateAsync(refereceData, cancellationToken);

                using (var orchestratorLifetimeScope = cacheLifetimeScope.BeginLifetimeScope(c =>
                    c.RegisterInstance(externalDataCache).As<IExternalDataCache>()))
                {
                    // resolve Orchestrator
                    var orchestrator = orchestratorLifetimeScope.Resolve<IFundingOrchestrationService>();

                    await orchestrator.ExecuteAsync(fundingServiceContext, cancellationToken);
                }
            }

            return desktopContext;
        }
    }
}
