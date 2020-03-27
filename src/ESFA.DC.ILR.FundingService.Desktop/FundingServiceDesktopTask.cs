using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.FundingService.Data.External;
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
                var externalDataCache = BuildExternalDataCache(cacheLifetimeScope.Resolve<IExternalDataCachePopulationService>().PopulateAsync(refereceData, cancellationToken));

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

        public IExternalDataCache BuildExternalDataCache(IExternalDataCache externalCache)
        {
            return new ExternalDataCache
            {
                AECLatestInYearEarningHistory = externalCache.AECLatestInYearEarningHistory,
                FCSContractAllocations = externalCache.FCSContractAllocations,
                LargeEmployers = externalCache.LargeEmployers,
                LARSCurrentVersion = externalCache.LARSCurrentVersion,
                LARSLearningDelivery = externalCache.LARSLearningDelivery.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase),
                LARSStandards = externalCache.LARSStandards,
                OrgFunding = externalCache.OrgFunding,
                OrgVersion = externalCache.OrgVersion,
                CampusIdentifierSpecResources = externalCache.CampusIdentifierSpecResources.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase),
                PostcodeCurrentVersion = externalCache.PostcodeCurrentVersion,
                PostcodeRoots = externalCache.PostcodeRoots.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase),
                Periods = externalCache.Periods,
            };
        }
    }
}
