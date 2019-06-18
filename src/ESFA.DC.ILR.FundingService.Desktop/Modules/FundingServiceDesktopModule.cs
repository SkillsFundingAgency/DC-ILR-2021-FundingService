using Autofac;
using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.FundingService.Desktop.FundingTasks;
using ESFA.DC.ILR.FundingService.Desktop.Services;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Modules.FundingModules;

namespace ESFA.DC.ILR.FundingService.Desktop.Modules
{
    public class FundingServiceDesktopModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterType<DesktopFundingTaskProvider>().As<IFundingTaskProvider>();

            containerBuilder.RegisterModule<ALBModule>();

            containerBuilder.RegisterType<DesktopALBTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskALB).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM25ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM25).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM35ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM35).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM36ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM36).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM70ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM70).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM81ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM81).InstancePerLifetimeScope();
        }
    }
}